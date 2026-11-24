"""
Background QThread workers for non-blocking laser marking and contour framing.
Ensures the Qt interface remains perfectly responsive during long laser jobs.
"""

import time
from typing import List, Dict, Any, Optional
from PyQt5.QtCore import QThread, pyqtSignal

from sdk.python.rhothor import RhothorController


class MarkWorker(QThread):
    """Executes vector toolpaths on the Rhothor controller."""
    progress = pyqtSignal(int, int)       # (current_pass, total_passes)
    status_msg = pyqtSignal(str)          # Status message
    finished = pyqtSignal(dict)           # Emits job execution metrics
    error = pyqtSignal(str)               # Error message

    def __init__(
        self,
        controller: RhothorController,
        strokes: List[List[Dict[str, Any]]],
        passes: int = 1,
        mark_speed: float = 2000.0,
        jump_speed: float = 6000.0,
        offset_x: float = 0.0,
        offset_y: float = 0.0,
        scale: float = 1.0,
        parent=None
    ):
        super().__init__(parent)
        self.controller = controller
        self.strokes = strokes
        self.passes = max(1, passes)
        self.mark_speed = mark_speed
        self.jump_speed = jump_speed
        self.offset_x = offset_x
        self.offset_y = offset_y
        self.scale = scale
        self._is_aborted = False

    def abort(self):
        self._is_aborted = True
        try:
            self.controller.abort()
        except Exception:
            pass

    def run(self):
        try:
            self.status_msg.emit("Initializing laser speeds and offsets...")
            self.controller.set_speed(self.mark_speed, self.jump_speed)
            if self.offset_x != 0.0 or self.offset_y != 0.0:
                self.controller.set_offsets(self.offset_x, self.offset_y)

            start_time = time.time()

            for p in range(1, self.passes + 1):
                if self._is_aborted:
                    self.status_msg.emit("Execution aborted by operator.")
                    return

                self.progress.emit(p, self.passes)
                self.status_msg.emit(f"Executing Pass {p}/{self.passes}...")

                self.controller.list_open(1)
                self.controller.set_speed(self.mark_speed, self.jump_speed)
                if self.offset_x != 0.0 or self.offset_y != 0.0:
                    self.controller.set_offsets(self.offset_x, self.offset_y)

                for stroke in self.strokes:
                    if self._is_aborted:
                        break

                    for pt in stroke:
                        pt_type = pt.get("type", "LINE")
                        px = pt.get("x", 0.0) * self.scale
                        py = pt.get("y", 0.0) * self.scale
                        pz = pt.get("z", 0.0)

                        if pt_type == "JUMP":
                            if "z" in pt and abs(pz) > 0.001:
                                self.controller.jump_to_3d(px, py, pz)
                            else:
                                self.controller.jump_to(px, py)
                        elif pt_type == "ARC":
                            bf = pt.get("bf", 1.0)
                            self.controller.arc_to(px, py, bf)
                        elif pt_type == "BURST":
                            time_us = int(pt.get("time_us", 100))
                            self.controller.burst(time_us)
                        elif pt_type == "PULSE":
                            self.controller.pulse(px, py)
                        else:  # LINE
                            if "z" in pt and abs(pz) > 0.001:
                                self.controller.line_to_3d(px, py, pz)
                            else:
                                self.controller.line_to(px, py)

                self.controller.list_close()

            elapsed = time.time() - start_time
            stats = {}
            if self.controller.simulate:
                stats = self.controller.get_simulation_stats()
            stats["actual_elapsed_seconds"] = round(elapsed, 3)

            self.status_msg.emit(f"Marking complete! ({self.passes} passes in {elapsed:.2f}s)")
            self.finished.emit(stats)

        except Exception as e:
            self.error.emit(str(e))


class FrameWorker(QThread):
    """
    Traces bounding box outline with laser off for physical workpiece alignment.
    Runs continuously or for a specified number of repetitions.
    """
    status_msg = pyqtSignal(str)
    finished = pyqtSignal()
    error = pyqtSignal(str)

    def __init__(
        self,
        controller: RhothorController,
        bbox: Dict[str, float],
        scale: float = 1.0,
        offset_x: float = 0.0,
        offset_y: float = 0.0,
        speed: float = 2500.0,
        continuous: bool = False,
        parent=None
    ):
        super().__init__(parent)
        self.controller = controller
        self.bbox = bbox
        self.scale = scale
        self.offset_x = offset_x
        self.offset_y = offset_y
        self.speed = speed
        self.continuous = continuous
        self._running = True

    def stop(self):
        self._running = False
        try:
            self.controller.abort()
        except Exception:
            pass

    def run(self):
        try:
            min_x = self.bbox["min_x"] * self.scale + self.offset_x
            max_x = self.bbox["max_x"] * self.scale + self.offset_x
            min_y = self.bbox["min_y"] * self.scale + self.offset_y
            max_y = self.bbox["max_y"] * self.scale + self.offset_y

            self.status_msg.emit("Tracing workpiece boundary frame (Laser OFF)...")

            while self._running:
                self.controller.list_open(1)
                self.controller.set_speed(mark_speed=self.speed, jump_speed=self.speed)
                self.controller.jump_to(min_x, min_y)
                self.controller.move_to(max_x, min_y)
                self.controller.move_to(max_x, max_y)
                self.controller.move_to(min_x, max_y)
                self.controller.move_to(min_x, min_y)
                self.controller.list_close()

                if not self.continuous:
                    break
                time.sleep(0.05)

            self.finished.emit()

        except Exception as e:
            self.error.emit(str(e))
