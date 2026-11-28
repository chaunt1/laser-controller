"""
Python SDK for Newson Rhothor 2D & 3D Galvo Scanning Systems.
Wraps 'rhothor.dll' C-API via ctypes with seamless cross-platform fallback to
RhothorSimulator for development, testing, and vector trajectory verification.
"""

import os
import sys
import time
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
    Defaults strictly to v7.3/v7.5 legacy DLL for LPKF hardware compatibility.
    """
    def __init__(self, dll_path: Optional[str] = None):
        self.dll = None
        self.is_loaded = False
        self.loaded_path = None
        self.version_type = "v7.3/v7.5 (LPKF Legacy)"
        
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

            # Strictly prioritize genuine v7.3/v7.5 legacy DLLs
            if app_dir:
                candidates.append(os.path.join(app_dir, "sdk", "lib", arch_dir, "rhothor.dll"))
                candidates.append(os.path.join(app_dir, "rhothor.dll"))
            if meipass:
                candidates.append(os.path.join(meipass, "sdk", "lib", arch_dir, "rhothor.dll"))
                candidates.append(os.path.join(meipass, "rhothor.dll"))

            candidates.append(os.path.join(base_dir, "..", "lib", arch_dir, "rhothor.dll"))
            candidates.append(os.path.join(base_dir, "rhothor.dll"))
            candidates.append("rhothor.dll")
            
            v7_dll = None
            v7_path = None
            fallback_dll = None
            fallback_path = None

            for path in candidates:
                if os.path.exists(path):
                    try:
                        temp_dll = ctypes.WinDLL(path)
                        # Check version characteristics:
                        # v11 exports rtGetTarget, while v7.3 / v7.5 does not.
                        # We default strictly to v7.3 / v7.5 for LPKF / Rhothor DSP v7.x hardware.
                        is_v11 = hasattr(temp_dll, "rtGetTarget")
                        if not is_v11:
                            v7_dll = temp_dll
                            v7_path = path
                            break  # Found genuine v7.3/v7.5 DLL
                        elif fallback_dll is None:
                            fallback_dll = temp_dll
                            fallback_path = path
                    except Exception as e:
                        print(f"Warning: Failed to load {path}: {e}")
            
            if v7_dll is not None:
                self.dll = v7_dll
                self.loaded_path = v7_path
                self.is_loaded = True
                self.version_type = "v7.3/v7.5 (LPKF Legacy)"
            elif fallback_dll is not None:
                self.dll = fallback_dll
                self.loaded_path = fallback_path
                self.is_loaded = True
                self.version_type = "v11 (Newson Official)"

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
        self.is_list_open = False
        self.sim: Optional[RhothorSimulator] = None

        # Operational hardware speed limit (queried on connect, default safe 2000.0)
        self.max_speed: float = 2000.0

        # Cached list control settings to be deferred until list_open()
        # Default speeds safely clamped within LPKF hardware limit (2000 mm/s)
        self.current_mark_speed: float = 1000.0
        self.current_jump_speed: float = 1500.0
        self.current_laser_delays: Optional[Tuple[int, int]] = None
        self.current_matrix: Optional[Tuple[float, float, float, float]] = None
        self.current_offsets: Tuple[float, float, float] = (0.0, 0.0, 0.0)
        # Default Oscillator 3 for Fiber Laser Sync/PRR on IO7 (50 kHz, 20 µs period, 5 µs pulse width)
        self.current_oscillators: Dict[int, Tuple[float, float]] = {3: (20.0, 5.0)}
        
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
            if self.serial_nr:
                self.sim.select_device(self.serial_nr)
            res = self.sim.initialise()
            try:
                self.max_speed = float(self.get_max_speed())
            except Exception:
                pass
            self.is_connected = True
            return

        # Note: According to the Newson Rhothor Manual (section 5.3):
        # rtSelectDevice MUST be called PRIOR to rtInitialise when addressing a specific device.
        # Calling rtSelectDevice after rtInitialise resets the internal DLL state machine to -1,
        # which causes rtListOpen to immediately return ERR_DATA (13).
        if self.serial_nr is not None and self.serial_nr > 0:
            try:
                res = self.native.dll.rtSelectDevice(ctypes.c_long(self.serial_nr))
            except Exception as e:
                print(f"Notice: rtSelectDevice({self.serial_nr}) call: {e}")

        res = self.native.dll.rtInitialise()
        if res != ERR_OK:
            raise RhothorError(res, "rtInitialise")

        # Allow DSP and USB subsystem to settle
        time.sleep(0.05)

        # Clear any pending list commands or aborted state in DSP RAM
        try:
            self.native.dll.rtAbort()
        except Exception:
            pass

        # Query actual connected hardware serial number and kinematics limit
        try:
            actual_serial = self.get_serial()
            if actual_serial > 0:
                self.serial_nr = actual_serial
        except Exception:
            pass

        try:
            hw_max_speed = self.get_max_speed()
            if hw_max_speed > 0:
                self.max_speed = float(hw_max_speed)
                safe_limit = self.max_speed - 1.0 if self.max_speed > 10.0 else self.max_speed
                if self.current_mark_speed > safe_limit:
                    self.current_mark_speed = safe_limit
                if self.current_jump_speed > safe_limit:
                    self.current_jump_speed = safe_limit
        except Exception:
            pass

        self.is_connected = True
        self.is_list_open = False

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
        try:
            if self.simulate:
                self.sim.abort()
            else:
                self.native.dll.rtAbort()
        finally:
            self.is_list_open = False

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
        """
        Sets marking speed and optional fast jump speed in mm/sec.
        If a command list is open, transmits immediately to DSP.
        If no list is open, caches parameters to be automatically flushed on list_open().
        Automatically clamps speeds to the scanner hardware limit (max_speed) to prevent ERR_DATA [13].
        """
        if mark_speed <= 0:
            raise RhothorError(ERR_DATA, f"Invalid mark_speed: {mark_speed}")
        if jump_speed is not None and jump_speed <= 0:
            raise RhothorError(ERR_DATA, f"Invalid jump_speed: {jump_speed}")

        # Protect against exceeding hardware max_speed (which DLL rejects with ERR_DATA 13)
        limit = getattr(self, "max_speed", 2000.0)
        if limit and limit > 0:
            safe_limit = limit - 1.0 if limit > 10.0 else limit
            if mark_speed > safe_limit:
                mark_speed = safe_limit
            if jump_speed is not None and jump_speed > safe_limit:
                jump_speed = safe_limit

        self.current_mark_speed = float(mark_speed)
        if jump_speed is not None:
            self.current_jump_speed = float(jump_speed)

        if self.simulate:
            res = self.sim.set_speed(mark_speed)
            if res != ERR_OK:
                raise RhothorError(res, "rtSetSpeed")
            if jump_speed:
                res2 = self.sim.set_jump_speed(jump_speed)
                if res2 != ERR_OK:
                    raise RhothorError(res2, "rtSetJumpSpeed")
            return

        # If a command list is open on hardware DSP, apply immediately.
        # Otherwise, defer sending until list_open() to avoid ERR_JOB (3).
        if self.is_list_open:
            res = self.native.dll.rtSetSpeed(ctypes.c_double(self.current_mark_speed))
            if res == ERR_DATA:
                try:
                    hw_limit = self.get_max_speed()
                    self.max_speed = hw_limit
                    safe_limit = hw_limit - 1.0 if hw_limit > 10.0 else hw_limit
                    self.current_mark_speed = min(self.current_mark_speed, safe_limit)
                    res = self.native.dll.rtSetSpeed(ctypes.c_double(self.current_mark_speed))
                except Exception:
                    pass
            if res != ERR_OK:
                raise RhothorError(res, "rtSetSpeed")

            if self.current_jump_speed is not None:
                res = self.native.dll.rtSetJumpSpeed(ctypes.c_double(self.current_jump_speed))
                if res == ERR_DATA:
                    try:
                        hw_limit = self.get_max_speed()
                        self.max_speed = hw_limit
                        safe_limit = hw_limit - 1.0 if hw_limit > 10.0 else hw_limit
                        self.current_jump_speed = min(self.current_jump_speed, safe_limit)
                        res = self.native.dll.rtSetJumpSpeed(ctypes.c_double(self.current_jump_speed))
                    except Exception:
                        pass
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
        self.current_laser_delays = (gate_on_delay_us, gate_off_delay_us)
        if self.simulate:
            self.sim.set_laser_times(gate_on_delay_us, gate_off_delay_us)
            return

        if self.is_list_open:
            res = self.native.dll.rtSetLaserTimes(ctypes.c_long(gate_on_delay_us),
                                                  ctypes.c_long(gate_off_delay_us))
            if res != ERR_OK:
                raise RhothorError(res, "rtSetLaserTimes")

    def set_laser(self, on_off: bool):
        """Manually forces the laser state on or off."""
        if self.simulate:
            self.sim.set_laser(on_off)
            return
        if self.is_list_open:
            res = self.native.dll.rtSetLaser(ctypes.c_bool(on_off))
            if res != ERR_OK:
                raise RhothorError(res, "rtSetLaser")
        else:
            # If no list is open, attempt momentary list execution safely
            try:
                if self.native.dll.rtListOpen(ctypes.c_long(1)) == ERR_OK:
                    self.native.dll.rtSetLaser(ctypes.c_bool(on_off))
                    self.native.dll.rtListClose()
            except Exception:
                pass

    def set_matrix(self, a11: float, a12: float, a21: float, a22: float):
        """Sets 2x2 coordinate transformation matrix (scaling, rotation, shear)."""
        self.current_matrix = (a11, a12, a21, a22)
        if self.simulate:
            self.sim.set_matrix(a11, a12, a21, a22)
            return
        if self.is_list_open:
            res = self.native.dll.rtSetMatrix(ctypes.c_double(a11), ctypes.c_double(a12),
                                              ctypes.c_double(a21), ctypes.c_double(a22))
            if res != ERR_OK:
                raise RhothorError(res, "rtSetMatrix")

    def set_offsets(self, x: float, y: float, z: float = 0.0):
        """Sets working coordinate offsets."""
        self.current_offsets = (x, y, z)
        if self.simulate:
            self.sim.set_offsets(x, y)
            self.sim.set_offset_z(z)
            return
        if self.is_list_open:
            res = self.native.dll.rtSetOffsXY(ctypes.c_double(x), ctypes.c_double(y))
            if res != ERR_OK:
                raise RhothorError(res, "rtSetOffsXY")
            if z != 0.0:
                res_z = self.native.dll.rtSetOffsZ(ctypes.c_double(z))
                if res_z != ERR_OK:
                    raise RhothorError(res_z, "rtSetOffsZ")

    def set_oscillator(self, nr: int, period_us: float, pulse_width_us: float):
        """Configures one of the 3 internal hardware oscillators for laser pulses."""
        self.current_oscillators[nr] = (period_us, pulse_width_us)
        if self.simulate:
            self.sim.oscillators[nr] = {"period": period_us, "width": pulse_width_us}
            return
        if self.is_list_open:
            res = self.native.dll.rtSetOscillator(ctypes.c_long(nr),
                                                  ctypes.c_double(period_us),
                                                  ctypes.c_double(pulse_width_us))
            if res != ERR_OK:
                raise RhothorError(res, f"rtSetOscillator({nr})")

    # --- Digital I/O & Laser Latch Controls ---

    def set_io(self, value: int, mask: int = 0x7F):
        """Sets digital output pins using bit mask."""
        if self.simulate:
            if hasattr(self.sim, "set_io"):
                self.sim.set_io(value, mask)
            return
        res = self.native.dll.rtSetIO(ctypes.c_long(value), ctypes.c_long(mask))
        if res != ERR_OK:
            # If rejected because no list buffer is open, execute inside a quick list transaction
            if not getattr(self, "_in_list", False):
                try:
                    self.list_open(1)
                    res2 = self.native.dll.rtSetIO(ctypes.c_long(value), ctypes.c_long(mask))
                    self.list_close()
                    if res2 == ERR_OK:
                        return
                except Exception:
                    pass
            raise RhothorError(res, f"rtSetIO({value}, {mask})")

    def set_io_port(self, port_nr: int, state: bool):
        """
        Sets the output state of a single digital I/O port (IO1 to IO7).
        port_nr: 1 to 7 (corresponds to bit 0 to bit 6 on X7 connector:
          1: IO1 (Pins 1/14)
          2: IO2 (Pins 2/15)
          3: IO3 (Pins 3/16)
          4: IO4 (Pins 4/17)
          5: IO5 (Pins 5/18)
          6: IO6 (Pins 6/19)
          7: IO7 (Pins 7/20)
        state: True for HIGH, False for LOW.
        """
        if not (1 <= port_nr <= 7):
            raise ValueError(f"port_nr must be between 1 and 7, got {port_nr}")
        bit = port_nr - 1
        mask = 1 << bit
        val = mask if state else 0
        self.set_io(value=val, mask=mask)

    def set_all_io_ports(self, state: bool):
        """Sets all 7 digital I/O ports (IO1 to IO7) to HIGH or LOW."""
        mask = 0x7F  # 7 bits for IO1..IO7
        val = 0x7F if state else 0
        self.set_io(value=val, mask=mask)

    def get_io_ports(self) -> dict:
        """
        Returns the states of all 7 digital I/O ports as a dict: {1: bool, ..., 7: bool}.
        """
        val = self.get_io()
        return {port: bool(val & (1 << (port - 1))) for port in range(1, 8)}

    def get_io(self) -> int:
        """Reads 8-bit digital input pins."""
        if self.simulate:
            if hasattr(self.sim, "get_io"):
                _, val = self.sim.get_io()
                return val
            return 0
        val = ctypes.c_long()
        res = self.native.dll.rtGetIO(ctypes.byref(val))
        if res != ERR_OK:
            raise RhothorError(res, "rtGetIO")
        return val.value

    def wait_io(self, value: int, mask: int):
        """Waits until digital input pins match specified pattern."""
        if self.simulate:
            return
        res = self.native.dll.rtWaitIO(ctypes.c_long(value), ctypes.c_long(mask))
        if res != ERR_OK:
            raise RhothorError(res, f"rtWaitIO({value}, {mask})")

    def send_latch_pulse(self, pin_bit: int = 4):
        """
        Sends a latch pulse on the designated digital I/O pin (default bit 4 / IO5 / Pins 5/18 / Latch pin).
        Pulses HIGH -> brief delay -> LOW (or LOW -> HIGH -> LOW) to latch power setting on Fiber Laser.
        """
        import time
        mask = 1 << pin_bit
        # Drive HIGH
        try:
            self.set_io(value=mask, mask=mask)
            time.sleep(0.005)  # 5 ms pulse width (standard fiber laser requires >= 10 us)
            # Drive LOW
            self.set_io(value=0, mask=mask)
            time.sleep(0.001)
        except Exception:
            pass

    # --- List Management ---

    def list_open(self, mode: int = LIST_START):
        """
        Opens a command list (LIST_START=1, AUTO_START=2, AUTO_REPEAT=3).
        Automatically flushes configured speeds, delays, offsets, matrix, and oscillators.
        Includes self-healing recovery if DSP state machine was desynced (ERR_DATA 13).
        """
        if self.simulate:
            res = self.sim.list_open(mode)
        else:
            res = self.native.dll.rtListOpen(ctypes.c_long(mode))
            if res == ERR_DATA:
                # ERR_DATA (13) indicates the internal DLL state machine is uninitialized (-1)
                # or DSP list buffer requires a reset. Attempt automatic re-synchronization:
                try:
                    self.native.dll.rtAbort()
                    time.sleep(0.02)
                    res = self.native.dll.rtListOpen(ctypes.c_long(mode))
                except Exception:
                    pass

                if res == ERR_DATA:
                    # Full session re-initialization
                    try:
                        if self.serial_nr is not None and self.serial_nr > 0:
                            self.native.dll.rtSelectDevice(ctypes.c_long(self.serial_nr))
                        self.native.dll.rtInitialise()
                        time.sleep(0.05)
                        self.native.dll.rtAbort()
                        res = self.native.dll.rtListOpen(ctypes.c_long(mode))
                    except Exception:
                        pass

        if res != ERR_OK:
            raise RhothorError(res, f"rtListOpen({mode})")

        self.is_list_open = True

        # Flush active configuration parameters into the open DSP list
        if not self.simulate:
            limit = getattr(self, "max_speed", 2000.0)
            safe_limit = limit - 1.0 if limit > 10.0 else limit
            if self.current_mark_speed is not None:
                spd = min(self.current_mark_speed, safe_limit) if limit > 0 else self.current_mark_speed
                res = self.native.dll.rtSetSpeed(ctypes.c_double(spd))
                if res == ERR_DATA:
                    try:
                        hw_limit = self.get_max_speed()
                        self.max_speed = hw_limit
                        safe_limit = hw_limit - 1.0 if hw_limit > 10.0 else hw_limit
                        spd = min(spd, safe_limit)
                        res = self.native.dll.rtSetSpeed(ctypes.c_double(spd))
                    except Exception:
                        pass
                if res != ERR_OK:
                    raise RhothorError(res, "rtSetSpeed")

            if self.current_jump_speed is not None:
                jspd = min(self.current_jump_speed, safe_limit) if limit > 0 else self.current_jump_speed
                res = self.native.dll.rtSetJumpSpeed(ctypes.c_double(jspd))
                if res == ERR_DATA:
                    try:
                        hw_limit = self.get_max_speed()
                        self.max_speed = hw_limit
                        safe_limit = hw_limit - 1.0 if hw_limit > 10.0 else hw_limit
                        jspd = min(jspd, safe_limit)
                        res = self.native.dll.rtSetJumpSpeed(ctypes.c_double(jspd))
                    except Exception:
                        pass
                if res != ERR_OK:
                    raise RhothorError(res, "rtSetJumpSpeed")
            if self.current_laser_delays is not None:
                on_d, off_d = self.current_laser_delays
                res = self.native.dll.rtSetLaserTimes(ctypes.c_long(on_d), ctypes.c_long(off_d))
                if res != ERR_OK:
                    raise RhothorError(res, "rtSetLaserTimes")
            if self.current_offsets != (0.0, 0.0, 0.0):
                ox, oy, oz = self.current_offsets
                res = self.native.dll.rtSetOffsXY(ctypes.c_double(ox), ctypes.c_double(oy))
                if res != ERR_OK:
                    raise RhothorError(res, "rtSetOffsXY")
                if oz != 0.0:
                    res_z = self.native.dll.rtSetOffsZ(ctypes.c_double(oz))
                    if res_z != ERR_OK:
                        raise RhothorError(res_z, "rtSetOffsZ")
            if self.current_matrix is not None:
                a11, a12, a21, a22 = self.current_matrix
                res = self.native.dll.rtSetMatrix(ctypes.c_double(a11), ctypes.c_double(a12),
                                                  ctypes.c_double(a21), ctypes.c_double(a22))
                if res != ERR_OK:
                    raise RhothorError(res, "rtSetMatrix")
            for osc_nr, (p, w) in self.current_oscillators.items():
                res = self.native.dll.rtSetOscillator(ctypes.c_long(osc_nr),
                                                      ctypes.c_double(p),
                                                      ctypes.c_double(w))
                if res != ERR_OK:
                    raise RhothorError(res, f"rtSetOscillator({osc_nr})")

            # Enable Fiber Laser IO outputs in the DSP list command stream:
            # Bit 4 (IO5 - Latch Pin 9), Bit 5 (IO6 - Gate Pin 19), Bit 6 (IO7 - Sync/PRR Pin 20)
            # 112 = 16 (IO5) + 32 (IO6) + 64 (IO7) = 0x70
            try:
                self.native.dll.rtSetIO(ctypes.c_long(112), ctypes.c_long(112))
            except Exception:
                pass

    def set_laser_frequency(self, frequency_khz: float, pulse_width_us: float = 3.0):
        """
        Configures the laser pulse repetition frequency (PRR) on IO7 (Oscillator 3) for Fiber Laser.
        frequency_khz: Frequency in kHz (e.g. 20.0 to 100.0 kHz, default 50.0 kHz).
        pulse_width_us: Pulse width in microseconds (default 3.0 µs).
        """
        if frequency_khz <= 0:
            raise ValueError("Laser frequency must be positive.")
        period_us = 1000.0 / frequency_khz
        self.set_oscillator(nr=3, period_us=period_us, pulse_width_us=pulse_width_us)

    def test_laser_emission(self, duration_s: float = 1.0, frequency_khz: float = 50.0, pulse_width_us: float = 3.0):
        """
        Fires a brief continuous emission test sequence according to the Fiber Laser pinout:
        1. Emits Latch pulse on IO5 (Pin 5/18 -> Laser Pin 9) to lock power setting.
        2. Configures Oscillator 3 on IO7 (Pin 7/20 -> Laser Pin 20 Sync/PRR).
        3. Drives IO6 (Pin 6/19 -> Laser Pin 19 Gate) and IO7 HIGH for duration_s.
        4. Turns all outputs OFF safely.
        """
        import time
        self.send_latch_pulse(pin_bit=4)
        time.sleep(0.01)
        self.set_laser_frequency(frequency_khz, pulse_width_us)
        # Drive IO6 (Gate) and IO7 (Sync) HIGH
        mask = (1 << 5) | (1 << 6)  # Bit 5 (IO6), Bit 6 (IO7)
        self.set_io(value=mask, mask=mask)
        time.sleep(duration_s)
        # Turn OFF
        self.set_io(value=0, mask=mask)

    def list_close(self):
        """Closes and dispatches the command list for execution."""
        try:
            if self.simulate:
                res = self.sim.list_close()
            else:
                res = self.native.dll.rtListClose()
            if res != ERR_OK:
                raise RhothorError(res, "rtListClose")
        finally:
            self.is_list_open = False

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
