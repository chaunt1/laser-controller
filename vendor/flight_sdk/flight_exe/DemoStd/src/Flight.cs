using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FlightLib;
using System.Globalization;
using global::Flight.Properties;
using System.Diagnostics;

namespace Flight
{
	public partial class Flight : Form
	{
		private string _FileName;
		private bool _Connected;
		private bool _Marking;
  		private rhothorLink _Link = new rhothorLink();
		private Stopwatch _Stopwatch = new Stopwatch();

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

			// hardware
			FlightMarker.FieldSize = Settings.Default.FieldSize;
			FlightMarker.Address = Settings.Default.Address;
			FlightMarker.Resolution = Settings.Default.Resolution;
            FlightMarker.TableAxisX = false;
            FlightMarker.TableAxisY = false;
            FlightMarker.TableAxisZ = false;
            FlightMarker.TableMode = FlightMarkerTableMode.SetTable;
            FlightMarker.TableOptimize = 0;
			FlightMarker.UpdateData();
            PropTree.Refresh();
            InitLaserParams();

            Fonts.FontHeightDefinition = 1;
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
			_Marking = false;
			_FileName = "";

			ObjectsInit();
			this.Height = 650;
			LoadSettings();
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

        private void InitLaserParams()
        {
            Colors.LaserEvent = FlightColorsLaserEvent.Never;
            Colors.PowerDutyCycle0 = Settings.Default.PowerDC0;
            Colors.PowerDutyCycle100 = Settings.Default.PowerDC100;
            Colors.PowerPulseRate = Settings.Default.PowerPulseRate;
            Colors.PowerMode = Settings.Default.PowerMode;
            Colors.GrayscaleMode = FlightColorsGrayscaleMode.PowerModulation;
			Colors.PowerOffOnIdle = Settings.Default.PowerOffOnIdle;
		}

		private void ObjectsInit()
		{
            // global laser parameters
            Colors.Speed = 1000;
            Colors.JumpSpeed = 1000;
            Colors.QswPulseRate = 50000;
            Colors.QswPulseWidth = 10;
            Colors.BurstRate = 50000;
            Colors.BurstTime = 15;
            InitLaserParams();
            
            Colors.AddColor();
            Colors.Height = 200;
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
					Layouts.ListIndex = 0;
					PropLayouts.PropSrc = Layouts.get_Data(0);
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
            else if (e.ClickedItem == CmdAddCircularText)
                b = obj.CreateCircularText("Caption");
            else if (e.ClickedItem == CmdAdd3DStruct)
                b = obj.CreateStruct3D("");
            else if (e.ClickedItem == CmdAdd3DScript)
                b = obj.CreateScript3D("", 0);
            else if (e.ClickedItem == CmdAddEvent)
                b = obj.CreateAutomationEvent(FlightObjectEventType.EventSetIO);
            else if (e.ClickedItem == CmdAddGroup)
                b = obj.CreateGroup();
            else if (e.ClickedItem == CmdAddDrawing)
                b = obj.CreateEmptyStruct();
            else if (e.ClickedItem == CmdAddSection)
            {
                b = obj.CreateSection(FlightObjectSectionType.SectionSetLoop);
                obj.Counter = 0;
            }
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
			UpdateUI();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
            Timer.Enabled = false;
			if (_Stopwatch.IsRunning) UpdateStopWatchInfo();
            Timer.Enabled = _Connected;
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
            else if (sender == menuToolbar)
                tbDocument.Visible = menuToolbar.Checked;
            else if (sender == menuStatusBar)
                StatusBar.Visible = menuStatusBar.Checked;
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
            else if (sender == menuEdit || sender == tbEdit)
                SetViewMode(8);
            else if (sender == menuRefresh || sender == tbRefresh)
                OnRefresh();
            else if (sender == menuSettingsEdit || sender == tbSettings)
                OnSettings();
            else if (sender == tbEditor)
                ShowEditor(Tree.GetFlightObject(Tree.GetSelectedItem()));
            else if (sender == tbMark)
                OnMark(0);
            else if (sender == tbInfinite)
                OnMark(1);
            else if (sender == tbUpload)
                OnMark(2);
            else if (sender == tbSimulate)
                OnMark(3);
            else if (sender == tbAbort)
                OnAbort();
            else if (sender == tbCalibrate)
                OnCalibrate();
            else if (sender == tbFlashExplorer)
                OnFlashExplorer();
            else if (sender == menuAbout)
                using (AboutFlight dlg = new AboutFlight()) dlg.ShowDialog();
		}

        private void OnCalibrate()
        {
            // use color 0 laser parameters
            FlightMarker.SelectColor(0);

            if (FlightMarker.Attach(_Link) == -1)
            {
                using (CalibrationDlg cd = new CalibrationDlg(ref _Link))
                    cd.ShowDialog();
                FlightMarker.Detach(_Link);
            }
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
        
		private void OnMark(int Type)
		{
            Colors.OperationMode = FlightColorsOperationMode.Scanner;
            Pens.TileMode = FlightPensTileMode.InfiniteField;


			if (_Marking == true) return;
			string s = "Job1";
			LabelTime.Text = "";
			FlightMarker.TargetFileName = "";
			FlightMarker.Loop = 1;
            InitLaserParams();
            
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

			int res = Type == 3 ? FlightMarker.Simulate(0): FlightMarker.Mark();
			if (res != -1)
			{
				MessageBox.Show("Can't run job! [" + res + "]", "Hardware error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				FlightMarker.TargetFileName = "";
			}
			else
			{
				_Marking = true;
				_Stopwatch.Restart();
				Timer.Enabled = true;
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
						dlg.ApplySettings();
						LoadSettings();
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

        private void SetView3D(int Axis)
        {
            if (Axis == -1 && Tree.GetSelectedItem()!=0 && Tree.GetFlightObject(Tree.GetSelectedItem()).Type != FlightObjectType.Event)
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
                menuZoomOut.Visible = tbZoomOut.Visible= true;
                menuZoom.Visible = tbZoom.Visible = false;
                menuRotate.Visible = tbRotate.Visible = false;
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
                menuZoom.Visible = tbZoom.Visible = true;
                menuRotate.Visible = tbRotate.Visible = true;
                tbTopView.Visible = true;
                tbSideView.Visible = true;
                tbFrontView.Visible = true;
                tbViewSelection.Visible = true;
                tbEdit.Visible = false;
                tbGrid.Visible = true;
            }
		}

		private void ShowEditor(FlightObject Obj)
		{
			Editor.Edit(Obj);
			Editor.ShowEditor();
		}

		private void OnBtnConnect_Click(object sender, EventArgs e)
		{
            btnConnect.Enabled = false;
			if (!_Marking && !_Connected)
			{
				FlightMarker.Address = Settings.Default.Address;
          
                _Connected = (FlightMarker.SetTarget(1) == -1);
                FlightMarker.SetQueryTarget(1);

                if (_Connected)
                {
                    if (Settings.Default.ResolverX || Settings.Default.ResolverY)
                    {
                        FlightMarker.Attach(_Link);
                        _Link.rtListOpen(1);
                        if (Settings.Default.ResolverX)
                            _Link.rtSetResolver(1, Settings.Default.ResolverStepsizeX, Settings.Default.ResolverRangeX);
                        if (Settings.Default.ResolverY)
                            _Link.rtSetResolver(2, Settings.Default.ResolverStepsizeY, Settings.Default.ResolverRangeY);
                       _Link.rtListClose();
                       FlightMarker.Detach(_Link);
                    }
                    Timer.Enabled = true;
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
			UpdateUI();
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

        private void UpdateStopWatchInfo()
        {
            double seconds = _Stopwatch.Elapsed.TotalSeconds;
            if (seconds >= 60.0)
            {
                int minutes = (int)Math.Floor(seconds / 60);
                seconds = seconds - minutes * 60.0;
                LabelTime.Text = $"T: {minutes}m {seconds:0.0}s";
            }
            else
            {
                LabelTime.Text = $"T: {seconds:0.0}s";
            }
        }

        private void UpdateUI()
		{
			btnConnect.Text = _Connected ? "DISCONNECT" : "CONNECT";
			btnConnect.Enabled = !_Marking;
			tbMark.Enabled = _Connected && !_Marking;
            tbInfinite.Enabled = _Connected && !_Marking;
            tbSimulate.Enabled = _Connected && !_Marking;
            tbUpload.Enabled = _Connected && !_Marking;
            tbCalibrate.Enabled = _Connected && !_Marking;
            tbFlashExplorer.Enabled = _Connected && !_Marking;
			tbAbort.Enabled = _Connected;
            lblCua.ImageIndex = _Connected ? 1 : 0;
			lblCua.Text = _Connected ? "ONLINE" : "OFFLINE";
			tbSettings.Enabled = !_Connected;
			menuSettingsEdit.Enabled = tbSettings.Enabled;
        }

        private void View_ObjectChanged(object sender, AxFlightLib._DFlightViewEvents_ObjectChangedEvent e)
        {
            UpdateData();
        }

        private void View3D_ObjectClick(object sender, AxFlightLib._DFlightView3DEvents_ObjectClickEvent e)
        {
            Tree.SelectItem(Tree.FindFlightObject(e.flightObject));
        }

        private void FlightMarker_SimulationFinished(object sender, AxFlightLib._DFlightMarkerEvents_SimulationFinishedEvent e)
        {
            FlightMarker.TargetFileName = "";
			UpdateStopWatchInfo();
			_Stopwatch.Stop();
			_Marking = false;
            UpdateUI();
        }

        private void tabViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetViewMode(FlightViewMode.Select);
        }
	}

}
