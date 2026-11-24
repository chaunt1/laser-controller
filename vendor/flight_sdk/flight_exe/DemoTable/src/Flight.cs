using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FlightLib;
using System.Globalization;
using global::Flight.Properties;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Flight
{
	public partial class Flight : Form
	{
        private string _FileName;
		private bool _Connected;
        private bool _Interlocked;
        private int _InterlockMask = 0;
        private bool _Marking;
        private int _TableUnit;

        public bool _TableCommand;        
        public double _TableX;
        public double _TableY;
        public double _TableZ;
        public int _StateIO = 0;

        private double _SR_StartX = 0;
        private double _SR_StartY = 0;
        private double _SR_DeltaX = 10;
        private double _SR_DeltaY = 10;
        private int _SR_Rows = 1;
        private int _SR_Columns = 1;

		public rhothorLink _Link = new rhothorLink();
        private Stopwatch _Stopwatch = new Stopwatch();

        private string m_CopyTree = "";
        private string m_CopyColors = "";
        private string m_CopyPens = "";
        private string m_CopyLayouts = "";
        private string m_CopyFonts = "";
        private FlightObject m_ClipBoard = new FlightObject();

		public Flight()
		{
			InitializeComponent();
		}

		private DialogResult InputBox(string title, string promptText, ref string value)
		{
			Form form = new Form();
			Label label = new Label();
			TextBox textBox = new TextBox();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = title;
			label.Text = promptText;
			textBox.Text = value;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(9, 20, 372, 13);
			textBox.SetBounds(12, 36, 372, 20);
			buttonOk.SetBounds(228, 72, 75, 23);
			buttonCancel.SetBounds(309, 72, 75, 23);

			label.AutoSize = true;
			textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new Size(396, 107);
			form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
			form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			value = textBox.Text;
			return dialogResult;
		}


        private void LoadProperties()
        {
            Pens.TileMode = FlightPensTileMode.InfiniteField;
            Pens.TileWidth = FlightMarker.FieldSize;
            Pens.TileHeight = FlightMarker.FieldSize;
            Pens.TileDepth = FlightMarker.FieldSizeZ;
            Colors.TableSnapX = Settings.Default.TableSnapX;
            Colors.TableSnapY = Settings.Default.TableSnapY;
            Colors.TableSnapZ = Settings.Default.TableSnapZ;
            Pens.TileOverlap = Settings.Default.TileOverlap;
            Pens.TileWidth = Settings.Default.TileWidth;
            Pens.TileHeight =  Settings.Default.TileHeight;
            Pens.TileDepth = Settings.Default.TileDepth;
            Pens.TileSnap = Settings.Default.TileSnap;
            FlightMarker.TableOptimize = Settings.Default.TableOptimize ? 1 : 0;     
        }

		private void LoadSettings()
		{
			// view
			View.FieldWidth = Settings.Default.FieldWidth;
			View.FieldHeight = Settings.Default.FieldHeight;
			View.FieldTop = Settings.Default.FieldTop;
			View.FieldLeft = Settings.Default.FieldLeft;

            // view
            View3D.FieldWidth = Settings.Default.FieldWidth;
            View3D.FieldHeight = Settings.Default.FieldHeight;
            View3D.FieldTop = Settings.Default.FieldTop;
            View3D.FieldLeft = Settings.Default.FieldLeft;
            View3D.FieldDepth = Settings.Default.FieldDepth;
            View3D.FieldFront = Settings.Default.FieldFront;
            View3D.ViewMode = FlightViewMode.Rotate;

            // laser
            Colors.PowerDutyCycle0 = Settings.Default.LaserPowerDC0;
            Colors.PowerDutyCycle100 = Settings.Default.LaserPowerDC100;
            Colors.PowerPulseRate = Settings.Default.LaserPowerRate;
            Colors.GrayscaleMode = Settings.Default.LaserGrayScale;
            Colors.PowerMode = Settings.Default.LaserPowerMode;
            Colors.PowerOffOnIdle = Settings.Default.LaserPwrOffOnIdle;
            Colors.LaserLink = FlightColorsLaserLink.Default;
            Colors.LaserEvent = FlightColorsLaserEvent.Never;
            Colors.LaserParamsMode = FlightColorsLaserParamsMode.SetLaserParams;

			// hardware
            FlightMarker.MessageMode = FlightMarkerMessageMode.AddMessage;
            FlightMarker.TableMode = FlightMarkerTableMode.AddTable;
            FlightMarker.FieldSize = Settings.Default.FieldSize;
            FlightMarker.FieldSizeZ = Settings.Default.FieldSizeZ;
            FlightMarker.Address = Settings.Default.Address;
			FlightMarker.Resolution = Settings.Default.Resolution;
			FlightMarker.TableAxisA = false;
			FlightMarker.TableAxisB = false;
			FlightMarker.TableAxisC = false;
			FlightMarker.TableAxisX = Settings.Default.EnableAxisX;
            FlightMarker.TableAxisY = Settings.Default.EnableAxisY;
            FlightMarker.TableAxisZ = Settings.Default.EnableAxisZ;
            FlightMarker.TablePositionMinX = Settings.Default.TableTravelMinX;
            FlightMarker.TablePositionMaxX = Settings.Default.TableTravelMaxX;
            FlightMarker.TablePositionMinY = Settings.Default.TableTravelMinY;
            FlightMarker.TablePositionMaxY = Settings.Default.TableTravelMaxY;
            FlightMarker.TablePositionMinZ = Settings.Default.TableTravelMinZ;
            FlightMarker.TablePositionMaxZ = Settings.Default.TableTravelMaxZ;
            FlightMarker.UpdateData();
            PropTree.Refresh();
		}

		private void Flight_Load(object sender, EventArgs e)
		{
			Application.CurrentCulture = new CultureInfo("en-us");

			FlightMarker.Attach(Colors);
			FlightMarker.Attach(Pens);
			FlightMarker.Attach(Fonts);
			FlightMarker.Attach(Layouts);
            FlightMarker.Attach(Surfaces);
            FlightMarker.Attach(Tree);
			FlightMarker.Attach(PropColors);
			FlightMarker.Attach(PropPens);
			FlightMarker.Attach(PropFonts);
			FlightMarker.Attach(PropLayouts);
            FlightMarker.Attach(PropSurfaces);
			FlightMarker.Attach(PropTree);
			FlightMarker.Attach(View);
            FlightMarker.Attach(View3D);
			FlightMarker.Attach(Editor);

            _Connected = false;
            _Interlocked = false;
			_Marking = false;
			_FileName = "";
            _TableUnit = 1000;

     		ObjectsInit();
			this.Height = 650;
			LoadSettings();
            LoadProperties();

			View.Focus();
			SetViewMode(FlightViewMode.Select);
			UpdateDocTitle();
			UpdateUI();
		}

		private void Flight_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (_Marking == true)
			{
				e.Cancel = true;
				return;
			}

			DialogResult res = MessageBox.Show("Save document before closing?", "Closing",
				MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
			if (res != DialogResult.Cancel)
			{
				FlightMarker.Address = "";
				if (res == DialogResult.Yes) ObjectsSave();
				Timer.Enabled = false;
				FlightMarker.Detach(Colors);
				FlightMarker.Detach(Pens);
				FlightMarker.Detach(Fonts);
				FlightMarker.Detach(Layouts);
				FlightMarker.Detach(Tree);
                FlightMarker.Detach(Surfaces);
                FlightMarker.Detach(PropColors);
				FlightMarker.Detach(PropPens);
				FlightMarker.Detach(PropFonts);
				FlightMarker.Detach(PropLayouts);
                FlightMarker.Detach(PropSurfaces);
				FlightMarker.Detach(PropTree);
				FlightMarker.Detach(View);
                FlightMarker.Detach(View3D);
				FlightMarker.Detach(Editor);
			}
			else e.Cancel = true;
		}

		private void ObjectsInit()
		{
            // global laser parameters
            Colors.OperationMode = 5;
            Colors.Speed = 500;
            Colors.JumpSpeed = 2000;
            Colors.QswPulseRate = 50000;
            Colors.QswPulseWidth = 10;
            Colors.BurstRate = 50000;
            Colors.BurstTime = 15;
         
            Colors.AddColor();
			Colors.ListIndex = 0;
			Fonts.AddTruetypeFont();
			Fonts.ListIndex = 0;
			Pens.AddPen();
			Pens.ListIndex = 0;
			Layouts.AddLayout();
			Layouts.ListIndex = 0;
            Surfaces.AddSurface();
            Surfaces.ListIndex = 0;
			PropColors.PropSrc = Colors.get_Data(0);
			PropFonts.PropSrc = Fonts.get_Data(0);
			PropPens.PropSrc = Pens.get_Data(0);
			PropLayouts.PropSrc = Layouts.get_Data(0);
            PropSurfaces.PropSrc = Surfaces.get_Data(0);
			PropTree.Clear();
			Colors.get_Data(0).RGB = 0xFF;
		}

		private void ObjectsNew()
		{
			DialogResult res = MessageBox.Show("Save previous document?", "New",
				MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
			if (res != DialogResult.Cancel)
			{
				if (res == DialogResult.OK) ObjectsSave();
				Tree.DeleteAllItems();
				Pens.Clear();
				Fonts.Clear();
				Colors.Clear();
				Layouts.Clear();
				ObjectsInit();
				_FileName = "";
				UpdateDocTitle();
				FlightMarker.UpdateData();
			}
		}

		private void ObjectsSave()
		{
			if (_FileName != "")
				ObjectsSaveToFile(_FileName);
			else ObjectsSaveAs();
		}

		private void ObjectsSaveAs()
		{
			using (SaveFileDialog dlg = new SaveFileDialog())
			{
				dlg.Filter = "flight xml files|*.xml|All files|*.*";
				dlg.Title = "Save flight xml File";
				dlg.DefaultExt = "xml";
				dlg.InitialDirectory = _FileName;
				dlg.FileName = (_FileName != "") ? _FileName : "FlightJob1.xml";
				dlg.RestoreDirectory = true;
				dlg.AddExtension = true;
				if (dlg.ShowDialog() == DialogResult.OK) ObjectsSaveToFile(dlg.FileName);
			}
		}

		private void ObjectsSaveToFile(String FileName)
		{
			_FileName = FileName;
            //FlightMarker.StoreJobFile(_FileName);
            Tree.Store(_FileName);
            Colors.Store(_FileName);
            Pens.Store(_FileName);
            Fonts.Store(_FileName);
            Layouts.Store(_FileName);
            Surfaces.Store(_FileName);
			UpdateDocTitle();
		}

		private void ObjectsLoad()
		{
			using (OpenFileDialog dlg = new OpenFileDialog())
			{
				dlg.Filter = "flight xml files|*.xml|All files|*.*";
				dlg.Title = "Load flight xml File";
				dlg.RestoreDirectory = true;
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					_FileName = dlg.FileName;
                    //FlightMarker.LoadJobFile(_FileName);
                    Tree.Load(_FileName);
                    Colors.Load(_FileName);
                    Pens.Load(_FileName);
                    Fonts.Load(_FileName);
                    Layouts.Load(_FileName);
                    Surfaces.Load(_FileName);
                    UpdateData();
					PropTree.Refresh();
					Colors.ListIndex = 0;
					PropColors.PropSrc = Colors.get_Data(0);
					Fonts.ListIndex = 0;
					PropFonts.PropSrc = Fonts.get_Data(0);
					Pens.ListIndex = 0;
					PropPens.PropSrc = Pens.get_Data(0);
                    if (Layouts.ListCount > 0)
                    {
                        Layouts.ListIndex = 0;
                        PropLayouts.PropSrc = Layouts.get_Data(0);
                    }
                    else
                        PropLayouts.Clear();
                    if (Surfaces.ListCount > 0)
                    {
                        Surfaces.ListIndex = 0;
                        PropSurfaces.PropSrc = Surfaces.get_Data(0);
                    }
                    else
                        PropSurfaces.Clear();

					PropTree.Clear();
					UpdateDocTitle();
				}
			}
		}


		private void tbColors_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem == CmdAddColor)
			{ 
				Colors.AddColor();
				Colors.ListIndex = Colors.ListCount - 1;
				PropColors.PropSrc = Colors.get_Data(Colors.ListIndex);
				Colors.get_Data(Colors.ListIndex).RGB = 0xFF;
			}
			else if (e.ClickedItem == CmdRemoveColor)
			{
				Colors.RemoveColor();
				Colors.ListIndex = Colors.ListCount - 1;
				if (Colors.ListCount>0)
					PropColors.PropSrc = Colors.get_Data(Colors.ListIndex);
				else
					PropColors.Clear();
			}
			else if (e.ClickedItem == CmdChangeColor)
			{
				FlightColor clr = Colors.get_Data(Colors.ListIndex);
				using (ColorDialog dlg = new ColorDialog())
				{
					dlg.AllowFullOpen = false;
					dlg.Color = ColorTranslator.FromOle((int)clr.RGB);
					dlg.ShowDialog();
					clr.RGB = (uint)ColorTranslator.ToOle(dlg.Color);
				}
			}
			UpdateData();
		}

		private void tbPens_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem == CmdAddPen)
			{
				Pens.AddPen();
				Pens.ListIndex = Pens.ListCount - 1;
				PropPens.PropSrc = Pens.get_Data(Pens.ListIndex);
			}
			else if (e.ClickedItem == CmdRemovePen)
			{
				Pens.RemovePen();
				Pens.ListIndex = Pens.ListCount - 1;
				if (Pens.ListCount > 0)
					PropPens.PropSrc = Pens.get_Data(Pens.ListIndex);
				else
					PropPens.Clear();
			}
			UpdateData();
		}

		private void tbFonts_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem == CmdAddTrueTypeFont)
			{
				Fonts.AddTruetypeFont();
				Fonts.ListIndex = Fonts.ListCount - 1;
				PropFonts.PropSrc = Fonts.get_Data(Fonts.ListIndex);
			}
			else if (e.ClickedItem == CmdAddSinglestrokeFont)
			{
				Fonts.AddSinglestrokeFont();
				Fonts.ListIndex = Fonts.ListCount - 1;
				PropFonts.PropSrc = Fonts.get_Data(Fonts.ListIndex);
			}
			else if (e.ClickedItem == CmdAddBarcodeFont)
			{
				Fonts.AddBarcodeFont();
				Fonts.ListIndex = Fonts.ListCount - 1;
				PropFonts.PropSrc = Fonts.get_Data(Fonts.ListIndex);
			}
			else if (e.ClickedItem == CmdRemoveFont)
			{
				Fonts.RemoveFont();
				Fonts.ListIndex = Fonts.ListCount - 1;
				if (Fonts.ListCount <0)
					PropFonts.PropSrc = Fonts.get_Data(Fonts.ListIndex);
				else
					PropFonts.Clear();
			}
			UpdateData();
		}

		private void tbLayouts_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem == CmdAddLayout)
			{
				Layouts.AddLayout();
				Layouts.ListIndex = Layouts.ListCount - 1;
				PropLayouts.PropSrc = Layouts.get_Data(Layouts.ListIndex);
			}
			else if (e.ClickedItem == CmdRemoveLayout)
			{
				Layouts.RemoveLayout();
				Layouts.ListIndex = Layouts.ListCount - 1;
				if (Layouts.ListCount > 0)
					PropLayouts.PropSrc = Layouts.get_Data(Layouts.ListIndex);
				else
					PropLayouts.Clear();
			}
            else if (e.ClickedItem == CmdEditLayout)
            {
                using (StepRepeatDlg dlg = new StepRepeatDlg())
                {
                    if (Layouts.ListIndex != -1)
                    {
                        FlightLayout fl = Layouts.get_Data(Layouts.ListIndex);
                        dlg.LayoutStartX = _SR_StartX;
                        dlg.LayoutStartY = _SR_StartY;
                        dlg.LayoutColumns = _SR_Columns;
                        dlg.LayoutRows = _SR_Rows;
                        dlg.LayoutDeltaX = _SR_DeltaX;
                        dlg.LayoutDeltaY = _SR_DeltaY;
                        int r, c, i;

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            _SR_StartX = dlg.LayoutStartX;
                            _SR_StartY = dlg.LayoutStartY;
                            _SR_Columns = dlg.LayoutColumns;
                            _SR_Rows = dlg.LayoutRows;
                            _SR_DeltaX = dlg.LayoutDeltaX;
                            _SR_DeltaY = dlg.LayoutDeltaY;
                            fl.Positions = dlg.LayoutColumns * dlg.LayoutRows;
                            i = 0;
                            for (r = 0; r < dlg.LayoutRows; r++)
                                for (c = 0; c < dlg.LayoutColumns; c++)
                                {
                                    fl.set_X(i, c * _SR_DeltaX + _SR_StartX);
                                    fl.set_Y(i, r * _SR_DeltaY + _SR_StartY);
                                    fl.set_Enabled(i, true);
                                    i++;
                                }
                            UpdateData();
                        }
                    }
                }
            }
			UpdateData();
		}

        private void tbSurface_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == CmdAddSurface)
            {
                Surfaces.AddSurface();
                Surfaces.ListIndex = Surfaces.ListCount - 1;
                PropSurfaces.PropSrc = Surfaces.get_Data(Surfaces.ListIndex);
            }
            else if (e.ClickedItem == CmdRemoveSurface)
            {
                Surfaces.RemoveSurface();
                Surfaces.ListIndex = Surfaces.ListCount - 1;
                if (Surfaces.ListCount > 0)
                    PropSurfaces.PropSrc = Surfaces.get_Data(Surfaces.ListIndex);
                else
                    PropSurfaces.Clear();
            }
            UpdateData();
        }

		private void tbObjects_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			FlightObject obj = new FlightObject();
			bool b = false;

			if (e.ClickedItem == CmdAddBitmap)
				b = obj.CreateBitmap("");
			else if (e.ClickedItem == CmdAddStruct)
				b = obj.CreateStruct("", 0);
			else if (e.ClickedItem == CmdAddText)
				b = obj.CreateText("Caption");
            else if (e.ClickedItem == CmdAdd3DStruct)
                b = obj.CreateStruct3D("");
            else if (e.ClickedItem == CmdAdd3DScript)
                b = obj.CreateScript3D("", 0);
            else if (e.ClickedItem == CmdAddCircularText)
				b = obj.CreateCircularText("Caption");
			else if (e.ClickedItem == CmdAddEvent)
                b = obj.CreateAutomationEvent(FlightObjectEventType.EventSetIO);
			else if (e.ClickedItem == CmdAddGroup)
				b = obj.CreateGroup();
			else if (e.ClickedItem == CmdAddDrawing)
				b = obj.CreateEmptyStruct();
            else if (e.ClickedItem == CmdAddWorkpiece)
                b = obj.CreateWorkpiece("");
            else if (e.ClickedItem == CmdRemoveObject)
			{
				Tree.RemoveObject(Tree.GetSelectedItem());
				obj = Tree.GetFlightObject(Tree.GetSelectedItem());
				if (obj != null)
					PropTree.PropSrc = obj;
				else
					PropTree.Clear();
				View.Select(obj);
			}

			if (b)
			{
				Tree.SelectItem(Tree.InsertObject(obj, FlightTreeViewItem.Root, FlightTreeViewItem.Last));
				// open editor when new drawing is added
				if (e.ClickedItem == CmdAddDrawing) ShowEditor(obj);
			}
			else if (e.ClickedItem == CmdCopy)
				DuplicateSelectedItem();
			UpdateData();
		}

		private void Colors_ClickEvent(object sender, EventArgs e)
		{
			PropColors.PropSrc = Colors.get_Data(Colors.ListIndex);
		}

		private void Pens_ClickEvent(object sender, EventArgs e)
		{
			PropPens.PropSrc = Pens.get_Data(Pens.ListIndex);
		}

		private void Fonts_ClickEvent(object sender, EventArgs e)
		{
			PropFonts.PropSrc = Fonts.get_Data(Fonts.ListIndex);
		}

		private void Layouts_ClickEvent(object sender, EventArgs e)
		{
			PropLayouts.PropSrc = Layouts.get_Data(Layouts.ListIndex);
		}

        private void Surfaces_ClickEvent(object sender, EventArgs e)
        {
            PropSurfaces.PropSrc = Surfaces.get_Data(Surfaces.ListIndex);
        }

     	private void Flight_Validating(object sender, CancelEventArgs e)
		{
			UpdateData();
		}

		private void Flight_PropertyChanged(object sender, EventArgs e)
		{
			UpdateData();
		}

		private void Tree_Change(object sender, EventArgs e)
		{
			FlightObject obj = Tree.GetFlightObject(Tree.GetSelectedItem());
			if (obj != null)
				PropTree.PropSrc = obj;
			else
				PropTree.Clear();
			View.Select(obj);
            View3D.Select(obj);
            Editor.Edit(obj);
		}

		private void View_ObjectClick(object sender, AxFlightLib._DFlightViewEvents_ObjectClickEvent e)
		{
			Tree.SelectItem(Tree.FindFlightObject(e.flightObject));
		}

		private void View_MouseMoveEvent(object sender, AxFlightLib._DFlightViewEvents_MouseMoveEvent e)
		{
			LabelCoX.Text = "X: " + View.ClientToDocX((short)e.x).ToString("0.0");
			LabelCoY.Text = "Y: " + View.ClientToDocY((short)e.y).ToString("0.0");
		}

		private void FlightMarker_MarkingFinished(object sender, AxFlightLib._DFlightMarkerEvents_MarkingFinishedEvent e)
		{
			FlightMarker.TargetFileName = "";
            UpdateStopWatchInfo();
            _Stopwatch.Stop(); 
            _Marking = false;
            if (e.error == FlightError.ErrInterlock)
            {
                _Interlocked = true;
                lblTable.Text = "Error Interlock";
            }
            UpdateUI();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
            Timer.Enabled = false;
            if (!_Connected) return;
            FlightMarker.GetIO(ref _StateIO);

            if (_Stopwatch.IsRunning) UpdateStopWatchInfo();
            UpdateTablePositions();
            UpdateScannerPositions();

            int msk;
            msk = GetMaskValue(Settings.Default.InputLimitSwitchNegX);
            lbLS1X.ImageIndex = (_Connected && msk > 0 && (_StateIO & msk) != msk) ? 2 : 0;
            msk = GetMaskValue(Settings.Default.InputLimitSwitchPosX);
            lbLS2X.ImageIndex = (_Connected && msk > 0 && (_StateIO & msk) != msk) ? 2 : 0;
            msk = GetMaskValue(Settings.Default.InputLimitSwitchNegY);
            lbLS1Y.ImageIndex = (_Connected && msk > 0 && (_StateIO & msk) != msk) ? 2 : 0;
            msk = GetMaskValue(Settings.Default.InputLimitSwitchPosY);
            lbLS2Y.ImageIndex = (_Connected && msk > 0 && (_StateIO & msk) != msk) ? 2 : 0;
            msk = GetMaskValue(Settings.Default.InputLimitSwitchNegZ);
            lbLS1Z.ImageIndex = (_Connected && msk > 0 && (_StateIO & msk) != msk) ? 2 : 0;
            msk = GetMaskValue(Settings.Default.InputLimitSwitchPosZ);
            lbLS2Z.ImageIndex = (_Connected && msk > 0 && (_StateIO & msk) != msk) ? 2 : 0;
            Timer.Enabled = true;
		}

        public void UpdateTablePositions()
        {
            double a, b, c;
            _TableX = 0;
            _TableY = 0;
            _TableZ = 0;
            a = 0;
            b = 0;
            c = 0;
            FlightMarker.GetTablePosition(ref _TableX, ref _TableY, ref _TableZ, ref a, ref b, ref c);
            lblTableX.Text = "TX: " + _TableX.ToString("0.000");
            lblTableY.Text = "TY: " + _TableY.ToString("0.000");
            lblTableZ.Text = "TZ: " + _TableZ.ToString("0.000");          
        }

        private void UpdateScannerPositions()
        {
            double x, y, z;
            int state;
            x = 0;
            y = 0;
            z = 0;
            state = 0;
            FlightMarker.GetScannerPosition(ref x, ref y, ref z, ref state);
            lblScannerX.Text = "SX: " + x.ToString("0.000");
            lblScannerY.Text = "SY: " + y.ToString("0.000");
            lblScannerZ.Text = "SZ: " + z.ToString("0.000");
        }


		private void menuMain_Click(object sender, EventArgs e)
		{
			View.Focus();
            if (sender == menuNew || sender == tbNew)
                ObjectsNew();
            else if (sender == menuOpen || sender == tbOpen)
                ObjectsLoad();
            else if (sender == menuSave || sender == tbSave)
                ObjectsSave();
            else if (sender == menuSaveAs)
                ObjectsSaveAs();
            else if (sender == menuExit)
                Close();
            else if (sender == menuSelect || sender == tbSelect)
                SetViewMode(FlightViewMode.Select);
            else if (sender == menuPan || sender == tbPan)
                SetViewMode(FlightViewMode.Pan);
            else if (sender == menuZoomIn || sender == tbZoomIn || sender == tbZoom)
                SetViewMode(FlightViewMode.ZoomIn);
            else if (sender == menuZoomOut || sender == tbZoomOut)
                SetViewMode(FlightViewMode.ZoomOut);
            else if (sender == menuZoomOut || sender == tbRotate)
                SetViewMode(FlightViewMode.Rotate);
            else if (sender == tbGrid)
                View3D.ShowGrid = !View3D.ShowGrid;
            else if (sender == tbFrontView)
                SetView3D(1);
            else if (sender == tbSideView)
                SetView3D(2);
            else if (sender == tbTopView)
                SetView3D(3);
            else if (sender == tbViewSelection)
                SetView3D(-1);
            else if (sender == menuToolbar)
                tbDocument.Visible = menuToolbar.Checked;
            else if (sender == menuStatusBar)
                StatusBar.Visible = menuStatusBar.Checked;
            else if (sender == menuSelect || sender == tbSelect)
                SetViewMode(FlightViewMode.Select);
            else if (sender == menuPan || sender == tbPan)
                SetViewMode(FlightViewMode.Pan);
            else if (sender == menuZoomIn || sender == tbZoomIn)
                SetViewMode(FlightViewMode.ZoomIn);
            else if (sender == menuZoomOut || sender == tbZoomOut)
                SetViewMode(FlightViewMode.ZoomOut);
            else if (sender == menuEdit || sender == tbEdit)
                SetViewMode(8);
            else if (sender == menuRefresh || sender == tbRefresh)
                OnRefresh();
            else if (sender == menuSettingsEdit || sender == tbSettings)
                OnSettings();
            else if (sender == menuPropertiesEdit || sender == tbProperties)
                OnProperties();
            else if (sender == tbEditor)
                ShowEditor(Tree.GetFlightObject(Tree.GetSelectedItem()));
            else if (sender == tbMark)
                OnMark(0);
            else if (sender == tbInfinite)
                OnMark(1);
            else if (sender == tbUpload)
                OnMark(2);
            else if (sender == tbSimulate)
                OnSimulate();
            else if (sender == tbAbort)
                OnAbort();
            else if (sender == tbFlashExplorer)
                OnFlashExplorer();
            else if (sender == tbCalibrate)
                OnCalibrate();
            else if (sender == menuSplit)
                OnCompile();
            else if (sender == menuRestore)
                OnDestroyCompilation();
            else if (sender == menuAbout)
                using (AboutFlight dlg = new AboutFlight()) dlg.ShowDialog();

		}

        public void StoreBackup()
        {
            m_CopyColors = Colors.GetXML();
            m_CopyPens = Pens.GetXML();
            m_CopyFonts = Fonts.GetXML();
            m_CopyLayouts = Layouts.GetXML();
            m_CopyTree = Tree.GetXML();
        }

        public void RestoreBackup()
        {
            Colors.LoadXML(m_CopyColors);
            Colors.ListIndex = 0;
            Pens.LoadXML(m_CopyPens);
            Pens.ListIndex = 0;
            Fonts.LoadXML(m_CopyFonts);
            Fonts.ListIndex = 0;
            Layouts.LoadXML(m_CopyLayouts);
            Layouts.ListIndex = 0;
            Tree.LoadXML(m_CopyTree);
        }

        private void OnCompile()
        {
            StoreBackup();
            FlightMarker.Fit();
            FlightMarker.UpdateData();
        }

        private void OnDestroyCompilation()
        {
            RestoreBackup();
            UpdateData();
        }


		private void OnAbort()
		{
			if (_Marking == true)
				FlightMarker.Abort();
		}

		private void OnFlashExplorer()
		{
			if (FlightMarker.Attach(_Link) == -1)
			{
				using (FlashExplorer fe = new FlashExplorer(ref _Link))
					fe.ShowDialog();
				FlightMarker.Detach(_Link);
			}
		}

        private void OnSimulate()
        {
            if (_Marking == true) return;
            FlightMarker.TargetFileName = "";
            FlightMarker.Loop = 0;

            int res = FlightMarker.Simulate(0);
            if (res == -1)
            {
                _Marking = true;
                _Stopwatch.Restart();
            }
        }


		private void OnMark(int Type)
		{
            LoadProperties();
			if (_Marking == true) return;
			string s = "Job1";
			lblTime.Text = "";
			FlightMarker.TargetFileName = "";
			FlightMarker.Loop = 1;

            if (_InterlockMask > 0)
            {
                FlightMarker.SetWhileIO(_InterlockMask, _InterlockMask);
            }

            if (Type == 1)
			{
				FlightMarker.Loop = 0;
			}
			else if (Type == 2)
			{
				FlightMarker.TargetFileName = s;
				if (InputBox("Enter Flash Job Name", "Upload Flash Job", ref s) == DialogResult.OK)
					FlightMarker.TargetFileName = s;
				if (FlightMarker.TargetFileName == "")
					FlightMarker.TargetFileName = "Job1";
			}

			int res = FlightMarker.Mark();
			if (res != -1)
			{
				MessageBox.Show("Can't run job! [" + res + "]", "Hardware error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				FlightMarker.TargetFileName = "";
			}
			else
			{
				_Marking = true;
                _Stopwatch.Restart();
			}
			UpdateUI();
		}

		private void OnRefresh()
		{
			UpdateData();
			PropTree.Refresh();
		}

		private void OnSettings()
		{
			if (!_Connected)
				using (SettingDlg dlg = new SettingDlg())
				{
					if (dlg.ShowDialog() == DialogResult.OK)
					{
						dlg.Apply();
						LoadSettings();
                        UpdateUI();
					}
				}
		}

        private void OnProperties()
        {
            if (!_Marking)
                using (PropertiesDlg dlg = new PropertiesDlg())
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        dlg.Apply();
                        LoadProperties();                  
                    }
                }
        }

		private void DuplicateSelectedItem()
		{
			FlightObject Obj = new FlightObject();
#if WIN64
            ulong hParent, hItem;
#else
            uint hParent, hItem;
#endif

            hItem = Tree.GetSelectedItem();
			if (hItem != 0)
			{
				hParent = Tree.GetParentItem(hItem);
				if (hParent == 0) hParent = FlightTreeViewItem.Root;
				Obj.CreateCopy(Tree.GetFlightObject(hItem));
				Tree.InsertObject(Obj, hParent, hItem);
				UpdateData();
			}
		}
		

		private void ShowEditor(FlightObject Obj)
		{
			Editor.Edit(Obj);
			Editor.ShowEditor();
		}

        private void HomingAxisX(double speed)
        {
            lblTable.Text = "Homing X";
            _TableCommand = true; // lock UI
            if (Settings.Default.HomingMethodX > 1)
            {
                if (Settings.Default.HomingPolarityX > 0) speed = -speed;
                if (Settings.Default.InputHomeX > 0) // reference to switch
                {
                    FlightMarker.JogTableAxisX(speed, Settings.Default.InputHomeX, 4 * Settings.Default.TableTrackDelayX);
                    while (_TableCommand == true) Application.DoEvents();
                }
            }
            else if (Settings.Default.HomingMethodX > 0 && Settings.Default.InputHomeX > 0)
            {
                while ((_StateIO & GetMaskValue(Settings.Default.InputHomeX)) == 0) Application.DoEvents();
            }
            _TableCommand = false; // unlock UI
            FlightMarker.SetTablePositionX(Settings.Default.HomingPositionX);
            if (Settings.Default.EnableEncoderX)
            {
                FlightMarker.InitResolverX(Settings.Default.StepSizeEncoderX, 0);
                FlightMarker.SetResolverPositionX(Settings.Default.HomingPositionX);
            }
        }

        private void HomingAxisY(double speed)
        {
            lblTable.Text = "Homing Y";
            _TableCommand = true; // lock UI
            if (Settings.Default.HomingMethodY > 1)
            {
                if (Settings.Default.HomingPolarityY > 0) speed = -speed;
                if (Settings.Default.InputHomeY > 0) // reference to switch
                {
                    FlightMarker.JogTableAxisY(speed, Settings.Default.InputHomeY, 4 * Settings.Default.TableTrackDelayY);
                    while (_TableCommand == true) Application.DoEvents();
                }
            }
            else if (Settings.Default.HomingMethodY > 0 && Settings.Default.InputHomeY > 0)
            {
                while ((_StateIO & GetMaskValue(Settings.Default.InputHomeY)) == 0) Application.DoEvents();
            }
            _TableCommand = false; // unlock UI
            FlightMarker.SetTablePositionY(Settings.Default.HomingPositionY);
            if (Settings.Default.EnableEncoderY)
            {
                FlightMarker.InitResolverY(Settings.Default.StepSizeEncoderY, 0);
                FlightMarker.SetResolverPositionY(Settings.Default.HomingPositionY);
            }
        }

        private void HomingAxisZ(double speed)
        {
            lblTable.Text = "Homing Z";
            _TableCommand = true; // lock UI
            if (Settings.Default.HomingMethodZ > 1)
            {
                if (Settings.Default.HomingPolarityZ > 0) speed = -speed;
                if (Settings.Default.InputHomeZ > 0) // reference to switch
                {
                    FlightMarker.JogTableAxisZ(speed, Settings.Default.InputHomeZ, 4 * Settings.Default.TableTrackDelayZ);
                    while (_TableCommand == true) Application.DoEvents();
                }
            }
            else if (Settings.Default.HomingMethodZ > 0 && Settings.Default.InputHomeZ > 0)
            {
                while ((_StateIO & GetMaskValue(Settings.Default.InputHomeZ)) == 0) Application.DoEvents();
            }
            _TableCommand = false; // unlock UI
            FlightMarker.SetTablePositionZ(Settings.Default.HomingPositionZ);
        }



		private void OnBtnConnect_Click(object sender, EventArgs e)
		{ 
            int mem = 0;

            btnConnect.Enabled = false;
			if (!_Marking && !_Connected)
			{
				FlightMarker.Address = Settings.Default.Address;
                _Connected = (FlightMarker.SetWhileIO(0, 0) == FlightError.ErrOK);
                System.Threading.Thread.Sleep(100);
                FlightMarker.JumpTo(0, 0, 0);

                if (_Connected)
                {
                    _InterlockMask = 0;
                    _Interlocked = false;
                    if (FlightMarker.GetStatus(ref mem) == FlightError.ErrInterlock)
                    {
                        FlightMarker.Abort();
                        System.Threading.Thread.Sleep(100);
                    }
                    FlightMarker.SetMatrix(
                        Math.Cos(Settings.Default.FieldRotation * Math.PI / 180.0),
                        -Math.Sin(Settings.Default.FieldRotation * Math.PI / 180.0),
                        Math.Sin(Settings.Default.FieldRotation * Math.PI / 180.0),
                        Math.Cos(Settings.Default.FieldRotation * Math.PI / 180.0));
                }

                if (_Connected &&
                    FlightMarker.InitTableAxisX(1, 1, Settings.Default.EnableAxisX ? Settings.Default.TableTrackDelayX : 0, Settings.Default.StepSizeAxisX,
                                                Settings.Default.TableMaxSpeedX, Settings.Default.TablePositionDelayX) != -1)
                    _Connected = false;
                if (_Connected && Settings.Default.EnableAxisY &&
                    FlightMarker.InitTableAxisY(1, 2, Settings.Default.EnableAxisY ? Settings.Default.TableTrackDelayY : 0, Settings.Default.StepSizeAxisY,
                                                Settings.Default.TableMaxSpeedX, Settings.Default.TablePositionDelayY) != -1)
                    _Connected = false;
                if (_Connected && Settings.Default.EnableAxisZ &&
                    FlightMarker.InitTableAxisZ(1, 3, Settings.Default.EnableAxisZ ? Settings.Default.TableTrackDelayZ : 0, Settings.Default.StepSizeAxisZ,
                                                Settings.Default.TableMaxSpeedZ, Settings.Default.TablePositionDelayZ) != -1)
                    _Connected = false;
                if (_Connected)
                {
                    double x, y, z;
                    Timer.Enabled = true;

                    x = Settings.Default.HomingPolarityX > 0 ? Settings.Default.TableTravelMinX : Settings.Default.TableTravelMaxX;
                    y = Settings.Default.HomingPolarityY > 0 ? Settings.Default.TableTravelMinY : Settings.Default.TableTravelMaxY;
                    z = Settings.Default.HomingPolarityZ > 0 ? Settings.Default.TableTravelMinZ : Settings.Default.TableTravelMaxZ;
                    if (Settings.Default.EnableAxisX) FlightMarker.SetTableLimitSwitchesX(Settings.Default.InputLimitSwitchNegX, Settings.Default.InputLimitSwitchPosX);
                    if (Settings.Default.EnableAxisY) FlightMarker.SetTableLimitSwitchesY(Settings.Default.InputLimitSwitchNegY, Settings.Default.InputLimitSwitchPosY);
                    if (Settings.Default.EnableAxisZ) FlightMarker.SetTableLimitSwitchesZ(Settings.Default.InputLimitSwitchNegZ, Settings.Default.InputLimitSwitchPosZ);

                    for (int r = 0; r < 2; r++)
                    {
                        if (r == 0 || (Settings.Default.HomingMethodX == 3 ||
                                       Settings.Default.HomingMethodY == 3 ||
                                       Settings.Default.HomingMethodZ == 3))
                        {
                            if (Settings.Default.EnableAxisZ) HomingAxisZ(Settings.Default.HomingSpeedZ / (9 * r + 1));
                            if (Settings.Default.EnableAxisY) HomingAxisY(Settings.Default.HomingSpeedY / (9 * r + 1));
                            if (Settings.Default.EnableAxisX) HomingAxisX(Settings.Default.HomingSpeedX / (9 * r + 1));
                        }
                        _TableCommand = true;
                        FlightMarker.TableJumpTo(x, y, z, 0, 0, 0, false);
                        while (_TableCommand == true) Application.DoEvents();
                    }

                    lblTable.Text = " ";

                    _InterlockMask = GetMaskValue(Settings.Default.InputInterlock);
                    if (Settings.Default.EStopOnLmtSwitchX)
                    {
                        _InterlockMask |= GetMaskValue(Settings.Default.InputLimitSwitchNegX);
                        _InterlockMask |= GetMaskValue(Settings.Default.InputLimitSwitchPosX);
                    }
                    if (Settings.Default.EStopOnLmtSwitchY)
                    {
                        _InterlockMask |= GetMaskValue(Settings.Default.InputLimitSwitchNegY);
                        _InterlockMask |= GetMaskValue(Settings.Default.InputLimitSwitchPosY);
                    }
                    if (Settings.Default.EStopOnLmtSwitchZ)
                    {
                        _InterlockMask |= GetMaskValue(Settings.Default.InputLimitSwitchNegZ);
                        _InterlockMask |= GetMaskValue(Settings.Default.InputLimitSwitchPosZ);
                    }
                    if (_InterlockMask > 0)
                    {
                        FlightMarker.SetWhileIO(_InterlockMask, _InterlockMask);
                        System.Threading.Thread.Sleep(100);
                        if (FlightMarker.GetStatus(ref mem) == FlightError.ErrInterlock)
                        {
                            _Interlocked = true;
                            lblTable.Text = "Error Interlock";
                            _Connected = false;
                        }
                    }
                }
                else
                {
                    FlightMarker.Address = "";
                    MessageBox.Show("Can't start hardware!", "Hardware error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
			}
			else if (!_Marking)
			{
				FlightMarker.Address = "";
				_Connected = false;
			}
            Timer.Enabled = _Connected;
            UpdateUI();
		}

        private int GetMaskValue(int Nr)
        {
            int i, val = 0;
            
            if (Nr>0) for (val = 1, i = 1; i < Nr; i++) val *= 2;
            return val;
        }
 
		private void UpdateData()
		{
			int mode = View.ViewMode;
            View.ViewMode = FlightViewMode.Static;
			this.Cursor = Cursors.WaitCursor;
			FlightMarker.UpdateData();
			View.ViewMode = mode;
			this.Cursor = Cursors.Default;
		}

		private void UpdateDocTitle()
		{
			string s = (_FileName != "") ? _FileName.Substring(_FileName.LastIndexOf("\\") + 1) : "Untitled";
			this.Text = s + " - Flight marker";
		}

		private void UpdateUI()
		{
			btnConnect.Text = _Connected ? "DISCONNECT" : "CONNECT";
			btnConnect.Enabled = !_Marking;
            gpTable.Enabled = _Connected && !_Marking;
			tbMark.Enabled = _Connected && !_Marking && !_TableCommand;
            tbSimulate.Enabled = _Connected && !_Marking && !_TableCommand;
            tbInfinite.Enabled = _Connected && !_Marking && !_TableCommand;
            tbUpload.Enabled = _Connected && !_Marking && !_TableCommand;
            tbFlashExplorer.Enabled = _Connected && !_Marking && !_TableCommand;
			tbAbort.Enabled = _Connected;
            lblCua.ImageIndex = _Connected ? (_Interlocked ? 2 : 1) : 0;
            lblCua.Text = _Connected ? "ONLINE" : "OFFLINE";
			tbSettings.Enabled = !_Connected;
			menuSettingsEdit.Enabled = tbSettings.Enabled;
            tbProperties.Enabled = !_Marking;
            menuPropertiesEdit.Enabled = tbProperties.Enabled;
            lbTableUnit.Text = (0.001 * (double)_TableUnit).ToString();
            btnTableUnit1.Enabled = _TableUnit > 1;
            btnTableUnit2.Enabled = _TableUnit < 100000;
            EnableTableUI(gpTable.Enabled && !_TableCommand);
            lbLS1X.Visible = Settings.Default.EnableAxisX && Settings.Default.HomingMethodX > 1;
            lbLS2X.Visible = lbLS1X.Visible;
            lbLS1Y.Visible = Settings.Default.EnableAxisY && Settings.Default.HomingMethodY > 1;
            lbLS2Y.Visible = lbLS1Y.Visible;
            lbLS1Z.Visible = Settings.Default.EnableAxisZ && Settings.Default.HomingMethodZ > 1;
            lbLS2Z.Visible = lbLS1Z.Visible;
        }

        private void EnableTableUI(bool Enable)
        {
            lblTableX.Enabled = Enable;
            lblTableY.Enabled = Enable;
            lblTableZ.Enabled = Enable;
            btnTableGoTo.Enabled = Enable;
            btnTableX1.Enabled = Enable;
            btnTableY1.Enabled = Enable;
            btnTableZ1.Enabled = Enable;
            btnTableX2.Enabled = Enable;
            btnTableY2.Enabled = Enable;
            btnTableZ2.Enabled = Enable;
            btnTableHome.Enabled = Enable;
        }
         
        private void btnTableUnit1_Click(object sender, EventArgs e)
        {
            if (_TableUnit > 1) _TableUnit = _TableUnit / 10;
            UpdateUI();
        }

        private void btnTableUnit2_Click(object sender, EventArgs e)
        {
            if (_TableUnit < 100000) _TableUnit = _TableUnit * 10;
            UpdateUI();
        }

        private void btnTableGoTo_Click(object sender, EventArgs e)
        {
            EnableTableUI(false);
            _TableCommand = true;
            FlightMarker.TableJumpTo(double.Parse(tbTableX.Text), double.Parse(tbTableY.Text), double.Parse(tbTableZ.Text), 0, 0, 0, false);
            while (_TableCommand == true) Application.DoEvents();
            UpdateUI();
        }

        private void btnTableX1_Click(object sender, EventArgs e)
        {
            EnableTableUI(false);
            UpdateTablePositions();
            _TableCommand = true;
            FlightMarker.TableJumpTo(_TableX - 0.001 * _TableUnit, _TableY, _TableZ, 0, 0, 0, false);
            while (_TableCommand == true) Application.DoEvents();
            UpdateUI();
        }

        private void btnTableX2_Click(object sender, EventArgs e)
        {
            EnableTableUI(false);
            UpdateTablePositions();
            _TableCommand = true;
            FlightMarker.TableJumpTo(_TableX + 0.001 * _TableUnit, _TableY, _TableZ, 0, 0, 0, false);
            while (_TableCommand == true) Application.DoEvents();
            UpdateUI();
        }

        private void btnTableY1_Click(object sender, EventArgs e)
        {
            EnableTableUI(false);
            UpdateTablePositions();
            _TableCommand = true;
            FlightMarker.TableJumpTo(_TableX, _TableY + 0.001 * _TableUnit, _TableZ, 0, 0, 0, false);
            while (_TableCommand == true) Application.DoEvents();
            UpdateUI();
        }

        private void btnTableY2_Click(object sender, EventArgs e)
        {
            EnableTableUI(false);
            UpdateTablePositions();
            _TableCommand = true;
            FlightMarker.TableJumpTo(_TableX, _TableY - 0.001 * _TableUnit, _TableZ, 0, 0, 0, false);
            while (_TableCommand == true) Application.DoEvents();
            UpdateUI();
        }

        private void btnTableZ1_Click(object sender, EventArgs e)
        {
            EnableTableUI(false);
            UpdateTablePositions();
            _TableCommand = true;
            FlightMarker.TableJumpTo(_TableX, _TableY, _TableZ + 0.001 * _TableUnit, 0, 0, 0, false);
            while (_TableCommand == true) Application.DoEvents();
            UpdateUI();
        }

        private void btnTableZ2_Click(object sender, EventArgs e)
        {
            EnableTableUI(false);
            UpdateTablePositions();
            _TableCommand = true;
            FlightMarker.TableJumpTo(_TableX, _TableY, _TableZ - 0.001 * _TableUnit, 0, 0, 0, false);
            while (_TableCommand == true) Application.DoEvents();
            UpdateUI();
        }

        private void btnTableHome_Click(object sender, EventArgs e)
        {
            EnableTableUI(false);
            double x, y, z;
            x = Settings.Default.HomingPolarityX > 0 ? Settings.Default.TableTravelMinX : Settings.Default.TableTravelMaxX;
            y = Settings.Default.HomingPolarityY >0 ? Settings.Default.TableTravelMinY : Settings.Default.TableTravelMaxY;
            z = Settings.Default.HomingPolarityZ >0 ? Settings.Default.TableTravelMinZ : Settings.Default.TableTravelMaxZ;
            _TableCommand = true;
            FlightMarker.TableJumpTo(x, y, z, 0, 0, 0, false);
            while (_TableCommand == true) Application.DoEvents();
            UpdateUI();
        }

        private void FlightMarker_ProcessFinished(object sender, AxFlightLib._DFlightMarkerEvents_ProcessFinishedEvent e)
        {
            _TableCommand = false;
            lblTable.Text = "";
        }

        private void View_ObjectChanged(object sender, AxFlightLib._DFlightViewEvents_ObjectChangedEvent e)
        {
            UpdateData();
        }

        private void UpdateStopWatchInfo()
        {
            double seconds = _Stopwatch.Elapsed.TotalSeconds;
            if (seconds >= 60.0)
            {
                int minutes = (int)Math.Floor(seconds / 60);
                seconds = seconds - minutes * 60.0;
                lblTime.Text = $"T: {minutes}m {seconds:0.0}s";
            }
            else
            {
                lblTime.Text = $"T: {seconds:0.0}s";
            }
        }

        private void FlightMarker_SimulationFinished(object sender, AxFlightLib._DFlightMarkerEvents_SimulationFinishedEvent e)
        {
            FlightMarker.TargetFileName = "";
            UpdateStopWatchInfo();
            _Stopwatch.Stop();
            _Marking = false;
            UpdateUI();
        }

        private void View3D_ObjectClick(object sender, AxFlightLib._DFlightView3DEvents_ObjectClickEvent e)
        {
            Tree.SelectItem(Tree.FindFlightObject(e.flightObject));
        }

        private void SetView3D(int Axis)
        {
            if (Axis == -1 && Tree.GetSelectedItem() != 0 && Tree.GetFlightObject(Tree.GetSelectedItem()).Type != FlightObjectType.Event)
            {
                FlightObject obj = Tree.GetFlightObject(Tree.GetSelectedItem());
                View3D.SetView(0, 0, 0);
                View3D.ViewportPivotpointX = obj.X;
                View3D.ViewportPivotpointY = obj.Y;
                View3D.ViewportPivotpointZ = obj.Z;
                View3D.ViewportZoom = 2;
            }
            else
            {
                if (Axis == 1)
                    View3D.SetView(-90, 0, 0);
                else if (Axis == 2)
                    View3D.SetView(-90, 0, -90);
                else
                    View3D.SetView(0, 0, 0);
                View3D.ViewportPivotpointX = 0;
                View3D.ViewportPivotpointY = 0;
                View3D.ViewportPivotpointZ = 0;
                View3D.ViewportZoom = 1;
            }
        }

        private void SetViewMode(int Mode)
        {
            menuSelect.Checked = tbSelect.Checked = (Mode == FlightViewMode.Select);
            menuPan.Checked = tbPan.Checked = (Mode == FlightViewMode.Pan);
            menuZoomIn.Checked = tbZoomIn.Checked = (Mode == FlightViewMode.ZoomIn);
            menuZoomOut.Checked = tbZoomOut.Checked = (Mode == FlightViewMode.ZoomOut);
            menuEdit.Checked = tbEdit.Checked = (Mode == 8);
            tbRotate.Checked = (Mode == FlightViewMode.Rotate);
            tbZoom.Checked = (Mode == FlightViewMode.ZoomIn);

            if (tabViews.SelectedIndex == 0)
            {
                View.ViewMode = Mode;
                View.Refresh();
                menuZoomIn.Visible = tbZoomIn.Visible = true;
                menuZoomOut.Visible = tbZoomOut.Visible = true;
                tbZoom.Visible = false;
                tbRotate.Visible = false;
                tbTopView.Visible = false;
                tbSideView.Visible = false;
                tbFrontView.Visible = false;
                tbViewSelection.Visible = false;
                tbEdit.Visible = true;
                tbGrid.Visible = false;
            }
            else
            {
                View3D.ViewMode = Mode;
                View3D.Refresh();
                menuZoomIn.Visible = tbZoomIn.Visible = false;
                menuZoomOut.Visible = tbZoomOut.Visible = false;
                tbZoom.Visible = true;
                tbRotate.Visible = true;
                tbTopView.Visible = true;
                tbSideView.Visible = true;
                tbFrontView.Visible = true;
                tbViewSelection.Visible = true;
                tbEdit.Visible = false;
                tbGrid.Visible = true;
            }
        }

        private void tabViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetViewMode(FlightViewMode.Select);
        }

        private void FlightMarker_MillToolStep(object sender, AxFlightLib._DFlightMarkerEvents_MillToolStepEvent e)
        {
            if (Settings.Default.MillOverwrite)
            {
                e.tileWidth = Settings.Default.MillTileWidth;
                e.tileHeight = Settings.Default.MillTileHeight;
                e.tableSnapX = Settings.Default.MillTableSnapX;
                e.tableSnapY = Settings.Default.MillTableSnapY;
                e.tableClipX = Settings.Default.MillTableClipX;
                e.tableClipY = Settings.Default.MillTableClipY;
                e.tableSpeedLimit = Settings.Default.MillTableSpeedLimit;
                e.tableSpeedProfile = Settings.Default.MillTableSpeedProfile;
            }
            lblTool.Text = "Mill D" +  e.millDepth.ToString("0.00") + " > " + e.orientation.ToString("0.0") + "° > F" + e.millFeed.ToString("0.00");
        }

        private void Flight_KeyDown(object sender, KeyEventArgs e)
        {
#if WIN64
            ulong hParent = FlightTreeViewItem.Root;
#else
            uint hParent = FlightTreeViewItem.Root;
#endif
            FlightObject obj;

            if (e.Control && e.KeyCode == Keys.C)
            {
                if (Tree.GetSelectedItem() != 0)
                {
                    obj = Tree.GetFlightObject(Tree.GetSelectedItem());
                    m_ClipBoard.CreateCopy(obj);
                }
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                if (Tree.GetSelectedItem()!=0 && m_ClipBoard.Type>0)
                {
                    obj = Tree.GetFlightObject(Tree.GetSelectedItem());
                    if (obj.Type == FlightObjectType.Group || obj.Type == FlightObjectType.Section)
                        hParent = Tree.GetSelectedItem();
                    obj = new FlightObject();
                    obj.CreateCopy(m_ClipBoard);
                    Tree.InsertObject(obj, hParent, FlightTreeViewItem.Last);
                    UpdateData();
                }
            }
        }

        public void OnBeaconTargetTableJog(double dx, double dy, double dz)
        {
            UpdateTablePositions();
            _TableCommand = true;
            FlightMarker.TableJumpTo(_TableX + dx, _TableY + dy, _TableZ + dz, 0, 0, 0, false);
        }

        public void OnBeaconTargetTableJumpTo(double x, double y, double z)
        {
            UpdateTablePositions();
            _TableCommand = true;
            FlightMarker.TableJumpTo(x, y, z, 0, 0, 0, false);
        }

        private void OnCalibrate()
        {
            if (_Connected == false) return;
            FlightMarker.SelectColor(0);
            using (CalibrationBCU bcu = new CalibrationBCU(Settings.Default.BeaconCOM,
                                                           new FlightBeaconTarget(this),
                                                           Settings.Default.CalibrationColumns,
                                                           Settings.Default.CalibrationRows,
                                                           Settings.Default.CalibrationWidth,
                                                           Settings.Default.CalibrationHeight))
            {
                bcu.ShowDialog();
                Settings.Default.CalibrationColumns = bcu.CalColums;
                Settings.Default.CalibrationRows = bcu.CalRows;
                Settings.Default.CalibrationWidth = bcu.CalWidth;
                Settings.Default.CalibrationHeight = bcu.CalHeight;
                Settings.Default.Save();
            }
        }
    }
}
