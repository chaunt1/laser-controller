using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Flight
{
    public partial class StepRepeatDlg : Form
    {
        public double LayoutStartX { set; get; }
        public double LayoutStartY { set; get; }
        public double LayoutDeltaX { set; get; }
        public double LayoutDeltaY { set; get; }
        public int LayoutColumns { set; get; }
        public int LayoutRows { set; get; }

        public StepRepeatDlg()
        {
            InitializeComponent();
        }

        private void StepRepeatDlg_Load(object sender, EventArgs e)
        {
            textBox1.DataBindings.Add(new Binding("Text", this, "LayoutStartX"));
            textBox2.DataBindings.Add(new Binding("Text", this, "LayoutStartY"));
            textBox3.DataBindings.Add(new Binding("Text", this, "LayoutDeltaX"));
            textBox4.DataBindings.Add(new Binding("Text", this, "LayoutDeltaY"));
            textBox5.DataBindings.Add(new Binding("Text", this, "LayoutColumns"));
            textBox6.DataBindings.Add(new Binding("Text", this, "LayoutRows"));
        }
    }
}
