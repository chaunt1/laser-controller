using System;
using System.Text;

namespace RhothorSDK
{
    public class RhothorException : Exception
    {
        public RhothorError ErrorCode { get; }
        public string FunctionName { get; }

        public RhothorException(RhothorError code, string functionName)
            : base($"Rhothor error in {functionName}: [{code}]")
        {
            ErrorCode = code;
            FunctionName = functionName;
        }
    }

    /// <summary>
    /// Safe, object-oriented .NET controller for Newson Rhothor 2D/3D galvo scanners.
    /// Implements IDisposable for automatic laser shutoff and resource management.
    /// </summary>
    public class RhothorController : IDisposable
    {
        private bool _disposed = false;
        private bool _connected = false;

        public int SerialNumber { get; private set; }
        public double FieldSizeMm { get; private set; }
        public double MaxSpeedMmS { get; private set; }
        public int ScannerDelayUs { get; private set; }

        public void Connect(int serialNr = 0)
        {
            int res = RhothorNative.rtInitialise();
            CheckError(res, nameof(RhothorNative.rtInitialise));

            if (serialNr > 0)
            {
                res = RhothorNative.rtSelectDevice(serialNr);
                CheckError(res, nameof(RhothorNative.rtSelectDevice));
                SerialNumber = serialNr;
            }
            else
            {
                if (RhothorNative.rtGetSerial(out int s) == (int)RhothorError.Ok)
                    SerialNumber = s;
            }

            if (RhothorNative.rtGetFieldSize(out double fs) == (int)RhothorError.Ok)
                FieldSizeMm = fs;

            if (RhothorNative.rtGetMaxSpeed(out double ms) == (int)RhothorError.Ok)
                MaxSpeedMmS = ms;

            if (RhothorNative.rtGetScannerDelay(out int sd) == (int)RhothorError.Ok)
                ScannerDelayUs = sd;

            _connected = true;
        }

        public void SetSpeed(double markSpeedMmS, double jumpSpeedMmS = -1)
        {
            int res = RhothorNative.rtSetSpeed(markSpeedMmS);
            CheckError(res, nameof(RhothorNative.rtSetSpeed));

            if (jumpSpeedMmS > 0)
            {
                res = RhothorNative.rtSetJumpSpeed(jumpSpeedMmS);
                CheckError(res, nameof(RhothorNative.rtSetJumpSpeed));
            }
        }

        public void SetLaserTimes(int onDelayUs, int offDelayUs)
        {
            int res = RhothorNative.rtSetLaserTimes(onDelayUs, offDelayUs);
            CheckError(res, nameof(RhothorNative.rtSetLaserTimes));
        }

        public void ListOpen(ListMode mode = ListMode.ListStart)
        {
            int res = RhothorNative.rtListOpen((int)mode);
            CheckError(res, nameof(RhothorNative.rtListOpen));
        }

        public void ListClose()
        {
            int res = RhothorNative.rtListClose();
            CheckError(res, nameof(RhothorNative.rtListClose));
        }

        public void JumpTo(double x, double y)
        {
            int res = RhothorNative.rtJumpTo(x, y);
            CheckError(res, nameof(RhothorNative.rtJumpTo));
        }

        public void LineTo(double x, double y)
        {
            int res = RhothorNative.rtLineTo(x, y);
            CheckError(res, nameof(RhothorNative.rtLineTo));
        }

        public void ArcTo(double x, double y, double bulgeFactor)
        {
            int res = RhothorNative.rtArcTo(x, y, bulgeFactor);
            CheckError(res, nameof(RhothorNative.rtArcTo));
        }

        public void JumpTo3D(double x, double y, double z)
        {
            int res = RhothorNative.rtJumpTo3D(x, y, z);
            CheckError(res, nameof(RhothorNative.rtJumpTo3D));
        }

        public void LineTo3D(double x, double y, double z)
        {
            int res = RhothorNative.rtLineTo3D(x, y, z);
            CheckError(res, nameof(RhothorNative.rtLineTo3D));
        }

        public void Abort()
        {
            RhothorNative.rtAbort();
        }

        private static void CheckError(int res, string func)
        {
            if (res != (int)RhothorError.Ok)
            {
                throw new RhothorException((RhothorError)res, func);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_connected)
                {
                    try
                    {
                        RhothorNative.rtAbort();
                        RhothorNative.rtSetLaser(false);
                    }
                    catch { }
                }
                _disposed = true;
            }
        }
    }
}
