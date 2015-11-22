using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CsmProtocol;
using CAN;

namespace JYLLCS
{
    public partial class Form1 : Form
    {

        ACUSB_132B device = new ACUSB_132B(0);
        CsmUsbCan csmcan = null;

        public Form1()
        {
            InitializeComponent();

            //device.CAN0.Start(new Configation());
            //device.CAN1.Start(new Configation());
            csmcan = new CsmUsbCan(device.CAN0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (textBox1.Text != null)
            //    MessageBox.Show(CsmDataFrame.ParserIdToString(Convert.ToUInt32(textBox1.Text, 16)));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //if (csmcan.Open(new Configation()))
            //{
            device.CAN0.Send(new DataFrame(0x00000703, new byte[] { 0, 0, 0x20, 0x90, 0, 0, 0, 0 }, 8));
            device.CAN0.Send(new DataFrame(0x00000703, new byte[] { 1, 0, 0, 0, 0, 0, 0, 0 }, 8));
            device.CAN0.Send(new DataFrame(0x00000703, new byte[] { 2, 0, 0, 0, 0, 0, 0, 0 }, 8));
            device.CAN0.Send(new DataFrame(0x00000702, new byte[] { 3, 0x16, 0, 0 }, 4));
            device.CAN0.Send(new DataFrame(0x00000504, new byte[] { 0, 0x11, 0, 0x0f, 0x20, 0xff }, 6));

            //}
            //else
            //{
            //    MessageBox.Show("打开设备失败!");
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(csmcan.CsmDataList.Count.ToString());
            MessageBox.Show(csmcan.ReceiveCount.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (device.Handle == 0)
                device.Open();
            csmcan.Open(new Configation());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CsmData temp = csmcan.GetData();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form f = new UsbCanConfig();
            f.Show();
        }
    }
}
