using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CAN;
using System.Text.RegularExpressions;

namespace CAN
{
    public partial class CanChannel : UserControl
    {
        private ICAN _channel = null;
        /// <summary>
        /// 是否在在使用ICAN对象
        /// </summary>
        private bool _using;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CanChannel()
        {
            InitializeComponent();
            InitializeComboBox();
            ControlLogic();
        }

        /// <summary>
        /// 获取或设置控件对就的ICAN对象
        /// </summary>
        public ICAN CANChannel
        {
            get
            {
                return this._channel;
            }
            set
            {
                if (!this._using)
                {
                    this._channel = value;
                    //注册CAN打开关闭事件
                    if (CANChannel != null)
                    {
                        CANChannel.EventStart += new EventHandler(CANChannel_EventStart);
                        CANChannel.EventStop += new EventHandler(CANChannel_EventStop);
                    }
                }
                else
                {
                    MessageBox.Show("当前ICAN对象正在使用！\n若要重新设置该对象应先调用Stop()再设置",
                        "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 初始化控件中的各个Combox
        /// </summary>
        private void InitializeComboBox()
        {
            DataTable dt1 = new DataTable("baudrate");
            dt1.Columns.Add("Name", Type.GetType("System.String"));
            dt1.Columns.Add("Value", typeof(CAN_Baudrate));
            dt1.Rows.Add(new object[] { CAN_Baudrate._5Kbps.ToString().Trim('_'), CAN_Baudrate._5Kbps });
            dt1.Rows.Add(new object[] { CAN_Baudrate._10Kbps.ToString().Trim('_'), CAN_Baudrate._10Kbps });
            dt1.Rows.Add(new object[] { CAN_Baudrate._20Kbps.ToString().Trim('_'), CAN_Baudrate._20Kbps });
            dt1.Rows.Add(new object[] { CAN_Baudrate._50Kbps.ToString().Trim('_'), CAN_Baudrate._50Kbps });
            dt1.Rows.Add(new object[] { CAN_Baudrate._100Kbps.ToString().Trim('_'), CAN_Baudrate._100Kbps });
            dt1.Rows.Add(new object[] { CAN_Baudrate._125Kbps.ToString().Trim('_'), CAN_Baudrate._125Kbps });
            dt1.Rows.Add(new object[] { CAN_Baudrate._250Kbps.ToString().Trim('_'), CAN_Baudrate._250Kbps });
            dt1.Rows.Add(new object[] { CAN_Baudrate._500Kbps.ToString().Trim('_'), CAN_Baudrate._500Kbps });
            dt1.Rows.Add(new object[] { CAN_Baudrate._1000Kbps.ToString().Trim('_'), CAN_Baudrate._1000Kbps });
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Value";
            comboBox1.DataSource = dt1;
            comboBox1.SelectedValue = CAN_Baudrate._250Kbps;

            DataTable dt2 = new DataTable("workMode");
            dt2.Columns.Add("Name", typeof(string));
            dt2.Columns.Add("Value", typeof(bool));
            dt2.Rows.Add(new object[] { "正常模式", false });
            dt2.Rows.Add(new object[] { "只听模式", true });
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "Value";
            comboBox2.DataSource = dt2;

            DataTable dt3 = new DataTable("filter");
            dt3.Columns.Add("Name", typeof(string));
            dt3.Columns.Add("Value", typeof(CAN_FilterMode));
            dt3.Rows.Add(new object[] { "无滤波", CAN_FilterMode.NoFiletr });
            dt3.Rows.Add(new object[] { "单滤波", CAN_FilterMode.SingleFilter });
            dt3.Rows.Add(new object[] { "双滤波", CAN_FilterMode.DoubleFilter });
            comboBox3.DisplayMember = "Name";
            comboBox3.ValueMember = "Value";
            comboBox3.DataSource = dt3;
            comboBox3.SelectedValue = CAN_FilterMode.NoFiletr;

            DataTable dt4 = new DataTable("sendMode");
            dt4.Columns.Add("Name", typeof(string));
            dt4.Columns.Add("Value", typeof(CAN_SendType));
            dt4.Rows.Add(new object[] { "正常发送", CAN_SendType.Nomal });
            dt4.Rows.Add(new object[] { "自发自收", CAN_SendType.SendAndReceive });
            dt4.Rows.Add(new object[] { "单次发送", CAN_SendType.SingleSend });
            dt4.Rows.Add(new object[] { "单次自发自收", CAN_SendType.SingleSendAndReceive });
            comboBox4.DisplayMember = "Name";
            comboBox4.ValueMember = "Value";
            comboBox4.DataSource = dt4;
            comboBox4.SelectedValue = CAN_SendType.SendAndReceive;

            DataTable dt5 = new DataTable("isRemoteFrame");
            dt5.Columns.Add("Name", typeof(string));
            dt5.Columns.Add("Value", typeof(bool));
            dt5.Rows.Add(new object[] { "数据帧", false });
            dt5.Rows.Add(new object[] { "遥控帧", true });
            comboBox5.DisplayMember = "Name";
            comboBox5.ValueMember = "Value";
            comboBox5.DataSource = dt5;

            DataTable dt6 = new DataTable("isExternFrame");
            dt6.Columns.Add("Name", typeof(string));
            dt6.Columns.Add("Value", typeof(bool));
            dt6.Rows.Add(new object[] { "标准帧", false });
            dt6.Rows.Add(new object[] { "扩展帧", true });
            comboBox6.DisplayMember = "Name";
            comboBox6.ValueMember = "Value";
            comboBox6.DataSource = dt6;
        }

        /// <summary>
        /// 初始化控件的状态
        /// </summary>
        private void ControlLogic()
        {
            button2.Enabled = false;
            button3.Enabled = false;
            //ToolTip tip = new ToolTip();
            //tip.SetToolTip(textBox1, "帧ID只能由：\n小于1FFFFFFF的十六进制数组成!");
            //tip.SetToolTip(textBox2, "数据只由：\n小于FF的16进制数据,并用空格分割开\n长度不能超过8个");
        }

        /// <summary>
        /// 成功关闭CAN端口的事件处理函数
        /// </summary>
        void CANChannel_EventStop(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button1.Enabled = true;
            button3.Enabled = false;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            ovalShape1.FillColor = Color.Black;
            this._using = false;
            comboBox3_SelectedIndexChanged(sender, e);
        }

        /// <summary>
        /// 成功打开CAN端口的事件处理函数
        /// </summary>
        void CANChannel_EventStart(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button3.Enabled = true;
            ovalShape1.FillColor = Color.Lime;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            this._using = true;
            button1.Enabled = false;
            maskedTextBox1.Enabled = false;
            maskedTextBox2.Enabled = false;

            //开启循环异步接收
            CANChannel.BeginReceive(PollReceive);
        }

        /// <summary>
        /// 滤波方法改变的事件处理函数
        /// </summary>
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            if ((CAN_FilterMode)comboBox3.SelectedValue == CAN_FilterMode.NoFiletr)
            {
                maskedTextBox1.Enabled = false;
                maskedTextBox2.Enabled = false;
                maskedTextBox1.Text = "00000000";
                maskedTextBox2.Text = "FFFFFFFF";
            }
            else
            {
                maskedTextBox1.Enabled = true;
                maskedTextBox2.Enabled = true;
            }
        }

        /// <summary>
        /// 打开CAN通道
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            uint acc = 0x00000000, mask = 0xFFFFFFFF;
            if (maskedTextBox1.Enabled) acc = Convert.ToUInt32(maskedTextBox1.Text, 16);
            if (maskedTextBox2.Enabled) mask = Convert.ToUInt32(maskedTextBox2.Text, 16);
            Configation cfg = new Configation((CAN_Baudrate)comboBox1.SelectedValue, true, (bool)comboBox2.SelectedValue,
                (CAN_FilterMode)comboBox3.SelectedValue, acc, mask);
            if (CANChannel != null)
            {
                if (CANChannel.Start(cfg))
                {
                    //在打开端口事件处理函数中改变控制状态
                }
                else
                {
                    MessageBox.Show("打开CAN口失败！", "提示", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("打开通道前\n请先通过CANChannel属性设置通道对象", "错误:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 异步接收回调函数
        /// </summary>
        void PollReceive(IAsyncResult ar)
        {
            //通过调用EndReceive函数取回值
            DataFrame dat = CANChannel.EndReceive(ar);
            if (dat != null)
                listView1.Invoke(new ChangeListView(DataFrameToListview), dat);
            //若通道未关闭
            if (this._using)
            {
                //重启异步接收
                CANChannel.BeginReceive(PollReceive);
            }
        }

        /// <summary>
        /// 定义改变ListView的委托
        /// </summary>
        delegate void ChangeListView(DataFrame data);

        /// <summary>
        /// 将DataFrame数据转为LitViewItem
        /// </summary>
        private void DataFrameToListview(DataFrame dat)
        {
            string str1, str2;
            if (dat.IsExternFrame)
                str1 = "扩展帧";
            else
                str1 = "标准帧";
            if (dat.IsRemoteFrame)
                str2 = "遥控帧";
            else
                str2 = "数据帧";


            ListViewItem lt = new ListViewItem(new string[] {
                dat.ReceiveTime.ToString(),str2,str1,
               /*Convert.ToString(dat.ID,16)*/
                dat.ID.ToString("X8")
            /*string.Format("{0,8:X8}",dat.ID)*/,dat.DataLength.ToString(),
                BitConverter .ToString(dat.Date).Replace('-',' ')});
            listView1.Items.Add(lt);
        }

        /// <summary>
        /// 关闭CAN通道
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            if (CANChannel != null)
            {
                bool flag = CANChannel.Stop();
                if (/*CANChannel.Stop()*/ flag)
                {
                    //在成功关闭CAN通道事件处理函数中改变控件状
                }
                else
                {
                    MessageBox.Show("关闭CAN口失败！", "提示", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 将ID文本框中的文本转为对应的ID
        /// </summary>
        private UInt32 ConvertToID(string stringID)
        {
            try
            {
                uint id = Convert.ToUInt32(textBox1.Text, 16);
                if ((bool)comboBox6.SelectedValue)
                {
                    if (id > 0x1fffffff)
                        throw new Exception("扩展帧ID只能是小于0x1FFFFFFF的数");
                }
                else
                {
                    if (id > 0x7FF)
                        throw new Exception("标准帧ID只能是小于0x7FF的数");
                }
                return id;
            }
            catch
            {
                if ((bool)comboBox6.SelectedValue)
                {
                    MessageBox.Show("扩展帧ID只能由：\n小于1FFFFFFF的十六进制数组成!", "警告",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("标准帧ID只能由：\n小于1FF的十六进制数组成!", "警告",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                return 0xFFFFFFFF;
            }
        }

        /// <summary>
        /// 将数据文本框中的文本转为数据数组
        /// </summary>
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

        /// <summary>
        /// 发送一帧CAN数据
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            uint id = ConvertToID(textBox1.Text);
            if (id == 0xFFFFFFFF)
                return;

            byte[] data = null;
            byte len = 0;
            //只有数据帧才会用数据
            if (!(bool)comboBox5.SelectedValue)
            {
                data = ConvertToData(textBox2.Text);
                if (data == null)
                    return;
                else
                    len = (byte)data.Length;
            }
            else //摇控帧无数据只有，请求回应的数据长度                
            {
                try
                {
                    byte i = Convert.ToByte(textBox2.Text);
                    if (i > 8)
                        throw new Exception("请求的数据长度不能大于8");
                }
                catch
                {
                    MessageBox.Show("数据长度应是0-8之间的数", "警告", MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                }
            }

            DataFrame send = new DataFrame(id, data, len, (bool)comboBox5.SelectedValue,
                (bool)comboBox6.SelectedValue, (CAN_SendType)comboBox4.SelectedValue);
            bool flag = CANChannel.Send(send);
            if (!flag)
            {
                //MessageBox.Show("发送该帧数失败", "提示：",
                //    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                ListViewItem lt = new ListViewItem(new string[]{
                    "发送->失败",comboBox5.Text,comboBox6.Text,
                    send.ID.ToString("X8"),send.DataLength.ToString(),BitConverter.ToString(send.Date)});
                lt.ForeColor = Color.Red;
                listView1.Items.Add(lt);

                ErrorCode err = CANChannel.GetErrorCode();
                if (err != null)
                    MessageBox.Show(UsbCanDevice.ErrorCodeDict[err.Code], "提示：",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ListViewItem lt = new ListViewItem(new string[]{
                    "发送->成功",comboBox5.Text,comboBox6.Text,
                    send.ID.ToString("X8"),send.DataLength.ToString(),BitConverter.ToString(send.Date)});
                lt.ForeColor = Color.Blue;

                listView1.Items.Add(lt);
            }
        }

        /// <summary>
        /// 帧类型改变的事件处理函数
        /// </summary>
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolTip tip = new ToolTip();
            if ((bool)comboBox5.SelectedValue)
            {
                label10.Text = "数据长度：";
                //tip.RemoveAll();
                //tip.SetToolTip(textBox2, "数据只由：\n小于FF的16进制数据,并用空格分割开\n长度不能超过8个");
            }
            else
            {
                label10.Text = "发送数据：";
            }
        }

        private void 清除记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 保存记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView lv = listView1;
            string s = string.Empty;
            for (int i = 0; i < lv.Columns.Count; i++)
            {
                s += lv.Columns[i].Text + ",";
            }
            s += "\n";
            for (int i = 0; i < lv.Items.Count; i++)
            {
                //s += (lv.Items[i].Text + ",");
                for (int j = 0; j < lv.Items[i].SubItems.Count; j++)
                {
                    s += lv.Items[i].SubItems[j].Text + ",";
                }
                s += "\n";
            }
            //MessageBox.Show(s);
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "CSV文件|*.csv";
            sf.AddExtension = true;
            sf.Title = "保存到文件:";
            sf.FileName = string.Format("CAN{0}数据文件{1}", CANChannel.Channel, DateTime.Now.ToString("yyyy-M-d-H-m-s").Replace(' ', '-'));

            if (sf.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream fs = new System.IO.FileStream(sf.FileName,
                    System.IO.FileMode.Create);
                byte[] data = Encoding.Default.GetBytes(s);
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
            }
        }

        public event EventHandler CsmButton;

        public void SetData(byte[] data)
        {
            textBox2.Text = BitConverter.ToString(data).Replace('-', ' ');
        }

        public void SetId(int id)
        {
            textBox1.Text = id.ToString("X8");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (CsmButton != null)
                CsmButton(sender, e);
        }
    }
}
