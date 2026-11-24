"""
Python SDK for Newson Rhothor 2D & 3D Galvo Scanning Systems.
Wraps 'rhothor.dll' C-API via ctypes with seamless cross-platform fallback to
RhothorSimulator for development, testing, and vector trajectory verification.
"""

import os
import sys
import ctypes
from typing import List, Tuple, Dict, Any, Optional

from .simulator import (
    RhothorSimulator,
    ERR_OK, ERR_BUSY, ERR_JOB, ERR_HARDWARE, ERR_MEMORY,
    ERR_DATA, ERR_IMPLEMENTATION, ERR_LASER_LOW, ERR_TRACKING, ERR_LASER_HIGH,
    LIST_START, AUTO_START, AUTO_REPEAT,
    ERROR_MESSAGES
)


class RhothorError(Exception):
    """Exception raised when a Rhothor DLL call returns an error code."""
    def __init__(self, error_code: int, func_name: str = ""):
        self.error_code = error_code
        self.func_name = func_name
        self.message = ERROR_MESSAGES.get(error_code, f"Unknown error code {error_code}")
        super().__init__(f"Rhothor error in {func_name}: [{error_code}] {self.message}")


class RhothorNative:
    """
    Direct low-level Ctypes bindings to rhothor.dll.
    Matches rhothorDll.h function declarations.
    """
    def __init__(self, dll_path: Optional[str] = None):
        self.dll = None
        self.is_loaded = False
        
        if sys.platform == "win32":
            candidates = []
            if dll_path:
                candidates.append(dll_path)
            
            # Standard locations relative to project, frozen app, or working dir
            app_dir = os.path.dirname(os.path.abspath(sys.executable)) if getattr(sys, "frozen", False) else ""
            meipass = getattr(sys, "_MEIPASS", "")
            base_dir = os.path.dirname(os.path.abspath(__file__))
            is_64bit = sys.maxsize > 2**32
            arch_dir = "x64" if is_64bit else "x86"

            if app_dir:
                candidates.append(os.path.join(app_dir, "rhothor.dll"))
                candidates.append(os.path.join(app_dir, "sdk", "lib", arch_dir, "rhothor.dll"))
            if meipass:
                candidates.append(os.path.join(meipass, "rhothor.dll"))
                candidates.append(os.path.join(meipass, "sdk", "lib", arch_dir, "rhothor.dll"))

            candidates.append(os.path.join(base_dir, "..", "lib", arch_dir, "rhothor.dll"))
            candidates.append(os.path.join(base_dir, "rhothor.dll"))
            candidates.append("rhothor.dll")
            
            for path in candidates:
                if os.path.exists(path):
                    try:
                        self.dll = ctypes.WinDLL(path)
                        self.is_loaded = True
                        break
                    except Exception as e:
                        print(f"Warning: Failed to load {path}: {e}")
            
            if self.is_loaded:
                self._bind_functions()

    def _bind_functions(self):
        """Configure argtypes and restype for all functions in rhothor.dll with safe fallback."""
        d = self.dll
        c_long = ctypes.c_long
        c_double = ctypes.c_double
        c_char_p = ctypes.c_char_p
        c_bool = ctypes.c_bool
        p_long = ctypes.POINTER(c_long)
        p_double = ctypes.POINTER(c_double)
        
        def _bind(name, argtypes, restype):
            if hasattr(d, name):
                try:
                    fn = getattr(d, name)
                    fn.argtypes = argtypes
                    fn.restype = restype
                    return fn
                except Exception as ex:
                    print(f"Notice: Failed to bind {name}: {ex}")
            return None

        # Accessing functions
        _bind("rtGetFreeDevList", [p_long, p_long], c_long)
        _bind("rtInitialise", [], c_long)
        _bind("rtSelectDevice", [c_long], c_long)

        # Query functions
        _bind("rtGetChannelData", [c_long, p_long, p_long, p_double], c_long)
        _bind("rtGetFieldSize", [p_double], c_long)
        _bind("rtGetIO", [p_long], c_long)
        _bind("rtGetMaxSpeed", [p_double], c_long)
        _bind("rtGetResolvers", [p_double, p_double], c_long)
        _bind("rtGetScannerDelay", [p_long], c_long)
        _bind("rtGetSerial", [p_long], c_long)
        _bind("rtGetSetpointFilter", [p_long], c_long)
        _bind("rtGetStatus", [p_long], c_long)
        _bind("rtGetVersion", [c_char_p, c_long], c_long)

        # Control functions
        _bind("rtAbort", [], c_long)
        _bind("rtArcTo", [c_double, c_double, c_double], c_long)
        _bind("rtBurst", [c_long], c_long)
        _bind("rtJumpTo", [c_double, c_double], c_long)
        _bind("rtLineTo", [c_double, c_double], c_long)
        _bind("rtListClose", [], c_long)
        _bind("rtListOpen", [c_long], c_long)
        _bind("rtMoveTo", [c_double, c_double], c_long)
        _bind("rtPulse", [c_double, c_double], c_long)
        _bind("rtSetIO", [c_long, c_long], c_long)
        _bind("rtSetLaser", [c_bool], c_long)
        _bind("rtSetLaserTimes", [c_long, c_long], c_long)
        _bind("rtSetMatrix", [c_double, c_double, c_double, c_double], c_long)
        _bind("rtSetMaxSpeed", [c_double], c_long)  # Present in v11, gracefully skipped in v7.5
        _bind("rtSetOffsXY", [c_double, c_double], c_long)
        _bind("rtSetOffsZ", [c_double], c_long)
        _bind("rtSetOscillator", [c_long, c_double, c_double], c_long)
        _bind("rtSetSpeed", [c_double], c_long)
        _bind("rtSetJumpSpeed", [c_double], c_long)
        _bind("rtSleep", [c_long], c_long)

        # 3D functions
        _bind("rtMoveTo3D", [c_double, c_double, c_double], c_long)
        _bind("rtLineTo3D", [c_double, c_double, c_double], c_long)
        _bind("rtJumpTo3D", [c_double, c_double, c_double], c_long)
        _bind("rtPulse3D", [c_double, c_double, c_double], c_long)


class RhothorController:
    """
    High-level, Pythonic interface to Newson Rhothor 2D & 3D Galvo Scanners.
    Operates transparently with the physical hardware (via rhothor.dll) or in
    simulation mode when hardware is not present.
    """

    def __init__(self, serial_nr: Optional[int] = 4281, name: str = "LPKF",
                 simulate: bool = False, dll_path: Optional[str] = None):
        self.serial_nr = serial_nr
        self.name = name
        self.simulate = simulate
        self.native: Optional[RhothorNative] = None
        self.is_connected = False
        self.sim: Optional[RhothorSimulator] = None
        
        # Try native DLL if not explicitly in simulate mode
        if not simulate and sys.platform == "win32":
            self.native = RhothorNative(dll_path)
            if not self.native.is_loaded:
                print("Notice: rhothor.dll not found or incompatible. Falling back to RhothorSimulator.")
                self.simulate = True
        else:
            self.simulate = True
            
        if self.simulate:
            self.sim = RhothorSimulator(serial_nr=serial_nr or 4281, name=name)

    def __enter__(self):
        self.connect()
        return self

    def __exit__(self, exc_type, exc_val, exc_tb):
        self.disconnect()

    def connect(self):
        """Initializes the controller and selects the target device."""
        if self.simulate:
            res = self.sim.initialise()
            if self.serial_nr:
                self.sim.select_device(self.serial_nr)
            self.is_connected = True
            return

        res = self.native.dll.rtInitialise()
        if res != ERR_OK:
            raise RhothorError(res, "rtInitialise")
            
        if self.serial_nr is not None:
            res = self.native.dll.rtSelectDevice(ctypes.c_long(self.serial_nr))
            if res != ERR_OK:
                raise RhothorError(res, f"rtSelectDevice({self.serial_nr})")
        self.is_connected = True

    def disconnect(self):
        """Safely stops motion and turns off laser."""
        try:
            self.abort()
            self.set_laser(False)
        except Exception:
            pass
        self.is_connected = False

    def abort(self):
        """Immediately aborts any active command list."""
        if self.simulate:
            self.sim.abort()
        else:
            self.native.dll.rtAbort()

    # --- Query Methods ---

    def get_serial(self) -> int:
        if self.simulate:
            _, s = self.sim.get_serial()
            return s
        val = ctypes.c_long()
        res = self.native.dll.rtGetSerial(ctypes.byref(val))
        if res != ERR_OK:
            raise RhothorError(res, "rtGetSerial")
        return val.value

    def get_field_size(self) -> float:
        if self.simulate:
            _, s = self.sim.get_field_size()
            return s
        val = ctypes.c_double()
        res = self.native.dll.rtGetFieldSize(ctypes.byref(val))
        if res != ERR_OK:
            raise RhothorError(res, "rtGetFieldSize")
        return val.value

    def get_max_speed(self) -> float:
        if self.simulate:
            _, s = self.sim.get_max_speed()
            return s
        val = ctypes.c_double()
        res = self.native.dll.rtGetMaxSpeed(ctypes.byref(val))
        if res != ERR_OK:
            raise RhothorError(res, "rtGetMaxSpeed")
        return val.value

    def get_scanner_delay(self) -> int:
        if self.simulate:
            _, s = self.sim.get_scanner_delay()
            return s
        val = ctypes.c_long()
        res = self.native.dll.rtGetScannerDelay(ctypes.byref(val))
        if res != ERR_OK:
            raise RhothorError(res, "rtGetScannerDelay")
        return val.value

    def get_version(self) -> str:
        if self.simulate:
            _, s = self.sim.get_version()
            return s
        buf = ctypes.create_string_buffer(256)
        res = self.native.dll.rtGetVersion(buf, 256)
        if res != ERR_OK:
            raise RhothorError(res, "rtGetVersion")
        return buf.value.decode("ascii", errors="ignore")

    def get_device_info(self) -> Dict[str, Any]:
        """Returns comprehensive status and configuration dictionary."""
        return {
            "mode": "Simulated" if self.simulate else "Hardware",
            "name": self.name,
            "serial": self.get_serial(),
            "version": self.get_version(),
            "field_size_mm": self.get_field_size(),
            "max_speed_mm_s": self.get_max_speed(),
            "scanner_delay_us": self.get_scanner_delay()
        }

    # --- Configuration Methods ---

    def set_speed(self, mark_speed: float, jump_speed: Optional[float] = None):
        """Sets marking speed and optional fast jump speed in mm/sec."""
        if self.simulate:
            res = self.sim.set_speed(mark_speed)
            if res != ERR_OK:
                raise RhothorError(res, "rtSetSpeed")
            if jump_speed:
                res2 = self.sim.set_jump_speed(jump_speed)
                if res2 != ERR_OK:
                    raise RhothorError(res2, "rtSetJumpSpeed")
            return

        res = self.native.dll.rtSetSpeed(ctypes.c_double(mark_speed))
        if res != ERR_OK:
            raise RhothorError(res, "rtSetSpeed")
        if jump_speed is not None:
            res = self.native.dll.rtSetJumpSpeed(ctypes.c_double(jump_speed))
            if res != ERR_OK:
                raise RhothorError(res, "rtSetJumpSpeed")

    def set_max_speed(self, max_speed: float):
        """Sets max speed if supported by firmware/DLL (v11+), or safely ignores on v7.5."""
        if self.simulate:
            if hasattr(self.sim, "set_max_speed"):
                self.sim.set_max_speed(max_speed)
            return

        if hasattr(self.native.dll, "rtSetMaxSpeed"):
            res = self.native.dll.rtSetMaxSpeed(ctypes.c_double(max_speed))
            if res != ERR_OK:
                raise RhothorError(res, "rtSetMaxSpeed")

    def set_laser_delays(self, gate_on_delay_us: int, gate_off_delay_us: int):
        """Configures laser gate on and off delays in microseconds."""
        if self.simulate:
            self.sim.set_laser_times(gate_on_delay_us, gate_off_delay_us)
            return
        res = self.native.dll.rtSetLaserTimes(ctypes.c_long(gate_on_delay_us),
                                              ctypes.c_long(gate_off_delay_us))
        if res != ERR_OK:
            raise RhothorError(res, "rtSetLaserTimes")

    def set_laser(self, on_off: bool):
        """Manually forces the laser state on or off."""
        if self.simulate:
            self.sim.set_laser(on_off)
            return
        res = self.native.dll.rtSetLaser(ctypes.c_bool(on_off))
        if res != ERR_OK:
            raise RhothorError(res, "rtSetLaser")

    def set_matrix(self, a11: float, a12: float, a21: float, a22: float):
        """Sets 2x2 coordinate transformation matrix (scaling, rotation, shear)."""
        if self.simulate:
            self.sim.set_matrix(a11, a12, a21, a22)
            return
        res = self.native.dll.rtSetMatrix(ctypes.c_double(a11), ctypes.c_double(a12),
                                          ctypes.c_double(a21), ctypes.c_double(a22))
        if res != ERR_OK:
            raise RhothorError(res, "rtSetMatrix")

    def set_offsets(self, x: float, y: float, z: float = 0.0):
        """Sets working coordinate offsets."""
        if self.simulate:
            self.sim.set_offsets(x, y)
            self.sim.set_offset_z(z)
            return
        self.native.dll.rtSetOffsXY(ctypes.c_double(x), ctypes.c_double(y))
        if z != 0.0:
            self.native.dll.rtSetOffsZ(ctypes.c_double(z))

    def set_oscillator(self, nr: int, period_us: float, pulse_width_us: float):
        """Configures one of the 3 internal hardware oscillators for laser pulses."""
        if self.simulate:
            self.sim.oscillators[nr] = {"period": period_us, "width": pulse_width_us}
            return
        res = self.native.dll.rtSetOscillator(ctypes.c_long(nr),
                                              ctypes.c_double(period_us),
                                              ctypes.c_double(pulse_width_us))
        if res != ERR_OK:
            raise RhothorError(res, f"rtSetOscillator({nr})")

    # --- List Management ---

    def list_open(self, mode: int = LIST_START):
        """Opens a command list (LIST_START=1, AUTO_START=2, AUTO_REPEAT=3)."""
        if self.simulate:
            res = self.sim.list_open(mode)
        else:
            res = self.native.dll.rtListOpen(ctypes.c_long(mode))
        if res != ERR_OK:
            raise RhothorError(res, f"rtListOpen({mode})")

    def list_close(self):
        """Closes and dispatches the command list for execution."""
        if self.simulate:
            res = self.sim.list_close()
        else:
            res = self.native.dll.rtListClose()
        if res != ERR_OK:
            raise RhothorError(res, "rtListClose")

    # --- Motion & Marking Commands ---

    def jump_to(self, x: float, y: float):
        """Fast positioning jump with laser OFF."""
        if self.simulate:
            res = self.sim.jump_to(x, y)
        else:
            res = self.native.dll.rtJumpTo(ctypes.c_double(x), ctypes.c_double(y))
        if res != ERR_OK:
            raise RhothorError(res, f"rtJumpTo({x}, {y})")

    def line_to(self, x: float, y: float):
        """Linear vector mark to (X, Y) with laser ON."""
        if self.simulate:
            res = self.sim.line_to(x, y)
        else:
            res = self.native.dll.rtLineTo(ctypes.c_double(x), ctypes.c_double(y))
        if res != ERR_OK:
            raise RhothorError(res, f"rtLineTo({x}, {y})")

    def arc_to(self, x: float, y: float, bf: float):
        """Circular arc mark to (X, Y) with bulge factor BF and laser ON."""
        if self.simulate:
            res = self.sim.arc_to(x, y, bf)
        else:
            res = self.native.dll.rtArcTo(ctypes.c_double(x), ctypes.c_double(y), ctypes.c_double(bf))
        if res != ERR_OK:
            raise RhothorError(res, f"rtArcTo({x}, {y}, {bf})")

    def move_to(self, x: float, y: float):
        """Move to (X, Y) at mark speed with laser OFF."""
        if self.simulate:
            res = self.sim.move_to(x, y)
        else:
            res = self.native.dll.rtMoveTo(ctypes.c_double(x), ctypes.c_double(y))
        if res != ERR_OK:
            raise RhothorError(res, f"rtMoveTo({x}, {y})")

    def pulse(self, x: float, y: float):
        """Single 5 us laser pulse at point (X, Y)."""
        if self.simulate:
            res = self.sim.pulse(x, y)
        else:
            res = self.native.dll.rtPulse(ctypes.c_double(x), ctypes.c_double(y))
        if res != ERR_OK:
            raise RhothorError(res, f"rtPulse({x}, {y})")

    def burst(self, time_us: int):
        """Laser burst on stationary spot for time_us microseconds."""
        if self.simulate:
            res = self.sim.burst(time_us)
        else:
            res = self.native.dll.rtBurst(ctypes.c_long(time_us))
        if res != ERR_OK:
            raise RhothorError(res, f"rtBurst({time_us})")

    def sleep(self, time_us: int):
        """Dwells for time_us microseconds with laser OFF."""
        if self.simulate:
            res = self.sim.sleep(time_us)
        else:
            res = self.native.dll.rtSleep(ctypes.c_long(time_us))
        if res != ERR_OK:
            raise RhothorError(res, f"rtSleep({time_us})")

    # --- 3D Motion Commands (ELEVAthor Z-Axis) ---

    def jump_to_3d(self, x: float, y: float, z: float):
        """Fast 3D jump positioning with laser OFF."""
        if self.simulate:
            res = self.sim.jump_to_3d(x, y, z)
        else:
            res = self.native.dll.rtJumpTo3D(ctypes.c_double(x), ctypes.c_double(y), ctypes.c_double(z))
        if res != ERR_OK:
            raise RhothorError(res, f"rtJumpTo3D({x}, {y}, {z})")

    def line_to_3d(self, x: float, y: float, z: float):
        """3D linear vector mark with laser ON."""
        if self.simulate:
            res = self.sim.line_to_3d(x, y, z)
        else:
            res = self.native.dll.rtLineTo3D(ctypes.c_double(x), ctypes.c_double(y), ctypes.c_double(z))
        if res != ERR_OK:
            raise RhothorError(res, f"rtLineTo3D({x}, {y}, {z})")

    def move_to_3d(self, x: float, y: float, z: float):
        """3D move at mark speed with laser OFF."""
        if self.simulate:
            res = self.sim.move_to_3d(x, y, z)
        else:
            res = self.native.dll.rtMoveTo3D(ctypes.c_double(x), ctypes.c_double(y), ctypes.c_double(z))
        if res != ERR_OK:
            raise RhothorError(res, f"rtMoveTo3D({x}, {y}, {z})")

    # --- High-Level Geometry Helpers ---

    def mark_rectangle(self, cx: float, cy: float, w: float, h: float):
        """Marks a rectangular outline centered at (cx, cy)."""
        hw, hh = w / 2.0, h / 2.0
        self.jump_to(cx - hw, cy - hh)
        self.line_to(cx + hw, cy - hh)
        self.line_to(cx + hw, cy + hh)
        self.line_to(cx - hw, cy + hh)
        self.line_to(cx - hw, cy - hh)

    def mark_circle(self, cx: float, cy: float, r: float):
        """Marks a circle using two semicircle arcs (BF=1)."""
        self.jump_to(cx - r, cy)
        # Semicircle 1: from (-r, 0) to (+r, 0)
        self.arc_to(cx + r, cy, 1.0)
        # Semicircle 2: from (+r, 0) back to (-r, 0)
        self.arc_to(cx - r, cy, 1.0)

    def mark_crosshair(self, cx: float, cy: float, size: float):
        """Marks a calibration crosshair."""
        hs = size / 2.0
        self.jump_to(cx - hs, cy)
        self.line_to(cx + hs, cy)
        self.jump_to(cx, cy - hs)
        self.line_to(cx, cy + hs)

    def export_simulation_svg(self, filename: str) -> Optional[str]:
        """If running in simulation mode, exports the toolpath to SVG."""
        if self.simulate and self.sim:
            return self.sim.export_svg(filename)
        return None

    def get_simulation_stats(self) -> Optional[Dict[str, Any]]:
        """If running in simulation mode, returns kinematic metrics."""
        if self.simulate and self.sim:
            return self.sim.get_kinematic_stats()
        return None
