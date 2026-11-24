using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Reflection;

namespace Flight
{
    public partial class CalibrationBCU : Form
    {
        private SerialPort _Port;

        enum CalState {  Idle=0 , Measure= 1 , Table = 2, Battery = 3 , Connecting = 4, Cancelled = 5, PowerScan = 6, PowerMax = 7 }
        enum HwState { Ok, Busy, Error }

        const int Beacon_TRIG = 30;
        const int Beacon_DIAM = 1000;
        const int Beacon_LVL1 = 5;  // < 5 µm darkgreen
        const int Beacon_LVL2 = 10; // < 10 µm green
        const int Beacon_LVL3 = 20; // 20 µm orange

        const int VBAT_LVL1 = 4200;
        const int VBAT_LVL2 = 4100;  // > 4100 darkgreen
        const int VBAT_LVL3 = 3750;  // > 4000 green
        const int VBAT_LVL4 = 3700;  // > 3700 yellow
        const int VBAT_LVL5 = 3650;  // > 3650 red
        const int VBAT_LVL6 = 3600;  // < 3600 dark red
        
        public class BcuSample
        {
            public int Index;
            public int SRow;
            public int SCol;
            public double OffsX;
            public double OffsY;
            public double X;
            public double Y;
            public double TableX;
            public double TableY;
            public double TableZ;
            public bool Sampled;
            public int Try;
            public List<PointF> History;

            public BcuSample(int iIndex, double SX, double SY, double TX, double TY, double TZ, int SensorRow, int SensorCol)
            {
                Index = iIndex;
                OffsX = 0;
                OffsY = 0;
                X = SX;
                Y = SY;
                TableX = TX;
                TableY = TY;
                TableZ = TZ;
                Sampled = false;
                Try = 0;
                SRow = SensorRow;
                SCol = SensorCol;
                History = new List<PointF>(); 
            }

            public void StoreSample()
            {
                History.Insert(0, new PointF((float)OffsX, (float)OffsY));
                if (History.Count > 5) History.RemoveAt(5);
            }

            public double MaxOffsXY
            {
                get{
                    return Math.Max(Math.Abs(OffsX), Math.Abs(OffsY));
                }
            }

            public double MaxOffsSqrt
            {
                get
                {
                    return Math.Sqrt(OffsX*OffsX+OffsY*OffsY);
                }
            }

            public int GetErrorLevel(bool Circular)
            {
                if (!Sampled) return -1;
                int i = (int)((Circular?MaxOffsSqrt:MaxOffsXY) * 1000);
                if (i < Beacon_LVL1) return 0;
                if (i < Beacon_LVL2) return 1;
                if (i < Beacon_LVL3) return 2;
                return 3;
            }
        }

        private BcuSample _Sample;
        private BcuSample _SampleInfo;


        private CalState _CalState;
        private List<BcuSample> _Measurement;
        private BeaconTarget _BeaconTgt;
        private int _Vbat;
        private int _Pwr;
        private int _JumpX;

        private int _TableUnit;
        private double _InfoZoom;

        public double _MaxOffsX;
        public double _MaxOffsY;
        public double _MaxDev;

        private double _CalW;
        private double _CalH;
        private int _CalCols;
        private int _CalRows;

        public bool _SettingsChanged = true;
        public bool _CalibrationChanged = false;
        public bool _SensorMode = false;

        public long _TicksCounter;
        public long _TicksStart;

        Brush[] _BrGrid = new Brush[4] { Brushes.ForestGreen, Brushes.Lime, Brushes.Orange, Brushes.Red };
        Pen[] _PnGrid = new Pen[4] { Pens.ForestGreen, Pens.Lime, Pens.Orange, Pens.Red};
        

        public CalibrationBCU(String ComPort, BeaconTarget BeaconTgt, int Columns, int Rows, double Width, double Height)
        {
            _Sample = null;
            _SampleInfo = null;
            _CalState = CalState.Idle;
            _Measurement = new List<BcuSample>();
            _BeaconTgt = BeaconTgt;
            _Vbat = 0;
            _Pwr = 0;
            _Port = new SerialPort(ComPort);
            _Port.BaudRate = 115200;
            _Port.DataBits = 8;
            _Port.Parity = Parity.None;
            _Port.StopBits = StopBits.One;
            _Port.Handshake = Handshake.None;
            _Port.NewLine = Environment.NewLine;
            _Port.ReadTimeout = 1000;
            _Port.WriteTimeout = 1000;
            _MaxOffsX = 0;
            _MaxOffsY = 0;
            _MaxDev = 0;
            SetCalibrationParams(Columns, Rows, Width, Height);
            _TicksCounter = 30000000;
            _TicksStart = 0;
            _InfoZoom = 0.5;
            _TableUnit = 1000;
            InitializeComponent();
        }

        private void SetCalibrationParams(int Columns, int Rows, double Width, double Height)
        {
            _CalCols = Columns;
            _CalRows = Rows;
            _CalW = Width;
            _CalH = Height;
            if (_CalCols > 33) _CalCols = 33; if (_CalCols < 2) { _CalCols = 1; _CalRows = 1; }
            if (_CalRows > 33) _CalRows = 33; if (_CalRows < 2) { _CalCols = 1; _CalRows = 1; }
            if (_CalW < 10) _CalW = 10;
            if (_CalH < 10) _CalH = 10;
        }

        public int CalColums { get{ return _CalCols;} }
        public int CalRows { get{ return _CalRows; } }
        public double CalWidth { get{ return _CalW;} }
        public double CalHeight { get{ return _CalH;} }

        private void CalibrationBCU_Load(object sender, EventArgs e)
        {
            pgCalibration.Minimum = 0;
            pgCalibration.Maximum = 1000;
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,panelView, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, panelInfo, new object[] { true });
            UpdateCalibrationData(false);
            cbErrField.SelectedIndex = 1;
            _CalState = CalState.Connecting;
            lblSensor.ImageIndex = 0;
            _BeaconTgt.OnInitTarget();
            UpdateTableRefData(false);
            GenerateMeasurement();
            lbTableUnit.Text = (0.001 * (double)_TableUnit).ToString();
            //return;
            while (!Open())
                if (MessageBox.Show("Cannot open Beacon COM port", "Beacon-CU16", MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                {
                    Close();
                    return;
                }
        }

        private bool Open()
        { 
            bool b = false;
            try
            {
                _Port.Open();
                b = _Port.IsOpen;
            }
            catch (Exception)
            { }
            return b;       
        }
        
        private void timerCmd_Tick(object sender, EventArgs e)
        {
            timerCmd.Enabled = false;
            HwState hs = UpdateState();
            if (hs != HwState.Busy) // success calibration
            {
                if (_SensorMode == true)
                    ScanPower();
                else if (hs == HwState.Ok && _Sample != null && _Measurement.Count > 3 &&
                    _Measurement[_Measurement.Count - 1].Sampled)
                {

                    if (cbLocked.Checked == false)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        PointF[] Data = new PointF[_Measurement.Count];
                        for (int i = 0; i < _Measurement.Count; i++)
                            Data[i] = new PointF((float)_Measurement[i].OffsX, (float)_Measurement[i].OffsY);
                        _BeaconTgt.ProcessCalibrationData(_CalCols, _CalRows, _CalW, _CalH, Data);
                        _CalibrationChanged = true;
                        this.Cursor = Cursors.Default;
                    }
                    ScanBattery();
                }
                _Sample = null;
                lblRemaining.Text = "";
                pgCalibration.Value = pgCalibration.Minimum;
                panelView.Refresh();
                if (btnOK.Enabled == false)
                    EnableCalibrationUI(true);
            }
            else
            {
                if (_Sample != null && _Measurement.Count>1)
                {
                    int todo = _Measurement.Count - _Sample.Index;
                    long ticks = DateTime.Now.Ticks - _TicksStart;
                    double done = ((double)todo - Math.Min(1.0, (double)ticks/(double)_TicksCounter))/ (double)_Measurement.Count;
                    pgCalibration.Value = Math.Min(1000, (int)(1000.0 - Math.Round(1000 * done)));
                    long sec = (todo * (Math.Max(_TicksCounter, ticks) / 10000) - ticks / 10000)/1000;
                    lblRemaining.Text = String.Format("Progress: {0:0}%, ", (double)pgCalibration.Value*100 / (double)pgCalibration.Maximum);
                    if (sec > 100) lblRemaining.Text += ((sec+59) / 60).ToString() + " minutes remaining";
                    else if (sec >= 0) lblRemaining.Text += sec.ToString() + " seconds remaining";
                }
                else pgCalibration.Value = pgCalibration.Minimum;
            }
            timerCmd.Enabled = true;
        }
        
        private void SamplePoint(BcuSample Sample, Double OffsX, Double OffsY, int Diam_µm)
        {
            SendCmd("SCOL=" + Sample.SCol.ToString());
            SendCmd("SROW=" + Sample.SRow.ToString());
            SendCmd("TRIG=" + Beacon_TRIG.ToString());
            SendCmd("DIAM=" + Diam_µm.ToString());
            SendCmd("SIZE=5000");

            _TicksStart = DateTime.Now.Ticks;
            if (_Sample != Sample)
                _BeaconTgt.TableJumpTo(Sample.TableX, Sample.TableY, Sample.TableZ);
            _Sample = Sample;
            _Sample.OffsX = OffsX;
            _Sample.OffsY = OffsY;
            _CalState = CalState.Table;
            panelView.Invalidate();
        }

        private void ScanBattery()
        {
            if (_CalState == CalState.Idle || _CalState == CalState.Connecting)
            {
                SendCmd("VBAT");
                _CalState = CalState.Battery;
            }
        }

        private void ScanPower()
        {
            if (_CalState == CalState.Idle)
            {
                SendCmd("SCOL=4");
                SendCmd("SROW=4");
                SendCmd("TRIG=3");
                SendCmd("_SWP");
                _CalState = CalState.PowerScan;
            }
        }

        private HwState UpdateState()
        { 
            int x,y;
            string s = ReadReply();
            if (s != "")
            {
                string[] arr;
                if (_CalState == CalState.Measure && s.StartsWith("JUMP"))
                {
                    arr = s.Split(' ');
                    if (arr.Length > 2 && int.TryParse(arr[1], out x) && int.TryParse(arr[2],out y))
                    {
                        _JumpX = x;
                        _BeaconTgt.ScannerJumpTo(_Sample.X + _Sample.OffsX + 0.001 * x, _Sample.Y + _Sample.OffsY + 0.001 * y);
                        if (SendCmd("NEXT")) return HwState.Busy; 
                    }
                    return HwState.Error;
                }
                if (_CalState == CalState.Measure && s.StartsWith("LINE"))
                {
                    arr = s.Split(' ');
                    if (arr.Length > 2 && int.TryParse(arr[1], out x) && int.TryParse(arr[2], out y))
                    {
                        lblSensor.ImageIndex = (Math.Abs(x-_JumpX) > 0) ? 1 : 2;
                        _BeaconTgt.ScannerLineTo(_Sample.X + _Sample.OffsX + 0.001 * x, _Sample.Y + _Sample.OffsY + 0.001 * y);
                        if (SendCmd("NEXT")) return HwState.Busy;
                    }
                    return HwState.Error;
                }
                if (_CalState == CalState.Measure && s.StartsWith("OFFS"))
                {
                    arr = s.Split(' ');
                    if (arr.Length > 2 && int.TryParse(arr[1], out x) && int.TryParse(arr[2], out y))
                    {
                        _TicksCounter = DateTime.Now.Ticks - _TicksStart;
                        lblSensor.ImageIndex = 3;
                        _Sample.Sampled = true;
                        _Sample.OffsX += 0.001 * x;
                        _Sample.OffsY += 0.001 * y;
                        _MaxOffsX = Math.Max(_MaxOffsX, Math.Abs(_Sample.OffsX));
                        _MaxOffsY = Math.Max(_MaxOffsY, Math.Abs(_Sample.OffsY));
                        _MaxDev = Math.Max(_MaxDev, _Sample.MaxOffsSqrt);
                        _SampleInfo = _Sample;
                        UpdateInfoUI();
                        if (_Sample.Index < _Measurement.Count - 1)
                        {
                            SamplePoint(_Measurement.Find(v => v.Index == _Sample.Index + 1), _Sample.OffsX, _Sample.OffsY, Beacon_DIAM);
                            return HwState.Busy;
                        }
                        else
                        {
                            foreach (BcuSample bs in _Measurement) bs.StoreSample();
                            _CalState = CalState.Idle;
                            return HwState.Ok;
                        }
                    }
                    return HwState.Error;
                }
                
                if (_CalState == CalState.Measure || s.StartsWith("ERR"))
                {
                    lblSensor.ImageIndex = 4;
                    _CalState = CalState.Idle;
                    _Sample.Try++;
                    if (_Sample.Try<4)
                    {
                        SamplePoint(_Sample, 0, 0, Beacon_DIAM-_Sample.Try*100);
                        return HwState.Busy;
                    }   
                    else if (MessageBox.Show("Sampling Failed", "Beacon Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry) 
                    {
                        _Sample.Try=0;
                        SamplePoint(_Sample, 0, 0, Beacon_DIAM);
                        return HwState.Busy;                 
                    }
                    _Sample = null;
                    UpdateInfoUI();
                    return HwState.Error;
                }

                if (_CalState == CalState.PowerMax)
                {
                    if (_SensorMode)
                    {
                        int.TryParse(s, out _Pwr);
                        lblSensor.ImageIndex = _Pwr > 5 ? 3 : 4;
                    }
                    else lblSensor.ImageIndex = 0;
                    _CalState = CalState.Idle;
                    return HwState.Ok;
                }
                if (_CalState == CalState.Connecting)
                {
                    ScanBattery();
                    return HwState.Ok;
                }
                if (_CalState == CalState.Battery)
                {
                    if (int.TryParse(s, out _Vbat))
                    {
                        if (_Vbat > VBAT_LVL2) lblBat.ImageIndex = 1;
                        else if (_Vbat > VBAT_LVL3) lblBat.ImageIndex = 2;
                        else if (_Vbat > VBAT_LVL4) lblBat.ImageIndex = 3;
                        else if (_Vbat > VBAT_LVL5) lblBat.ImageIndex = 4;
                        else if (_Vbat > VBAT_LVL6) lblBat.ImageIndex = 5;
                        else lblBat.ImageIndex = 0;
                    }
                }
                _CalState = CalState.Idle;
                return HwState.Ok;
            }

            if (_CalState == CalState.PowerScan)
            {
                SendCmd("SMAX");
                _CalState = CalState.PowerMax;
                return HwState.Busy;
            }
            if (_CalState == CalState.Cancelled)
                return HwState.Error;
            if (_CalState == CalState.Table)
            { 
                if (_BeaconTgt.TableInPosition == true)
                {
                    if (_Sample != null)
                    {
                        lblSensor.ImageIndex = 0;
                        _CalState = CalState.Measure;
                        SendCmd("START");
                    }
                    else _CalState = CalState.Idle;
                }
            }
            if (_CalState == CalState.Idle)
                return HwState.Ok;
            return HwState.Busy;
        }

        private bool SendCmd(string Cmd)
        {
            bool b = true;
            try { _Port.Write(Cmd + Environment.NewLine);  }
            catch (Exception) {  b = false;  }
            return b;
        }

        private string ReadReply()
        {
            string s = "";
            try {
                if (_Port.BytesToRead>0)
                    s = _Port.ReadLine(); 
            }
            catch (Exception) { s = ""; }
            return s;
        }
        
        public void UpdateCalibrationData(bool Save)
        {
            int r, c;
            double w, h;
            if (Save)
            {
                r= _CalRows;
                c= _CalCols;
                w= _CalW;
                h= _CalH;
                int.TryParse(tbRows.Text, out r);
                int.TryParse(tbCols.Text, out c);
                double.TryParse(tbWidth.Text, out w);
                double.TryParse(tbHeight.Text, out h);
                SetCalibrationParams(r, c, w, h);
            }
            else
            {
                tbRows.Text = _CalRows.ToString();
                tbCols.Text = _CalCols.ToString();
                tbWidth.Text = _CalW.ToString("0.0");
                tbHeight.Text = _CalH.ToString("0.0");
            }
        }


        public void GenerateMeasurement()
        {
            int c, r, i, j;
            double dx, dy, tableX, tableY, tableZ;
            if (_SettingsChanged == false) return;
            timerCmd.Enabled = false;
            tableX = tableY = tableZ = 0;
            _BeaconTgt.GetTableCalibrationRef(ref tableX, ref tableY, ref tableZ);
            UpdateCalibrationData(true);
            _SettingsChanged = false;
            _Measurement.Clear();
            if (_CalCols > 1)
            {
                dx = _CalW / (_CalCols - 1);
                dy = _CalH / (_CalRows - 1);
                for (j = 0; j < _CalRows; j++)
                    for (i = 0; i < _CalCols; i++)
                        _Measurement.Add(new BcuSample(0, -_CalW / 2 + dx * i, -_CalH / 2 + dy * j,
                                                           tableX + _CalW / 2 - dx * i, tableY + _CalH / 2 - dy * j, tableZ, 4, 4));
                // spiral stepping
                int[,] dirSteps = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
                int dir = 2;
                i = _CalRows * _CalCols - 1;
                r = _CalRows - 1;
                c = _CalCols - 1;
                _Measurement[r * _CalCols + c].Index = i;
                while (i > 0)
                {
                    if (c + dirSteps[dir, 0] == _CalCols || c + dirSteps[dir, 0] < 0 ||
                        r + dirSteps[dir, 1] == _CalRows || r + dirSteps[dir, 1] < 0 ||
                        _Measurement[(r + dirSteps[dir, 1]) * _CalCols + c + dirSteps[dir, 0]].Index > 0)
                        dir = (dir + 1) % 4;
                    c += dirSteps[dir, 0];
                    r += dirSteps[dir, 1];
                    _Measurement[r * _CalCols + c].Index = --i;
                }
            }
            else
                _Measurement.Add(new BcuSample(0, 0, 0, tableX, tableY, tableZ, 4, 4));

            panelView.Invalidate();
            timerCmd.Enabled = true;
        }




        private void CalibrationBCU_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_Sample != null)
                e.Cancel = true;
            _BeaconTgt.OnExitTarget();
            _Port.Close();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            EnableCalibrationUI(false);
            pgCalibration.Value = 0;
            _MaxOffsX = 0;
            _MaxOffsY = 0;
            _MaxDev = 0;
            GenerateMeasurement();
            foreach (BcuSample smp in _Measurement)
            {
                smp.Sampled = false;
                smp.Try = 0;
            }
            SamplePoint(_Measurement.Find(v => v.Index == 0), 0, 0, Beacon_DIAM);
        }

        private void UpdateInfoUI()
        {
            bool b = _SampleInfo != null && _SampleInfo.Sampled; 
            lblOffsX.Text = b ? Convert.ToInt32(_SampleInfo.OffsX * 1000).ToString() : "-";
            lblOffsY.Text = b ?Convert.ToInt32(_SampleInfo.OffsY * 1000).ToString(): "-";
            lblOffsXY.Text = b ?Convert.ToInt32(_SampleInfo.MaxOffsSqrt * 1000).ToString(): "-";
            lblOffsMaxX.Text = b ? Convert.ToInt32(_MaxOffsX * 1000).ToString(): "-";
            lblOffsMaxY.Text = b ? Convert.ToInt32(_MaxOffsY * 1000).ToString(): "-";
            lblOffsMaxXY.Text = b ? Convert.ToInt32(_MaxDev * 1000).ToString(): "-";
            panelInfo.Invalidate();
        }

        private void EnableCalibrationUI(bool Enable)
        {
            tabCalSettings.Enabled = Enable;
            tabTableSettings.Enabled = Enable;
            btnOK.Enabled = Enable;
            btnRun.Enabled = Enable;
            btnAbort.Enabled = !Enable;
        }


        private void CalMsr_TextChanged(object sender, EventArgs e)
        {
            _SettingsChanged = true;
            GenerateMeasurement();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            if (_Sample!=null)
            {
                _Sample = null;
                _CalState = CalState.Cancelled;
                lblSensor.ImageIndex = 0;
            }
        }

        private void btnLoadCalibration_Click(object sender, EventArgs e)
        {
            if (_Sample != null) return;
            _BeaconTgt.OnLoadCalibrationFile();
        }

        private void btnStoreCalibration_Click(object sender, EventArgs e)
        {
            if (_Sample != null) return;
            _BeaconTgt.OnStoreCalibrationFile();
        }

        private void btnResetCalibration_Click(object sender, EventArgs e)
        {
            if (_Sample != null) return;
            _BeaconTgt.OnResetCalibration();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void CalibrationBCU_SizeChanged(object sender, EventArgs e)
        {
            panelInfo.Invalidate();
            panelView.Invalidate();
        }




        #region CustomDraw

        private RectangleF GetRect(float x, float y, double scale, double w, double h)
        {
            return new RectangleF((float)(x - 0.5 * w * scale),
                                  (float)(y - 0.5 * h * scale),
                                  (float)(w * scale),
                                  (float)(h * scale));
        }

        private void panelInfo_Paint(object sender, PaintEventArgs e)
        {
            RectangleF rcf, rcClient;
            PointF pt = new PointF();
            PointF ctr = new PointF(panelInfo.Width / 2, panelInfo.Height / 2);
            Graphics g = e.Graphics;
            double sc = 480 * _InfoZoom * Math.Min(panelInfo.Width, panelInfo.Height) / Beacon_LVL3;

            rcClient = new RectangleF(0, 0, panelInfo.Width, panelInfo.Height);
            g.FillRectangle(Brushes.White, rcClient);
            g.DrawLine(Pens.LightGray, ctr.X, 0, ctr.X, panelInfo.Height);
            g.DrawLine(Pens.LightGray, 0, ctr.Y, panelInfo.Width, ctr.Y);

            if (cbErrField.SelectedIndex == 0)
            {
                rcf = GetRect(ctr.X, ctr.Y, sc, 0.002f * Beacon_LVL1, 0.002f * Beacon_LVL1);
                g.DrawRectangle(_PnGrid[0], rcf.Left, rcf.Top, rcf.Width, rcf.Height);
                rcf = GetRect(ctr.X, ctr.Y, sc, 0.002f * Beacon_LVL2, 0.002f * Beacon_LVL2);
                g.DrawRectangle(_PnGrid[1], rcf.Left, rcf.Top, rcf.Width, rcf.Height);
                rcf = GetRect(ctr.X, ctr.Y, sc, 0.002f * Beacon_LVL3, 0.002f * Beacon_LVL3);
                g.DrawRectangle(_PnGrid[2], rcf.Left, rcf.Top, rcf.Width, rcf.Height);
            }
            else
            {
                rcf = GetRect(ctr.X, ctr.Y, sc, 0.002f * Beacon_LVL1, 0.002f * Beacon_LVL1);
                g.DrawEllipse(_PnGrid[0], rcf.Left, rcf.Top, rcf.Width, rcf.Height);
                rcf = GetRect(ctr.X, ctr.Y, sc, 0.002f * Beacon_LVL2, 0.002f * Beacon_LVL2);
                g.DrawEllipse(_PnGrid[1], rcf.Left, rcf.Top, rcf.Width, rcf.Height);
                rcf = GetRect(ctr.X, ctr.Y, sc, 0.002f * Beacon_LVL3, 0.002f * Beacon_LVL3);
                g.DrawEllipse(_PnGrid[2], rcf.Left, rcf.Top, rcf.Width, rcf.Height);
            }

            if (_SampleInfo != null)
            {
                foreach (PointF ptI in _SampleInfo.History)
                {
                    pt.X = (float)Math.Round(ctr.X + sc * ptI.X);
                    pt.Y = (float)Math.Round(ctr.Y - sc * ptI.Y);
                    g.DrawArc(Pens.LightGray, new RectangleF(pt.X - 2, pt.Y - 2, 5, 5), 0, 360);
                    if (rcClient.Contains(pt) == false)
                        g.DrawLine(Pens.LightGray, pt.X - 2, pt.Y, ctr.X, ctr.Y);
                }
                if (_SampleInfo.Sampled)
                {
                    int err = _SampleInfo.GetErrorLevel(cbErrField.SelectedIndex == 1);
                    pt.X = (float)Math.Round(ctr.X + sc * _SampleInfo.OffsX);
                    pt.Y = (float)Math.Round(ctr.Y - sc * _SampleInfo.OffsY);
                    g.FillEllipse(_BrGrid[err], new RectangleF(pt.X - 3f, pt.Y - 3f, 6f, 6f));
                    g.DrawArc(Pens.LightGray, new RectangleF(pt.X - 3f, pt.Y - 3f, 6f, 6f), 0, 360);
                    if (rcClient.Contains(pt) == false)
                        g.DrawLine(_PnGrid[err], pt.X - 2, pt.Y, ctr.X, ctr.Y);
                }
            }
        }

        private void panelView_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rc = new Rectangle(0, 0, panelView.Width, panelView.Height);

            double sc = _Measurement.Count > 1 ? Math.Min((panelView.Width - 40) / _CalW, (panelView.Height - 40) / _CalH)
                                              : (panelView.Width - 40) / _BeaconTgt.FieldSize;
            g.FillRectangle(Brushes.White, rc);
            g.DrawRectangle(Pens.Indigo,
                (float)(panelView.Width / 2 - sc * _BeaconTgt.FieldSize / 2), (float)(panelView.Height / 2 - sc * _BeaconTgt.FieldSize / 2),
                (float)(sc * _BeaconTgt.FieldSize), (float)(sc * _BeaconTgt.FieldSize));

            for (int i = 0; i < _Measurement.Count; i++)
            {
                rc = new Rectangle(panelView.Width / 2 + (int)Math.Round(_Measurement[i].X * sc) - 4,
                                   panelView.Height / 2 - (int)Math.Round(_Measurement[i].Y * sc) - 4, 7, 7);
                if (_Measurement[i].Sampled)
                    g.FillEllipse(_BrGrid[_Measurement[i].GetErrorLevel(cbErrField.SelectedIndex == 1)], rc);
                else
                    g.FillEllipse(_Sample == _Measurement[i] ? Brushes.LightGray : Brushes.White, rc);
                g.DrawArc(Pens.Gray, rc, 0, 360);
            }
        }
        
        private void cbErrField_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelInfo.Invalidate();
            panelView.Invalidate();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            if (_InfoZoom > 0.01) _InfoZoom /= Math.Sqrt(2);
            panelInfo.Invalidate();
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            if (_InfoZoom < 1.4) _InfoZoom *= Math.Sqrt(2);
            panelInfo.Invalidate();
        }

        #endregion

        #region TableControl

        private void btnSetRef_Click(object sender, EventArgs e)
        {
            _SettingsChanged = true;
            UpdateTableRefData(true);
        }
        
        private void btnTableUnit1_Click(object sender, EventArgs e)
        {
            if (_TableUnit > 1) _TableUnit = _TableUnit / 10;
            lbTableUnit.Text = (0.001 * (double)_TableUnit).ToString();
            btnTableUnit1.Enabled = _TableUnit > 1;
            btnTableUnit2.Enabled = _TableUnit < 100000;
        }

        private void btnTableUnit2_Click(object sender, EventArgs e)
        {
            if (_TableUnit < 100000) _TableUnit = _TableUnit * 10;
            lbTableUnit.Text = (0.001 * (double)_TableUnit).ToString();
            btnTableUnit1.Enabled = _TableUnit > 1;
            btnTableUnit2.Enabled = _TableUnit < 100000;
        }

        private void btnTableXmin_Click(object sender, EventArgs e)
        {
            OnTableJog(-0.001 * _TableUnit, 0, 0);
        }

        private void btnTableXplus_Click(object sender, EventArgs e)
        {
            OnTableJog(0.001 * _TableUnit, 0, 0);
        }

        private void btnTableYmin_Click(object sender, EventArgs e)
        {
            OnTableJog(0, -0.001 * _TableUnit, 0);
        }

        private void btnTableYplus_Click(object sender, EventArgs e)
        {
            OnTableJog(0, 0.001 * _TableUnit, 0);
        }

        private void btnTableZmin_Click(object sender, EventArgs e)
        {
            OnTableJog(0, 0, -0.001 * _TableUnit);
        }

        private void btnTableZplus_Click(object sender, EventArgs e)
        {
            OnTableJog(0, 0, 0.001 * _TableUnit);
        }

        private void btnTableCalRef_Click(object sender, EventArgs e)
        {
            EnableCalibrationUI(false);
            timerCmd.Enabled = false;
            if (_CalState == CalState.Idle && _Sample == null)
            {
                double x, y, z;
                x = y = z = 0;
                _BeaconTgt.GetTableCalibrationRef(ref x, ref y, ref z);
                _BeaconTgt.TableJumpTo(x, y, z);
                _CalState = CalState.Table;
            }
            timerCmd.Enabled = true;
        }

        private void OnTableJog(double dx, double dy, double dz)
        {
            EnableCalibrationUI(false);
            timerCmd.Enabled = false;
            if (_CalState == CalState.Idle && _Sample == null)
            {
                _BeaconTgt.TableJumpToRel(dx, dy, dz);
                _CalState = CalState.Table;
            }
            timerCmd.Enabled = true;
        }

        public void UpdateTableRefData(bool Save)
        {
            double x, y, z;

            x = y = z = 0;
            if (Save)
            {
                _BeaconTgt.GetCurrentTablePosition(ref x, ref y, ref z);
                _BeaconTgt.SetTableCalibrationRef(x, y, z);
            }
            _BeaconTgt.GetTableCalibrationRef(ref x, ref y, ref z);
            lblCalRefX.Text = "Reference X: " + x.ToString("0.000");
            lblCalRefY.Text = "Reference Y: " + y.ToString("0.000");
            lblCalRefZ.Text = "Reference Z: " + z.ToString("0.000");
        }

        private void btnSensor_MouseDown(object sender, MouseEventArgs e)
        {
            if (_CalState == CalState.Idle)
            {
                btnSensor.Capture = true;
                timerCmd.Enabled = false;
                _SensorMode = true;
                _BeaconTgt.OnSensorTestStart();
                lblSensor.ImageIndex = 0;
                lblSensor.Invalidate();
                timerCmd.Enabled = true;
            }

        }

        private void btnSensor_MouseUp(object sender, MouseEventArgs e)
        {
            if (_SensorMode)
            {
                timerCmd.Enabled = false;
                _SensorMode = false;
                _BeaconTgt.OnSensorTestStop();
                lblSensor.ImageIndex = 0;
                lblSensor.Invalidate();
                btnSensor.Capture = false;
                timerCmd.Enabled = true;
            }
        }

        #endregion
    }
}
