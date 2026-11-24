using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AxFlightLib;
using global::Flight.Properties;

namespace Flight
{
	public partial class SettingDlg : Form
	{
		public SettingDlg()
		{
			InitializeComponent();
			txtAddress.Text = Settings.Default.Address;
			txtResolution.Text = Settings.Default.Resolution.ToString("0.000000");
			txtFieldWidth.Text = Settings.Default.FieldWidth.ToString("0.000");
			txtFieldHeight.Text = Settings.Default.FieldHeight.ToString("0.000");
            txtFieldDepth.Text = Settings.Default.FieldDepth.ToString("0.000");
            txtFieldSize.Text = Settings.Default.FieldSize.ToString("0.000");
			txtFieldTop.Text = Settings.Default.FieldTop.ToString("0.000");
			txtFieldLeft.Text = Settings.Default.FieldLeft.ToString("0.000");
            txtFieldFront.Text = Settings.Default.FieldFront.ToString("0.000");
            txtResolverStepsizeX.Text = Settings.Default.ResolverStepsizeX.ToString("0.000000");
            txtResolverStepsizeY.Text = Settings.Default.ResolverStepsizeY.ToString("0.000000");
            txtResolverRangeX.Text = Settings.Default.ResolverRangeX.ToString("0.000");
            txtResolverRangeY.Text = Settings.Default.ResolverRangeY.ToString("0.000");
            cbResolverX.Checked = Settings.Default.ResolverX;
            cbResolverY.Checked = Settings.Default.ResolverY;
            txtPwrDC0.Text = Settings.Default.PowerDC0.ToString("0.000");
            txtPwrDC100.Text = Settings.Default.PowerDC100.ToString("0.000");
            txtPwrRate.Text = Settings.Default.PowerPulseRate.ToString();
            cbPwrMode.SelectedIndex = Settings.Default.PowerMode;
            cbPowerOffOnIdle.Checked = Settings.Default.PowerOffOnIdle;
        }

		public void ApplySettings()
		{
			Double fval;
            Int32 ival;
			Settings.Default.Address = txtAddress.Text;
			if (!Double.TryParse(txtResolution.Text, out fval)) fval = 0.0030;
			Settings.Default.Resolution = fval;
			if (!Double.TryParse(txtFieldWidth.Text, out fval)) fval = 100;
			Settings.Default.FieldWidth = fval;
			if (!Double.TryParse(txtFieldHeight.Text, out fval)) fval = 100;
			Settings.Default.FieldHeight = fval;
            if (!Double.TryParse(txtFieldDepth.Text, out fval)) fval = 100;
            Settings.Default.FieldDepth = fval; 
            if (!Double.TryParse(txtFieldTop.Text, out fval)) fval = 50;
			Settings.Default.FieldTop = fval;
			if (!Double.TryParse(txtFieldLeft.Text, out fval)) fval = -50;
			Settings.Default.FieldLeft = fval;
            if (!Double.TryParse(txtFieldFront.Text, out fval)) fval = 5;
            Settings.Default.FieldFront = fval; 
            if (!Double.TryParse(txtFieldSize.Text, out fval)) fval = 100;
			Settings.Default.FieldSize = fval;

            if (!Double.TryParse(txtResolverStepsizeX.Text, out fval)) fval = 0.001;
            Settings.Default.ResolverStepsizeX = fval;
            if (!Double.TryParse(txtResolverStepsizeY.Text, out fval)) fval = 0.001;
            Settings.Default.ResolverStepsizeY = fval;
            if (!Double.TryParse(txtResolverRangeX.Text, out fval)) fval = 0.0;
            Settings.Default.ResolverRangeX = fval;
            if (!Double.TryParse(txtResolverRangeY.Text, out fval)) fval = 0.0;
            Settings.Default.ResolverRangeY = fval;
            if (!Double.TryParse(txtPwrDC0.Text, out fval)) fval = 0.0;
            Settings.Default.PowerDC0 = fval;
            if (!Double.TryParse(txtPwrDC100.Text, out fval)) fval = 100.0;
            Settings.Default.PowerDC100 = fval;
            if (!Int32.TryParse(txtPwrRate.Text, out ival)) ival = 10000;
            Settings.Default.PowerPulseRate = ival;
            Settings.Default.PowerMode = cbPwrMode.SelectedIndex;
            Settings.Default.PowerOffOnIdle = cbPowerOffOnIdle.Checked;

            Settings.Default.ResolverX = cbResolverX.Checked;
            Settings.Default.ResolverY = cbResolverY.Checked;
            Settings.Default.Save();
		}


	}
}
