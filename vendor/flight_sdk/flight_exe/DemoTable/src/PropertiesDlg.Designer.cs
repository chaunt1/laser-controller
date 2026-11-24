namespace Flight
{
    partial class PropertiesDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertiesDlg));
            this.tabSettings = new System.Windows.Forms.TabControl();
            this.tabTiling = new System.Windows.Forms.TabPage();
            this.layoutAxisZ = new System.Windows.Forms.TableLayoutPanel();
            this.lblTileWidth = new System.Windows.Forms.Label();
            this.lblTileSnap = new System.Windows.Forms.Label();
            this.lblTileDepth = new System.Windows.Forms.Label();
            this.txtTileSnap = new System.Windows.Forms.TextBox();
            this.lblUnitTileDepth = new System.Windows.Forms.Label();
            this.lblTileOverlap = new System.Windows.Forms.Label();
            this.lblTileHeight = new System.Windows.Forms.Label();
            this.txtTileDepth = new System.Windows.Forms.TextBox();
            this.txtTileWidth = new System.Windows.Forms.TextBox();
            this.lblUnitTileWidth = new System.Windows.Forms.Label();
            this.txtTileHeight = new System.Windows.Forms.TextBox();
            this.txtTileOverlap = new System.Windows.Forms.TextBox();
            this.lblUnitTileHeight = new System.Windows.Forms.Label();
            this.lblUnitTileSnap = new System.Windows.Forms.Label();
            this.lblUnitStructOverlap = new System.Windows.Forms.Label();
            this.tabTable = new System.Windows.Forms.TabPage();
            this.cbTableOptimize = new System.Windows.Forms.CheckBox();
            this.cbAutoTable = new System.Windows.Forms.CheckBox();
            this.tabHybrid = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label51 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.txtTableSnapZ = new System.Windows.Forms.TextBox();
            this.label50 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.txtTableSnapX = new System.Windows.Forms.TextBox();
            this.txtTableSnapY = new System.Windows.Forms.TextBox();
            this.tabMilling = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMillTableSpeedLimit = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTableClipY = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMillTableSnapY = new System.Windows.Forms.TextBox();
            this.txtMillTileWidth = new System.Windows.Forms.TextBox();
            this.txtMillTileHeight = new System.Windows.Forms.TextBox();
            this.txtMillTableClipX = new System.Windows.Forms.TextBox();
            this.txtMillTableSnapX = new System.Windows.Forms.TextBox();
            this.cbMillSpeedProfile = new System.Windows.Forms.CheckBox();
            this.cbMillOverwrite = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label41 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label48 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label49 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.tabSettings.SuspendLayout();
            this.tabTiling.SuspendLayout();
            this.layoutAxisZ.SuspendLayout();
            this.tabTable.SuspendLayout();
            this.tabHybrid.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabMilling.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.tabTiling);
            this.tabSettings.Controls.Add(this.tabTable);
            this.tabSettings.Controls.Add(this.tabHybrid);
            this.tabSettings.Controls.Add(this.tabMilling);
            this.tabSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabSettings.Location = new System.Drawing.Point(0, 0);
            this.tabSettings.Multiline = true;
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.SelectedIndex = 0;
            this.tabSettings.Size = new System.Drawing.Size(438, 429);
            this.tabSettings.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabSettings.TabIndex = 35;
            // 
            // tabTiling
            // 
            this.tabTiling.Controls.Add(this.layoutAxisZ);
            this.tabTiling.Location = new System.Drawing.Point(4, 22);
            this.tabTiling.Name = "tabTiling";
            this.tabTiling.Padding = new System.Windows.Forms.Padding(3);
            this.tabTiling.Size = new System.Drawing.Size(430, 403);
            this.tabTiling.TabIndex = 5;
            this.tabTiling.Text = "Tiling";
            this.tabTiling.UseVisualStyleBackColor = true;
            // 
            // layoutAxisZ
            // 
            this.layoutAxisZ.ColumnCount = 3;
            this.layoutAxisZ.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutAxisZ.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutAxisZ.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.layoutAxisZ.Controls.Add(this.lblTileWidth, 0, 0);
            this.layoutAxisZ.Controls.Add(this.lblTileSnap, 0, 4);
            this.layoutAxisZ.Controls.Add(this.lblTileDepth, 0, 2);
            this.layoutAxisZ.Controls.Add(this.txtTileSnap, 1, 4);
            this.layoutAxisZ.Controls.Add(this.lblUnitTileDepth, 2, 2);
            this.layoutAxisZ.Controls.Add(this.lblTileOverlap, 0, 3);
            this.layoutAxisZ.Controls.Add(this.lblTileHeight, 0, 1);
            this.layoutAxisZ.Controls.Add(this.txtTileDepth, 1, 2);
            this.layoutAxisZ.Controls.Add(this.txtTileWidth, 1, 0);
            this.layoutAxisZ.Controls.Add(this.lblUnitTileWidth, 2, 0);
            this.layoutAxisZ.Controls.Add(this.txtTileHeight, 1, 1);
            this.layoutAxisZ.Controls.Add(this.txtTileOverlap, 1, 3);
            this.layoutAxisZ.Controls.Add(this.lblUnitTileHeight, 2, 1);
            this.layoutAxisZ.Controls.Add(this.lblUnitTileSnap, 2, 4);
            this.layoutAxisZ.Controls.Add(this.lblUnitStructOverlap, 2, 3);
            this.layoutAxisZ.Location = new System.Drawing.Point(25, 35);
            this.layoutAxisZ.Name = "layoutAxisZ";
            this.layoutAxisZ.RowCount = 5;
            this.layoutAxisZ.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.layoutAxisZ.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.layoutAxisZ.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.layoutAxisZ.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.layoutAxisZ.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.layoutAxisZ.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.layoutAxisZ.Size = new System.Drawing.Size(350, 200);
            this.layoutAxisZ.TabIndex = 64;
            // 
            // lblTileWidth
            // 
            this.lblTileWidth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTileWidth.AutoSize = true;
            this.lblTileWidth.Location = new System.Drawing.Point(3, 13);
            this.lblTileWidth.Name = "lblTileWidth";
            this.lblTileWidth.Size = new System.Drawing.Size(58, 13);
            this.lblTileWidth.TabIndex = 17;
            this.lblTileWidth.Text = "Tile Width:";
            // 
            // lblTileSnap
            // 
            this.lblTileSnap.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTileSnap.AutoSize = true;
            this.lblTileSnap.Location = new System.Drawing.Point(3, 171);
            this.lblTileSnap.Name = "lblTileSnap";
            this.lblTileSnap.Size = new System.Drawing.Size(55, 13);
            this.lblTileSnap.TabIndex = 22;
            this.lblTileSnap.Text = "Tile Snap:";
            // 
            // lblTileDepth
            // 
            this.lblTileDepth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTileDepth.AutoSize = true;
            this.lblTileDepth.Location = new System.Drawing.Point(3, 91);
            this.lblTileDepth.Name = "lblTileDepth";
            this.lblTileDepth.Size = new System.Drawing.Size(59, 13);
            this.lblTileDepth.TabIndex = 28;
            this.lblTileDepth.Text = "Tile Depth:";
            // 
            // txtTileSnap
            // 
            this.txtTileSnap.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTileSnap.Location = new System.Drawing.Point(151, 168);
            this.txtTileSnap.Name = "txtTileSnap";
            this.txtTileSnap.Size = new System.Drawing.Size(92, 20);
            this.txtTileSnap.TabIndex = 21;
            // 
            // lblUnitTileDepth
            // 
            this.lblUnitTileDepth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblUnitTileDepth.AutoSize = true;
            this.lblUnitTileDepth.Location = new System.Drawing.Point(299, 91);
            this.lblUnitTileDepth.Name = "lblUnitTileDepth";
            this.lblUnitTileDepth.Size = new System.Drawing.Size(23, 13);
            this.lblUnitTileDepth.TabIndex = 29;
            this.lblUnitTileDepth.Text = "mm";
            // 
            // lblTileOverlap
            // 
            this.lblTileOverlap.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTileOverlap.AutoSize = true;
            this.lblTileOverlap.Location = new System.Drawing.Point(3, 130);
            this.lblTileOverlap.Name = "lblTileOverlap";
            this.lblTileOverlap.Size = new System.Drawing.Size(67, 13);
            this.lblTileOverlap.TabIndex = 19;
            this.lblTileOverlap.Text = "Tile Overlap:";
            // 
            // lblTileHeight
            // 
            this.lblTileHeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTileHeight.AutoSize = true;
            this.lblTileHeight.Location = new System.Drawing.Point(3, 52);
            this.lblTileHeight.Name = "lblTileHeight";
            this.lblTileHeight.Size = new System.Drawing.Size(61, 13);
            this.lblTileHeight.TabIndex = 25;
            this.lblTileHeight.Text = "Tile Height:";
            // 
            // txtTileDepth
            // 
            this.txtTileDepth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTileDepth.Location = new System.Drawing.Point(151, 87);
            this.txtTileDepth.Name = "txtTileDepth";
            this.txtTileDepth.Size = new System.Drawing.Size(92, 20);
            this.txtTileDepth.TabIndex = 27;
            // 
            // txtTileWidth
            // 
            this.txtTileWidth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTileWidth.Location = new System.Drawing.Point(151, 9);
            this.txtTileWidth.Name = "txtTileWidth";
            this.txtTileWidth.Size = new System.Drawing.Size(92, 20);
            this.txtTileWidth.TabIndex = 15;
            // 
            // lblUnitTileWidth
            // 
            this.lblUnitTileWidth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblUnitTileWidth.AutoSize = true;
            this.lblUnitTileWidth.Location = new System.Drawing.Point(299, 13);
            this.lblUnitTileWidth.Name = "lblUnitTileWidth";
            this.lblUnitTileWidth.Size = new System.Drawing.Size(23, 13);
            this.lblUnitTileWidth.TabIndex = 18;
            this.lblUnitTileWidth.Text = "mm";
            // 
            // txtTileHeight
            // 
            this.txtTileHeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTileHeight.Location = new System.Drawing.Point(151, 48);
            this.txtTileHeight.Name = "txtTileHeight";
            this.txtTileHeight.Size = new System.Drawing.Size(92, 20);
            this.txtTileHeight.TabIndex = 24;
            // 
            // txtTileOverlap
            // 
            this.txtTileOverlap.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTileOverlap.Location = new System.Drawing.Point(151, 126);
            this.txtTileOverlap.Name = "txtTileOverlap";
            this.txtTileOverlap.Size = new System.Drawing.Size(92, 20);
            this.txtTileOverlap.TabIndex = 16;
            // 
            // lblUnitTileHeight
            // 
            this.lblUnitTileHeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblUnitTileHeight.AutoSize = true;
            this.lblUnitTileHeight.Location = new System.Drawing.Point(299, 52);
            this.lblUnitTileHeight.Name = "lblUnitTileHeight";
            this.lblUnitTileHeight.Size = new System.Drawing.Size(23, 13);
            this.lblUnitTileHeight.TabIndex = 26;
            this.lblUnitTileHeight.Text = "mm";
            // 
            // lblUnitTileSnap
            // 
            this.lblUnitTileSnap.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblUnitTileSnap.AutoSize = true;
            this.lblUnitTileSnap.Location = new System.Drawing.Point(299, 171);
            this.lblUnitTileSnap.Name = "lblUnitTileSnap";
            this.lblUnitTileSnap.Size = new System.Drawing.Size(23, 13);
            this.lblUnitTileSnap.TabIndex = 23;
            this.lblUnitTileSnap.Text = "mm";
            // 
            // lblUnitStructOverlap
            // 
            this.lblUnitStructOverlap.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblUnitStructOverlap.AutoSize = true;
            this.lblUnitStructOverlap.Location = new System.Drawing.Point(299, 130);
            this.lblUnitStructOverlap.Name = "lblUnitStructOverlap";
            this.lblUnitStructOverlap.Size = new System.Drawing.Size(23, 13);
            this.lblUnitStructOverlap.TabIndex = 20;
            this.lblUnitStructOverlap.Text = "mm";
            // 
            // tabTable
            // 
            this.tabTable.Controls.Add(this.cbTableOptimize);
            this.tabTable.Controls.Add(this.cbAutoTable);
            this.tabTable.Location = new System.Drawing.Point(4, 22);
            this.tabTable.Name = "tabTable";
            this.tabTable.Size = new System.Drawing.Size(430, 403);
            this.tabTable.TabIndex = 6;
            this.tabTable.Text = "Table";
            this.tabTable.UseVisualStyleBackColor = true;
            // 
            // cbTableOptimize
            // 
            this.cbTableOptimize.AutoSize = true;
            this.cbTableOptimize.Location = new System.Drawing.Point(42, 78);
            this.cbTableOptimize.Name = "cbTableOptimize";
            this.cbTableOptimize.Size = new System.Drawing.Size(93, 17);
            this.cbTableOptimize.TabIndex = 45;
            this.cbTableOptimize.Text = "TableOptimize";
            this.cbTableOptimize.UseVisualStyleBackColor = true;
            // 
            // cbAutoTable
            // 
            this.cbAutoTable.AutoSize = true;
            this.cbAutoTable.Location = new System.Drawing.Point(42, 40);
            this.cbAutoTable.Name = "cbAutoTable";
            this.cbAutoTable.Size = new System.Drawing.Size(75, 17);
            this.cbAutoTable.TabIndex = 44;
            this.cbAutoTable.Text = "AutoTable";
            this.cbAutoTable.UseVisualStyleBackColor = true;
            this.cbAutoTable.Visible = false;
            // 
            // tabHybrid
            // 
            this.tabHybrid.Controls.Add(this.tableLayoutPanel1);
            this.tabHybrid.Location = new System.Drawing.Point(4, 22);
            this.tabHybrid.Name = "tabHybrid";
            this.tabHybrid.Padding = new System.Windows.Forms.Padding(3);
            this.tabHybrid.Size = new System.Drawing.Size(430, 403);
            this.tabHybrid.TabIndex = 11;
            this.tabHybrid.Text = "Hybrid";
            this.tabHybrid.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.Controls.Add(this.label51, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label57, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label53, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label52, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtTableSnapZ, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label50, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label56, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtTableSnapX, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtTableSnapY, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(25, 35);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(350, 200);
            this.tableLayoutPanel1.TabIndex = 65;
            // 
            // label51
            // 
            this.label51.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(3, 13);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(75, 13);
            this.label51.TabIndex = 50;
            this.label51.Text = "Table Snap X:";
            // 
            // label57
            // 
            this.label57.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(299, 91);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(23, 13);
            this.label57.TabIndex = 57;
            this.label57.Text = "mm";
            // 
            // label53
            // 
            this.label53.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(3, 52);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(75, 13);
            this.label53.TabIndex = 53;
            this.label53.Text = "Table Snap Y:";
            // 
            // label52
            // 
            this.label52.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(299, 52);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(23, 13);
            this.label52.TabIndex = 54;
            this.label52.Text = "mm";
            // 
            // txtTableSnapZ
            // 
            this.txtTableSnapZ.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTableSnapZ.Location = new System.Drawing.Point(151, 87);
            this.txtTableSnapZ.Name = "txtTableSnapZ";
            this.txtTableSnapZ.Size = new System.Drawing.Size(92, 20);
            this.txtTableSnapZ.TabIndex = 56;
            // 
            // label50
            // 
            this.label50.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(299, 13);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(23, 13);
            this.label50.TabIndex = 51;
            this.label50.Text = "mm";
            // 
            // label56
            // 
            this.label56.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(3, 91);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(75, 13);
            this.label56.TabIndex = 55;
            this.label56.Text = "Table Snap Z:";
            // 
            // txtTableSnapX
            // 
            this.txtTableSnapX.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTableSnapX.Location = new System.Drawing.Point(151, 9);
            this.txtTableSnapX.Name = "txtTableSnapX";
            this.txtTableSnapX.Size = new System.Drawing.Size(92, 20);
            this.txtTableSnapX.TabIndex = 49;
            // 
            // txtTableSnapY
            // 
            this.txtTableSnapY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTableSnapY.Location = new System.Drawing.Point(151, 48);
            this.txtTableSnapY.Name = "txtTableSnapY";
            this.txtTableSnapY.Size = new System.Drawing.Size(92, 20);
            this.txtTableSnapY.TabIndex = 52;
            // 
            // tabMilling
            // 
            this.tabMilling.Controls.Add(this.groupBox1);
            this.tabMilling.Location = new System.Drawing.Point(4, 22);
            this.tabMilling.Name = "tabMilling";
            this.tabMilling.Padding = new System.Windows.Forms.Padding(3);
            this.tabMilling.Size = new System.Drawing.Size(430, 403);
            this.tabMilling.TabIndex = 12;
            this.tabMilling.Text = "Milling";
            this.tabMilling.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Controls.Add(this.cbMillSpeedProfile);
            this.groupBox1.Controls.Add(this.cbMillOverwrite);
            this.groupBox1.Location = new System.Drawing.Point(8, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 378);
            this.groupBox1.TabIndex = 55;
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label13, 2, 6);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtMillTableSpeedLimit, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.label9, 2, 5);
            this.tableLayoutPanel2.Controls.Add(this.label14, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.label11, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label5, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label7, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtTableClipY, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.label1, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label12, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.txtMillTableSnapY, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtMillTileWidth, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtMillTileHeight, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtMillTableClipX, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtMillTableSnapX, 1, 2);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(28, 23);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28816F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(350, 280);
            this.tableLayoutPanel2.TabIndex = 65;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "Tile Width:";
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(299, 250);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(23, 13);
            this.label13.TabIndex = 57;
            this.label13.Text = "mm";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 41;
            this.label2.Text = "Tile Height:";
            // 
            // txtMillTableSpeedLimit
            // 
            this.txtMillTableSpeedLimit.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMillTableSpeedLimit.Location = new System.Drawing.Point(151, 247);
            this.txtMillTableSpeedLimit.Name = "txtMillTableSpeedLimit";
            this.txtMillTableSpeedLimit.Size = new System.Drawing.Size(92, 20);
            this.txtMillTableSpeedLimit.TabIndex = 55;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(299, 208);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 13);
            this.label9.TabIndex = 54;
            this.label9.Text = "mm";
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 250);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(95, 13);
            this.label14.TabIndex = 56;
            this.label14.Text = "Table Speed Limit:";
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(299, 169);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(23, 13);
            this.label11.TabIndex = 51;
            this.label11.Text = "mm";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 91);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 44;
            this.label8.Text = "Table Snap X:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(299, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 13);
            this.label5.TabIndex = 48;
            this.label5.Text = "mm";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 47;
            this.label6.Text = "Table Clip X:";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(299, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 13);
            this.label7.TabIndex = 45;
            this.label7.Text = "mm";
            // 
            // txtTableClipY
            // 
            this.txtTableClipY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTableClipY.Location = new System.Drawing.Point(151, 204);
            this.txtTableClipY.Name = "txtTableClipY";
            this.txtTableClipY.Size = new System.Drawing.Size(92, 20);
            this.txtTableClipY.TabIndex = 52;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(299, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 42;
            this.label1.Text = "mm";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 169);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(75, 13);
            this.label12.TabIndex = 50;
            this.label12.Text = "Table Snap Y:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(299, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 39;
            this.label3.Text = "mm";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 208);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 13);
            this.label10.TabIndex = 53;
            this.label10.Text = "Table Clip Y:";
            // 
            // txtMillTableSnapY
            // 
            this.txtMillTableSnapY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMillTableSnapY.Location = new System.Drawing.Point(151, 165);
            this.txtMillTableSnapY.Name = "txtMillTableSnapY";
            this.txtMillTableSnapY.Size = new System.Drawing.Size(92, 20);
            this.txtMillTableSnapY.TabIndex = 49;
            // 
            // txtMillTileWidth
            // 
            this.txtMillTileWidth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMillTileWidth.Location = new System.Drawing.Point(151, 9);
            this.txtMillTileWidth.Name = "txtMillTileWidth";
            this.txtMillTileWidth.Size = new System.Drawing.Size(92, 20);
            this.txtMillTileWidth.TabIndex = 37;
            // 
            // txtMillTileHeight
            // 
            this.txtMillTileHeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMillTileHeight.Location = new System.Drawing.Point(151, 48);
            this.txtMillTileHeight.Name = "txtMillTileHeight";
            this.txtMillTileHeight.Size = new System.Drawing.Size(92, 20);
            this.txtMillTileHeight.TabIndex = 40;
            // 
            // txtMillTableClipX
            // 
            this.txtMillTableClipX.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMillTableClipX.Location = new System.Drawing.Point(151, 126);
            this.txtMillTableClipX.Name = "txtMillTableClipX";
            this.txtMillTableClipX.Size = new System.Drawing.Size(92, 20);
            this.txtMillTableClipX.TabIndex = 46;
            // 
            // txtMillTableSnapX
            // 
            this.txtMillTableSnapX.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMillTableSnapX.Location = new System.Drawing.Point(151, 87);
            this.txtMillTableSnapX.Name = "txtMillTableSnapX";
            this.txtMillTableSnapX.Size = new System.Drawing.Size(92, 20);
            this.txtMillTableSnapX.TabIndex = 43;
            // 
            // cbMillSpeedProfile
            // 
            this.cbMillSpeedProfile.AutoSize = true;
            this.cbMillSpeedProfile.Location = new System.Drawing.Point(28, 309);
            this.cbMillSpeedProfile.Name = "cbMillSpeedProfile";
            this.cbMillSpeedProfile.Size = new System.Drawing.Size(130, 17);
            this.cbMillSpeedProfile.TabIndex = 58;
            this.cbMillSpeedProfile.Text = "Variable Speed Profile";
            this.cbMillSpeedProfile.UseVisualStyleBackColor = true;
            // 
            // cbMillOverwrite
            // 
            this.cbMillOverwrite.AutoSize = true;
            this.cbMillOverwrite.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cbMillOverwrite.Location = new System.Drawing.Point(17, 0);
            this.cbMillOverwrite.Name = "cbMillOverwrite";
            this.cbMillOverwrite.Size = new System.Drawing.Size(201, 17);
            this.cbMillOverwrite.TabIndex = 36;
            this.cbMillOverwrite.Text = "Overwrite default process parameters";
            this.cbMillOverwrite.UseVisualStyleBackColor = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(149, 78);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(92, 20);
            this.textBox2.TabIndex = 44;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(28, 81);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(71, 13);
            this.label41.TabIndex = 47;
            this.label41.Text = "Table Speed:";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(247, 41);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(18, 13);
            this.label42.TabIndex = 42;
            this.label42.Text = "µs";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(246, 81);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(33, 13);
            this.label40.TabIndex = 49;
            this.label40.Text = "mm/s";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(246, 122);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(33, 13);
            this.label38.TabIndex = 54;
            this.label38.Text = "mm/s";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(149, 119);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(92, 20);
            this.textBox1.TabIndex = 50;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(26, 41);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(68, 13);
            this.label43.TabIndex = 41;
            this.label43.Text = "Track Delay:";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(28, 123);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(102, 13);
            this.label39.TabIndex = 52;
            this.label39.Text = "Referencing Speed:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(149, 38);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(92, 20);
            this.textBox3.TabIndex = 38;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(247, 41);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(18, 13);
            this.label48.TabIndex = 43;
            this.label48.Text = "µs";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(246, 122);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(33, 13);
            this.label44.TabIndex = 55;
            this.label44.Text = "mm/s";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(149, 78);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(92, 20);
            this.textBox5.TabIndex = 45;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(26, 41);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(68, 13);
            this.label49.TabIndex = 40;
            this.label49.Text = "Track Delay:";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(28, 81);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(71, 13);
            this.label47.TabIndex = 46;
            this.label47.Text = "Table Speed:";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(28, 123);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(102, 13);
            this.label45.TabIndex = 53;
            this.label45.Text = "Referencing Speed:";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(246, 81);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(33, 13);
            this.label46.TabIndex = 48;
            this.label46.Text = "mm/s";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(266, 435);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 37;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(149, 119);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(92, 20);
            this.textBox4.TabIndex = 51;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(347, 435);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 36;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(149, 38);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(92, 20);
            this.textBox6.TabIndex = 39;
            // 
            // PropertiesDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 470);
            this.Controls.Add(this.tabSettings);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label41);
            this.Controls.Add(this.label42);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.label38);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label43);
            this.Controls.Add(this.label39);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label48);
            this.Controls.Add(this.label44);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label49);
            this.Controls.Add(this.label47);
            this.Controls.Add(this.label45);
            this.Controls.Add(this.label46);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.textBox6);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PropertiesDlg";
            this.Text = "Process Parameters";
            this.Load += new System.EventHandler(this.PropertiesDlg_Load);
            this.tabSettings.ResumeLayout(false);
            this.tabTiling.ResumeLayout(false);
            this.layoutAxisZ.ResumeLayout(false);
            this.layoutAxisZ.PerformLayout();
            this.tabTable.ResumeLayout(false);
            this.tabTable.PerformLayout();
            this.tabHybrid.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabMilling.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabSettings;
        private System.Windows.Forms.TabPage tabTiling;
        private System.Windows.Forms.Label lblUnitTileDepth;
        private System.Windows.Forms.Label lblTileDepth;
        private System.Windows.Forms.TextBox txtTileDepth;
        private System.Windows.Forms.Label lblUnitTileHeight;
        private System.Windows.Forms.Label lblTileHeight;
        private System.Windows.Forms.TextBox txtTileHeight;
        private System.Windows.Forms.Label lblUnitTileSnap;
        private System.Windows.Forms.Label lblTileSnap;
        private System.Windows.Forms.TextBox txtTileSnap;
        private System.Windows.Forms.Label lblUnitStructOverlap;
        private System.Windows.Forms.Label lblTileOverlap;
        private System.Windows.Forms.Label lblUnitTileWidth;
        private System.Windows.Forms.Label lblTileWidth;
        private System.Windows.Forms.TextBox txtTileOverlap;
        private System.Windows.Forms.TextBox txtTileWidth;
        private System.Windows.Forms.TabPage tabTable;
        private System.Windows.Forms.CheckBox cbTableOptimize;
        private System.Windows.Forms.CheckBox cbAutoTable;
        private System.Windows.Forms.TabPage tabHybrid;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.TextBox txtTableSnapZ;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.TextBox txtTableSnapY;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.TextBox txtTableSnapX;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TabPage tabMilling;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMillTileHeight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMillTileWidth;
        private System.Windows.Forms.CheckBox cbMillOverwrite;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtTableClipY;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtMillTableSnapY;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMillTableClipX;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtMillTableSnapX;
        private System.Windows.Forms.CheckBox cbMillSpeedProfile;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtMillTableSpeedLimit;
        private System.Windows.Forms.TableLayoutPanel layoutAxisZ;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;

    }
}