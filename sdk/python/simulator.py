"""
Rhothor Galvo Deflection Simulator & Kinematics Engine
Simulates the internal controller of Newson Rhothor 2D/3D galvo scanners.
Used for offline development, software verification, and path visualization.
"""

import math
import time
from typing import List, Tuple, Dict, Any, Optional

# Standard Rhothor Error Constants
ERR_OK = -1
ERR_BUSY = 2
ERR_JOB = 3
ERR_HARDWARE = 5
ERR_MEMORY = 15
ERR_DATA = 13
ERR_IMPLEMENTATION = 23
ERR_LASER_LOW = 42
ERR_TRACKING = 43
ERR_LASER_HIGH = 46

# List modes
LIST_START = 1
AUTO_START = 2
AUTO_REPEAT = 3

ERROR_MESSAGES = {
    ERR_OK: "OK: Command completed successfully",
    ERR_BUSY: "System is busy processing job",
    ERR_JOB: "Error processing job / empty job / no list open",
    ERR_HARDWARE: "Hardware not responding",
    ERR_MEMORY: "Cannot allocate memory",
    ERR_DATA: "Cannot process data / invalid parameters",
    ERR_IMPLEMENTATION: "Function not supported",
    ERR_LASER_LOW: "Too little laser power detected",
    ERR_TRACKING: "System is not running / tracking stopped",
    ERR_LASER_HIGH: "Too much laser power detected"
}


class VectorCommand:
    """Represents an atomic galvo motion command with laser state."""
    def __init__(self, cmd_type: str, x: float, y: float, z: float = 0.0,
                 laser_on: bool = False, speed: float = 1000.0,
                 params: Optional[Dict[str, Any]] = None):
        self.cmd_type = cmd_type  # 'JUMP', 'LINE', 'ARC', 'MOVE', 'PULSE', 'BURST', 'SLEEP'
        self.x = x
        self.y = y
        self.z = z
        self.laser_on = laser_on
        self.speed = speed
        self.params = params or {}
        self.timestamp: float = 0.0
        self.duration_us: float = 0.0


class RhothorSimulator:
    """
    High-fidelity software simulation of Newson Rhothor Galvo Controller.
    Emulates the 160 MIPS DSP controller, list buffering, coordinate transformation,
    and laser gate timing.
    """

    def __init__(self, serial_nr: int = 4281, name: str = "LPKF",
                 field_size_xy: float = 201.65, field_size_z: float = 147.0,
                 max_speed_xy: float = 6000.0, max_speed_z: float = 4000.0,
                 scanner_delay_us: int = 60):
        self.serial_nr = serial_nr
        self.name = name
        self.version = "Rhothor DSP v7.5.0.27 (Simulated)"
        
        # Configuration
        self.field_size_xy = field_size_xy
        self.field_size_z = field_size_z
        self.max_speed_xy = max_speed_xy
        self.max_speed_z = max_speed_z
        self.scanner_delay_us = scanner_delay_us
        
        # Operational State
        self.is_initialized = False
        self.is_tracking = True
        self.selected_device = serial_nr
        self.current_x = 0.0
        self.current_y = 0.0
        self.current_z = 0.0
        self.mark_speed = 1000.0  # mm/sec
        self.jump_speed = max_speed_xy  # mm/sec
        
        # Laser Timing & Gate Configuration
        self.laser_on = False
        self.gate_on_delay_us = 50
        self.gate_off_delay_us = 50
        self.min_gate_period_us = 10
        self.pulse_bulge_factor = 1.0
        
        # Digital IO & Oscillators
        self.io_state = 0
        self.oscillators = {1: {"period": 20.0, "width": 5.0},
                            2: {"period": 20.0, "width": 5.0},
                            3: {"period": 20.0, "width": 5.0}}
        
        # Coordinate Offsets and Matrix
        self.offset_x = 0.0
        self.offset_y = 0.0
        self.offset_z = 0.0
        self.matrix = [[1.0, 0.0], [0.0, 1.0]]  # a11, a12, a21, a22
        
        # List buffering
        self.list_mode: Optional[int] = None
        self.is_list_open = False
        self.command_buffer: List[VectorCommand] = []
        self.history: List[VectorCommand] = []
        self.total_simulated_time_us: float = 0.0

    def initialise(self) -> int:
        """Initialise controller and servo loops."""
        self.is_initialized = True
        self.is_tracking = True
        self.current_x = 0.0
        self.current_y = 0.0
        self.current_z = 0.0
        self.command_buffer.clear()
        self.history.clear()
        self.total_simulated_time_us = 0.0
        return ERR_OK

    def get_free_dev_list(self) -> Tuple[int, List[int]]:
        """Returns list of available simulator devices."""
        return (ERR_OK, [self.serial_nr])

    def select_device(self, serial: int) -> int:
        if serial != self.serial_nr and serial != 0:
            return ERR_HARDWARE
        self.selected_device = serial
        return ERR_OK

    def get_serial(self) -> Tuple[int, int]:
        return (ERR_OK, self.serial_nr)

    def get_field_size(self) -> Tuple[int, float]:
        return (ERR_OK, self.field_size_xy)

    def get_max_speed(self) -> Tuple[int, float]:
        return (ERR_OK, self.max_speed_xy)

    def get_scanner_delay(self) -> Tuple[int, int]:
        return (ERR_OK, self.scanner_delay_us)

    def get_version(self) -> Tuple[int, str]:
        return (ERR_OK, self.version)

    def get_status(self) -> Tuple[int, int]:
        # Returns free buffer memory in bytes
        free_bytes = max(0, 1024 * 1024 - len(self.command_buffer) * 64)
        return (ERR_OK, free_bytes)

    def get_io(self) -> Tuple[int, int]:
        return (ERR_OK, self.io_state)

    def set_io(self, value: int, mask: int) -> int:
        self.io_state = (self.io_state & ~mask) | (value & mask)
        return ERR_OK

    def set_speed(self, speed: float) -> int:
        if speed <= 0 or speed > self.max_speed_xy:
            return ERR_DATA
        self.mark_speed = speed
        return ERR_OK

    def set_jump_speed(self, speed: float) -> int:
        if speed <= 0 or speed > self.max_speed_xy:
            return ERR_DATA
        self.jump_speed = speed
        return ERR_OK

    def set_laser(self, on_off: bool) -> int:
        self.laser_on = bool(on_off)
        return ERR_OK

    def set_laser_times(self, on_delay: int, off_delay: int) -> int:
        self.gate_on_delay_us = on_delay
        self.gate_off_delay_us = off_delay
        return ERR_OK

    def set_matrix(self, a11: float, a12: float, a21: float, a22: float) -> int:
        self.matrix = [[a11, a12], [a21, a22]]
        return ERR_OK

    def set_offsets(self, x: float, y: float) -> int:
        self.offset_x = x
        self.offset_y = y
        return ERR_OK

    def set_offset_z(self, z: float) -> int:
        self.offset_z = z
        return ERR_OK

    def list_open(self, mode: int) -> int:
        if mode not in (LIST_START, AUTO_START, AUTO_REPEAT):
            return ERR_DATA
        self.list_mode = mode
        self.is_list_open = True
        return ERR_OK

    def list_close(self) -> int:
        if not self.is_list_open:
            return ERR_JOB
        self.is_list_open = False
        # In simulator, execute buffered commands
        self._execute_buffer()
        return ERR_OK

    def abort(self) -> int:
        self.command_buffer.clear()
        self.is_list_open = False
        self.laser_on = False
        return ERR_OK

    def _apply_transform(self, x: float, y: float) -> Tuple[float, float]:
        tx = self.matrix[0][0] * x + self.matrix[0][1] * y + self.offset_x
        ty = self.matrix[1][0] * x + self.matrix[1][1] * y + self.offset_y
        return (tx, ty)

    def jump_to(self, x: float, y: float) -> int:
        return self.jump_to_3d(x, y, self.current_z)

    def jump_to_3d(self, x: float, y: float, z: float) -> int:
        if not self.is_list_open and self.list_mode != AUTO_START:
            return ERR_JOB
        tx, ty = self._apply_transform(x, y)
        tz = z + self.offset_z
        cmd = VectorCommand("JUMP", tx, ty, tz, laser_on=False, speed=self.jump_speed)
        self._add_command(cmd)
        return ERR_OK

    def line_to(self, x: float, y: float) -> int:
        return self.line_to_3d(x, y, self.current_z)

    def line_to_3d(self, x: float, y: float, z: float) -> int:
        if not self.is_list_open and self.list_mode != AUTO_START:
            return ERR_JOB
        tx, ty = self._apply_transform(x, y)
        tz = z + self.offset_z
        cmd = VectorCommand("LINE", tx, ty, tz, laser_on=True, speed=self.mark_speed)
        self._add_command(cmd)
        return ERR_OK

    def arc_to(self, x: float, y: float, bf: float) -> int:
        if not self.is_list_open and self.list_mode != AUTO_START:
            return ERR_JOB
        tx, ty = self._apply_transform(x, y)
        cmd = VectorCommand("ARC", tx, ty, self.current_z, laser_on=True,
                            speed=self.mark_speed, params={"bf": bf})
        self._add_command(cmd)
        return ERR_OK

    def move_to(self, x: float, y: float) -> int:
        return self.move_to_3d(x, y, self.current_z)

    def move_to_3d(self, x: float, y: float, z: float) -> int:
        if not self.is_list_open and self.list_mode != AUTO_START:
            return ERR_JOB
        tx, ty = self._apply_transform(x, y)
        tz = z + self.offset_z
        cmd = VectorCommand("MOVE", tx, ty, tz, laser_on=False, speed=self.mark_speed)
        self._add_command(cmd)
        return ERR_OK

    def pulse(self, x: float, y: float) -> int:
        return self.pulse_3d(x, y, self.current_z)

    def pulse_3d(self, x: float, y: float, z: float) -> int:
        if not self.is_list_open and self.list_mode != AUTO_START:
            return ERR_JOB
        tx, ty = self._apply_transform(x, y)
        tz = z + self.offset_z
        cmd = VectorCommand("PULSE", tx, ty, tz, laser_on=True, speed=self.mark_speed)
        self._add_command(cmd)
        return ERR_OK

    def burst(self, time_us: int) -> int:
        if not self.is_list_open and self.list_mode != AUTO_START:
            return ERR_JOB
        cmd = VectorCommand("BURST", self.current_x, self.current_y, self.current_z,
                            laser_on=True, speed=0, params={"duration_us": time_us})
        self._add_command(cmd)
        return ERR_OK

    def sleep(self, time_us: int) -> int:
        if not self.is_list_open and self.list_mode != AUTO_START:
            return ERR_JOB
        cmd = VectorCommand("SLEEP", self.current_x, self.current_y, self.current_z,
                            laser_on=False, speed=0, params={"duration_us": time_us})
        self._add_command(cmd)
        return ERR_OK

    def _add_command(self, cmd: VectorCommand):
        self.command_buffer.append(cmd)
        if self.list_mode == AUTO_START:
            self._execute_buffer()

    def _execute_buffer(self):
        for cmd in self.command_buffer:
            dx = cmd.x - self.current_x
            dy = cmd.y - self.current_y
            dz = cmd.z - self.current_z
            dist = math.sqrt(dx * dx + dy * dy + dz * dz)
            
            speed = cmd.speed if cmd.speed > 0 else 1000.0
            motion_time_us = (dist / speed) * 1_000_000.0
            
            # Incorporate scanner delay and laser delays
            if cmd.cmd_type == "JUMP":
                cmd.duration_us = motion_time_us + self.scanner_delay_us
            elif cmd.cmd_type in ("LINE", "ARC"):
                cmd.duration_us = motion_time_us + self.gate_on_delay_us + self.gate_off_delay_us
            elif cmd.cmd_type in ("BURST", "SLEEP"):
                cmd.duration_us = cmd.params.get("duration_us", 0.0)
            elif cmd.cmd_type == "PULSE":
                cmd.duration_us = 5.0  # standard 5 us pulse
            else:
                cmd.duration_us = motion_time_us
                
            cmd.timestamp = self.total_simulated_time_us
            self.total_simulated_time_us += cmd.duration_us
            
            self.current_x = cmd.x
            self.current_y = cmd.y
            self.current_z = cmd.z
            self.history.append(cmd)
            
        self.command_buffer.clear()

    def get_kinematic_stats(self) -> Dict[str, Any]:
        """Calculates total mark length, jump length, execution time, and duty cycle."""
        total_mark_dist = 0.0
        total_jump_dist = 0.0
        mark_count = 0
        jump_count = 0
        
        last_x, last_y, last_z = 0.0, 0.0, 0.0
        if self.history:
            last_x = self.history[0].x
            last_y = self.history[0].y
            last_z = self.history[0].z

        for cmd in self.history:
            dx = cmd.x - last_x
            dy = cmd.y - last_y
            dz = cmd.z - last_z
            dist = math.sqrt(dx * dx + dy * dy + dz * dz)
            if cmd.laser_on:
                total_mark_dist += dist
                mark_count += 1
            else:
                total_jump_dist += dist
                jump_count += 1
            last_x, last_y, last_z = cmd.x, cmd.y, cmd.z

        total_time_s = self.total_simulated_time_us / 1_000_000.0
        return {
            "total_commands": len(self.history),
            "mark_commands": mark_count,
            "jump_commands": jump_count,
            "total_mark_distance_mm": round(total_mark_dist, 3),
            "total_jump_distance_mm": round(total_jump_dist, 3),
            "total_distance_mm": round(total_mark_dist + total_jump_dist, 3),
            "estimated_time_seconds": round(total_time_s, 4),
            "duty_cycle_percent": round((total_mark_dist / (total_mark_dist + total_jump_dist + 1e-9)) * 100, 2)
        }

    def export_svg(self, filename: str, width_px: int = 800, height_px: int = 800) -> str:
        """
        Exports recorded toolpath to SVG.
        Marking lines are red, jumps are dashed cyan.
        """
        half_field = self.field_size_xy / 2.0
        margin = 40.0
        scale = (width_px - 2 * margin) / self.field_size_xy
        
        def to_svg(x: float, y: float) -> Tuple[float, float]:
            sx = margin + (x + half_field) * scale
            sy = margin + (half_field - y) * scale  # Invert Y for screen coordinates
            return (sx, sy)

        svg_lines = [
            f'<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 {width_px} {height_px}" '
            f'width="{width_px}" height="{height_px}" style="background-color: #121316; font-family: monospace;">',
            f'<!-- Galvanometer Working Field Boundary ({self.field_size_xy} mm) -->',
            f'<rect x="{margin}" y="{margin}" width="{width_px - 2*margin}" height="{height_px - 2*margin}" '
            f'fill="#1a1c23" stroke="#2e3440" stroke-width="2" rx="6" />',
            f'<line x1="{width_px/2}" y1="{margin}" x2="{width_px/2}" y2="{height_px-margin}" stroke="#3b4252" stroke-dasharray="4,4" />',
            f'<line x1="{margin}" y1="{height_px/2}" x2="{width_px-margin}" y2="{height_px/2}" stroke="#3b4252" stroke-dasharray="4,4" />'
        ]

        # Draw paths
        cur_x, cur_y = 0.0, 0.0
        for cmd in self.history:
            x1, y1 = to_svg(cur_x, cur_y)
            x2, y2 = to_svg(cmd.x, cmd.y)
            
            if cmd.laser_on:
                # Laser Mark: Glowing Red/Orange line
                svg_lines.append(
                    f'<line x1="{x1:.2f}" y1="{y1:.2f}" x2="{x2:.2f}" y2="{y2:.2f}" '
                    f'stroke="#ff3366" stroke-width="2.0" stroke-linecap="round" />'
                )
            else:
                # Fast Galvo Jump: Dashed Cyan line
                svg_lines.append(
                    f'<line x1="{x1:.2f}" y1="{y1:.2f}" x2="{x2:.2f}" y2="{y2:.2f}" '
                    f'stroke="#00d8d6" stroke-width="0.8" stroke-dasharray="3,3" opacity="0.6" />'
                )
            cur_x, cur_y = cmd.x, cmd.y

        # Header title and statistics overlay
        stats = self.get_kinematic_stats()
        svg_lines.append(
            f'<text x="{margin + 10}" y="{margin + 25}" fill="#88c0d0" font-size="14" font-weight="bold">'
            f'Rhothor Galvo Trajectory - {self.name} (SN: {self.serial_nr})</text>'
        )
        svg_lines.append(
            f'<text x="{margin + 10}" y="{margin + 45}" fill="#d8dee9" font-size="11">'
            f'Mark: {stats["total_mark_distance_mm"]}mm | Jump: {stats["total_jump_distance_mm"]}mm | '
            f'Est. Time: {stats["estimated_time_seconds"]}s</text>'
        )
        svg_lines.append('</svg>')

        svg_content = "\n".join(svg_lines)
        with open(filename, "w", encoding="utf-8") as f:
            f.write(svg_content)
        return filename
