using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ACUSB_132B_Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //label2.Hide();
            //textBox1.Hide();
            flowLayoutPanel1.Hide();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            flowLayoutPanel3.Hide();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Show();
            flowLayoutPanel3.Show();
        }
    }
}
