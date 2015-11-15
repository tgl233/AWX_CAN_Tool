namespace ACUSB_132B_Tool
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_opendevice = new System.Windows.Forms.Button();
            this.combox_device = new System.Windows.Forms.ComboBox();
            this.button_closedevice = new System.Windows.Forms.Button();
            this.button_information = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.canChannel2 = new CAN.CanChannel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.canChannel1 = new CAN.CanChannel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_opendevice
            // 
            this.button_opendevice.Location = new System.Drawing.Point(258, 5);
            this.button_opendevice.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.button_opendevice.Name = "button_opendevice";
            this.button_opendevice.Size = new System.Drawing.Size(75, 23);
            this.button_opendevice.TabIndex = 0;
            this.button_opendevice.Text = "打开设备";
            this.button_opendevice.UseVisualStyleBackColor = true;
            this.button_opendevice.Click += new System.EventHandler(this.button_opendevice_Click);
            // 
            // combox_device
            // 
            this.combox_device.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combox_device.FormattingEnabled = true;
            this.combox_device.Items.AddRange(new object[] {
            "USB0",
            "USB1",
            "USB2",
            "USB3",
            "USB4",
            "USB5",
            "USB6",
            "USB7",
            "USB8"});
            this.combox_device.Location = new System.Drawing.Point(120, 5);
            this.combox_device.Margin = new System.Windows.Forms.Padding(0, 5, 7, 5);
            this.combox_device.Name = "combox_device";
            this.combox_device.Size = new System.Drawing.Size(121, 20);
            this.combox_device.TabIndex = 1;
            // 
            // button_closedevice
            // 
            this.button_closedevice.Location = new System.Drawing.Point(353, 5);
            this.button_closedevice.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.button_closedevice.Name = "button_closedevice";
            this.button_closedevice.Size = new System.Drawing.Size(75, 23);
            this.button_closedevice.TabIndex = 2;
            this.button_closedevice.Text = "关闭设备";
            this.button_closedevice.UseVisualStyleBackColor = true;
            this.button_closedevice.Click += new System.EventHandler(this.button_closedevice_Click);
            // 
            // button_information
            // 
            this.button_information.Location = new System.Drawing.Point(448, 5);
            this.button_information.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.button_information.Name = "button_information";
            this.button_information.Size = new System.Drawing.Size(75, 23);
            this.button_information.TabIndex = 3;
            this.button_information.Text = "设备信息";
            this.button_information.UseVisualStyleBackColor = true;
            this.button_information.Click += new System.EventHandler(this.button_information_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(566, 392);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.combox_device);
            this.flowLayoutPanel1.Controls.Add(this.button_opendevice);
            this.flowLayoutPanel1.Controls.Add(this.button_closedevice);
            this.flowLayoutPanel1.Controls.Add(this.button_information);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(560, 34);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "ACUSB_132B端口：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 372);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(566, 20);
            this.statusStrip1.TabIndex = 2;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 15);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 15);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Transparent;
            this.tabPage2.Controls.Add(this.canChannel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(552, 300);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "CAN1";
            // 
            // canChannel2
            // 
            this.canChannel2.CANChannel = null;
            this.canChannel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canChannel2.Location = new System.Drawing.Point(3, 3);
            this.canChannel2.Name = "canChannel2";
            this.canChannel2.Size = new System.Drawing.Size(546, 294);
            this.canChannel2.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.canChannel1);
            this.tabPage1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(552, 300);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "CAN0";
            // 
            // canChannel1
            // 
            this.canChannel1.CANChannel = null;
            this.canChannel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canChannel1.Location = new System.Drawing.Point(3, 3);
            this.canChannel1.Name = "canChannel1";
            this.canChannel1.Size = new System.Drawing.Size(546, 294);
            this.canChannel1.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 43);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(560, 326);
            this.tabControl1.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 392);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CAN收发工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_opendevice;
        private System.Windows.Forms.ComboBox combox_device;
        private System.Windows.Forms.Button button_closedevice;
        private System.Windows.Forms.Button button_information;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private CAN.CanChannel canChannel1;
        private System.Windows.Forms.TabPage tabPage2;
        private CAN.CanChannel canChannel2;
    }
}

