using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RhothorSDK
{
    public enum RhothorError : int
    {
        Ok = -1,
        Busy = 2,
        Job = 3,
        Hardware = 5,
        Memory = 15,
        Data = 13,
        Implementation = 23,
        LaserLow = 42,
        Tracking = 43,
        LaserHigh = 46
    }

    public enum ListMode : int
    {
        ListStart = 1,
        AutoStart = 2,
        AutoRepeat = 3
    }

    /// <summary>
    /// Low-level P/Invoke wrapper for Newson Rhothor C API (rhothor.dll).
    /// </summary>
    public static class RhothorNative
    {
        private const string DllName = "rhothor.dll";

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtGetFreeDevList(out int serialNrs, out int size);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtInitialise();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtSelectDevice(int serialNr);

        // --- Query Functions ---
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtGetChannelData(int nr, out int setpoint, out int actual, out double iEff);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtGetFieldSize(out double size);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtGetIO(out int value);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtGetMaxSpeed(out double speed);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtGetResolvers(out double x, out double y);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtGetScannerDelay(out int delay);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtGetSerial(out int serial);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtGetSetpointFilter(out int timeConst);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtGetStatus(out int memory);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int rtGetVersion(StringBuilder version, int maxSize);

        // --- Alignment & Calibration ---
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int rtLoadCalibrationFile(string fileName);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtResetCalibration();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int rtStoreCalibrationFile(string fileName);

        // --- Control Functions ---
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtAbort();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtArcTo(double x, double y, double bf);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtBurst(int time);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtJumpTo(double x, double y);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtLineTo(double x, double y);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtListClose();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtListOpen(int mode);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtMoveTo(double x, double y);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtPulse(double x, double y);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtSetIO(int value, int mask);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtSetLaser(bool onOff);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtSetLaserTimes(int gateOnDelay, int gateOffDelay);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtSetMatrix(double a11, double a12, double a21, double a22);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtSetMaxSpeed(double speed);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtSetOffsXY(double x, double y);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtSetOffsZ(double z);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtSetOscillator(int nr, double period, double pulseWidth);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtSetSpeed(double speed);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtSetJumpSpeed(double speed);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtSleep(int time);

        // --- 3D Motion (ELEVAthor Z) ---
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtMoveTo3D(double x, double y, double z);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtLineTo3D(double x, double y, double z);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtJumpTo3D(double x, double y, double z);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int rtPulse3D(double x, double y, double z);
    }
}
