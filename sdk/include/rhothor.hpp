#pragma once

/**
 * Modern C++ RAII Wrapper for Newson Rhothor 2D & 3D Deflection Systems
 * Compatible with MSVC, MinGW, and Clang on Windows.
 */

#include <string>
#include <vector>
#include <stdexcept>
#include <iostream>
#include "rhothorDll.h"

namespace rhothor {

class RhothorException : public std::runtime_error {
public:
    explicit RhothorException(long errorCode, const std::string& context = "")
        : std::runtime_error(formatError(errorCode, context)), m_code(errorCode) {}

    long code() const noexcept { return m_code; }

private:
    long m_code;

    static std::string formatError(long code, const std::string& context) {
        std::string msg = "Rhothor Error [" + std::to_string(code) + "]";
        if (!context.empty()) {
            msg += " in " + context;
        }
        switch (code) {
            case ERR_BUSY:           msg += ": System busy"; break;
            case ERR_JOB:            msg += ": Error processing job / empty job"; break;
            case ERR_HARDWARE:       msg += ": Hardware not responding"; break;
            case ERR_MEMORY:         msg += ": Cannot allocate memory"; break;
            case ERR_DATA:           msg += ": Invalid data / parameter"; break;
            case ERR_IMPLEMENTATION: msg += ": Function not supported"; break;
            case ERR_LASER_LOW:      msg += ": Too little laser power"; break;
            case ERR_TRACKING:       msg += ": System is not running"; break;
            case ERR_LASER_HIGH:     msg += ": Too much laser power"; break;
            default:                 msg += ": Unknown error"; break;
        }
        return msg;
    }
};

class Controller {
public:
    Controller() : m_connected(false), m_serial(0) {}

    ~Controller() {
        if (m_connected) {
            try {
                abort();
                setLaser(false);
            } catch (...) {}
        }
    }

    void connect(long serial = 0) {
        long res = rtInitialise();
        if (res != ERR_OK) {
            throw RhothorException(res, "rtInitialise");
        }
        if (serial > 0) {
            res = rtSelectDevice(serial);
            if (res != ERR_OK) {
                throw RhothorException(res, "rtSelectDevice");
            }
            m_serial = serial;
        } else {
            long s = 0;
            if (rtGetSerial(&s) == ERR_OK) {
                m_serial = s;
            }
        }
        m_connected = true;
    }

    void disconnect() {
        if (m_connected) {
            abort();
            setLaser(false);
            m_connected = false;
        }
    }

    void abort() {
        long res = rtAbort();
        if (res != ERR_OK) throw RhothorException(res, "rtAbort");
    }

    // --- Query Functions ---
    long serial() const { return m_serial; }

    double fieldSize() const {
        double sz = 0.0;
        long res = rtGetFieldSize(&sz);
        if (res != ERR_OK) throw RhothorException(res, "rtGetFieldSize");
        return sz;
    }

    double maxSpeed() const {
        double sp = 0.0;
        long res = rtGetMaxSpeed(&sp);
        if (res != ERR_OK) throw RhothorException(res, "rtGetMaxSpeed");
        return sp;
    }

    long scannerDelay() const {
        long d = 0;
        long res = rtGetScannerDelay(&d);
        if (res != ERR_OK) throw RhothorException(res, "rtGetScannerDelay");
        return d;
    }

    std::string version() const {
        char buf[256] = {0};
        long res = rtGetVersion(buf, sizeof(buf));
        if (res != ERR_OK) throw RhothorException(res, "rtGetVersion");
        return std::string(buf);
    }

    // --- Control Functions ---
    void setSpeed(double markSpeed, double jumpSpeed = -1.0) {
        long res = rtSetSpeed(markSpeed);
        if (res != ERR_OK) throw RhothorException(res, "rtSetSpeed");
        if (jumpSpeed > 0.0) {
            res = rtSetJumpSpeed(jumpSpeed);
            if (res != ERR_OK) throw RhothorException(res, "rtSetJumpSpeed");
        }
    }

    void setLaserTimes(long onDelayUs, long offDelayUs) {
        long res = rtSetLaserTimes(onDelayUs, offDelayUs);
        if (res != ERR_OK) throw RhothorException(res, "rtSetLaserTimes");
    }

    void setLaser(bool onOff) {
        long res = rtSetLaser(onOff);
        if (res != ERR_OK) throw RhothorException(res, "rtSetLaser");
    }

    void listOpen(long mode = LIST_START) {
        long res = rtListOpen(mode);
        if (res != ERR_OK) throw RhothorException(res, "rtListOpen");
    }

    void listClose() {
        long res = rtListClose();
        if (res != ERR_OK) throw RhothorException(res, "rtListClose");
    }

    void jumpTo(double x, double y) {
        long res = rtJumpTo(x, y);
        if (res != ERR_OK) throw RhothorException(res, "rtJumpTo");
    }

    void lineTo(double x, double y) {
        long res = rtLineTo(x, y);
        if (res != ERR_OK) throw RhothorException(res, "rtLineTo");
    }

    void arcTo(double x, double y, double bf) {
        long res = rtArcTo(x, y, bf);
        if (res != ERR_OK) throw RhothorException(res, "rtArcTo");
    }

    void moveTo(double x, double y) {
        long res = rtMoveTo(x, y);
        if (res != ERR_OK) throw RhothorException(res, "rtMoveTo");
    }

    void pulse(double x, double y) {
        long res = rtPulse(x, y);
        if (res != ERR_OK) throw RhothorException(res, "rtPulse");
    }

    // 3D Extensions (ELEVAthor Z)
    void jumpTo3D(double x, double y, double z) {
        long res = rtJumpTo3D(x, y, z);
        if (res != ERR_OK) throw RhothorException(res, "rtJumpTo3D");
    }

    void lineTo3D(double x, double y, double z) {
        long res = rtLineTo3D(x, y, z);
        if (res != ERR_OK) throw RhothorException(res, "rtLineTo3D");
    }

    void moveTo3D(double x, double y, double z) {
        long res = rtMoveTo3D(x, y, z);
        if (res != ERR_OK) throw RhothorException(res, "rtMoveTo3D");
    }

private:
    bool m_connected;
    long m_serial;
};

} // namespace rhothor
