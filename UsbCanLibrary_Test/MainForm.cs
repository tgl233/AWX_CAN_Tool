using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CAN;
using CsmProtocl;

namespace ACUSB_132B_Tool
{
    public partial class MainForm : Form
    {
        ACUSB_132B device1 = null;
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            combox_device.SelectedIndex = 0;
            button_information.Enabled = false;
            button_closedevice.Enabled = false;
            tabControl1.Enabled = false;
        }

        private void button_opendevice_Click(object sender, EventArgs e)
        {
            device1 = new ACUSB_132B((uint)combox_device.SelectedIndex);
            if (device1.Handle != 0)
                device1.Open();
            if (device1.Handle != 0)
            {
                toolStripStatusLabel1.Text = "设备打开成功!";
                button_opendevice.Enabled = false;
                combox_device.Enabled = false;
                button_closedevice.Enabled = true;
                button_information.Enabled = true;
                tabControl1.Enabled = true;
                canChannel1.CANChannel = device1.CAN0;
                canChannel2.CANChannel = device1.CAN1;
            }
            else
            {
                toolStripStatusLabel1.Text = "设备打开失败!";
            }
        }

        private void button_closedevice_Click(object sender, EventArgs e)
        {
            if (device1.Handle != 0)
            {
                device1.CAN0.Stop();
                device1.CAN1.Stop();
                if (device1.Close())
                {
                    button_opendevice.Enabled = true;
                    combox_device.Enabled = true;
                    button_information.Enabled = false;
                    button_closedevice.Enabled = false;
                    tabControl1.Enabled = false;
                    toolStripStatusLabel1.Text = "设备已关闭";
                }
            }
        }

        private void button_information_Click(object sender, EventArgs e)
        {
            if (device1.Handle != 0)
            {
                Information info = device1.GetDeviceInfomation();
                if (info != null)
                {
                    string str = string.Format(
                        "硬件版本：{0}\n固件版本：{1}\n驱动版本：{2}\n接口版本：{3}\n中断号：{4}\nCAN通道数：{5}\n",
                        info.HardWareVersion, info.FirmWareVersion, info.DriverVersion, info.InterfaceVersion,
                        info.InterruptNumber, info.ChannelNumber);
                    string str1 = string.Format(
                        "设备串号：{0}\n硬件类型：{1}\n描述：{2}",
                        info.SerialNumber, info.HardWrareType, info.Description);

                    MessageBox.Show(str + str1, "设备信息：");

                }
            }
        }
    }
}
