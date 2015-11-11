using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace CAN
{
    /// <summary>
    /// 是所有USBCAN设备的抽象基类
    ///     实现了USBCAN设备 打开、关闭、读取设备信息的基本方法。
    ///     还包含一个实现了ICAN接口的类usbcan。
    ///         UsbCan是USBCAN设备的每的CAN通道的类，即每个USBCAN设备的每个CAN通道都是UsbCan的一个对象。
    /// </summary>
    public abstract class UsbCanDevice
    {
        /// <summary>
        /// 设备句柄
        /// </summary>
        private uint _handle = 0;
        /// <summary>
        /// 设备类型
        /// </summary>
        private uint _deviceType;
        /// <summary>
        /// USB设备索引
        /// </summary>
        private uint _index;

        public static readonly Dictionary<uint, string> ErrorCodeDict = new Dictionary<uint, string>{
            {0x0,"没有错误"},{0x01,"CAN控制器内部FIFO溢出"},{0x02,"CAN控制器错误报警"},
            {0x4,"CAN控制器消极错误"},{0x08,"CAN控制器仲裁丢失"},{0x10,"CAN控制器总线错误"},
            {0x100,"设备已打开"},{0x200,"打开设备错误"},{0x400,"设备没有打开"},{0x800,"缓冲溢出"},
            {0x1000,"此设备不存在"},{0x2000,"装载动态库失败"},{0x4000,"执行命令失败"},{0x8000,"内存不足"}};

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">USB设备索引</param>
        public UsbCanDevice(DeviceType deviceType, uint index)
        {
            #region 判断CanCmd.dll是否存在，不存在生成一个
            Debug.WriteLine(System.IO.File.Exists(@"CanCmd.dll"), "判断是否存在CanCmd.dll库文件");
            if (System.IO.File.Exists(@"CanCmd.dll") == false)
            {
                byte[] buf = CsmProtocl.Properties.Resources.CanCmd;
                System.IO.FileStream f = new System.IO.FileStream(@".\CanCmd.dll", System.IO.FileMode.Create);
                System.IO.BinaryWriter br = new System.IO.BinaryWriter(f);
                br.Write(buf, 0, buf.Length);
                br.Close();
                f.Close();
            }
            #endregion

            this._index = index;
            this._deviceType = (uint)deviceType;
            Open();
        }

        /// <summary>
        /// 设备句柄
        /// </summary>
        public uint Handle
        {
            get
            {
                //throw new System.NotImplementedException();
                return this._handle;
            }
        }

        /// <summary>
        /// USB端口索引
        /// </summary>
        public uint Index
        {
            get
            {
                //throw new System.NotImplementedException();
                return this._index;
            }
        }
        /// <summary>
        /// 打开USB设备
        /// </summary>
        public bool Open()
        {
            //throw new System.NotImplementedException();
            string str = "UsbCan";
            uint resualt = Like.CAN_DeviceOpen(this._deviceType, this._index, ref str);
            if (resualt != 0)
            {
                Debug.WriteLine(string.Format("打开\t USB{0}<->{1}\t成功\t设备句柄：{2}", this._index, (DeviceType)this._deviceType, resualt));
                this._handle = resualt;
                return true;
            }
            else
            {
                Debug.WriteLine(string.Format("打开\t USB{0}<->{1}\t失败", this.Index, (DeviceType)this._deviceType));
                return false;
            }
        }

        /// <summary>
        /// 关闭USB设备
        /// </summary>
        public bool Close()
        {
            if (this.Handle != 0)
            {
                uint resualt = Like.CAN_DeviceClose(this.Handle);
                if (resualt != 0)
                {
                    Debug.WriteLine(string.Format("关闭\t USB{0}<->{1}\t成功", this.Index, (DeviceType)this._deviceType));
                    return true;
                }
                else
                {
                    Debug.WriteLine(string.Format("关闭\t USB{0}<->{1}\t失败", this.Index, (DeviceType)this._deviceType));
                    return false;
                }
            }
            else
            {
                Debug.WriteLine(string.Format("USB{0}<->{1}\t设备未打开需关闭", this.Index, (DeviceType)this._deviceType));
                return false;
            }
        }

        /// <summary>
        /// 获取USB设备信息
        /// </summary>
        public Information GetDeviceInfomation()
        {
            Like.CAN_DeviceInformation info = new Like.CAN_DeviceInformation();
            info.szSerialNumber = new byte[20];
            info.szHardWareType = new byte[40];
            info.szDescription = new byte[20];
            uint resualt = Like.CAN_GetDeviceInfo(this.Handle, ref info);

            if (resualt != 0)
            {
                Debug.WriteLine("读取设备信息成功");
                return Conver2Information(info);
            }
            else
            {
                Debug.WriteLine("读取设备信息失败");
                return null;
            }
        }

        /// <summary>
        /// 把设备信息结构转为设备信息类
        /// </summary>
        private Information Conver2Information(Like.CAN_DeviceInformation Info)
        {
            return new Information(Conver2StringVersion(Info.uHardWareVersion),
                Conver2StringVersion(Info.uFirmWareVersion),
                Conver2StringVersion(Info.uDriverVersion),
                Conver2StringVersion(Info.uInterfaceVersion),
                Info.uInterruptNumber, Info.bChannelNumber,
                Encoding.Default.GetString(Info.szSerialNumber).Trim('\0'),
                 Encoding.Default.GetString(Info.szHardWareType).Trim('\0'),
                /*Encoding.Default.GetString(Info.szDescription).Trim('\0')+*/"TGL233@qq.com");
        }

        /// <summary>
        /// 把数据字表示的版本转为文本
        /// </summary>
        private string Conver2StringVersion(ushort version)
        {
            string str = string.Format("{0:X}", version);
            float f = float.Parse(str) / 100;
            return "V" + f.ToString("F");
        }

        /// <summary>
        /// USBCAN设备的每个CAN通道类。
        ///     它实现了每个CAN通道的 启动、停止、发送、接收、异步接收功能。
        /// </summary>
        protected class UsbCan : ICAN
        {
            /// <summary>
            /// CAN通道所在设备的句柄
            /// </summary>
            private uint _handle = 0;
            /// <summary>
            /// CAN通道号
            /// </summary>
            private uint _channel = 0;
            /// <summary>
            /// CAN通道打开时间
            /// </summary>
            private DateTime _startTime = new DateTime(1970, 1, 1);
            /// <summary>
            /// 是否开启
            /// </summary>
            private bool _isRuning = false;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="handle">通道所属的设备句柄</param>
            /// <param name="channel">通道号</param>
            public UsbCan(uint handle, byte channel)
            {
                this._handle = handle;
                this._channel = channel;
            }

            #region ICAN 成员

            /// <summary>
            /// 接收缓存中的数据帧数
            /// </summary>
            public uint ReceiveCount
            {
                get
                {
                    if (this.IsRuning)
                    {
                        uint res = Like.CAN_GetReceiveCount(this._handle, this.Channel);
                        //if (res != 0)
                        //{
                        //    Debug.WriteLine(res, "接收缓存区CAN帧数");
                        return res;
                        //}
                        //else
                        //{
                        //    Debug.WriteLine("读取缓冲区帧数失败");
                        //    return 0;
                        //}
                    }
                    else
                    {
                        Debug.WriteLine("设备未打开！", "ReceiveCount");
                        System.Windows.Forms.MessageBox.Show("设备未打开！", "错误:",
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Warning);

                        return 0;
                    }
                }
            }

            /// <summary>
            /// CAN通道号
            /// </summary>
            public uint Channel { get { return this._channel; } }

            /// <summary>
            /// 开启该通道的时间
            /// </summary>
            public DateTime StartTime { get { return this._startTime; } }

            /// <summary>
            /// 是否开启
            /// </summary>
            public bool IsRuning { get { return this._isRuning; } }

            /// <summary>
            /// 开启CAN通道
            /// </summary>
            public bool Start(Configation conf)
            {
                if (this.IsRuning)
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("CAN{0}已经打开,若要重新打开请先关闭!", this.Channel),
                        "提示:", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    return false;
                }
                else
                {
                    Like.CAN_InitConfig init = Conver2InitConfig(conf);
                    uint resualt = Like.CAN_ChannelStart(this._handle, this.Channel, ref init);
                    if (resualt != 0)
                    {
                        this._isRuning = true;
                        this._startTime = DateTime.Now;
                        Debug.WriteLine(string.Format("打开\t CAN{0}\t成功", this.Channel), this.StartTime.ToString());
                        //触发CAN通道打开事件
                        if (EventStart != null)
                            EventStart(this, new EventArgs());
                        return true;
                    }
                    else
                    {
                        this._isRuning = false;
                        Debug.WriteLine(string.Format("打开\t CAN{0}\t失败", this.Channel));
                        return false;
                    }
                }
            }

            /// <summary>
            /// 把Configation对象转为结构体CAN_InitConfig
            /// </summary>
            private Like.CAN_InitConfig Conver2InitConfig(Configation cfg)
            {
                Like.CAN_InitConfig res = new Like.CAN_InitConfig();
                res.dwBtr = new byte[4];
                res.dwReserved = 0;

                if (cfg.IsListenOnly)
                    res.bMode = 1;
                else
                    res.bMode = 0;

                if (cfg.BaudrateTypeIsSJA1000)
                    res.nBtrType = 1;
                else
                    res.nBtrType = 0;

                byte[] btr = new byte[4];
                btr = BitConverter.GetBytes((uint)cfg.Baudrate);
                res.dwBtr = btr;

                res.nFilter = (byte)cfg.FilterMode;
                res.dwAccCode = cfg.AccCode;
                res.dwAccMask = cfg.AccMask;

                return res;
            }

            /// <summary>
            /// 关闭CAN通道
            /// </summary>
            public bool Stop()
            {
                if (this.IsRuning)
                {
                    uint res = Like.CAN_ChannelStop(this._handle, this.Channel);
                    if (res != 0)
                    {
                        Debug.WriteLine(string.Format("关闭\t CAN{0}\t成功", this.Channel));
                        this.ClearReceive();
                        this._isRuning = false;
                        //触发CAN关闭事件
                        if (EventStop != null)
                            EventStop(this, new EventArgs());
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine(string.Format("关闭\t CAN{0}\t失败", this.Channel));
                        return false;
                    }
                }
                else
                {
                    Debug.WriteLine("设备未打开无需关闭");
                    return false;
                }
            }

            /// <summary>
            /// 同步发送数据
            /// </summary>
            public bool Send(DataFrame sendData)
            {
                //if (sendData.ID == 0xFFFFFFFF) return false;
                if (this.IsRuning)
                {
                    Like.CAN_DataFrame dat = Conver2StructDataFrame(sendData);
                    uint res = Like.CAN_ChannelSend(this._handle, this.Channel, ref dat);
                    if (res == 1)
                    {
                        Debug.WriteLine("发送\t一帧数据\t成功", DateTime.Now.ToString());
                        //触发数据发送事件
                        if (EventSendHander != null)
                        {
                            EventSendHander(this, sendData);
                        }
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine("发送\t一帧数据\t失败");
                        return false;
                    }
                }
                else
                {
                    Debug.WriteLine("对应的CAN通道未打开,无法发送数据");
                    System.Windows.Forms.MessageBox.Show(string.Format("CAN{0}未开启,发送数据前请先开启CAN通道", this.Channel),
                        "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);
                    return false;
                }
            }

            /// <summary>
            /// dll库文件中的取值函数封装
            /// </summary>
            private DataFrame GetData()
            {
                if (this.ReceiveCount != 0)
                {
                    Like.CAN_DataFrame temp = new Like.CAN_DataFrame();
                    temp.arryData = new byte[8];

                    uint res = Like.CAN_ChannelReceive(this._handle, this.Channel, ref temp);
                    if (res == 1)
                    {
                        DataFrame resualt = Conver2ClassDataFrame(temp);
                        Debug.WriteLine("读取\t一帧数据\t成功", resualt.ReceiveTime.ToString());
                        if (EventSendHander != null)
                        {
                            EventReceiveHander(this, resualt);
                        }
                        return resualt;
                    }
                    else
                    {
                        Debug.WriteLine("读取\t一帧数据\t失败");
                        return null;
                    }
                }
                else
                {
                    if (this.IsRuning)
                    {
                        Debug.WriteLine("缓冲区暂未接收到数据");
                        return null;
                    }
                    else
                    {
                        Debug.WriteLine("对应CAN通道未开启");
                        return null;
                    }
                }
            }

            /// <summary>
            /// 把DataFrame对象转为结构本CAN_DataFrame
            /// </summary>
            private Like.CAN_DataFrame Conver2StructDataFrame(DataFrame data)
            {
                Like.CAN_DataFrame res = new Like.CAN_DataFrame();
                res.arryData = new byte[8];
                Array.Copy(data.Date, res.arryData, data.Date.Length);
                res.nDataLen = data.DataLength;
                res.nSendType = (byte)data.SendType;
                res.uID = data.ID;
                if (data.IsExternFrame)
                    res.bExternFlag = 1;
                else
                    res.bExternFlag = 0;
                if (data.IsRemoteFrame)
                    res.bRemoteFlag = 1;
                else
                    res.bRemoteFlag = 0;
                res.uTimeFlag = 0;

                return res;
            }
            /// <summary>
            /// 把结构CAN_DataFrame转为DataFrame对象
            /// </summary>
            private DataFrame Conver2ClassDataFrame(Like.CAN_DataFrame data)
            {
                bool remote, exter;
                if (data.bExternFlag == 1)
                    exter = true;
                else
                    exter = false;
                if (data.bRemoteFlag == 1)
                    remote = true;
                else
                    remote = false;

                uint timeflag = DateTime2Uinx(this.StartTime) + (data.uTimeFlag / 1000000);
                byte[] dat = new byte[data.nDataLen];
                Array.Copy(data.arryData, 0, dat, 0, data.nDataLen);

                DataFrame res = new DataFrame(data.uID, dat, data.nDataLen,
                    remote, exter, (CAN_SendType)data.nSendType, timeflag);

                return res;
            }
            /// <summary>
            /// 将Uinx表示的时间转为DateTime
            /// </summary>
            private static uint DateTime2Uinx(DateTime time)
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                uint resualt = (uint)(time - startTime).TotalSeconds;
                return resualt;
            }

            /// <summary>
            /// 同步接收函数,在未接收到数据前将阻塞线程
            /// </summary>
            public DataFrame Receive()
            {
                if (this.IsRuning)
                {
                    //若缓存区无接收数据，则循环等待接收数据帧
                    //若在等待期间通道关闭则跳出循环
                    while (this.IsRuning && this.ReceiveCount == 0) ;
                    //若通道未关闭，侧取数
                    if (this.IsRuning)
                        return GetData();
                    else
                        return null;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("CAN{0}未开启,接收数据前请先开启CAN通道", this.Channel),
                        "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);
                    return null;
                }
            }

            /// <summary>
            /// 清空接收缓存区数据
            /// </summary>
            public void ClearReceive()
            {
                Like.CAN_ClearReceiveBuffer(this._handle, this.Channel);
            }

            /// <summary>
            /// 读取错误信息
            /// </summary>
            public ErrorCode GetErrorCode()
            {
                Like.CAN_ErrorInformation temp = new Like.CAN_ErrorInformation();
                temp.PassiveErrData = new byte[3];

                uint res = Like.CAN_GetErrorInfo(this._handle, this.Channel, ref temp);
                if (res != 0)
                {
                    Debug.WriteLine("获取错误信息\t成功");
                    return (new ErrorCode(temp.uErrorCode, temp.PassiveErrData, temp.ArLostErrData));
                }
                else
                {
                    Debug.WriteLine("获取错误信息\t失败");
                    return null;
                }

            }

            /// <summary>
            /// 定义一异步接收的委托
            /// </summary>
            private delegate DataFrame ReceiveDelegate();

            /// <summary>
            /// 开始异步接收
            /// </summary>
            /// <param name="requestCallBack">当接收一帧数据的回调函数</param>
            public IAsyncResult BeginReceive(AsyncCallback requestCallBack)
            {
                if (this.IsRuning)
                {
                    //把Recive函数实例化为一个ReceiveDelegate(上面已定义好)的委托
                    ReceiveDelegate dl = new ReceiveDelegate(Receive);
                    //调用委托的BeginInvoke开始异步接收，第一个参数为异步完成回调函数，第二个参数是传线回调函数的对象
                    IAsyncResult ar = dl.BeginInvoke(requestCallBack, dl);
                    return ar;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 停止异步接收并反回接收到的数据帧
            /// </summary>
            /// <remarks>
            /// 若在异步接收还未完成前调用该函数,将阻塞调用线程!
            /// 一般在异步接收回调函数中调用该函数.
            /// </remarks>
            /// <returns>接收到的数据帧</returns>
            public DataFrame EndReceive(IAsyncResult ar)
            {
                if (this.IsRuning)
                {
                    DataFrame dat = null;
                    ReceiveDelegate dl = ar.AsyncState as ReceiveDelegate;
                    dat = dl.EndInvoke(ar);
                    return dat;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 数据发送事件
            /// </summary>
            public event EventHandler<DataFrame> EventSendHander;

            /// <summary>
            /// 接收一帧数据事件
            /// </summary>
            public event EventHandler<DataFrame> EventReceiveHander;

            public event EventHandler EventStart;

            public event EventHandler EventStop;

            #endregion
        }
    }
}
