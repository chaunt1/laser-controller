namespace Flight
{
	partial class Flight
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Flight));
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeperator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuToolbar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStatusBar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPan = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuZoomIn = new System.Windows.Forms.ToolStripMenuItem();
            this.menuZoomOut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingsEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPropertiesEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCompile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSplit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.LabelCoX = new System.Windows.Forms.ToolStripStatusLabel();
            this.LabelCoY = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTableX = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTableY = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTableZ = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblScannerX = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblScannerY = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblScannerZ = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTool = new System.Windows.Forms.ToolStripStatusLabel();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.tbDocument = new System.Windows.Forms.ToolStrip();
            this.tbNew = new System.Windows.Forms.ToolStripButton();
            this.tbOpen = new System.Windows.Forms.ToolStripButton();
            this.tbSave = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tbMark = new System.Windows.Forms.ToolStripButton();
            this.tbInfinite = new System.Windows.Forms.ToolStripButton();
            this.tbSimulate = new System.Windows.Forms.ToolStripButton();
            this.tbAbort = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbUpload = new System.Windows.Forms.ToolStripButton();
            this.tbFlashExplorer = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tbSelect = new System.Windows.Forms.ToolStripButton();
            this.tbPan = new System.Windows.Forms.ToolStripButton();
            this.tbRotate = new System.Windows.Forms.ToolStripButton();
            this.tbZoom = new System.Windows.Forms.ToolStripButton();
            this.tbZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tbZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tbViewSelection = new System.Windows.Forms.ToolStripButton();
            this.tbGrid = new System.Windows.Forms.ToolStripButton();
            this.tbTopView = new System.Windows.Forms.ToolStripButton();
            this.tbFrontView = new System.Windows.Forms.ToolStripButton();
            this.tbSideView = new System.Windows.Forms.ToolStripButton();
            this.tbEdit = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tbEditor = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tbCalibrate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tbProperties = new System.Windows.Forms.ToolStripButton();
            this.tbSettings = new System.Windows.Forms.ToolStripButton();
            this.Leds = new System.Windows.Forms.ImageList(this.components);
            this.SplitView = new System.Windows.Forms.SplitContainer();
            this.FlightTab = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.PropColors = new AxFlightLib.AxFlightPropList();
            this.Colors = new AxFlightLib.AxFlightColors();
            this.tbColors = new System.Windows.Forms.ToolStrip();
            this.CmdRemoveColor = new System.Windows.Forms.ToolStripButton();
            this.CmdAddColor = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.CmdChangeColor = new System.Windows.Forms.ToolStripButton();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.tbPens = new System.Windows.Forms.ToolStrip();
            this.CmdRemovePen = new System.Windows.Forms.ToolStripButton();
            this.CmdAddPen = new System.Windows.Forms.ToolStripButton();
            this.PropPens = new AxFlightLib.AxFlightPropList();
            this.Pens = new AxFlightLib.AxFlightPens();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.PropFonts = new AxFlightLib.AxFlightPropList();
            this.tbFonts = new System.Windows.Forms.ToolStrip();
            this.CmdRemoveFont = new System.Windows.Forms.ToolStripButton();
            this.CmdAddTrueTypeFont = new System.Windows.Forms.ToolStripButton();
            this.CmdAddSinglestrokeFont = new System.Windows.Forms.ToolStripButton();
            this.CmdAddBarcodeFont = new System.Windows.Forms.ToolStripButton();
            this.Fonts = new AxFlightLib.AxFlightFonts();
            this.TabPage4 = new System.Windows.Forms.TabPage();
            this.PropLayouts = new AxFlightLib.AxFlightPropList();
            this.tbLayouts = new System.Windows.Forms.ToolStrip();
            this.CmdRemoveLayout = new System.Windows.Forms.ToolStripButton();
            this.CmdEditLayout = new System.Windows.Forms.ToolStripButton();
            this.CmdAddLayout = new System.Windows.Forms.ToolStripButton();
            this.Layouts = new AxFlightLib.AxFlightLayouts();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tbSurface = new System.Windows.Forms.ToolStrip();
            this.CmdAddSurface = new System.Windows.Forms.ToolStripButton();
            this.CmdRemoveSurface = new System.Windows.Forms.ToolStripButton();
            this.PropSurfaces = new AxFlightLib.AxFlightPropList();
            this.Surfaces = new AxFlightLib.AxFlightSurfaces();
            this.splitView2 = new System.Windows.Forms.SplitContainer();
            this.tabViews = new System.Windows.Forms.TabControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.View = new AxFlightLib.AxFlightView();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.View3D = new AxFlightLib.AxFlightView3D();
            this.FlightTab2 = new System.Windows.Forms.TabControl();
            this.tabHardware = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbLS2Z = new System.Windows.Forms.Label();
            this.ImgBars = new System.Windows.Forms.ImageList(this.components);
            this.lblTable = new System.Windows.Forms.Label();
            this.lbLS1Y = new System.Windows.Forms.Label();
            this.lbLS2Y = new System.Windows.Forms.Label();
            this.lbLS1X = new System.Windows.Forms.Label();
            this.lbLS2X = new System.Windows.Forms.Label();
            this.lbLS1Z = new System.Windows.Forms.Label();
            this.gpTable = new System.Windows.Forms.GroupBox();
            this.btnTableUnit1 = new System.Windows.Forms.Button();
            this.btnTableUnit2 = new System.Windows.Forms.Button();
            this.lbTableUnit = new System.Windows.Forms.Label();
            this.btnTableHome = new System.Windows.Forms.Button();
            this.TabulaImg = new System.Windows.Forms.ImageList(this.components);
            this.btnTableZ1 = new System.Windows.Forms.Button();
            this.btnTableZ2 = new System.Windows.Forms.Button();
            this.btnTableX2 = new System.Windows.Forms.Button();
            this.btnTableX1 = new System.Windows.Forms.Button();
            this.btnTableY2 = new System.Windows.Forms.Button();
            this.btnTableY1 = new System.Windows.Forms.Button();
            this.btnTableGoTo = new System.Windows.Forms.Button();
            this.tbTableZ = new System.Windows.Forms.TextBox();
            this.tbTableY = new System.Windows.Forms.TextBox();
            this.tbTableX = new System.Windows.Forms.TextBox();
            this.lblCua = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.TabObjects = new System.Windows.Forms.TabPage();
            this.Tree = new AxFlightLib.AxFlightTree();
            this.PropTree = new AxFlightLib.AxFlightPropList();
            this.tbObjects = new System.Windows.Forms.ToolStrip();
            this.CmdRemoveObject = new System.Windows.Forms.ToolStripButton();
            this.CmdCopy = new System.Windows.Forms.ToolStripButton();
            this.CmdAddGroup = new System.Windows.Forms.ToolStripButton();
            this.CmdAddBitmap = new System.Windows.Forms.ToolStripButton();
            this.CmdAddStruct = new System.Windows.Forms.ToolStripButton();
            this.CmdAddDrawing = new System.Windows.Forms.ToolStripButton();
            this.CmdAddText = new System.Windows.Forms.ToolStripButton();
            this.CmdAddCircularText = new System.Windows.Forms.ToolStripButton();
            this.CmdAddEvent = new System.Windows.Forms.ToolStripButton();
            this.CmdAdd3DStruct = new System.Windows.Forms.ToolStripButton();
            this.CmdAdd3DScript = new System.Windows.Forms.ToolStripButton();
            this.CmdAddWorkpiece = new System.Windows.Forms.ToolStripButton();
            this.FlightMarker = new AxFlightLib.AxFlightMarker();
            this.Editor = new AxFlightLib.AxFlightEditor();
            this.menuMain.SuspendLayout();
            this.StatusBar.SuspendLayout();
            this.tbDocument.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitView)).BeginInit();
            this.SplitView.Panel1.SuspendLayout();
            this.SplitView.Panel2.SuspendLayout();
            this.SplitView.SuspendLayout();
            this.FlightTab.SuspendLayout();
            this.TabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropColors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Colors)).BeginInit();
            this.tbColors.SuspendLayout();
            this.TabPage2.SuspendLayout();
            this.tbPens.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropPens)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pens)).BeginInit();
            this.TabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropFonts)).BeginInit();
            this.tbFonts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Fonts)).BeginInit();
            this.TabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropLayouts)).BeginInit();
            this.tbLayouts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Layouts)).BeginInit();
            this.tabPage5.SuspendLayout();
            this.tbSurface.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropSurfaces)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Surfaces)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitView2)).BeginInit();
            this.splitView2.Panel1.SuspendLayout();
            this.splitView2.Panel2.SuspendLayout();
            this.splitView2.SuspendLayout();
            this.tabViews.SuspendLayout();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.View)).BeginInit();
            this.tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.View3D)).BeginInit();
            this.FlightTab2.SuspendLayout();
            this.tabHardware.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gpTable.SuspendLayout();
            this.TabObjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropTree)).BeginInit();
            this.tbObjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FlightMarker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Editor)).BeginInit();
            this.SuspendLayout();
            // 
            // Timer
            // 
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuView,
            this.menuSettings,
            this.menuCompile,
            this.menuHelp});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(1213, 24);
            this.menuMain.TabIndex = 15;
            this.menuMain.Text = "MenuStrip";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNew,
            this.menuOpen,
            this.menuSeparator1,
            this.menuSave,
            this.menuSaveAs,
            this.menuSeperator2,
            this.menuExit});
            this.menuFile.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder;
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            // 
            // menuNew
            // 
            this.menuNew.Image = ((System.Drawing.Image)(resources.GetObject("menuNew.Image")));
            this.menuNew.ImageTransparentColor = System.Drawing.Color.Black;
            this.menuNew.Name = "menuNew";
            this.menuNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menuNew.Size = new System.Drawing.Size(146, 22);
            this.menuNew.Text = "&New";
            this.menuNew.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // menuOpen
            // 
            this.menuOpen.Image = ((System.Drawing.Image)(resources.GetObject("menuOpen.Image")));
            this.menuOpen.ImageTransparentColor = System.Drawing.Color.Black;
            this.menuOpen.Name = "menuOpen";
            this.menuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuOpen.Size = new System.Drawing.Size(146, 22);
            this.menuOpen.Text = "&Open";
            this.menuOpen.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // menuSeparator1
            // 
            this.menuSeparator1.Name = "menuSeparator1";
            this.menuSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // menuSave
            // 
            this.menuSave.Image = ((System.Drawing.Image)(resources.GetObject("menuSave.Image")));
            this.menuSave.ImageTransparentColor = System.Drawing.Color.Black;
            this.menuSave.Name = "menuSave";
            this.menuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuSave.Size = new System.Drawing.Size(146, 22);
            this.menuSave.Text = "&Save";
            this.menuSave.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.Name = "menuSaveAs";
            this.menuSaveAs.Size = new System.Drawing.Size(146, 22);
            this.menuSaveAs.Text = "Save &As";
            this.menuSaveAs.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // menuSeperator2
            // 
            this.menuSeperator2.Name = "menuSeperator2";
            this.menuSeperator2.Size = new System.Drawing.Size(143, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(146, 22);
            this.menuExit.Text = "E&xit";
            this.menuExit.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // menuView
            // 
            this.menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolbar,
            this.menuStatusBar,
            this.menuSeparator3,
            this.menuSelect,
            this.menuPan,
            this.menuEdit,
            this.menuZoomIn,
            this.menuZoomOut,
            this.menuSeparator4,
            this.menuRefresh});
            this.menuView.Name = "menuView";
            this.menuView.Size = new System.Drawing.Size(44, 20);
            this.menuView.Text = "&View";
            // 
            // menuToolbar
            // 
            this.menuToolbar.Checked = true;
            this.menuToolbar.CheckOnClick = true;
            this.menuToolbar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuToolbar.Name = "menuToolbar";
            this.menuToolbar.Size = new System.Drawing.Size(129, 22);
            this.menuToolbar.Text = "&Toolbar";
            this.menuToolbar.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // menuStatusBar
            // 
            this.menuStatusBar.Checked = true;
            this.menuStatusBar.CheckOnClick = true;
            this.menuStatusBar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuStatusBar.Name = "menuStatusBar";
            this.menuStatusBar.Size = new System.Drawing.Size(129, 22);
            this.menuStatusBar.Text = "Status &Bar";
            this.menuStatusBar.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // menuSeparator3
            // 
            this.menuSeparator3.Name = "menuSeparator3";
            this.menuSeparator3.Size = new System.Drawing.Size(126, 6);
            // 
            // menuSelect
            // 
            this.menuSelect.Name = "menuSelect";
            this.menuSelect.Size = new System.Drawing.Size(129, 22);
            this.menuSelect.Text = "&Select";
            this.menuSelect.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // menuPan
            // 
            this.menuPan.Name = "menuPan";
            this.menuPan.Size = new System.Drawing.Size(129, 22);
            this.menuPan.Text = "&Pan";
            this.menuPan.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // menuEdit
            // 
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(129, 22);
            this.menuEdit.Text = "Edit";
            // 
            // menuZoomIn
            // 
            this.menuZoomIn.Name = "menuZoomIn";
            this.menuZoomIn.Size = new System.Drawing.Size(129, 22);
            this.menuZoomIn.Text = "&Zoom In";
            this.menuZoomIn.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // menuZoomOut
            // 
            this.menuZoomOut.Name = "menuZoomOut";
            this.menuZoomOut.Size = new System.Drawing.Size(129, 22);
            this.menuZoomOut.Text = "Zoom &Out";
            this.menuZoomOut.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // menuSeparator4
            // 
            this.menuSeparator4.Name = "menuSeparator4";
            this.menuSeparator4.Size = new System.Drawing.Size(126, 6);
            // 
            // menuRefresh
            // 
            this.menuRefresh.Name = "menuRefresh";
            this.menuRefresh.Size = new System.Drawing.Size(129, 22);
            this.menuRefresh.Text = "Refresh";
            this.menuRefresh.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // menuSettings
            // 
            this.menuSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSettingsEdit,
            this.menuPropertiesEdit});
            this.menuSettings.Name = "menuSettings";
            this.menuSettings.Size = new System.Drawing.Size(61, 20);
            this.menuSettings.Text = "&Settings";
            // 
            // menuSettingsEdit
            // 
            this.menuSettingsEdit.Name = "menuSettingsEdit";
            this.menuSettingsEdit.Size = new System.Drawing.Size(192, 22);
            this.menuSettingsEdit.Text = "&Edit Hardware settings";
            this.menuSettingsEdit.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // menuPropertiesEdit
            // 
            this.menuPropertiesEdit.Name = "menuPropertiesEdit";
            this.menuPropertiesEdit.Size = new System.Drawing.Size(192, 22);
            this.menuPropertiesEdit.Text = "Edit &Process settings";
            // 
            // menuCompile
            // 
            this.menuCompile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSplit,
            this.menuRestore});
            this.menuCompile.Name = "menuCompile";
            this.menuCompile.Size = new System.Drawing.Size(64, 20);
            this.menuCompile.Text = "&Compile";
            // 
            // menuSplit
            // 
            this.menuSplit.Name = "menuSplit";
            this.menuSplit.Size = new System.Drawing.Size(113, 22);
            this.menuSplit.Text = "&Split";
            this.menuSplit.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // menuRestore
            // 
            this.menuRestore.Name = "menuRestore";
            this.menuRestore.Size = new System.Drawing.Size(113, 22);
            this.menuRestore.Text = "&Restore";
            this.menuRestore.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 20);
            this.menuHelp.Text = "&Help";
            // 
            // menuAbout
            // 
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Size = new System.Drawing.Size(140, 22);
            this.menuAbout.Text = "&About Flight";
            this.menuAbout.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // StatusBar
            // 
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LabelCoX,
            this.LabelCoY,
            this.lblTime,
            this.lblTableX,
            this.lblTableY,
            this.lblTableZ,
            this.lblScannerX,
            this.lblScannerY,
            this.lblScannerZ,
            this.lblTool});
            this.StatusBar.Location = new System.Drawing.Point(0, 611);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.StatusBar.Size = new System.Drawing.Size(1213, 22);
            this.StatusBar.TabIndex = 19;
            // 
            // LabelCoX
            // 
            this.LabelCoX.AutoSize = false;
            this.LabelCoX.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.LabelCoX.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.LabelCoX.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LabelCoX.Margin = new System.Windows.Forms.Padding(0, 3, 2, 2);
            this.LabelCoX.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.LabelCoX.Name = "LabelCoX";
            this.LabelCoX.Size = new System.Drawing.Size(60, 17);
            this.LabelCoX.Text = "X: -";
            this.LabelCoX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabelCoY
            // 
            this.LabelCoY.AutoSize = false;
            this.LabelCoY.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.LabelCoY.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LabelCoY.Margin = new System.Windows.Forms.Padding(0, 3, 2, 2);
            this.LabelCoY.Name = "LabelCoY";
            this.LabelCoY.Padding = new System.Windows.Forms.Padding(1);
            this.LabelCoY.Size = new System.Drawing.Size(60, 17);
            this.LabelCoY.Text = "Y: -";
            this.LabelCoY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = false;
            this.lblTime.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblTime.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(140, 17);
            this.lblTime.Text = "-";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTableX
            // 
            this.lblTableX.AutoSize = false;
            this.lblTableX.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblTableX.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblTableX.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTableX.Margin = new System.Windows.Forms.Padding(0, 3, 2, 2);
            this.lblTableX.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.lblTableX.Name = "lblTableX";
            this.lblTableX.Size = new System.Drawing.Size(80, 17);
            this.lblTableX.Text = "TX: -";
            this.lblTableX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTableY
            // 
            this.lblTableY.AutoSize = false;
            this.lblTableY.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblTableY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblTableY.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTableY.Margin = new System.Windows.Forms.Padding(0, 3, 2, 2);
            this.lblTableY.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.lblTableY.Name = "lblTableY";
            this.lblTableY.Size = new System.Drawing.Size(80, 17);
            this.lblTableY.Text = "TY: -";
            this.lblTableY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTableZ
            // 
            this.lblTableZ.AutoSize = false;
            this.lblTableZ.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblTableZ.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblTableZ.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTableZ.Margin = new System.Windows.Forms.Padding(0, 3, 2, 2);
            this.lblTableZ.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.lblTableZ.Name = "lblTableZ";
            this.lblTableZ.Size = new System.Drawing.Size(80, 17);
            this.lblTableZ.Text = "TZ: -";
            this.lblTableZ.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblScannerX
            // 
            this.lblScannerX.AutoSize = false;
            this.lblScannerX.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblScannerX.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.lblScannerX.Name = "lblScannerX";
            this.lblScannerX.Size = new System.Drawing.Size(80, 17);
            this.lblScannerX.Text = "SX: -";
            this.lblScannerX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblScannerY
            // 
            this.lblScannerY.AutoSize = false;
            this.lblScannerY.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblScannerY.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.lblScannerY.Name = "lblScannerY";
            this.lblScannerY.Size = new System.Drawing.Size(80, 17);
            this.lblScannerY.Text = "SY: -";
            this.lblScannerY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblScannerZ
            // 
            this.lblScannerZ.AutoSize = false;
            this.lblScannerZ.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblScannerZ.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.lblScannerZ.Name = "lblScannerZ";
            this.lblScannerZ.Size = new System.Drawing.Size(80, 17);
            this.lblScannerZ.Text = "SZ: -";
            this.lblScannerZ.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTool
            // 
            this.lblTool.AutoSize = false;
            this.lblTool.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblTool.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTool.Margin = new System.Windows.Forms.Padding(0, 3, 2, 2);
            this.lblTool.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.lblTool.Name = "lblTool";
            this.lblTool.Size = new System.Drawing.Size(200, 17);
            this.lblTool.Text = "-";
            this.lblTool.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(439, 474);
            // 
            // tbDocument
            // 
            this.tbDocument.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tbDocument.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tbDocument.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbNew,
            this.tbOpen,
            this.tbSave,
            this.ToolStripSeparator3,
            this.tbRefresh,
            this.tbMark,
            this.tbInfinite,
            this.tbSimulate,
            this.tbAbort,
            this.toolStripSeparator2,
            this.tbUpload,
            this.tbFlashExplorer,
            this.ToolStripSeparator5,
            this.tbSelect,
            this.tbPan,
            this.tbRotate,
            this.tbZoom,
            this.tbZoomIn,
            this.tbZoomOut,
            this.toolStripSeparator7,
            this.tbViewSelection,
            this.tbGrid,
            this.tbTopView,
            this.tbFrontView,
            this.tbSideView,
            this.tbEdit,
            this.ToolStripSeparator4,
            this.tbEditor,
            this.toolStripSeparator6,
            this.tbCalibrate,
            this.toolStripSeparator8,
            this.tbProperties,
            this.tbSettings});
            this.tbDocument.Location = new System.Drawing.Point(0, 24);
            this.tbDocument.Name = "tbDocument";
            this.tbDocument.Padding = new System.Windows.Forms.Padding(0);
            this.tbDocument.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tbDocument.Size = new System.Drawing.Size(1213, 31);
            this.tbDocument.TabIndex = 21;
            this.tbDocument.Text = "Colors";
            // 
            // tbNew
            // 
            this.tbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbNew.Image = ((System.Drawing.Image)(resources.GetObject("tbNew.Image")));
            this.tbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbNew.Name = "tbNew";
            this.tbNew.Size = new System.Drawing.Size(28, 28);
            this.tbNew.Text = "New document";
            this.tbNew.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbOpen
            // 
            this.tbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbOpen.Image = ((System.Drawing.Image)(resources.GetObject("tbOpen.Image")));
            this.tbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbOpen.Name = "tbOpen";
            this.tbOpen.Size = new System.Drawing.Size(28, 28);
            this.tbOpen.Text = "Load document";
            this.tbOpen.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbSave
            // 
            this.tbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbSave.Image = ((System.Drawing.Image)(resources.GetObject("tbSave.Image")));
            this.tbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbSave.Name = "tbSave";
            this.tbSave.Size = new System.Drawing.Size(28, 28);
            this.tbSave.Text = "Save document";
            this.tbSave.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            // 
            // tbRefresh
            // 
            this.tbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tbRefresh.Image")));
            this.tbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbRefresh.Name = "tbRefresh";
            this.tbRefresh.Size = new System.Drawing.Size(28, 28);
            this.tbRefresh.Text = "Refresh";
            this.tbRefresh.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbMark
            // 
            this.tbMark.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbMark.Image = ((System.Drawing.Image)(resources.GetObject("tbMark.Image")));
            this.tbMark.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbMark.Name = "tbMark";
            this.tbMark.Size = new System.Drawing.Size(28, 28);
            this.tbMark.Text = "Start marking";
            this.tbMark.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbInfinite
            // 
            this.tbInfinite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbInfinite.Image = ((System.Drawing.Image)(resources.GetObject("tbInfinite.Image")));
            this.tbInfinite.ImageTransparentColor = System.Drawing.Color.White;
            this.tbInfinite.Name = "tbInfinite";
            this.tbInfinite.Size = new System.Drawing.Size(28, 28);
            this.tbInfinite.Text = "Start Infinite Marking";
            this.tbInfinite.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbSimulate
            // 
            this.tbSimulate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbSimulate.Image = ((System.Drawing.Image)(resources.GetObject("tbSimulate.Image")));
            this.tbSimulate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbSimulate.Name = "tbSimulate";
            this.tbSimulate.Size = new System.Drawing.Size(28, 28);
            this.tbSimulate.Text = "Simulate";
            this.tbSimulate.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbAbort
            // 
            this.tbAbort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbAbort.Image = ((System.Drawing.Image)(resources.GetObject("tbAbort.Image")));
            this.tbAbort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbAbort.Name = "tbAbort";
            this.tbAbort.Size = new System.Drawing.Size(28, 28);
            this.tbAbort.Text = "Abort Marking";
            this.tbAbort.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // tbUpload
            // 
            this.tbUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbUpload.Image = ((System.Drawing.Image)(resources.GetObject("tbUpload.Image")));
            this.tbUpload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbUpload.Name = "tbUpload";
            this.tbUpload.Size = new System.Drawing.Size(28, 28);
            this.tbUpload.Text = "Store To Flash";
            this.tbUpload.Visible = false;
            this.tbUpload.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbFlashExplorer
            // 
            this.tbFlashExplorer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbFlashExplorer.Image = ((System.Drawing.Image)(resources.GetObject("tbFlashExplorer.Image")));
            this.tbFlashExplorer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbFlashExplorer.Name = "tbFlashExplorer";
            this.tbFlashExplorer.Size = new System.Drawing.Size(28, 28);
            this.tbFlashExplorer.Text = "Flash Explorer";
            this.tbFlashExplorer.Visible = false;
            this.tbFlashExplorer.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // ToolStripSeparator5
            // 
            this.ToolStripSeparator5.Name = "ToolStripSeparator5";
            this.ToolStripSeparator5.Size = new System.Drawing.Size(6, 31);
            // 
            // tbSelect
            // 
            this.tbSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbSelect.Image = ((System.Drawing.Image)(resources.GetObject("tbSelect.Image")));
            this.tbSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbSelect.Name = "tbSelect";
            this.tbSelect.Size = new System.Drawing.Size(28, 28);
            this.tbSelect.Text = "Select";
            this.tbSelect.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbPan
            // 
            this.tbPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbPan.Image = ((System.Drawing.Image)(resources.GetObject("tbPan.Image")));
            this.tbPan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbPan.Name = "tbPan";
            this.tbPan.Size = new System.Drawing.Size(28, 28);
            this.tbPan.Text = "Pan document";
            this.tbPan.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbRotate
            // 
            this.tbRotate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbRotate.Image = ((System.Drawing.Image)(resources.GetObject("tbRotate.Image")));
            this.tbRotate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbRotate.Name = "tbRotate";
            this.tbRotate.Size = new System.Drawing.Size(28, 28);
            this.tbRotate.Text = "Rotate View";
            this.tbRotate.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbZoom
            // 
            this.tbZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbZoom.Image = ((System.Drawing.Image)(resources.GetObject("tbZoom.Image")));
            this.tbZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbZoom.Name = "tbZoom";
            this.tbZoom.Size = new System.Drawing.Size(28, 28);
            this.tbZoom.Text = "Zoom In/Out";
            this.tbZoom.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbZoomIn
            // 
            this.tbZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("tbZoomIn.Image")));
            this.tbZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbZoomIn.Name = "tbZoomIn";
            this.tbZoomIn.Size = new System.Drawing.Size(28, 28);
            this.tbZoomIn.Text = "Zoom In";
            this.tbZoomIn.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbZoomOut
            // 
            this.tbZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("tbZoomOut.Image")));
            this.tbZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbZoomOut.Name = "tbZoomOut";
            this.tbZoomOut.Size = new System.Drawing.Size(28, 28);
            this.tbZoomOut.Text = "Zoom Out";
            this.tbZoomOut.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 31);
            // 
            // tbViewSelection
            // 
            this.tbViewSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbViewSelection.Image = ((System.Drawing.Image)(resources.GetObject("tbViewSelection.Image")));
            this.tbViewSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbViewSelection.Name = "tbViewSelection";
            this.tbViewSelection.Size = new System.Drawing.Size(28, 28);
            this.tbViewSelection.Text = "View Selection";
            this.tbViewSelection.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbGrid
            // 
            this.tbGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbGrid.Image = ((System.Drawing.Image)(resources.GetObject("tbGrid.Image")));
            this.tbGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbGrid.Name = "tbGrid";
            this.tbGrid.Size = new System.Drawing.Size(28, 28);
            this.tbGrid.Text = "Toggle Grid";
            this.tbGrid.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbTopView
            // 
            this.tbTopView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbTopView.Image = ((System.Drawing.Image)(resources.GetObject("tbTopView.Image")));
            this.tbTopView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbTopView.Name = "tbTopView";
            this.tbTopView.Size = new System.Drawing.Size(28, 28);
            this.tbTopView.Text = "TopView";
            this.tbTopView.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbFrontView
            // 
            this.tbFrontView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbFrontView.Image = ((System.Drawing.Image)(resources.GetObject("tbFrontView.Image")));
            this.tbFrontView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbFrontView.Name = "tbFrontView";
            this.tbFrontView.Size = new System.Drawing.Size(28, 28);
            this.tbFrontView.Text = "FrontView";
            this.tbFrontView.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbSideView
            // 
            this.tbSideView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbSideView.Image = ((System.Drawing.Image)(resources.GetObject("tbSideView.Image")));
            this.tbSideView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbSideView.Name = "tbSideView";
            this.tbSideView.Size = new System.Drawing.Size(28, 28);
            this.tbSideView.Text = "SideView";
            this.tbSideView.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbEdit
            // 
            this.tbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbEdit.Image = ((System.Drawing.Image)(resources.GetObject("tbEdit.Image")));
            this.tbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbEdit.Name = "tbEdit";
            this.tbEdit.Size = new System.Drawing.Size(28, 28);
            this.tbEdit.Text = "Edit objects";
            this.tbEdit.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // ToolStripSeparator4
            // 
            this.ToolStripSeparator4.Name = "ToolStripSeparator4";
            this.ToolStripSeparator4.Size = new System.Drawing.Size(6, 31);
            // 
            // tbEditor
            // 
            this.tbEditor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbEditor.Image = ((System.Drawing.Image)(resources.GetObject("tbEditor.Image")));
            this.tbEditor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbEditor.Name = "tbEditor";
            this.tbEditor.Size = new System.Drawing.Size(28, 28);
            this.tbEditor.Text = "Editor";
            this.tbEditor.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 31);
            // 
            // tbCalibrate
            // 
            this.tbCalibrate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbCalibrate.Image = ((System.Drawing.Image)(resources.GetObject("tbCalibrate.Image")));
            this.tbCalibrate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbCalibrate.Name = "tbCalibrate";
            this.tbCalibrate.Size = new System.Drawing.Size(28, 28);
            this.tbCalibrate.Text = "Calibrate";
            this.tbCalibrate.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 31);
            // 
            // tbProperties
            // 
            this.tbProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbProperties.Image = ((System.Drawing.Image)(resources.GetObject("tbProperties.Image")));
            this.tbProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbProperties.Name = "tbProperties";
            this.tbProperties.Size = new System.Drawing.Size(28, 28);
            this.tbProperties.Text = "Process Parameters";
            this.tbProperties.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // tbSettings
            // 
            this.tbSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbSettings.Image = ((System.Drawing.Image)(resources.GetObject("tbSettings.Image")));
            this.tbSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbSettings.Name = "tbSettings";
            this.tbSettings.Size = new System.Drawing.Size(28, 28);
            this.tbSettings.Text = "Settings";
            this.tbSettings.Click += new System.EventHandler(this.menuMain_Click);
            // 
            // Leds
            // 
            this.Leds.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Leds.ImageStream")));
            this.Leds.TransparentColor = System.Drawing.Color.Fuchsia;
            this.Leds.Images.SetKeyName(0, "Leds_big_grey.bmp");
            this.Leds.Images.SetKeyName(1, "Leds_big_green.bmp");
            this.Leds.Images.SetKeyName(2, "Leds_big_red.bmp");
            // 
            // SplitView
            // 
            this.SplitView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitView.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.SplitView.IsSplitterFixed = true;
            this.SplitView.Location = new System.Drawing.Point(0, 55);
            this.SplitView.Name = "SplitView";
            // 
            // SplitView.Panel1
            // 
            this.SplitView.Panel1.Controls.Add(this.FlightTab);
            this.SplitView.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // SplitView.Panel2
            // 
            this.SplitView.Panel2.Controls.Add(this.splitView2);
            this.SplitView.Panel2.Controls.Add(this.FlightMarker);
            this.SplitView.Panel2.Controls.Add(this.Editor);
            this.SplitView.Size = new System.Drawing.Size(1213, 556);
            this.SplitView.SplitterDistance = 311;
            this.SplitView.TabIndex = 20;
            // 
            // FlightTab
            // 
            this.FlightTab.Controls.Add(this.TabPage1);
            this.FlightTab.Controls.Add(this.TabPage2);
            this.FlightTab.Controls.Add(this.TabPage3);
            this.FlightTab.Controls.Add(this.TabPage4);
            this.FlightTab.Controls.Add(this.tabPage5);
            this.FlightTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FlightTab.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.FlightTab.Location = new System.Drawing.Point(3, 3);
            this.FlightTab.Multiline = true;
            this.FlightTab.Name = "FlightTab";
            this.FlightTab.Padding = new System.Drawing.Point(13, 3);
            this.FlightTab.SelectedIndex = 0;
            this.FlightTab.Size = new System.Drawing.Size(305, 550);
            this.FlightTab.TabIndex = 15;
            // 
            // TabPage1
            // 
            this.TabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.TabPage1.Controls.Add(this.PropColors);
            this.TabPage1.Controls.Add(this.Colors);
            this.TabPage1.Controls.Add(this.tbColors);
            this.TabPage1.Location = new System.Drawing.Point(4, 22);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(297, 524);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Colors";
            this.TabPage1.UseVisualStyleBackColor = true;
            // 
            // PropColors
            // 
            this.PropColors.Location = new System.Drawing.Point(36, 33);
            this.PropColors.Name = "PropColors";
            this.PropColors.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("PropColors.OcxState")));
            this.PropColors.Size = new System.Drawing.Size(255, 450);
            this.PropColors.TabIndex = 2;
            this.PropColors.PropertyChanged += new System.EventHandler(this.Flight_PropertyChanged);
            // 
            // Colors
            // 
            this.Colors.Location = new System.Drawing.Point(36, 6);
            this.Colors.Name = "Colors";
            this.Colors.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Colors.OcxState")));
            this.Colors.Size = new System.Drawing.Size(255, 21);
            this.Colors.TabIndex = 7;
            this.Colors.ClickEvent += new System.EventHandler(this.Colors_ClickEvent);
            // 
            // tbColors
            // 
            this.tbColors.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbColors.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tbColors.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tbColors.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tbColors.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CmdRemoveColor,
            this.CmdAddColor,
            this.ToolStripSeparator1,
            this.CmdChangeColor});
            this.tbColors.Location = new System.Drawing.Point(3, 3);
            this.tbColors.Name = "tbColors";
            this.tbColors.Padding = new System.Windows.Forms.Padding(0);
            this.tbColors.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tbColors.Size = new System.Drawing.Size(28, 518);
            this.tbColors.TabIndex = 6;
            this.tbColors.Text = "Colors";
            this.tbColors.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tbColors_ItemClicked);
            // 
            // CmdRemoveColor
            // 
            this.CmdRemoveColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdRemoveColor.Image = ((System.Drawing.Image)(resources.GetObject("CmdRemoveColor.Image")));
            this.CmdRemoveColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdRemoveColor.Name = "CmdRemoveColor";
            this.CmdRemoveColor.Size = new System.Drawing.Size(27, 28);
            this.CmdRemoveColor.Text = "Remove Color";
            // 
            // CmdAddColor
            // 
            this.CmdAddColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAddColor.Image = ((System.Drawing.Image)(resources.GetObject("CmdAddColor.Image")));
            this.CmdAddColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAddColor.Name = "CmdAddColor";
            this.CmdAddColor.Size = new System.Drawing.Size(27, 28);
            this.CmdAddColor.Text = "Add Color";
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(27, 6);
            // 
            // CmdChangeColor
            // 
            this.CmdChangeColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdChangeColor.Image = ((System.Drawing.Image)(resources.GetObject("CmdChangeColor.Image")));
            this.CmdChangeColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdChangeColor.Name = "CmdChangeColor";
            this.CmdChangeColor.Size = new System.Drawing.Size(27, 28);
            this.CmdChangeColor.Text = "Change Color";
            // 
            // TabPage2
            // 
            this.TabPage2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.TabPage2.Controls.Add(this.tbPens);
            this.TabPage2.Controls.Add(this.PropPens);
            this.TabPage2.Controls.Add(this.Pens);
            this.TabPage2.Location = new System.Drawing.Point(4, 22);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage2.Size = new System.Drawing.Size(297, 524);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "Pens";
            this.TabPage2.UseVisualStyleBackColor = true;
            // 
            // tbPens
            // 
            this.tbPens.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbPens.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tbPens.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tbPens.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CmdRemovePen,
            this.CmdAddPen});
            this.tbPens.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.tbPens.Location = new System.Drawing.Point(3, 3);
            this.tbPens.Name = "tbPens";
            this.tbPens.Padding = new System.Windows.Forms.Padding(0);
            this.tbPens.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tbPens.Size = new System.Drawing.Size(28, 518);
            this.tbPens.TabIndex = 9;
            this.tbPens.Text = "Pens";
            this.tbPens.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tbPens_ItemClicked);
            // 
            // CmdRemovePen
            // 
            this.CmdRemovePen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdRemovePen.Image = ((System.Drawing.Image)(resources.GetObject("CmdRemovePen.Image")));
            this.CmdRemovePen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdRemovePen.Name = "CmdRemovePen";
            this.CmdRemovePen.Size = new System.Drawing.Size(27, 28);
            this.CmdRemovePen.Text = "Remove Pen";
            // 
            // CmdAddPen
            // 
            this.CmdAddPen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAddPen.Image = ((System.Drawing.Image)(resources.GetObject("CmdAddPen.Image")));
            this.CmdAddPen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAddPen.Name = "CmdAddPen";
            this.CmdAddPen.Size = new System.Drawing.Size(27, 28);
            this.CmdAddPen.Text = "Add Pen";
            // 
            // PropPens
            // 
            this.PropPens.Location = new System.Drawing.Point(36, 33);
            this.PropPens.Name = "PropPens";
            this.PropPens.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("PropPens.OcxState")));
            this.PropPens.Size = new System.Drawing.Size(255, 450);
            this.PropPens.TabIndex = 1;
            this.PropPens.PropertyChanged += new System.EventHandler(this.Flight_PropertyChanged);
            // 
            // Pens
            // 
            this.Pens.Location = new System.Drawing.Point(36, 6);
            this.Pens.Name = "Pens";
            this.Pens.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Pens.OcxState")));
            this.Pens.Size = new System.Drawing.Size(255, 21);
            this.Pens.TabIndex = 0;
            this.Pens.ClickEvent += new System.EventHandler(this.Pens_ClickEvent);
            // 
            // TabPage3
            // 
            this.TabPage3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.TabPage3.Controls.Add(this.PropFonts);
            this.TabPage3.Controls.Add(this.tbFonts);
            this.TabPage3.Controls.Add(this.Fonts);
            this.TabPage3.Location = new System.Drawing.Point(4, 22);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage3.Size = new System.Drawing.Size(297, 524);
            this.TabPage3.TabIndex = 2;
            this.TabPage3.Text = "Fonts";
            this.TabPage3.UseVisualStyleBackColor = true;
            // 
            // PropFonts
            // 
            this.PropFonts.Location = new System.Drawing.Point(36, 33);
            this.PropFonts.Name = "PropFonts";
            this.PropFonts.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("PropFonts.OcxState")));
            this.PropFonts.Size = new System.Drawing.Size(255, 450);
            this.PropFonts.TabIndex = 1;
            this.PropFonts.PropertyChanged += new System.EventHandler(this.Flight_PropertyChanged);
            // 
            // tbFonts
            // 
            this.tbFonts.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbFonts.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tbFonts.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tbFonts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CmdRemoveFont,
            this.CmdAddTrueTypeFont,
            this.CmdAddSinglestrokeFont,
            this.CmdAddBarcodeFont});
            this.tbFonts.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.tbFonts.Location = new System.Drawing.Point(3, 3);
            this.tbFonts.Name = "tbFonts";
            this.tbFonts.Padding = new System.Windows.Forms.Padding(0);
            this.tbFonts.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tbFonts.Size = new System.Drawing.Size(28, 518);
            this.tbFonts.TabIndex = 8;
            this.tbFonts.Text = "Fonts";
            this.tbFonts.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tbFonts_ItemClicked);
            // 
            // CmdRemoveFont
            // 
            this.CmdRemoveFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdRemoveFont.Image = ((System.Drawing.Image)(resources.GetObject("CmdRemoveFont.Image")));
            this.CmdRemoveFont.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdRemoveFont.Name = "CmdRemoveFont";
            this.CmdRemoveFont.Size = new System.Drawing.Size(27, 28);
            this.CmdRemoveFont.Text = "Remove font";
            // 
            // CmdAddTrueTypeFont
            // 
            this.CmdAddTrueTypeFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAddTrueTypeFont.Image = ((System.Drawing.Image)(resources.GetObject("CmdAddTrueTypeFont.Image")));
            this.CmdAddTrueTypeFont.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAddTrueTypeFont.Name = "CmdAddTrueTypeFont";
            this.CmdAddTrueTypeFont.Size = new System.Drawing.Size(27, 28);
            this.CmdAddTrueTypeFont.Text = "Add TrueType Font";
            // 
            // CmdAddSinglestrokeFont
            // 
            this.CmdAddSinglestrokeFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAddSinglestrokeFont.Image = ((System.Drawing.Image)(resources.GetObject("CmdAddSinglestrokeFont.Image")));
            this.CmdAddSinglestrokeFont.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAddSinglestrokeFont.Name = "CmdAddSinglestrokeFont";
            this.CmdAddSinglestrokeFont.Size = new System.Drawing.Size(27, 28);
            this.CmdAddSinglestrokeFont.Text = "Add Singlestroke Font";
            // 
            // CmdAddBarcodeFont
            // 
            this.CmdAddBarcodeFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAddBarcodeFont.Image = ((System.Drawing.Image)(resources.GetObject("CmdAddBarcodeFont.Image")));
            this.CmdAddBarcodeFont.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAddBarcodeFont.Name = "CmdAddBarcodeFont";
            this.CmdAddBarcodeFont.Size = new System.Drawing.Size(27, 28);
            this.CmdAddBarcodeFont.Text = "Add Barcode Font";
            // 
            // Fonts
            // 
            this.Fonts.Location = new System.Drawing.Point(36, 6);
            this.Fonts.Name = "Fonts";
            this.Fonts.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Fonts.OcxState")));
            this.Fonts.Size = new System.Drawing.Size(255, 21);
            this.Fonts.TabIndex = 0;
            this.Fonts.ClickEvent += new System.EventHandler(this.Fonts_ClickEvent);
            // 
            // TabPage4
            // 
            this.TabPage4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.TabPage4.Controls.Add(this.PropLayouts);
            this.TabPage4.Controls.Add(this.tbLayouts);
            this.TabPage4.Controls.Add(this.Layouts);
            this.TabPage4.Location = new System.Drawing.Point(4, 22);
            this.TabPage4.Name = "TabPage4";
            this.TabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage4.Size = new System.Drawing.Size(297, 524);
            this.TabPage4.TabIndex = 3;
            this.TabPage4.Text = "Layouts";
            this.TabPage4.UseVisualStyleBackColor = true;
            // 
            // PropLayouts
            // 
            this.PropLayouts.Location = new System.Drawing.Point(36, 33);
            this.PropLayouts.Name = "PropLayouts";
            this.PropLayouts.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("PropLayouts.OcxState")));
            this.PropLayouts.Size = new System.Drawing.Size(255, 450);
            this.PropLayouts.TabIndex = 1;
            this.PropLayouts.PropertyChanged += new System.EventHandler(this.Flight_PropertyChanged);
            // 
            // tbLayouts
            // 
            this.tbLayouts.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbLayouts.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tbLayouts.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tbLayouts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CmdRemoveLayout,
            this.CmdEditLayout,
            this.CmdAddLayout});
            this.tbLayouts.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.tbLayouts.Location = new System.Drawing.Point(3, 3);
            this.tbLayouts.Name = "tbLayouts";
            this.tbLayouts.Padding = new System.Windows.Forms.Padding(0);
            this.tbLayouts.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tbLayouts.Size = new System.Drawing.Size(28, 518);
            this.tbLayouts.TabIndex = 11;
            this.tbLayouts.Text = "Layouts";
            this.tbLayouts.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tbLayouts_ItemClicked);
            // 
            // CmdRemoveLayout
            // 
            this.CmdRemoveLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdRemoveLayout.Image = ((System.Drawing.Image)(resources.GetObject("CmdRemoveLayout.Image")));
            this.CmdRemoveLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdRemoveLayout.Name = "CmdRemoveLayout";
            this.CmdRemoveLayout.Size = new System.Drawing.Size(27, 28);
            this.CmdRemoveLayout.Text = "Remove Layout";
            // 
            // CmdEditLayout
            // 
            this.CmdEditLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdEditLayout.Image = ((System.Drawing.Image)(resources.GetObject("CmdEditLayout.Image")));
            this.CmdEditLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdEditLayout.Name = "CmdEditLayout";
            this.CmdEditLayout.Size = new System.Drawing.Size(27, 28);
            this.CmdEditLayout.Text = "Edit Step Repeat";
            // 
            // CmdAddLayout
            // 
            this.CmdAddLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAddLayout.Image = ((System.Drawing.Image)(resources.GetObject("CmdAddLayout.Image")));
            this.CmdAddLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAddLayout.Name = "CmdAddLayout";
            this.CmdAddLayout.Size = new System.Drawing.Size(27, 28);
            this.CmdAddLayout.Text = "Add Layout";
            // 
            // Layouts
            // 
            this.Layouts.Location = new System.Drawing.Point(36, 6);
            this.Layouts.Name = "Layouts";
            this.Layouts.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Layouts.OcxState")));
            this.Layouts.Size = new System.Drawing.Size(255, 21);
            this.Layouts.TabIndex = 0;
            this.Layouts.ClickEvent += new System.EventHandler(this.Layouts_ClickEvent);
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage5.Controls.Add(this.tbSurface);
            this.tabPage5.Controls.Add(this.PropSurfaces);
            this.tabPage5.Controls.Add(this.Surfaces);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(297, 524);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Surfaces";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tbSurface
            // 
            this.tbSurface.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbSurface.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tbSurface.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tbSurface.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CmdAddSurface,
            this.CmdRemoveSurface});
            this.tbSurface.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.tbSurface.Location = new System.Drawing.Point(3, 3);
            this.tbSurface.Name = "tbSurface";
            this.tbSurface.Padding = new System.Windows.Forms.Padding(0);
            this.tbSurface.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tbSurface.Size = new System.Drawing.Size(28, 518);
            this.tbSurface.TabIndex = 11;
            this.tbSurface.Text = "Pens";
            this.tbSurface.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tbSurface_ItemClicked);
            // 
            // CmdAddSurface
            // 
            this.CmdAddSurface.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAddSurface.Image = ((System.Drawing.Image)(resources.GetObject("CmdAddSurface.Image")));
            this.CmdAddSurface.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAddSurface.Name = "CmdAddSurface";
            this.CmdAddSurface.Size = new System.Drawing.Size(27, 28);
            this.CmdAddSurface.Text = "Add Surface";
            // 
            // CmdRemoveSurface
            // 
            this.CmdRemoveSurface.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdRemoveSurface.Image = ((System.Drawing.Image)(resources.GetObject("CmdRemoveSurface.Image")));
            this.CmdRemoveSurface.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdRemoveSurface.Name = "CmdRemoveSurface";
            this.CmdRemoveSurface.Size = new System.Drawing.Size(27, 28);
            this.CmdRemoveSurface.Text = "Remove Surface";
            // 
            // PropSurfaces
            // 
            this.PropSurfaces.Location = new System.Drawing.Point(36, 33);
            this.PropSurfaces.Name = "PropSurfaces";
            this.PropSurfaces.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("PropSurfaces.OcxState")));
            this.PropSurfaces.Size = new System.Drawing.Size(255, 450);
            this.PropSurfaces.TabIndex = 1;
            this.PropSurfaces.PropertyChanged += new System.EventHandler(this.Flight_PropertyChanged);
            // 
            // Surfaces
            // 
            this.Surfaces.Location = new System.Drawing.Point(36, 6);
            this.Surfaces.Name = "Surfaces";
            this.Surfaces.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Surfaces.OcxState")));
            this.Surfaces.Size = new System.Drawing.Size(255, 21);
            this.Surfaces.TabIndex = 0;
            this.Surfaces.ClickEvent += new System.EventHandler(this.Surfaces_ClickEvent);
            // 
            // splitView2
            // 
            this.splitView2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitView2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitView2.IsSplitterFixed = true;
            this.splitView2.Location = new System.Drawing.Point(0, 0);
            this.splitView2.Name = "splitView2";
            // 
            // splitView2.Panel1
            // 
            this.splitView2.Panel1.Controls.Add(this.tabViews);
            // 
            // splitView2.Panel2
            // 
            this.splitView2.Panel2.Controls.Add(this.FlightTab2);
            this.splitView2.Panel2MinSize = 50;
            this.splitView2.Size = new System.Drawing.Size(898, 556);
            this.splitView2.SplitterDistance = 621;
            this.splitView2.TabIndex = 29;
            // 
            // tabViews
            // 
            this.tabViews.Controls.Add(this.tabPage6);
            this.tabViews.Controls.Add(this.tabPage7);
            this.tabViews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabViews.Location = new System.Drawing.Point(0, 0);
            this.tabViews.Name = "tabViews";
            this.tabViews.SelectedIndex = 0;
            this.tabViews.Size = new System.Drawing.Size(619, 554);
            this.tabViews.TabIndex = 3;
            this.tabViews.SelectedIndexChanged += new System.EventHandler(this.tabViews_SelectedIndexChanged);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.View);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(611, 528);
            this.tabPage6.TabIndex = 0;
            this.tabPage6.Text = "2D VIEW";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // View
            // 
            this.View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.View.Enabled = true;
            this.View.Location = new System.Drawing.Point(3, 3);
            this.View.Name = "View";
            this.View.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("View.OcxState")));
            this.View.Size = new System.Drawing.Size(605, 522);
            this.View.TabIndex = 2;
            this.View.MouseMoveEvent += new AxFlightLib._DFlightViewEvents_MouseMoveEventHandler(this.View_MouseMoveEvent);
            this.View.ObjectClick += new AxFlightLib._DFlightViewEvents_ObjectClickEventHandler(this.View_ObjectClick);
            this.View.ObjectChanged += new AxFlightLib._DFlightViewEvents_ObjectChangedEventHandler(this.View_ObjectChanged);
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.View3D);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(611, 528);
            this.tabPage7.TabIndex = 1;
            this.tabPage7.Text = "3D VIEW";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // View3D
            // 
            this.View3D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.View3D.Enabled = true;
            this.View3D.Location = new System.Drawing.Point(3, 3);
            this.View3D.Name = "View3D";
            this.View3D.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("View3D.OcxState")));
            this.View3D.Size = new System.Drawing.Size(605, 522);
            this.View3D.TabIndex = 0;
            this.View3D.ObjectClick += new AxFlightLib._DFlightView3DEvents_ObjectClickEventHandler(this.View3D_ObjectClick);
            // 
            // FlightTab2
            // 
            this.FlightTab2.Controls.Add(this.tabHardware);
            this.FlightTab2.Controls.Add(this.TabObjects);
            this.FlightTab2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FlightTab2.Location = new System.Drawing.Point(0, 0);
            this.FlightTab2.Name = "FlightTab2";
            this.FlightTab2.Padding = new System.Drawing.Point(13, 3);
            this.FlightTab2.SelectedIndex = 0;
            this.FlightTab2.Size = new System.Drawing.Size(271, 554);
            this.FlightTab2.TabIndex = 16;
            // 
            // tabHardware
            // 
            this.tabHardware.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabHardware.Controls.Add(this.tableLayoutPanel1);
            this.tabHardware.Controls.Add(this.lblCua);
            this.tabHardware.Controls.Add(this.btnConnect);
            this.tabHardware.Location = new System.Drawing.Point(4, 22);
            this.tabHardware.Name = "tabHardware";
            this.tabHardware.Padding = new System.Windows.Forms.Padding(3);
            this.tabHardware.Size = new System.Drawing.Size(263, 528);
            this.tabHardware.TabIndex = 1;
            this.tabHardware.Text = "Hardware";
            this.tabHardware.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gpTable, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 196);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 61.70213F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38.29787F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(257, 329);
            this.tableLayoutPanel1.TabIndex = 51;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbLS2Z);
            this.panel1.Controls.Add(this.lblTable);
            this.panel1.Controls.Add(this.lbLS1Y);
            this.panel1.Controls.Add(this.lbLS2Y);
            this.panel1.Controls.Add(this.lbLS1X);
            this.panel1.Controls.Add(this.lbLS2X);
            this.panel1.Controls.Add(this.lbLS1Z);
            this.panel1.Location = new System.Drawing.Point(3, 206);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(225, 116);
            this.panel1.TabIndex = 3;
            // 
            // lbLS2Z
            // 
            this.lbLS2Z.BackColor = System.Drawing.Color.Transparent;
            this.lbLS2Z.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbLS2Z.ImageIndex = 0;
            this.lbLS2Z.ImageList = this.ImgBars;
            this.lbLS2Z.Location = new System.Drawing.Point(130, 86);
            this.lbLS2Z.Name = "lbLS2Z";
            this.lbLS2Z.Size = new System.Drawing.Size(25, 24);
            this.lbLS2Z.TabIndex = 60;
            this.lbLS2Z.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ImgBars
            // 
            this.ImgBars.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImgBars.ImageStream")));
            this.ImgBars.TransparentColor = System.Drawing.Color.Transparent;
            this.ImgBars.Images.SetKeyName(0, "BarsGrey.bmp");
            this.ImgBars.Images.SetKeyName(1, "BarsGreen.bmp");
            this.ImgBars.Images.SetKeyName(2, "BarsRed.bmp");
            // 
            // lblTable
            // 
            this.lblTable.AutoSize = true;
            this.lblTable.Location = new System.Drawing.Point(14, 14);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(10, 13);
            this.lblTable.TabIndex = 50;
            this.lblTable.Text = "-";
            // 
            // lbLS1Y
            // 
            this.lbLS1Y.BackColor = System.Drawing.Color.Transparent;
            this.lbLS1Y.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbLS1Y.ImageIndex = 0;
            this.lbLS1Y.ImageList = this.ImgBars;
            this.lbLS1Y.Location = new System.Drawing.Point(14, 62);
            this.lbLS1Y.Name = "lbLS1Y";
            this.lbLS1Y.Size = new System.Drawing.Size(116, 24);
            this.lbLS1Y.TabIndex = 59;
            this.lbLS1Y.Text = "      Limit Switch Y";
            this.lbLS1Y.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbLS2Y
            // 
            this.lbLS2Y.BackColor = System.Drawing.Color.Transparent;
            this.lbLS2Y.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbLS2Y.ImageIndex = 0;
            this.lbLS2Y.ImageList = this.ImgBars;
            this.lbLS2Y.Location = new System.Drawing.Point(130, 62);
            this.lbLS2Y.Name = "lbLS2Y";
            this.lbLS2Y.Size = new System.Drawing.Size(25, 24);
            this.lbLS2Y.TabIndex = 56;
            this.lbLS2Y.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbLS1X
            // 
            this.lbLS1X.BackColor = System.Drawing.Color.Transparent;
            this.lbLS1X.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbLS1X.ImageIndex = 0;
            this.lbLS1X.ImageList = this.ImgBars;
            this.lbLS1X.Location = new System.Drawing.Point(14, 38);
            this.lbLS1X.Name = "lbLS1X";
            this.lbLS1X.Size = new System.Drawing.Size(116, 24);
            this.lbLS1X.TabIndex = 51;
            this.lbLS1X.Text = "      Limit Switch X";
            this.lbLS1X.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbLS2X
            // 
            this.lbLS2X.BackColor = System.Drawing.Color.Transparent;
            this.lbLS2X.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbLS2X.ImageIndex = 0;
            this.lbLS2X.ImageList = this.ImgBars;
            this.lbLS2X.Location = new System.Drawing.Point(130, 38);
            this.lbLS2X.Name = "lbLS2X";
            this.lbLS2X.Size = new System.Drawing.Size(25, 24);
            this.lbLS2X.TabIndex = 52;
            this.lbLS2X.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbLS1Z
            // 
            this.lbLS1Z.BackColor = System.Drawing.Color.Transparent;
            this.lbLS1Z.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbLS1Z.ImageIndex = 0;
            this.lbLS1Z.ImageList = this.ImgBars;
            this.lbLS1Z.Location = new System.Drawing.Point(14, 86);
            this.lbLS1Z.Name = "lbLS1Z";
            this.lbLS1Z.Size = new System.Drawing.Size(116, 24);
            this.lbLS1Z.TabIndex = 55;
            this.lbLS1Z.Text = "      Limit Switch Z";
            this.lbLS1Z.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gpTable
            // 
            this.gpTable.Controls.Add(this.btnTableUnit1);
            this.gpTable.Controls.Add(this.btnTableUnit2);
            this.gpTable.Controls.Add(this.lbTableUnit);
            this.gpTable.Controls.Add(this.btnTableHome);
            this.gpTable.Controls.Add(this.btnTableZ1);
            this.gpTable.Controls.Add(this.btnTableZ2);
            this.gpTable.Controls.Add(this.btnTableX2);
            this.gpTable.Controls.Add(this.btnTableX1);
            this.gpTable.Controls.Add(this.btnTableY2);
            this.gpTable.Controls.Add(this.btnTableY1);
            this.gpTable.Controls.Add(this.btnTableGoTo);
            this.gpTable.Controls.Add(this.tbTableZ);
            this.gpTable.Controls.Add(this.tbTableY);
            this.gpTable.Controls.Add(this.tbTableX);
            this.gpTable.Location = new System.Drawing.Point(3, 3);
            this.gpTable.Name = "gpTable";
            this.gpTable.Size = new System.Drawing.Size(251, 197);
            this.gpTable.TabIndex = 50;
            this.gpTable.TabStop = false;
            this.gpTable.Text = "Table";
            // 
            // btnTableUnit1
            // 
            this.btnTableUnit1.Location = new System.Drawing.Point(12, 78);
            this.btnTableUnit1.Name = "btnTableUnit1";
            this.btnTableUnit1.Size = new System.Drawing.Size(20, 20);
            this.btnTableUnit1.TabIndex = 49;
            this.btnTableUnit1.Text = "-";
            this.btnTableUnit1.UseVisualStyleBackColor = true;
            this.btnTableUnit1.Click += new System.EventHandler(this.btnTableUnit1_Click);
            // 
            // btnTableUnit2
            // 
            this.btnTableUnit2.Location = new System.Drawing.Point(34, 78);
            this.btnTableUnit2.Name = "btnTableUnit2";
            this.btnTableUnit2.Size = new System.Drawing.Size(20, 20);
            this.btnTableUnit2.TabIndex = 48;
            this.btnTableUnit2.Text = "+";
            this.btnTableUnit2.UseVisualStyleBackColor = true;
            this.btnTableUnit2.Click += new System.EventHandler(this.btnTableUnit2_Click);
            // 
            // lbTableUnit
            // 
            this.lbTableUnit.Location = new System.Drawing.Point(12, 60);
            this.lbTableUnit.Name = "lbTableUnit";
            this.lbTableUnit.Size = new System.Drawing.Size(42, 15);
            this.lbTableUnit.TabIndex = 47;
            this.lbTableUnit.Text = "1.0";
            this.lbTableUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnTableHome
            // 
            this.btnTableHome.Enabled = false;
            this.btnTableHome.ImageIndex = 6;
            this.btnTableHome.ImageList = this.TabulaImg;
            this.btnTableHome.Location = new System.Drawing.Point(57, 99);
            this.btnTableHome.Name = "btnTableHome";
            this.btnTableHome.Size = new System.Drawing.Size(42, 42);
            this.btnTableHome.TabIndex = 45;
            this.btnTableHome.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnTableHome.UseVisualStyleBackColor = true;
            this.btnTableHome.Click += new System.EventHandler(this.btnTableHome_Click);
            // 
            // TabulaImg
            // 
            this.TabulaImg.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TabulaImg.ImageStream")));
            this.TabulaImg.TransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(163)))), ((int)(((byte)(177)))));
            this.TabulaImg.Images.SetKeyName(0, "Reference.bmp");
            this.TabulaImg.Images.SetKeyName(1, "Stop.bmp");
            this.TabulaImg.Images.SetKeyName(2, "Up.bmp");
            this.TabulaImg.Images.SetKeyName(3, "Down.bmp");
            this.TabulaImg.Images.SetKeyName(4, "Left.bmp");
            this.TabulaImg.Images.SetKeyName(5, "Right.bmp");
            this.TabulaImg.Images.SetKeyName(6, "IconHome.bmp");
            this.TabulaImg.Images.SetKeyName(7, "IconP1.bmp");
            // 
            // btnTableZ1
            // 
            this.btnTableZ1.Enabled = false;
            this.btnTableZ1.ImageIndex = 2;
            this.btnTableZ1.ImageList = this.TabulaImg;
            this.btnTableZ1.Location = new System.Drawing.Point(101, 56);
            this.btnTableZ1.Name = "btnTableZ1";
            this.btnTableZ1.Size = new System.Drawing.Size(42, 42);
            this.btnTableZ1.TabIndex = 44;
            this.btnTableZ1.Text = "Z";
            this.btnTableZ1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnTableZ1.UseVisualStyleBackColor = true;
            this.btnTableZ1.Click += new System.EventHandler(this.btnTableZ1_Click);
            // 
            // btnTableZ2
            // 
            this.btnTableZ2.Enabled = false;
            this.btnTableZ2.ImageIndex = 3;
            this.btnTableZ2.ImageList = this.TabulaImg;
            this.btnTableZ2.Location = new System.Drawing.Point(101, 143);
            this.btnTableZ2.Name = "btnTableZ2";
            this.btnTableZ2.Size = new System.Drawing.Size(42, 42);
            this.btnTableZ2.TabIndex = 43;
            this.btnTableZ2.Text = "Z";
            this.btnTableZ2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnTableZ2.UseVisualStyleBackColor = true;
            this.btnTableZ2.Click += new System.EventHandler(this.btnTableZ2_Click);
            // 
            // btnTableX2
            // 
            this.btnTableX2.Enabled = false;
            this.btnTableX2.ImageIndex = 5;
            this.btnTableX2.ImageList = this.TabulaImg;
            this.btnTableX2.Location = new System.Drawing.Point(101, 99);
            this.btnTableX2.Name = "btnTableX2";
            this.btnTableX2.Size = new System.Drawing.Size(42, 42);
            this.btnTableX2.TabIndex = 42;
            this.btnTableX2.Text = "X";
            this.btnTableX2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnTableX2.UseVisualStyleBackColor = true;
            this.btnTableX2.Click += new System.EventHandler(this.btnTableX2_Click);
            // 
            // btnTableX1
            // 
            this.btnTableX1.Enabled = false;
            this.btnTableX1.ImageIndex = 4;
            this.btnTableX1.ImageList = this.TabulaImg;
            this.btnTableX1.Location = new System.Drawing.Point(12, 99);
            this.btnTableX1.Name = "btnTableX1";
            this.btnTableX1.Size = new System.Drawing.Size(42, 42);
            this.btnTableX1.TabIndex = 41;
            this.btnTableX1.Text = "X";
            this.btnTableX1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnTableX1.UseVisualStyleBackColor = true;
            this.btnTableX1.Click += new System.EventHandler(this.btnTableX1_Click);
            // 
            // btnTableY2
            // 
            this.btnTableY2.Enabled = false;
            this.btnTableY2.ImageIndex = 3;
            this.btnTableY2.ImageList = this.TabulaImg;
            this.btnTableY2.Location = new System.Drawing.Point(57, 143);
            this.btnTableY2.Name = "btnTableY2";
            this.btnTableY2.Size = new System.Drawing.Size(42, 42);
            this.btnTableY2.TabIndex = 40;
            this.btnTableY2.Text = "Y";
            this.btnTableY2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnTableY2.UseVisualStyleBackColor = true;
            this.btnTableY2.Click += new System.EventHandler(this.btnTableY2_Click);
            // 
            // btnTableY1
            // 
            this.btnTableY1.Enabled = false;
            this.btnTableY1.ImageIndex = 2;
            this.btnTableY1.ImageList = this.TabulaImg;
            this.btnTableY1.Location = new System.Drawing.Point(57, 56);
            this.btnTableY1.Name = "btnTableY1";
            this.btnTableY1.Size = new System.Drawing.Size(42, 42);
            this.btnTableY1.TabIndex = 39;
            this.btnTableY1.Text = "Y";
            this.btnTableY1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnTableY1.UseVisualStyleBackColor = true;
            this.btnTableY1.Click += new System.EventHandler(this.btnTableY1_Click);
            // 
            // btnTableGoTo
            // 
            this.btnTableGoTo.Location = new System.Drawing.Point(147, 22);
            this.btnTableGoTo.Name = "btnTableGoTo";
            this.btnTableGoTo.Size = new System.Drawing.Size(42, 20);
            this.btnTableGoTo.TabIndex = 38;
            this.btnTableGoTo.Text = "Goto";
            this.btnTableGoTo.UseVisualStyleBackColor = true;
            this.btnTableGoTo.Click += new System.EventHandler(this.btnTableGoTo_Click);
            // 
            // tbTableZ
            // 
            this.tbTableZ.Location = new System.Drawing.Point(101, 23);
            this.tbTableZ.Name = "tbTableZ";
            this.tbTableZ.Size = new System.Drawing.Size(42, 20);
            this.tbTableZ.TabIndex = 37;
            this.tbTableZ.Text = "0";
            // 
            // tbTableY
            // 
            this.tbTableY.Location = new System.Drawing.Point(57, 23);
            this.tbTableY.Name = "tbTableY";
            this.tbTableY.Size = new System.Drawing.Size(42, 20);
            this.tbTableY.TabIndex = 36;
            this.tbTableY.Text = "0";
            // 
            // tbTableX
            // 
            this.tbTableX.Location = new System.Drawing.Point(12, 23);
            this.tbTableX.Name = "tbTableX";
            this.tbTableX.Size = new System.Drawing.Size(42, 20);
            this.tbTableX.TabIndex = 35;
            this.tbTableX.Text = "0";
            // 
            // lblCua
            // 
            this.lblCua.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblCua.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCua.ImageIndex = 0;
            this.lblCua.ImageList = this.Leds;
            this.lblCua.Location = new System.Drawing.Point(39, 29);
            this.lblCua.Name = "lblCua";
            this.lblCua.Size = new System.Drawing.Size(141, 24);
            this.lblCua.TabIndex = 9;
            this.lblCua.Text = "OFFLINE";
            this.lblCua.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(39, 56);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(141, 36);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "CONNECT";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.OnBtnConnect_Click);
            // 
            // TabObjects
            // 
            this.TabObjects.BackColor = System.Drawing.Color.WhiteSmoke;
            this.TabObjects.Controls.Add(this.Tree);
            this.TabObjects.Controls.Add(this.PropTree);
            this.TabObjects.Controls.Add(this.tbObjects);
            this.TabObjects.Location = new System.Drawing.Point(4, 22);
            this.TabObjects.Name = "TabObjects";
            this.TabObjects.Padding = new System.Windows.Forms.Padding(3);
            this.TabObjects.Size = new System.Drawing.Size(263, 528);
            this.TabObjects.TabIndex = 0;
            this.TabObjects.Text = "Objects";
            this.TabObjects.UseVisualStyleBackColor = true;
            // 
            // Tree
            // 
            this.Tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tree.Location = new System.Drawing.Point(31, 3);
            this.Tree.Margin = new System.Windows.Forms.Padding(0);
            this.Tree.Name = "Tree";
            this.Tree.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Tree.OcxState")));
            this.Tree.Size = new System.Drawing.Size(229, 110);
            this.Tree.TabIndex = 5;
            this.Tree.Change += new System.EventHandler(this.Tree_Change);
            // 
            // PropTree
            // 
            this.PropTree.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PropTree.Location = new System.Drawing.Point(31, 113);
            this.PropTree.Name = "PropTree";
            this.PropTree.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("PropTree.OcxState")));
            this.PropTree.Size = new System.Drawing.Size(229, 412);
            this.PropTree.TabIndex = 6;
            this.PropTree.PropertyChanged += new System.EventHandler(this.Flight_PropertyChanged);
            // 
            // tbObjects
            // 
            this.tbObjects.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbObjects.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tbObjects.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tbObjects.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.tbObjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CmdRemoveObject,
            this.CmdCopy,
            this.CmdAddGroup,
            this.CmdAddBitmap,
            this.CmdAddStruct,
            this.CmdAddDrawing,
            this.CmdAddText,
            this.CmdAddCircularText,
            this.CmdAddEvent,
            this.CmdAdd3DStruct,
            this.CmdAdd3DScript,
            this.CmdAddWorkpiece});
            this.tbObjects.Location = new System.Drawing.Point(3, 3);
            this.tbObjects.Name = "tbObjects";
            this.tbObjects.Padding = new System.Windows.Forms.Padding(0);
            this.tbObjects.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tbObjects.Size = new System.Drawing.Size(28, 522);
            this.tbObjects.TabIndex = 12;
            this.tbObjects.Text = "Pens";
            this.tbObjects.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tbObjects_ItemClicked);
            // 
            // CmdRemoveObject
            // 
            this.CmdRemoveObject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdRemoveObject.Image = ((System.Drawing.Image)(resources.GetObject("CmdRemoveObject.Image")));
            this.CmdRemoveObject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdRemoveObject.Name = "CmdRemoveObject";
            this.CmdRemoveObject.Size = new System.Drawing.Size(27, 28);
            this.CmdRemoveObject.Text = "Remove Object";
            // 
            // CmdCopy
            // 
            this.CmdCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdCopy.Image = ((System.Drawing.Image)(resources.GetObject("CmdCopy.Image")));
            this.CmdCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdCopy.Name = "CmdCopy";
            this.CmdCopy.Size = new System.Drawing.Size(27, 28);
            this.CmdCopy.Text = "Duplicate Item";
            // 
            // CmdAddGroup
            // 
            this.CmdAddGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAddGroup.Image = ((System.Drawing.Image)(resources.GetObject("CmdAddGroup.Image")));
            this.CmdAddGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAddGroup.Name = "CmdAddGroup";
            this.CmdAddGroup.Size = new System.Drawing.Size(27, 28);
            this.CmdAddGroup.Text = "Add Group";
            // 
            // CmdAddBitmap
            // 
            this.CmdAddBitmap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAddBitmap.Image = ((System.Drawing.Image)(resources.GetObject("CmdAddBitmap.Image")));
            this.CmdAddBitmap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAddBitmap.Name = "CmdAddBitmap";
            this.CmdAddBitmap.Size = new System.Drawing.Size(27, 28);
            this.CmdAddBitmap.Text = "Add Bitmap";
            // 
            // CmdAddStruct
            // 
            this.CmdAddStruct.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAddStruct.Image = ((System.Drawing.Image)(resources.GetObject("CmdAddStruct.Image")));
            this.CmdAddStruct.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAddStruct.Name = "CmdAddStruct";
            this.CmdAddStruct.Size = new System.Drawing.Size(27, 28);
            this.CmdAddStruct.Text = "Add Structure";
            // 
            // CmdAddDrawing
            // 
            this.CmdAddDrawing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAddDrawing.Image = ((System.Drawing.Image)(resources.GetObject("CmdAddDrawing.Image")));
            this.CmdAddDrawing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAddDrawing.Name = "CmdAddDrawing";
            this.CmdAddDrawing.Size = new System.Drawing.Size(27, 28);
            this.CmdAddDrawing.Text = "Create Drawing";
            // 
            // CmdAddText
            // 
            this.CmdAddText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAddText.Image = ((System.Drawing.Image)(resources.GetObject("CmdAddText.Image")));
            this.CmdAddText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAddText.Name = "CmdAddText";
            this.CmdAddText.Size = new System.Drawing.Size(27, 28);
            this.CmdAddText.Text = "Add Text";
            // 
            // CmdAddCircularText
            // 
            this.CmdAddCircularText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAddCircularText.Image = ((System.Drawing.Image)(resources.GetObject("CmdAddCircularText.Image")));
            this.CmdAddCircularText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAddCircularText.Name = "CmdAddCircularText";
            this.CmdAddCircularText.Size = new System.Drawing.Size(27, 28);
            this.CmdAddCircularText.Text = "Add Circular Text";
            // 
            // CmdAddEvent
            // 
            this.CmdAddEvent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAddEvent.Image = ((System.Drawing.Image)(resources.GetObject("CmdAddEvent.Image")));
            this.CmdAddEvent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAddEvent.Name = "CmdAddEvent";
            this.CmdAddEvent.Size = new System.Drawing.Size(27, 28);
            this.CmdAddEvent.Text = "Add Event";
            // 
            // CmdAdd3DStruct
            // 
            this.CmdAdd3DStruct.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAdd3DStruct.Image = ((System.Drawing.Image)(resources.GetObject("CmdAdd3DStruct.Image")));
            this.CmdAdd3DStruct.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAdd3DStruct.Name = "CmdAdd3DStruct";
            this.CmdAdd3DStruct.Size = new System.Drawing.Size(27, 28);
            this.CmdAdd3DStruct.Text = "Add 3D Structure";
            // 
            // CmdAdd3DScript
            // 
            this.CmdAdd3DScript.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAdd3DScript.Image = ((System.Drawing.Image)(resources.GetObject("CmdAdd3DScript.Image")));
            this.CmdAdd3DScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAdd3DScript.Name = "CmdAdd3DScript";
            this.CmdAdd3DScript.Size = new System.Drawing.Size(27, 28);
            this.CmdAdd3DScript.Text = "Add 3D Script";
            this.CmdAdd3DScript.ToolTipText = "Add 3D G-code";
            // 
            // CmdAddWorkpiece
            // 
            this.CmdAddWorkpiece.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CmdAddWorkpiece.Image = ((System.Drawing.Image)(resources.GetObject("CmdAddWorkpiece.Image")));
            this.CmdAddWorkpiece.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CmdAddWorkpiece.Name = "CmdAddWorkpiece";
            this.CmdAddWorkpiece.Size = new System.Drawing.Size(27, 28);
            this.CmdAddWorkpiece.Text = "ToolStripButton7";
            this.CmdAddWorkpiece.ToolTipText = "Add Workpiece";
            // 
            // FlightMarker
            // 
            this.FlightMarker.Location = new System.Drawing.Point(235, 78);
            this.FlightMarker.Name = "FlightMarker";
            this.FlightMarker.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("FlightMarker.OcxState")));
            this.FlightMarker.Size = new System.Drawing.Size(69, 65);
            this.FlightMarker.TabIndex = 27;
            this.FlightMarker.Visible = false;
            this.FlightMarker.MarkingFinished += new AxFlightLib._DFlightMarkerEvents_MarkingFinishedEventHandler(this.FlightMarker_MarkingFinished);
            this.FlightMarker.SimulationFinished += new AxFlightLib._DFlightMarkerEvents_SimulationFinishedEventHandler(this.FlightMarker_SimulationFinished);
            this.FlightMarker.ProcessFinished += new AxFlightLib._DFlightMarkerEvents_ProcessFinishedEventHandler(this.FlightMarker_ProcessFinished);
            this.FlightMarker.MillToolStep += new AxFlightLib._DFlightMarkerEvents_MillToolStepEventHandler(this.FlightMarker_MillToolStep);
            // 
            // Editor
            // 
            this.Editor.Enabled = true;
            this.Editor.Location = new System.Drawing.Point(86, 25);
            this.Editor.Name = "Editor";
            this.Editor.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Editor.OcxState")));
            this.Editor.Size = new System.Drawing.Size(94, 99);
            this.Editor.TabIndex = 28;
            this.Editor.Visible = false;
            // 
            // Flight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 633);
            this.Controls.Add(this.SplitView);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.tbDocument);
            this.Controls.Add(this.menuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Flight";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Flight Marker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Flight_FormClosing);
            this.Load += new System.EventHandler(this.Flight_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Flight_KeyDown);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.tbDocument.ResumeLayout(false);
            this.tbDocument.PerformLayout();
            this.SplitView.Panel1.ResumeLayout(false);
            this.SplitView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitView)).EndInit();
            this.SplitView.ResumeLayout(false);
            this.FlightTab.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropColors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Colors)).EndInit();
            this.tbColors.ResumeLayout(false);
            this.tbColors.PerformLayout();
            this.TabPage2.ResumeLayout(false);
            this.TabPage2.PerformLayout();
            this.tbPens.ResumeLayout(false);
            this.tbPens.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropPens)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pens)).EndInit();
            this.TabPage3.ResumeLayout(false);
            this.TabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropFonts)).EndInit();
            this.tbFonts.ResumeLayout(false);
            this.tbFonts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Fonts)).EndInit();
            this.TabPage4.ResumeLayout(false);
            this.TabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropLayouts)).EndInit();
            this.tbLayouts.ResumeLayout(false);
            this.tbLayouts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Layouts)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tbSurface.ResumeLayout(false);
            this.tbSurface.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropSurfaces)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Surfaces)).EndInit();
            this.splitView2.Panel1.ResumeLayout(false);
            this.splitView2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitView2)).EndInit();
            this.splitView2.ResumeLayout(false);
            this.tabViews.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.View)).EndInit();
            this.tabPage7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.View3D)).EndInit();
            this.FlightTab2.ResumeLayout(false);
            this.tabHardware.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gpTable.ResumeLayout(false);
            this.gpTable.PerformLayout();
            this.TabObjects.ResumeLayout(false);
            this.TabObjects.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropTree)).EndInit();
            this.tbObjects.ResumeLayout(false);
            this.tbObjects.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FlightMarker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Editor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		internal System.Windows.Forms.Timer Timer;
		private System.Windows.Forms.MenuStrip menuMain;
		private System.Windows.Forms.ToolStripMenuItem menuView;
		private System.Windows.Forms.ToolStripMenuItem menuToolbar;
		private System.Windows.Forms.ToolStripMenuItem menuStatusBar;
		private System.Windows.Forms.ToolStripMenuItem menuSettings;
		private System.Windows.Forms.ToolStripMenuItem menuSettingsEdit;
		private System.Windows.Forms.ToolStripMenuItem menuHelp;
		private System.Windows.Forms.ToolStripMenuItem menuAbout;
		internal System.Windows.Forms.StatusStrip StatusBar;
		internal System.Windows.Forms.ToolStripStatusLabel LabelCoX;
		internal System.Windows.Forms.ToolStripStatusLabel LabelCoY;
		internal System.Windows.Forms.ToolStripStatusLabel lblTime;
		private System.Windows.Forms.SplitContainer SplitView;
		internal AxFlightLib.AxFlightMarker FlightMarker;
		internal AxFlightLib.AxFlightEditor Editor;
		private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
		private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
		private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
		private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
		private System.Windows.Forms.ToolStripContentPanel ContentPanel;
		internal System.Windows.Forms.ToolStrip tbDocument;
		internal System.Windows.Forms.ToolStripButton tbNew;
		internal System.Windows.Forms.ToolStripButton tbOpen;
		internal System.Windows.Forms.ToolStripButton tbSave;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
		internal System.Windows.Forms.ToolStripButton tbRefresh;
		internal System.Windows.Forms.ToolStripButton tbMark;
		internal System.Windows.Forms.ToolStripButton tbAbort;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator5;
        internal System.Windows.Forms.ToolStripButton tbSelect;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator4;
		internal System.Windows.Forms.ToolStripButton tbEditor;
		internal System.Windows.Forms.TabControl FlightTab2;
		internal System.Windows.Forms.TabPage TabObjects;
		internal AxFlightLib.AxFlightTree Tree;
		internal AxFlightLib.AxFlightPropList PropTree;
		internal System.Windows.Forms.ToolStrip tbObjects;
		internal System.Windows.Forms.ToolStripButton CmdAddBitmap;
		internal System.Windows.Forms.ToolStripButton CmdAddStruct;
		internal System.Windows.Forms.ToolStripButton CmdAddDrawing;
		internal System.Windows.Forms.ToolStripButton CmdAddText;
		internal System.Windows.Forms.ToolStripButton CmdAddCircularText;
		internal System.Windows.Forms.ToolStripButton CmdAddEvent;
		internal System.Windows.Forms.ToolStripButton CmdAddGroup;
		internal System.Windows.Forms.ToolStripButton CmdRemoveObject;
		internal System.Windows.Forms.TabControl FlightTab;
		internal System.Windows.Forms.TabPage TabPage1;
		internal AxFlightLib.AxFlightPropList PropColors;
		internal AxFlightLib.AxFlightColors Colors;
		internal System.Windows.Forms.ToolStrip tbColors;
		internal System.Windows.Forms.ToolStripButton CmdAddColor;
		internal System.Windows.Forms.ToolStripButton CmdRemoveColor;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
		internal System.Windows.Forms.ToolStripButton CmdChangeColor;
		internal System.Windows.Forms.TabPage TabPage2;
		internal System.Windows.Forms.ToolStrip tbPens;
		internal System.Windows.Forms.ToolStripButton CmdAddPen;
		internal System.Windows.Forms.ToolStripButton CmdRemovePen;
		internal AxFlightLib.AxFlightPropList PropPens;
		internal AxFlightLib.AxFlightPens Pens;
		internal System.Windows.Forms.TabPage TabPage3;
		internal AxFlightLib.AxFlightPropList PropFonts;
		internal System.Windows.Forms.ToolStrip tbFonts;
		internal System.Windows.Forms.ToolStripButton CmdAddTrueTypeFont;
		internal System.Windows.Forms.ToolStripButton CmdAddSinglestrokeFont;
		internal System.Windows.Forms.ToolStripButton CmdAddBarcodeFont;
		internal System.Windows.Forms.ToolStripButton CmdRemoveFont;
		internal AxFlightLib.AxFlightFonts Fonts;
		internal System.Windows.Forms.TabPage TabPage4;
		internal AxFlightLib.AxFlightPropList PropLayouts;
		internal System.Windows.Forms.ToolStrip tbLayouts;
		internal System.Windows.Forms.ToolStripButton CmdAddLayout;
		internal System.Windows.Forms.ToolStripButton CmdRemoveLayout;
		internal AxFlightLib.AxFlightLayouts Layouts;
		private System.Windows.Forms.ToolStripMenuItem menuFile;
		private System.Windows.Forms.ToolStripMenuItem menuNew;
		private System.Windows.Forms.ToolStripMenuItem menuOpen;
		private System.Windows.Forms.ToolStripSeparator menuSeparator1;
		private System.Windows.Forms.ToolStripMenuItem menuSave;
		private System.Windows.Forms.ToolStripMenuItem menuSaveAs;
		private System.Windows.Forms.ToolStripSeparator menuSeperator2;
		private System.Windows.Forms.ToolStripMenuItem menuExit;
		private System.Windows.Forms.ToolStripSeparator menuSeparator3;
		private System.Windows.Forms.ToolStripMenuItem menuSelect;
		private System.Windows.Forms.ToolStripMenuItem menuPan;
		private System.Windows.Forms.ToolStripMenuItem menuZoomIn;
		private System.Windows.Forms.ToolStripMenuItem menuZoomOut;
		private System.Windows.Forms.ToolStripSeparator menuSeparator4;
		private System.Windows.Forms.ToolStripMenuItem menuRefresh;
        private System.Windows.Forms.ToolStripButton CmdCopy;
		private System.Windows.Forms.SplitContainer splitView2;
        private AxFlightLib.AxFlightView View;
		internal System.Windows.Forms.ToolStripButton tbInfinite;
		internal System.Windows.Forms.ToolStripButton tbUpload;
		internal System.Windows.Forms.ToolStripButton tbFlashExplorer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		internal System.Windows.Forms.ImageList Leds;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripButton tbSettings;
        private System.Windows.Forms.ToolStripMenuItem menuCompile;
        private System.Windows.Forms.ToolStripMenuItem menuSplit;
        private System.Windows.Forms.ToolStripMenuItem menuRestore;
        internal System.Windows.Forms.ImageList TabulaImg;
        internal System.Windows.Forms.ToolStripStatusLabel lblTableX;
        internal System.Windows.Forms.ToolStripStatusLabel lblTableY;
        internal System.Windows.Forms.ToolStripStatusLabel lblTableZ;
        private System.Windows.Forms.TabPage tabPage5;
        private AxFlightLib.AxFlightPropList PropSurfaces;
        private AxFlightLib.AxFlightSurfaces Surfaces;
        internal System.Windows.Forms.ToolStrip tbSurface;
        internal System.Windows.Forms.ToolStripButton CmdAddSurface;
        internal System.Windows.Forms.ToolStripButton CmdRemoveSurface;
        private System.Windows.Forms.TabControl tabViews;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
        private AxFlightLib.AxFlightView3D View3D;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripButton tbEdit;
        private System.Windows.Forms.ToolStripButton tbSimulate;
        internal System.Windows.Forms.ToolStripButton tbPan;
        internal System.Windows.Forms.ToolStripButton tbRotate;
        internal System.Windows.Forms.ToolStripButton tbZoom;
        internal System.Windows.Forms.ToolStripButton tbZoomIn;
        internal System.Windows.Forms.ToolStripButton tbZoomOut;
        internal System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        internal System.Windows.Forms.ToolStripButton tbViewSelection;
        internal System.Windows.Forms.ToolStripButton tbGrid;
        internal System.Windows.Forms.ToolStripButton tbTopView;
        internal System.Windows.Forms.ToolStripButton tbFrontView;
        internal System.Windows.Forms.ToolStripButton tbSideView;
        internal System.Windows.Forms.ToolStripButton CmdAdd3DStruct;
        internal System.Windows.Forms.ToolStripStatusLabel lblTool;
        internal System.Windows.Forms.ToolStripButton CmdAddWorkpiece;
        private System.Windows.Forms.ToolStripStatusLabel lblScannerX;
        private System.Windows.Forms.ToolStripStatusLabel lblScannerY;
        private System.Windows.Forms.ToolStripStatusLabel lblScannerZ;
        internal System.Windows.Forms.ToolStripButton CmdAdd3DScript;
        private System.Windows.Forms.ToolStripButton tbProperties;
        private System.Windows.Forms.ToolStripMenuItem menuPropertiesEdit;
        internal System.Windows.Forms.ToolStripButton CmdEditLayout;
        private System.Windows.Forms.TabPage tabHardware;
        private System.Windows.Forms.GroupBox gpTable;
        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.Button btnTableUnit1;
        private System.Windows.Forms.Button btnTableUnit2;
        private System.Windows.Forms.Label lbTableUnit;
        internal System.Windows.Forms.Button btnTableHome;
        internal System.Windows.Forms.Button btnTableZ1;
        internal System.Windows.Forms.Button btnTableZ2;
        internal System.Windows.Forms.Button btnTableX2;
        internal System.Windows.Forms.Button btnTableX1;
        internal System.Windows.Forms.Button btnTableY2;
        internal System.Windows.Forms.Button btnTableY1;
        private System.Windows.Forms.Button btnTableGoTo;
        private System.Windows.Forms.TextBox tbTableZ;
        private System.Windows.Forms.TextBox tbTableY;
        private System.Windows.Forms.TextBox tbTableX;
        private System.Windows.Forms.Label lblCua;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ToolStripButton tbCalibrate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.Label lbLS2X;
        private System.Windows.Forms.Label lbLS1X;
        private System.Windows.Forms.Label lbLS2Z;
        private System.Windows.Forms.Label lbLS1Y;
        private System.Windows.Forms.Label lbLS2Y;
        private System.Windows.Forms.Label lbLS1Z;
        private System.Windows.Forms.ImageList ImgBars;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;

	}
}

