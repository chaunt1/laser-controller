using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FlightLib;
using global::Flight.Properties;

namespace Flight
{
    public class BeaconTarget
    {
        // When software positions scanner 
        public virtual void ScannerJumpTo(double x, double y)
        { }
        // When software positions scanner 
        public virtual void ScannerLineTo(double x, double y)
        { }
        // When software positions table 
        public virtual void TableJumpTo(double x, double y, double z)
        {  }
        // When software positions table 
        public virtual void TableJumpToRel(double x, double y, double z)
        {  }
        // When user updates calibration table reference 
        public virtual void GetCurrentTablePosition(ref double x, ref double y, ref double z)
        { x = 0; y = 0; z = 0; }
        // When software queries calibration table reference 
        public virtual void GetTableCalibrationRef(ref double x, ref double y, ref double z)
        {  x = 0; y = 0; z = 0;  }
        // When user updates calibration table reference 
        public virtual void SetTableCalibrationRef(double x, double y, double z)
        { }
        // When calibration needs update
        public virtual void ProcessCalibrationData(int Columns, int Rows, double Width, double Height, PointF[] Data)
        { }
        // Init function
        public virtual void OnInitTarget()
        { }
        // Exit function
        public virtual void OnExitTarget()
        { }
        // When user loads calibration
        public virtual void OnLoadCalibrationFile()
        { }
        // When user stores calibration
        public virtual void OnStoreCalibrationFile()
        { }
        // When user resets calibration
        public virtual void OnResetCalibration()
        { }
        // When user resets calibration
        public virtual void OnSensorTestStart()
        { }
        // When user resets calibration
        public virtual void OnSensorTestStop()
        { }
        
        // property FieldSize - GetScannerFieldSize
        public virtual double FieldSize
        { get { return 100; } }
        // property TableInPosition - true when table is in position
        public virtual bool TableInPosition
        { get { return false; } }
    }

    public class FlightBeaconTarget : BeaconTarget
    {
        private rhothorLink _Link;
        private Flight _Master;
        private double _FieldSize;
        private bool _TableMove;
        private bool _Attached;

        public FlightBeaconTarget(Flight MasterDlg)
        {
            _Link = MasterDlg._Link;
            _Master = MasterDlg;
            _FieldSize = 100;
            _TableMove = false;
            _Attached = false;
        }

        public override double FieldSize
        { get { return _FieldSize; } }

        public override void ScannerJumpTo(double x, double y)
        {
            if (_Link.rtListOpen(1) == -1)
            {
                _Link.rtSetJumpSpeed(10 * _FieldSize);
                _Link.rtJumpTo(x, y);
                _Link.rtSleep(200);
                _Link.rtListClose();
                //while (_Link.rtGetStatus(ref mem) == 2) ;
            }
        }

        public override void ScannerLineTo(double x, double y)
        {
            if (_Link.rtListOpen(1) == -1)
            {
                _Link.rtSetSpeed(10 * _FieldSize);
                _Link.rtLineTo(x, y);
                _Link.rtListClose();
                //while (_Link.rtGetStatus(ref mem) == 2) ;
            }
        }

        public override void TableJumpTo(double x, double y, double z)
        {
            // table movement performed by master - re-attached when in position
            Detach();
            _TableMove = true;
            _Master.OnBeaconTargetTableJumpTo(x, y, z);
        }

        public override void TableJumpToRel(double dx, double dy, double dz)
        {
            // table movement performed by master - re-attached when in position
            Detach();
            _TableMove = true;
            _Master.OnBeaconTargetTableJog(dx, dy, dz);
        }

        public override void GetTableCalibrationRef(ref double x, ref double y, ref double z)
        { 
            x = Settings.Default.CalibrationPositionX;
            y = Settings.Default.CalibrationPositionY;
            z = Settings.Default.CalibrationPositionZ; 
        }

        public override void SetTableCalibrationRef(double x, double y, double z)
        {
            Settings.Default.CalibrationPositionX = x;
            Settings.Default.CalibrationPositionY = y;
            Settings.Default.CalibrationPositionZ = z;
        }
 
        public override void GetCurrentTablePosition(ref double x, ref double y, ref double z)
        {
            Detach();
            _Master.UpdateTablePositions();
            Attach();
            x = _Master._TableX;
            y = _Master._TableY;
            z = _Master._TableZ;
        }
        
        public override bool TableInPosition
        {
            get
            {
                if (_Master._TableCommand)
                    return false;
                if (_TableMove)
                {
                    _TableMove = false;
                    Attach();
                }
                return true;
            }
            // polling 
            //get {
            //int mem = 0;
            //return (_Link.rtGetStatus(ref mem) == -1);
            //} 
        }

        public override void ProcessCalibrationData(int Columns, int Rows, double Width, double Height, PointF[] Data)
        {
            Attach();
            string filename = Application.ExecutablePath + @"rhothor.tmp";
            using (TextWriter tw = new StreamWriter(filename))
            {
                if (tw != null)
                {
                    tw.WriteLine("X:{0:0.000};{1}", Width, Columns);
                    tw.WriteLine("Y:{0:0.000};{1}", Height, Rows);
                    for (int i = 0; i < Data.Length; i++)
                    {
                        tw.Write("({0:0.000};{1:0.000})", -Data[i].X, -Data[i].Y);
                        if (i % Columns == Columns - 1) tw.Write((i == Data.Length - 1) ? ";" : "\r\n");
                    }
                }
            }
            _Link.rtAddCalibrationData(filename);
            _Link.rtStoreCalibration();
        }

        public override void OnLoadCalibrationFile()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Calibration txt files|*.txt|All files|*.*";
                dlg.Title = "Load calibration file";
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == DialogResult.OK && File.Exists(dlg.FileName))
                {
                    if (_Link.rtLoadCalibrationFile(dlg.FileName) == -1)
                        MessageBox.Show("Loading calibration file succeeded.", "Calibration", MessageBoxButtons.OK);
                    else
                        MessageBox.Show("Loading calibration file failed!", "Calibration", MessageBoxButtons.OK);
                }
            }
        }

        public override void OnStoreCalibrationFile()
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "Calibration txt files|*.txt|All files|*.*";
                dlg.Title = "Store calibration file";
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (_Link.rtStoreCalibrationFile(dlg.FileName) == -1)
                        MessageBox.Show("Storing calibration file succeeded.", "Calibration", MessageBoxButtons.OK);
                    else
                        MessageBox.Show("Storing calibration file failed!", "Calibration", MessageBoxButtons.OK);
                }
            }
        }

        public override void OnResetCalibration()
        {
            if (_Link.rtResetCalibration() == -1)
                MessageBox.Show("Calibration data cleared.", "Calibration", MessageBoxButtons.OK);
        }


        public override void OnSensorTestStart()
        {
            if (_Link.rtListOpen(4) == -1)
            {
                _Link.rtSetSpeed(10 * _FieldSize);
                _Link.rtSetJumpSpeed(10 * _FieldSize);
                _Link.rtSetLoop(0);
                _Link.rtJumpTo(-2, 0);
                _Link.rtLineTo(2, 0);
                _Link.rtJumpTo(0, -2);
                _Link.rtLineTo(0, 2);
                _Link.rtDoLoop();
                _Link.rtListClose();
            }
        }

        public override void OnSensorTestStop()
        {
            if (!_Attached)
                _Master.FlightMarker.Abort();
            else
                _Link.rtAbort();
        }

        private void Attach()
        {
            if (!_Attached) _Master.FlightMarker.Attach(_Link);
            _Attached = true;
        }

        private void Detach()
        {
            if (_Attached) _Master.FlightMarker.Detach(_Link);
            _Attached = false;
        }

        public override void OnInitTarget()
        {
            Attach();
            _Link.rtGetFieldSize(ref _FieldSize);
        }

        public override void OnExitTarget()
        {
            Detach();
            //Settings.Default.Save();
        }

    }
}
