using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CsmProtocol;

namespace ACUSB_132B_Tool
{
    public partial class CsmPackageForm : Form
    {
        DataTable dt1 = new DataTable("csmType1");
        DataTable dt2 = new DataTable("csmType2");
        CsmData _Data = null;
        public CsmPackageForm()
        {
            InitializeComponent();
            InitCombox();
        }

        /// <summary>
        /// Combox初始化
        /// </summary>
        private void InitCombox()
        {
            dt1.Columns.Add("Name", typeof(string));
            dt1.Columns.Add("Value", typeof(CsmPackageType));

            dt2.Columns.Add("Name", typeof(string));
            dt2.Columns.Add("Value", typeof(CsmPackageType));

            //dt.Rows.Add(new object[] { "命令包", CsmPackageType.Command });
            //dt.Rows.Add(new object[] { "高优级先级自主包", CsmPackageType.IndepandHigh });
            //dt.Rows.Add(new object[] { "低优先级自主包", CsmPackageType.IndependLow });
            //dt.Rows.Add(new object[] { "高优先级单帧应答包", CsmPackageType.SingleAnswerHigh });
            //dt.Rows.Add(new object[] { "低优先级单帧应答包", CsmPackageType.SingleAnswerLow });
            //dt.Rows.Add(new object[] { "高优先级多帧应答包", CsmPackageType.MoreAnswerHigh});
            //dt.Rows.Add(new object[] { "高优先级多帧应答包", CsmPackageType.MoreAnswerLow });
            //dt.Rows.Add(new object[] { "高优先级多帧应答结束包", CsmPackageType.EndMoreAnswerHigh });
            //dt.Rows.Add(new object[] { "低优先级多帧应答结束包", CsmPackageType.EndMoreAnswerLow});

            //comboBox1.DisplayMember = "Name";
            //comboBox1.ValueMember = "Value";
            //comboBox1.DataSource = dt;

            dt1.Rows.Add(new object[] { "命令包", CsmPackageType.Command });
            dt1.Rows.Add(new object[] { "自主帧", CsmPackageType.IndepandHigh });
            dt1.Rows.Add(new object[] { "应答单帧", CsmPackageType.SingleAnswerHigh });

            dt2.Rows.Add(new object[] { "应答多帧", CsmPackageType.MoreAnswerHigh });
            dt2.Rows.Add(new object[] { "应答多帧结尾帧", CsmPackageType.EndMoreAnswerHigh });

            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Value";

            radioButton1.Checked = true;
            radioButton3.Checked = true;

            button1.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 单帧
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            flowLayoutPanel1.Hide();
            flowLayoutPanel3.Hide();

            //dt1.DefaultView.RowFilter = "Name like '*单帧*' ";
            comboBox1.DataSource = dt1;
        }

        /// <summary>
        /// 多帧
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            flowLayoutPanel3.Show();

            comboBox1.DataSource = dt2;
        }

        /// <summary>
        /// 将文本转换为对应数组的函数
        /// </summary>
        /// <param name="stringData">要转换的十六进制文本</param>
        /// <returns>转换后的字节数组</returns>
        private byte[] ConvertToData(string stringData)
        {
            try
            {
                string[] strArray = stringData.Trim().Split(' ');
                if (strArray.Length > 8)
                    throw new Exception();
                byte[] res = new byte[strArray.Length];
                for (int i = 0; i < strArray.Length; i++)
                {
                    res[i] = Convert.ToByte(strArray[i].Trim(), 16);
                }
                return res;
            }
            catch
            {
                MessageBox.Show("数据只由：\n小于FF的16进制数据,并用空格分割开\n长度不能超过8个", "警告",
                   MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return null;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((CsmPackageType)comboBox1.SelectedValue == CsmPackageType.EndMoreAnswerHigh)
            {
                flowLayoutPanel1.Show();
            }
            if ((CsmPackageType)comboBox1.SelectedValue == CsmPackageType.Command)
            {
                radioButton3.Checked = true;
                radioButton4.Enabled = false;
            }
            else
            {
                radioButton4.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        public CsmData GetValue()
        {
            int typed;
            if (radioButton3.Checked)
                typed = (int)comboBox1.SelectedValue;
            else
                typed = (int)comboBox1.SelectedValue | 0x100;

            CsmData res = new CsmData((int)numericUpDown1.Value, ConvertToData(textBox3.Text), (CsmPackageType)typed,
                (int)numericUpDown2.Value, (int)numericUpDown3.Value);

            return res;
        }

    }
}
