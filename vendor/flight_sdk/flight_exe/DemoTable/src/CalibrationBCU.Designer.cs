namespace Flight
{
    partial class CalibrationBCU
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalibrationBCU));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label31 = new System.Windows.Forms.Label();
            this.cbErrField = new System.Windows.Forms.ComboBox();
            this.cbLocked = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.tabCalSettings = new System.Windows.Forms.TabPage();
            this.layoutCalSettings = new System.Windows.Forms.TableLayoutPanel();
            this.tbCols = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbRows = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbHeight = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbWidth = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnStoreCalibration = new System.Windows.Forms.Button();
            this.btnLoadCalibration = new System.Windows.Forms.Button();
            this.btnResetCalibration = new System.Windows.Forms.Button();
            this.tabTableSettings = new System.Windows.Forms.TabPage();
            this.btnTableCalRef = new System.Windows.Forms.Button();
            this.imgJog = new System.Windows.Forms.ImageList(this.components);
            this.btnSensor = new System.Windows.Forms.Button();
            this.btnSetRef = new System.Windows.Forms.Button();
            this.btnTableUnit1 = new System.Windows.Forms.Button();
            this.btnTableUnit2 = new System.Windows.Forms.Button();
            this.lbTableUnit = new System.Windows.Forms.Label();
            this.btnTableZplus = new System.Windows.Forms.Button();
            this.btnTableZmin = new System.Windows.Forms.Button();
            this.btnTableXplus = new System.Windows.Forms.Button();
            this.btnTableXmin = new System.Windows.Forms.Button();
            this.btnTableYmin = new System.Windows.Forms.Button();
            this.btnTableYplus = new System.Windows.Forms.Button();
            this.lblCalRefZ = new System.Windows.Forms.Label();
            this.lblCalRefX = new System.Windows.Forms.Label();
            this.lblCalRefY = new System.Windows.Forms.Label();
            this.panelView = new System.Windows.Forms.Panel();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblOffsMaxXY = new System.Windows.Forms.Label();
            this.lblOffsXY = new System.Windows.Forms.Label();
            this.lblOffsMaxY = new System.Windows.Forms.Label();
            this.lblOffsMaxX = new System.Windows.Forms.Label();
            this.lblOffsY = new System.Windows.Forms.Label();
            this.lblOffsX = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.lblBat = new System.Windows.Forms.Label();
            this.imgBattery = new System.Windows.Forms.ImageList(this.components);
            this.label17 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.lblSensor = new System.Windows.Forms.Label();
            this.imgSensor = new System.Windows.Forms.ImageList(this.components);
            this.timerCmd = new System.Windows.Forms.Timer(this.components);
            this.label16 = new System.Windows.Forms.Label();
            this.layoutLegend = new System.Windows.Forms.TableLayoutPanel();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblRemaining = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.pgCalibration = new System.Windows.Forms.ProgressBar();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabCalSettings.SuspendLayout();
            this.layoutCalSettings.SuspendLayout();
            this.tabTableSettings.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.layoutLegend.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabCalSettings);
            this.tabControl1.Controls.Add(this.tabTableSettings);
            this.tabControl1.Location = new System.Drawing.Point(3, 354);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(357, 194);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label31);
            this.tabPage1.Controls.Add(this.cbErrField);
            this.tabPage1.Controls.Add(this.cbLocked);
            this.tabPage1.Controls.Add(this.btnOK);
            this.tabPage1.Controls.Add(this.btnAbort);
            this.tabPage1.Controls.Add(this.btnRun);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(349, 168);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Control";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label31
            // 
            this.label31.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(18, 91);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(77, 13);
            this.label31.TabIndex = 23;
            this.label31.Text = "Error Field Def:";
            // 
            // cbErrField
            // 
            this.cbErrField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbErrField.FormattingEnabled = true;
            this.cbErrField.Items.AddRange(new object[] {
            "Rectangular",
            "Circular"});
            this.cbErrField.Location = new System.Drawing.Point(101, 88);
            this.cbErrField.Name = "cbErrField";
            this.cbErrField.Size = new System.Drawing.Size(110, 21);
            this.cbErrField.TabIndex = 22;
            this.cbErrField.SelectedIndexChanged += new System.EventHandler(this.cbErrField_SelectedIndexChanged);
            // 
            // cbLocked
            // 
            this.cbLocked.AutoSize = true;
            this.cbLocked.Location = new System.Drawing.Point(21, 29);
            this.cbLocked.Name = "cbLocked";
            this.cbLocked.Size = new System.Drawing.Size(97, 17);
            this.cbLocked.TabIndex = 3;
            this.cbLocked.Text = "Measuring only";
            this.cbLocked.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(269, 110);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(74, 42);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "Exit";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnAbort
            // 
            this.btnAbort.Location = new System.Drawing.Point(269, 63);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(74, 41);
            this.btnAbort.TabIndex = 1;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(269, 16);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(74, 41);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // tabCalSettings
            // 
            this.tabCalSettings.Controls.Add(this.layoutCalSettings);
            this.tabCalSettings.Controls.Add(this.btnStoreCalibration);
            this.tabCalSettings.Controls.Add(this.btnLoadCalibration);
            this.tabCalSettings.Controls.Add(this.btnResetCalibration);
            this.tabCalSettings.Location = new System.Drawing.Point(4, 22);
            this.tabCalSettings.Name = "tabCalSettings";
            this.tabCalSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabCalSettings.Size = new System.Drawing.Size(349, 168);
            this.tabCalSettings.TabIndex = 1;
            this.tabCalSettings.Text = "Calibration Settings";
            this.tabCalSettings.UseVisualStyleBackColor = true;
            // 
            // layoutCalSettings
            // 
            this.layoutCalSettings.ColumnCount = 2;
            this.layoutCalSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutCalSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutCalSettings.Controls.Add(this.tbCols, 1, 3);
            this.layoutCalSettings.Controls.Add(this.label7, 0, 0);
            this.layoutCalSettings.Controls.Add(this.tbRows, 1, 2);
            this.layoutCalSettings.Controls.Add(this.label6, 0, 1);
            this.layoutCalSettings.Controls.Add(this.tbHeight, 1, 1);
            this.layoutCalSettings.Controls.Add(this.label9, 0, 3);
            this.layoutCalSettings.Controls.Add(this.tbWidth, 1, 0);
            this.layoutCalSettings.Controls.Add(this.label8, 0, 2);
            this.layoutCalSettings.Location = new System.Drawing.Point(22, 19);
            this.layoutCalSettings.Name = "layoutCalSettings";
            this.layoutCalSettings.RowCount = 5;
            this.layoutCalSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.layoutCalSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.layoutCalSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.layoutCalSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.layoutCalSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.layoutCalSettings.Size = new System.Drawing.Size(207, 141);
            this.layoutCalSettings.TabIndex = 20;
            // 
            // tbCols
            // 
            this.tbCols.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbCols.Location = new System.Drawing.Point(106, 88);
            this.tbCols.Name = "tbCols";
            this.tbCols.Size = new System.Drawing.Size(74, 20);
            this.tbCols.TabIndex = 12;
            this.tbCols.Text = "3";
            this.tbCols.TextChanged += new System.EventHandler(this.CalMsr_TextChanged);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Field Width:";
            // 
            // tbRows
            // 
            this.tbRows.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbRows.Location = new System.Drawing.Point(106, 60);
            this.tbRows.Name = "tbRows";
            this.tbRows.Size = new System.Drawing.Size(74, 20);
            this.tbRows.TabIndex = 11;
            this.tbRows.Text = "3";
            this.tbRows.TextChanged += new System.EventHandler(this.CalMsr_TextChanged);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Field Height:";
            // 
            // tbHeight
            // 
            this.tbHeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbHeight.Location = new System.Drawing.Point(106, 32);
            this.tbHeight.Name = "tbHeight";
            this.tbHeight.Size = new System.Drawing.Size(74, 20);
            this.tbHeight.TabIndex = 8;
            this.tbHeight.Text = "100";
            this.tbHeight.TextChanged += new System.EventHandler(this.CalMsr_TextChanged);
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 91);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Columns:";
            // 
            // tbWidth
            // 
            this.tbWidth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbWidth.Location = new System.Drawing.Point(106, 4);
            this.tbWidth.Name = "tbWidth";
            this.tbWidth.Size = new System.Drawing.Size(74, 20);
            this.tbWidth.TabIndex = 7;
            this.tbWidth.Text = "100";
            this.tbWidth.TextChanged += new System.EventHandler(this.CalMsr_TextChanged);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Rows:";
            // 
            // btnStoreCalibration
            // 
            this.btnStoreCalibration.Location = new System.Drawing.Point(269, 110);
            this.btnStoreCalibration.Name = "btnStoreCalibration";
            this.btnStoreCalibration.Size = new System.Drawing.Size(74, 41);
            this.btnStoreCalibration.TabIndex = 19;
            this.btnStoreCalibration.Text = "Store Calibration";
            this.btnStoreCalibration.UseVisualStyleBackColor = true;
            this.btnStoreCalibration.Click += new System.EventHandler(this.btnStoreCalibration_Click);
            // 
            // btnLoadCalibration
            // 
            this.btnLoadCalibration.Location = new System.Drawing.Point(269, 16);
            this.btnLoadCalibration.Name = "btnLoadCalibration";
            this.btnLoadCalibration.Size = new System.Drawing.Size(74, 41);
            this.btnLoadCalibration.TabIndex = 18;
            this.btnLoadCalibration.Text = "Load Calibration";
            this.btnLoadCalibration.UseVisualStyleBackColor = true;
            this.btnLoadCalibration.Click += new System.EventHandler(this.btnLoadCalibration_Click);
            // 
            // btnResetCalibration
            // 
            this.btnResetCalibration.Location = new System.Drawing.Point(269, 63);
            this.btnResetCalibration.Name = "btnResetCalibration";
            this.btnResetCalibration.Size = new System.Drawing.Size(74, 41);
            this.btnResetCalibration.TabIndex = 17;
            this.btnResetCalibration.Text = "Reset Calibration";
            this.btnResetCalibration.UseVisualStyleBackColor = true;
            this.btnResetCalibration.Click += new System.EventHandler(this.btnResetCalibration_Click);
            // 
            // tabTableSettings
            // 
            this.tabTableSettings.Controls.Add(this.btnTableCalRef);
            this.tabTableSettings.Controls.Add(this.btnSensor);
            this.tabTableSettings.Controls.Add(this.btnSetRef);
            this.tabTableSettings.Controls.Add(this.btnTableUnit1);
            this.tabTableSettings.Controls.Add(this.btnTableUnit2);
            this.tabTableSettings.Controls.Add(this.lbTableUnit);
            this.tabTableSettings.Controls.Add(this.btnTableZplus);
            this.tabTableSettings.Controls.Add(this.btnTableZmin);
            this.tabTableSettings.Controls.Add(this.btnTableXplus);
            this.tabTableSettings.Controls.Add(this.btnTableXmin);
            this.tabTableSettings.Controls.Add(this.btnTableYmin);
            this.tabTableSettings.Controls.Add(this.btnTableYplus);
            this.tabTableSettings.Controls.Add(this.lblCalRefZ);
            this.tabTableSettings.Controls.Add(this.lblCalRefX);
            this.tabTableSettings.Controls.Add(this.lblCalRefY);
            this.tabTableSettings.Location = new System.Drawing.Point(4, 22);
            this.tabTableSettings.Name = "tabTableSettings";
            this.tabTableSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabTableSettings.Size = new System.Drawing.Size(349, 168);
            this.tabTableSettings.TabIndex = 2;
            this.tabTableSettings.Text = "Table";
            this.tabTableSettings.UseVisualStyleBackColor = true;
            // 
            // btnTableCalRef
            // 
            this.btnTableCalRef.ImageIndex = 8;
            this.btnTableCalRef.ImageList = this.imgJog;
            this.btnTableCalRef.Location = new System.Drawing.Point(239, 64);
            this.btnTableCalRef.Name = "btnTableCalRef";
            this.btnTableCalRef.Size = new System.Drawing.Size(42, 42);
            this.btnTableCalRef.TabIndex = 62;
            this.btnTableCalRef.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnTableCalRef.UseVisualStyleBackColor = true;
            this.btnTableCalRef.Click += new System.EventHandler(this.btnTableCalRef_Click);
            // 
            // imgJog
            // 
            this.imgJog.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgJog.ImageStream")));
            this.imgJog.TransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(163)))), ((int)(((byte)(177)))));
            this.imgJog.Images.SetKeyName(0, "Reference.bmp");
            this.imgJog.Images.SetKeyName(1, "Stop.bmp");
            this.imgJog.Images.SetKeyName(2, "Up.bmp");
            this.imgJog.Images.SetKeyName(3, "Down.bmp");
            this.imgJog.Images.SetKeyName(4, "Left.bmp");
            this.imgJog.Images.SetKeyName(5, "Right.bmp");
            this.imgJog.Images.SetKeyName(6, "IconHome.bmp");
            this.imgJog.Images.SetKeyName(7, "IconP1.bmp");
            this.imgJog.Images.SetKeyName(8, "IconRef.bmp");
            // 
            // btnSensor
            // 
            this.btnSensor.Location = new System.Drawing.Point(6, 125);
            this.btnSensor.Name = "btnSensor";
            this.btnSensor.Size = new System.Drawing.Size(117, 27);
            this.btnSensor.TabIndex = 61;
            this.btnSensor.Text = "Scan Sensor";
            this.btnSensor.UseVisualStyleBackColor = true;
            this.btnSensor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSensor_MouseDown);
            this.btnSensor.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSensor_MouseUp);
            // 
            // btnSetRef
            // 
            this.btnSetRef.Location = new System.Drawing.Point(6, 92);
            this.btnSetRef.Name = "btnSetRef";
            this.btnSetRef.Size = new System.Drawing.Size(117, 27);
            this.btnSetRef.TabIndex = 60;
            this.btnSetRef.Text = "Update Reference";
            this.btnSetRef.UseVisualStyleBackColor = true;
            this.btnSetRef.Click += new System.EventHandler(this.btnSetRef_Click);
            // 
            // btnTableUnit1
            // 
            this.btnTableUnit1.Location = new System.Drawing.Point(194, 43);
            this.btnTableUnit1.Name = "btnTableUnit1";
            this.btnTableUnit1.Size = new System.Drawing.Size(20, 20);
            this.btnTableUnit1.TabIndex = 58;
            this.btnTableUnit1.Text = "-";
            this.btnTableUnit1.UseVisualStyleBackColor = true;
            this.btnTableUnit1.Click += new System.EventHandler(this.btnTableUnit1_Click);
            // 
            // btnTableUnit2
            // 
            this.btnTableUnit2.Location = new System.Drawing.Point(216, 43);
            this.btnTableUnit2.Name = "btnTableUnit2";
            this.btnTableUnit2.Size = new System.Drawing.Size(20, 20);
            this.btnTableUnit2.TabIndex = 57;
            this.btnTableUnit2.Text = "+";
            this.btnTableUnit2.UseVisualStyleBackColor = true;
            this.btnTableUnit2.Click += new System.EventHandler(this.btnTableUnit2_Click);
            // 
            // lbTableUnit
            // 
            this.lbTableUnit.Location = new System.Drawing.Point(194, 25);
            this.lbTableUnit.Name = "lbTableUnit";
            this.lbTableUnit.Size = new System.Drawing.Size(42, 15);
            this.lbTableUnit.TabIndex = 56;
            this.lbTableUnit.Text = "1.0";
            this.lbTableUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnTableZplus
            // 
            this.btnTableZplus.ImageIndex = 2;
            this.btnTableZplus.ImageList = this.imgJog;
            this.btnTableZplus.Location = new System.Drawing.Point(283, 21);
            this.btnTableZplus.Name = "btnTableZplus";
            this.btnTableZplus.Size = new System.Drawing.Size(42, 42);
            this.btnTableZplus.TabIndex = 55;
            this.btnTableZplus.Text = "Z";
            this.btnTableZplus.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnTableZplus.UseVisualStyleBackColor = true;
            this.btnTableZplus.Click += new System.EventHandler(this.btnTableZplus_Click);
            // 
            // btnTableZmin
            // 
            this.btnTableZmin.ImageIndex = 3;
            this.btnTableZmin.ImageList = this.imgJog;
            this.btnTableZmin.Location = new System.Drawing.Point(283, 108);
            this.btnTableZmin.Name = "btnTableZmin";
            this.btnTableZmin.Size = new System.Drawing.Size(42, 42);
            this.btnTableZmin.TabIndex = 54;
            this.btnTableZmin.Text = "Z";
            this.btnTableZmin.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnTableZmin.UseVisualStyleBackColor = true;
            this.btnTableZmin.Click += new System.EventHandler(this.btnTableZmin_Click);
            // 
            // btnTableXplus
            // 
            this.btnTableXplus.ImageIndex = 5;
            this.btnTableXplus.ImageList = this.imgJog;
            this.btnTableXplus.Location = new System.Drawing.Point(283, 64);
            this.btnTableXplus.Name = "btnTableXplus";
            this.btnTableXplus.Size = new System.Drawing.Size(42, 42);
            this.btnTableXplus.TabIndex = 53;
            this.btnTableXplus.Text = "X";
            this.btnTableXplus.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnTableXplus.UseVisualStyleBackColor = true;
            this.btnTableXplus.Click += new System.EventHandler(this.btnTableXplus_Click);
            // 
            // btnTableXmin
            // 
            this.btnTableXmin.ImageIndex = 4;
            this.btnTableXmin.ImageList = this.imgJog;
            this.btnTableXmin.Location = new System.Drawing.Point(194, 64);
            this.btnTableXmin.Name = "btnTableXmin";
            this.btnTableXmin.Size = new System.Drawing.Size(42, 42);
            this.btnTableXmin.TabIndex = 52;
            this.btnTableXmin.Text = "X";
            this.btnTableXmin.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnTableXmin.UseVisualStyleBackColor = true;
            this.btnTableXmin.Click += new System.EventHandler(this.btnTableXmin_Click);
            // 
            // btnTableYmin
            // 
            this.btnTableYmin.ImageIndex = 3;
            this.btnTableYmin.ImageList = this.imgJog;
            this.btnTableYmin.Location = new System.Drawing.Point(239, 108);
            this.btnTableYmin.Name = "btnTableYmin";
            this.btnTableYmin.Size = new System.Drawing.Size(42, 42);
            this.btnTableYmin.TabIndex = 51;
            this.btnTableYmin.Text = "Y";
            this.btnTableYmin.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnTableYmin.UseVisualStyleBackColor = true;
            this.btnTableYmin.Click += new System.EventHandler(this.btnTableYmin_Click);
            // 
            // btnTableYplus
            // 
            this.btnTableYplus.ImageIndex = 2;
            this.btnTableYplus.ImageList = this.imgJog;
            this.btnTableYplus.Location = new System.Drawing.Point(239, 21);
            this.btnTableYplus.Name = "btnTableYplus";
            this.btnTableYplus.Size = new System.Drawing.Size(42, 42);
            this.btnTableYplus.TabIndex = 50;
            this.btnTableYplus.Text = "Y";
            this.btnTableYplus.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnTableYplus.UseVisualStyleBackColor = true;
            this.btnTableYplus.Click += new System.EventHandler(this.btnTableYplus_Click);
            // 
            // lblCalRefZ
            // 
            this.lblCalRefZ.AutoSize = true;
            this.lblCalRefZ.Location = new System.Drawing.Point(11, 64);
            this.lblCalRefZ.Name = "lblCalRefZ";
            this.lblCalRefZ.Size = new System.Drawing.Size(37, 13);
            this.lblCalRefZ.TabIndex = 2;
            this.lblCalRefZ.Text = "Ref Z:";
            // 
            // lblCalRefX
            // 
            this.lblCalRefX.AutoSize = true;
            this.lblCalRefX.Location = new System.Drawing.Point(11, 21);
            this.lblCalRefX.Name = "lblCalRefX";
            this.lblCalRefX.Size = new System.Drawing.Size(37, 13);
            this.lblCalRefX.TabIndex = 0;
            this.lblCalRefX.Text = "Ref X:";
            // 
            // lblCalRefY
            // 
            this.lblCalRefY.AutoSize = true;
            this.lblCalRefY.Location = new System.Drawing.Point(11, 43);
            this.lblCalRefY.Name = "lblCalRefY";
            this.lblCalRefY.Size = new System.Drawing.Size(37, 13);
            this.lblCalRefY.TabIndex = 1;
            this.lblCalRefY.Text = "Ref Y:";
            // 
            // panelView
            // 
            this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelView.Location = new System.Drawing.Point(3, 3);
            this.panelView.Name = "panelView";
            this.panelView.Size = new System.Drawing.Size(525, 525);
            this.panelView.TabIndex = 1;
            this.panelView.Paint += new System.Windows.Forms.PaintEventHandler(this.panelView_Paint);
            // 
            // panelInfo
            // 
            this.panelInfo.Controls.Add(this.btnZoomOut);
            this.panelInfo.Controls.Add(this.btnZoomIn);
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInfo.Location = new System.Drawing.Point(3, 103);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(357, 245);
            this.panelInfo.TabIndex = 2;
            this.panelInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.panelInfo_Paint);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomIn.FlatAppearance.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.btnZoomIn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Navy;
            this.btnZoomIn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomIn.Location = new System.Drawing.Point(328, 220);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(22, 22);
            this.btnZoomIn.TabIndex = 0;
            this.btnZoomIn.Text = "+";
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tableLayoutPanel1);
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(357, 94);
            this.panel3.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Controls.Add(this.lblOffsMaxXY, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblOffsXY, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblOffsMaxY, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblOffsMaxX, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblOffsY, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblOffsX, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label20, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label19, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label18, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label28, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblBat, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label17, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label29, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblSensor, 4, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(357, 94);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblOffsMaxXY
            // 
            this.lblOffsMaxXY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblOffsMaxXY.AutoSize = true;
            this.lblOffsMaxXY.Location = new System.Drawing.Point(129, 75);
            this.lblOffsMaxXY.Name = "lblOffsMaxXY";
            this.lblOffsMaxXY.Size = new System.Drawing.Size(10, 13);
            this.lblOffsMaxXY.TabIndex = 19;
            this.lblOffsMaxXY.Text = "-";
            // 
            // lblOffsXY
            // 
            this.lblOffsXY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblOffsXY.AutoSize = true;
            this.lblOffsXY.Location = new System.Drawing.Point(66, 75);
            this.lblOffsXY.Name = "lblOffsXY";
            this.lblOffsXY.Size = new System.Drawing.Size(10, 13);
            this.lblOffsXY.TabIndex = 2;
            this.lblOffsXY.Text = "-";
            // 
            // lblOffsMaxY
            // 
            this.lblOffsMaxY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblOffsMaxY.AutoSize = true;
            this.lblOffsMaxY.Location = new System.Drawing.Point(129, 51);
            this.lblOffsMaxY.Name = "lblOffsMaxY";
            this.lblOffsMaxY.Size = new System.Drawing.Size(10, 13);
            this.lblOffsMaxY.TabIndex = 18;
            this.lblOffsMaxY.Text = "-";
            // 
            // lblOffsMaxX
            // 
            this.lblOffsMaxX.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblOffsMaxX.AutoSize = true;
            this.lblOffsMaxX.Location = new System.Drawing.Point(129, 28);
            this.lblOffsMaxX.Name = "lblOffsMaxX";
            this.lblOffsMaxX.Size = new System.Drawing.Size(10, 13);
            this.lblOffsMaxX.TabIndex = 2;
            this.lblOffsMaxX.Text = "-";
            // 
            // lblOffsY
            // 
            this.lblOffsY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblOffsY.AutoSize = true;
            this.lblOffsY.Location = new System.Drawing.Point(66, 51);
            this.lblOffsY.Name = "lblOffsY";
            this.lblOffsY.Size = new System.Drawing.Size(10, 13);
            this.lblOffsY.TabIndex = 1;
            this.lblOffsY.Text = "-";
            // 
            // lblOffsX
            // 
            this.lblOffsX.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblOffsX.AutoSize = true;
            this.lblOffsX.Location = new System.Drawing.Point(66, 28);
            this.lblOffsX.Name = "lblOffsX";
            this.lblOffsX.Size = new System.Drawing.Size(10, 13);
            this.lblOffsX.TabIndex = 0;
            this.lblOffsX.Text = "-";
            // 
            // label20
            // 
            this.label20.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(3, 75);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(55, 13);
            this.label20.TabIndex = 22;
            this.label20.Text = "Deviation:";
            // 
            // label19
            // 
            this.label19.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(3, 51);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(17, 13);
            this.label19.TabIndex = 21;
            this.label19.Text = "Y:\r\n";
            // 
            // label18
            // 
            this.label18.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(3, 28);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(17, 13);
            this.label18.TabIndex = 20;
            this.label18.Text = "X:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(129, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Max [µm]";
            // 
            // label28
            // 
            this.label28.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label28.Location = new System.Drawing.Point(209, 16);
            this.label28.Name = "label28";
            this.tableLayoutPanel1.SetRowSpan(this.label28, 2);
            this.label28.Size = new System.Drawing.Size(43, 13);
            this.label28.TabIndex = 10;
            this.label28.Text = "Battery:";
            // 
            // lblBat
            // 
            this.lblBat.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblBat.ImageList = this.imgBattery;
            this.lblBat.Location = new System.Drawing.Point(272, 11);
            this.lblBat.Name = "lblBat";
            this.tableLayoutPanel1.SetRowSpan(this.lblBat, 2);
            this.lblBat.Size = new System.Drawing.Size(57, 23);
            this.lblBat.TabIndex = 26;
            // 
            // imgBattery
            // 
            this.imgBattery.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgBattery.ImageStream")));
            this.imgBattery.TransparentColor = System.Drawing.Color.Transparent;
            this.imgBattery.Images.SetKeyName(0, "Battery6.png");
            this.imgBattery.Images.SetKeyName(1, "Battery1.png");
            this.imgBattery.Images.SetKeyName(2, "Battery2.png");
            this.imgBattery.Images.SetKeyName(3, "Battery3.png");
            this.imgBattery.Images.SetKeyName(4, "Battery4.png");
            this.imgBattery.Images.SetKeyName(5, "Battery5.png");
            // 
            // label17
            // 
            this.label17.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(66, 5);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(52, 13);
            this.label17.TabIndex = 19;
            this.label17.Text = "Error [µm]";
            // 
            // label29
            // 
            this.label29.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label29.Location = new System.Drawing.Point(209, 63);
            this.label29.Name = "label29";
            this.tableLayoutPanel1.SetRowSpan(this.label29, 2);
            this.label29.Size = new System.Drawing.Size(56, 13);
            this.label29.TabIndex = 11;
            this.label29.Text = "Sensor:";
            // 
            // lblSensor
            // 
            this.lblSensor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSensor.ImageIndex = 0;
            this.lblSensor.ImageList = this.imgSensor;
            this.lblSensor.Location = new System.Drawing.Point(272, 58);
            this.lblSensor.Name = "lblSensor";
            this.tableLayoutPanel1.SetRowSpan(this.lblSensor, 2);
            this.lblSensor.Size = new System.Drawing.Size(57, 23);
            this.lblSensor.TabIndex = 12;
            // 
            // imgSensor
            // 
            this.imgSensor.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgSensor.ImageStream")));
            this.imgSensor.TransparentColor = System.Drawing.Color.Transparent;
            this.imgSensor.Images.SetKeyName(0, "Sensor1.png");
            this.imgSensor.Images.SetKeyName(1, "Sensor2.png");
            this.imgSensor.Images.SetKeyName(2, "Sensor3.png");
            this.imgSensor.Images.SetKeyName(3, "Sensor4.png");
            this.imgSensor.Images.SetKeyName(4, "Sensor5.png");
            // 
            // timerCmd
            // 
            this.timerCmd.Tick += new System.EventHandler(this.timerCmd_Tick);
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label16.BackColor = System.Drawing.Color.ForestGreen;
            this.label16.Location = new System.Drawing.Point(3, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(14, 14);
            this.label16.TabIndex = 6;
            // 
            // layoutLegend
            // 
            this.layoutLegend.ColumnCount = 8;
            this.layoutLegend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLegend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutLegend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLegend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutLegend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLegend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutLegend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLegend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutLegend.Controls.Add(this.label27, 7, 0);
            this.layoutLegend.Controls.Add(this.label26, 5, 0);
            this.layoutLegend.Controls.Add(this.label25, 3, 0);
            this.layoutLegend.Controls.Add(this.label24, 1, 0);
            this.layoutLegend.Controls.Add(this.label16, 0, 0);
            this.layoutLegend.Controls.Add(this.label21, 2, 0);
            this.layoutLegend.Controls.Add(this.label22, 4, 0);
            this.layoutLegend.Controls.Add(this.label23, 6, 0);
            this.layoutLegend.Location = new System.Drawing.Point(3, 534);
            this.layoutLegend.Name = "layoutLegend";
            this.layoutLegend.RowCount = 1;
            this.layoutLegend.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLegend.Size = new System.Drawing.Size(479, 14);
            this.layoutLegend.TabIndex = 7;
            // 
            // label27
            // 
            this.label27.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(380, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(51, 13);
            this.label27.TabIndex = 14;
            this.label27.Text = ">= 20 µm";
            // 
            // label26
            // 
            this.label26.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(261, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(39, 13);
            this.label26.TabIndex = 13;
            this.label26.Text = "<20µm";
            // 
            // label25
            // 
            this.label25.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(142, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(42, 13);
            this.label25.TabIndex = 12;
            this.label25.Text = "<10 µm";
            // 
            // label24
            // 
            this.label24.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(23, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(39, 13);
            this.label24.TabIndex = 11;
            this.label24.Text = "< 5 µm";
            // 
            // label21
            // 
            this.label21.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label21.BackColor = System.Drawing.Color.Lime;
            this.label21.Location = new System.Drawing.Point(122, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(14, 14);
            this.label21.TabIndex = 7;
            // 
            // label22
            // 
            this.label22.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label22.BackColor = System.Drawing.Color.Orange;
            this.label22.Location = new System.Drawing.Point(241, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(14, 14);
            this.label22.TabIndex = 8;
            // 
            // label23
            // 
            this.label23.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label23.BackColor = System.Drawing.Color.Red;
            this.label23.Location = new System.Drawing.Point(360, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(14, 14);
            this.label23.TabIndex = 9;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tabControl1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.panelInfo, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblRemaining, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(531, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(363, 571);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // lblRemaining
            // 
            this.lblRemaining.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblRemaining.AutoSize = true;
            this.lblRemaining.Location = new System.Drawing.Point(3, 554);
            this.lblRemaining.Name = "lblRemaining";
            this.lblRemaining.Size = new System.Drawing.Size(10, 13);
            this.lblRemaining.TabIndex = 4;
            this.lblRemaining.Text = "-";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.pgCalibration, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.layoutLegend, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.panelView, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(531, 571);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // pgCalibration
            // 
            this.pgCalibration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgCalibration.Location = new System.Drawing.Point(3, 554);
            this.pgCalibration.Name = "pgCalibration";
            this.pgCalibration.Size = new System.Drawing.Size(525, 14);
            this.pgCalibration.TabIndex = 0;
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomOut.FlatAppearance.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.btnZoomOut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Navy;
            this.btnZoomOut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomOut.Location = new System.Drawing.Point(300, 220);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(22, 22);
            this.btnZoomOut.TabIndex = 1;
            this.btnZoomOut.Text = "-";
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // CalibrationBCU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 571);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "CalibrationBCU";
            this.Text = "Calibration";
            this.Load += new System.EventHandler(this.CalibrationBCU_Load);
            this.SizeChanged += new System.EventHandler(this.CalibrationBCU_SizeChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CalibrationBCU_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabCalSettings.ResumeLayout(false);
            this.layoutCalSettings.ResumeLayout(false);
            this.layoutCalSettings.PerformLayout();
            this.tabTableSettings.ResumeLayout(false);
            this.tabTableSettings.PerformLayout();
            this.panelInfo.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.layoutLegend.ResumeLayout(false);
            this.layoutLegend.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabCalSettings;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox tbHeight;
        private System.Windows.Forms.TextBox tbWidth;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.TextBox tbCols;
        private System.Windows.Forms.TextBox tbRows;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panelView;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Timer timerCmd;
        private System.Windows.Forms.Label lblOffsX;
        private System.Windows.Forms.Label lblOffsY;
        private System.Windows.Forms.Label lblOffsMaxX;
        private System.Windows.Forms.TabPage tabTableSettings;
        private System.Windows.Forms.Button btnStoreCalibration;
        private System.Windows.Forms.Button btnLoadCalibration;
        private System.Windows.Forms.Button btnResetCalibration;
        private System.Windows.Forms.Label lblCalRefZ;
        private System.Windows.Forms.Label lblCalRefY;
        private System.Windows.Forms.Label lblCalRefX;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblOffsMaxY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TableLayoutPanel layoutLegend;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TableLayoutPanel layoutCalSettings;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label lblBat;
        private System.Windows.Forms.ImageList imgBattery;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ImageList imgSensor;
        private System.Windows.Forms.Label lblSensor;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblRemaining;
        private System.Windows.Forms.ProgressBar pgCalibration;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.CheckBox cbLocked;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.ComboBox cbErrField;
        private System.Windows.Forms.Label lblOffsMaxXY;
        private System.Windows.Forms.Label lblOffsXY;
        internal System.Windows.Forms.ImageList imgJog;
        private System.Windows.Forms.Button btnSetRef;
        private System.Windows.Forms.Button btnTableUnit1;
        private System.Windows.Forms.Button btnTableUnit2;
        private System.Windows.Forms.Label lbTableUnit;
        internal System.Windows.Forms.Button btnTableZplus;
        internal System.Windows.Forms.Button btnTableZmin;
        internal System.Windows.Forms.Button btnTableXplus;
        internal System.Windows.Forms.Button btnTableXmin;
        internal System.Windows.Forms.Button btnTableYmin;
        internal System.Windows.Forms.Button btnTableYplus;
        private System.Windows.Forms.Button btnSensor;
        internal System.Windows.Forms.Button btnTableCalRef;
        private System.Windows.Forms.Button btnZoomOut;
    }
}