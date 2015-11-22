using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace JYLLCS
{
    public partial class SelectNumber : UserControl
    {
        public SelectNumber()
        {
            InitializeComponent();
            InitialCombox();
        }

        void InitialCombox()
        {
            DataTable dt1, dt2, dt3, dt4;



            List<string> list1, list2, list3, list4;
            list1 = new List<string> { "组合架1", "组合架2", "组合架3", "组合架4" };
            list2 = new List<string> { "A", "B", "C", "D", "A'", "B'", "C'", "D'" };
            list3 = new List<string> { "01", "02", "03", "04" };
            list4 = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };

            dt1 = List2DataTable(list1);
            //dataGridView1.DataSource = dt1;
            dt2 = List2DataTable(list2);
            //dataGridView1.DataSource = dt2;
            dt3 = List2DataTable(list3);
            //dataGridView1.DataSource = dt3;
            dt4 = List2DataTable(list4);
            //dataGridView1.DataSource = dt4;

            ComboxBindDataTable(comboBox1, dt1);
            ComboxBindDataTable(comboBox2, dt2);
            ComboxBindDataTable(comboBox3, dt3);
            ComboxBindDataTable(comboBox4, dt4);

        }

        DataTable List2DataTable(List<string> plist)
        {
            DataTable pdt = new DataTable();

            pdt.Columns.Add("Name", typeof(string));
            pdt.Columns.Add("Value", typeof(int));

            for (int i = 0; i < plist.Count; i++)
            {
                pdt.Rows.Add(new object[] { plist[i], i });
            }

            return pdt;
        }

        void ComboxBindDataTable(ComboBox combox, DataTable dt)
        {
            //combox.DataSource = dt;
            combox.ValueMember = "Value";
            combox.DisplayMember = "Name";
            //combox.ValueMember = "Value";
            combox.DataSource = dt;
        }

        private void OnNumericUpDownValueChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedValue = (int)((numericUpDown1.Value - 1) / 512);

            comboBox2.SelectedValue = (int)(numericUpDown1.Value - 1) % 512 / 64;

            comboBox3.SelectedValue = (int)(numericUpDown1.Value - 1) % 512 % 64 / 16;

            comboBox4.SelectedValue = (int)(numericUpDown1.Value - 1) % 512 % 64 % 16;
        }

        private void OnSelectValueChangeCommit(object sender, EventArgs e)
        {
            numericUpDown1.Value =
                (int)comboBox1.SelectedValue * 512 +
                (int)comboBox2.SelectedValue * 64 +
                (int)comboBox3.SelectedValue * 16 +
                (int)comboBox4.SelectedValue +
                1;
        }

        public int Number { get { return (int)numericUpDown1.Value; } }

        public override string Text
        {
            get
            {
                return ((int)comboBox1.SelectedValue + 1).ToString() + comboBox2.Text + "-" +
                    comboBox3.Text + "-" + comboBox4.Text;
            }
        }

    }
}
