using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using global::Flight.Properties;

namespace Flight
{
    public partial class PropertiesDlg : Form
    {
        public PropertiesDlg()
        {
            InitializeComponent();
        }

        private void PropertiesDlg_Load(object sender, EventArgs e)
        {
            txtTileWidth.Text = Settings.Default.TileWidth.ToString("0.000");
            txtTileHeight.Text = Settings.Default.TileHeight.ToString("0.000");
            txtTileDepth.Text = Settings.Default.TileDepth.ToString("0.000");
            txtTileOverlap.Text = Settings.Default.TileOverlap.ToString("0.000");
            txtTileSnap.Text = Settings.Default.TileSnap.ToString("0.000");
            txtTableSnapX.Text = Settings.Default.TableSnapX.ToString("0.000");
            txtTableSnapY.Text = Settings.Default.TableSnapY.ToString("0.000");
            txtTableSnapZ.Text = Settings.Default.TableSnapZ.ToString("0.000");
            cbAutoTable.Checked = Settings.Default.AutoTable;
            cbTableOptimize.Checked = Settings.Default.TableOptimize;
            cbMillOverwrite.Checked = Settings.Default.MillOverwrite;
            txtMillTableSnapX.Text = Settings.Default.MillTableSnapX.ToString("0.000");
            txtMillTableSnapY.Text = Settings.Default.MillTableSnapY.ToString("0.000");
            txtMillTableClipX.Text = Settings.Default.MillTableClipX.ToString("0.000");
            txtTableClipY.Text = Settings.Default.MillTableClipY.ToString("0.000");
            txtMillTileWidth.Text = Settings.Default.MillTileWidth.ToString("0.000");
            txtMillTileHeight.Text = Settings.Default.MillTileHeight.ToString("0.000");
            txtMillTableSpeedLimit.Text = Settings.Default.MillTableSpeedLimit.ToString("0.000");
            cbMillSpeedProfile.Checked = Settings.Default.MillTableSpeedProfile > 0;
        }

        public void Apply()
        {
            Double fval;
            if (!Double.TryParse(txtTileWidth.Text, out fval)) fval = 80;
            Settings.Default.TileWidth = fval;
            if (!Double.TryParse(txtTileHeight.Text, out fval)) fval = 80;
            Settings.Default.TileHeight = fval;
            if (!Double.TryParse(txtTileDepth.Text, out fval)) fval = 80;
            Settings.Default.TileDepth = fval;
            if (!Double.TryParse(txtTileOverlap.Text, out fval)) fval = 1;
            Settings.Default.TileOverlap = fval;
            if (!Double.TryParse(txtTileSnap.Text, out fval)) fval = 10;
            Settings.Default.TileSnap = fval;
            if (!Double.TryParse(txtTableSnapX.Text, out fval)) fval = 20;
            Settings.Default.TableSnapX = fval;
            if (!Double.TryParse(txtTableSnapY.Text, out fval)) fval = 20;
            Settings.Default.TableSnapY = fval;
            if (!Double.TryParse(txtTableSnapZ.Text, out fval)) fval = 0;
            Settings.Default.TableSnapZ = fval;
            Settings.Default.AutoTable = cbAutoTable.Checked;
            Settings.Default.TableOptimize = cbTableOptimize.Checked;
            Settings.Default.MillOverwrite = cbMillOverwrite.Checked;

            if (!Double.TryParse(txtMillTileWidth.Text, out fval)) fval = 30;
            Settings.Default.MillTileWidth = fval;
            if (!Double.TryParse(txtMillTileHeight.Text, out fval)) fval = 30;
            Settings.Default.MillTileHeight = fval;
            if (!Double.TryParse(txtMillTableSnapX.Text, out fval)) fval = 20;
            Settings.Default.MillTableSnapX = fval;
            if (!Double.TryParse(txtMillTableSnapY.Text, out fval)) fval = 20;
            Settings.Default.MillTableSnapY = fval;
            if (!Double.TryParse(txtMillTableClipX.Text, out fval)) fval = 45;
            Settings.Default.MillTableClipX = fval;
            if (!Double.TryParse(txtTableClipY.Text, out fval)) fval = 45;
            Settings.Default.MillTableClipY = fval;
            if (!Double.TryParse(txtMillTableSpeedLimit.Text, out fval)) fval = 10;
            Settings.Default.MillTableSpeedLimit = fval;
            Settings.Default.MillTableSpeedProfile = cbMillSpeedProfile.Checked ?1 : 0;

            Settings.Default.Save();
        }
    }
}
