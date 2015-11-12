using System;
using System.Collections.Generic;
using System.Text;

/*
 * @brief: 定义跟CAN相关的类及接口
 */

namespace CAN
{
    //CAN数据帧类
    public class DataFrame:EventArgs
    {
        private uint _receiveTime = 0;
        private CAN_SendType _sendType = CAN_SendType.SendAndReceive;
        private bool _remoteFlag = false;
        private bool _externFlag = false;
        private uint _id =0xFFFFFFFF;
        private byte[] _data = new byte[8];
        private byte _len = 0;

        public DataFrame(uint id, byte[] data, byte length, bool remoteFrame = false, bool externFrame = false, 
            CAN_SendType sendType = CAN_SendType.SendAndReceive,uint receiveTime = 0)
        {
            if (remoteFrame)//遥控帧无数据
            {
                this._len = length;
            }
            else
            {
                if (data != null)
                {
                    if (data.Length > 8 )
                    {
                        System.Windows.Forms.MessageBox.Show("CAN每帧数据长度不能大于8", "错误",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        throw new Exception("CAN每帧数据长度不能大于8");
                    }
                    else
                    {
                        _data = data;
                        this._len = (byte)data.Length;
                    }
                }
                else
                {
                    throw new Exception("数据帧CAN数据不能为空");
                }
            }

            if (externFrame)    //扩展帧ID应是0-1FFFFFFF之间
            {
                if (id > 0x1FFFFFFF)
                    throw new Exception("扩展帧ID不以大于0x1FFFFFFF");
                _id = id;
            }
            else    //标准帧ID应0-7FF之间
            {
                if (id > 0x7FF)
                    throw new Exception("扩展帧ID不以大于0x7FF");
                _id = id;
            }
            _sendType = sendType;
            _remoteFlag = remoteFrame;
            _externFlag = externFrame;
            _receiveTime = receiveTime;
        }

        public DataFrame()
        {
        }

        public DateTime ReceiveTime
        {
            get
            {
                DateTime res = TimeZone.CurrentTimeZone.ToLocalTime(
                    new DateTime(1970, 1, 1).AddSeconds(this._receiveTime));
                return res;
            }
        }

        public CAN_SendType SendType
        {
            get
            {
                return this._sendType;
            }
        }

        public bool IsRemoteFrame
        {
            get
            {
                return this._remoteFlag;
            }
        }

        public bool IsExternFrame
        {
            get
            {
                return this._externFlag;
            }
        }

        public uint ID
        {
            get
            {
                return this._id;
            }
        }

        public byte[] Date
        {
            get
            {
                return this._data;
            }
        }

        public byte DataLength
        {
            get
            {
                return this._len;
            }
        }
    }

    //CAN通道配置类
    public class Configation
    {

        private bool _listenOnly = false;
        private bool _isSJA1000 = true;
        private CAN_Baudrate _btr = CAN_Baudrate._250Kbps;
        private uint _accCode = 0x00000000;
        private uint _accMask = 0xFFFFFFFF;
        private CAN_FilterMode _filterMode = CAN_FilterMode.NoFiletr;

        public Configation(CAN_Baudrate baudRate, bool isSJA100 = true, bool listenOnly = false,
            CAN_FilterMode filter = CAN_FilterMode.NoFiletr, uint acc = 0x0, uint mask = 0xFFFFFFFF)
        {
            this._accCode = acc;
            this._accMask = mask;
            this._btr = baudRate;
            this._filterMode = filter;
            this._isSJA1000 = isSJA100;
            this._listenOnly = listenOnly;
        }

        public Configation() { }

        public bool IsListenOnly { get { return this._listenOnly; } }

        public bool BaudrateTypeIsSJA1000 { get { return this._isSJA1000; } }

        public CAN_Baudrate Baudrate { get { return this._btr; } }

        public CAN_FilterMode FilterMode { get { return this._filterMode; } }

        public uint AccCode { get { return this._accCode; } }

        public uint AccMask { get { return this._accMask; } }
    }

    //CAN错误信息类
    public class ErrorCode:EventArgs
    {
        private uint _errorCode = 0;
        private byte[] _passiveErrData = new byte[3];
        private byte _arLostErrData =0;

        public ErrorCode(uint code,byte[] passive, byte arlost)
        {
            this._errorCode = code;
            this._passiveErrData = passive;
            this._arLostErrData = arlost;
        }


        public byte[] PassiveErrData
        {
            get
            {
                return this._passiveErrData;
            }
        }

        public byte ArLostErrData
        {
            get
            {
                return this._arLostErrData;
            }
        }

        public uint Code
        {
            get
            {
                return this._errorCode;
            }
        }
    }

    //USB设备信息类
    public class Information
    {
        private string _HardWareVersion;
        private string _FirmWareVersion;
        private string _DriverVersion;
        private string _InterfaceVersion;
        private ushort _InterruptNumber;
        private byte _ChannelNumber;
        private string _SerialNumber;
        private string _HardWrareType;
        private string _Description;

        public Information(string hardVersion, string frimVersion, string driverVersion, string interfaceVersion,
            ushort interruptNumber, byte channelNumber, string serialNumber, string hardType, string description)
        {
            //throw new System.NotImplementedException();
            this._HardWareVersion = hardVersion;
            this._FirmWareVersion = frimVersion;
            this._DriverVersion = driverVersion;
            _InterfaceVersion = interfaceVersion;
            _InterruptNumber = interruptNumber;
            _ChannelNumber = channelNumber;
            _SerialNumber = serialNumber;
            _HardWrareType = hardType;
            _Description = description;
        }

        public string HardWareVersion { get { return this._HardWareVersion; } }

        public string FirmWareVersion { get { return this._FirmWareVersion; } }

        public string DriverVersion { get { return this._DriverVersion; } }

        public string InterfaceVersion { get { return this._InterfaceVersion; } }

        public ushort InterruptNumber { get { return this._InterruptNumber; } }

        public byte ChannelNumber { get { return this._ChannelNumber; } }

        public string SerialNumber { get { return this._SerialNumber; } }

        public string HardWrareType { get { return this._HardWrareType; } }

        public string Description { get { return this._Description; } }
    }

    //USBCAN设备枚举
    public enum DeviceType
    {
        ACUSB_131B = 1,
        ACUSB_132B,
        ACUSB_251,
        ACPCI_252,
        ACPCI_254
    }

    //CAN波特率枚举
    public enum CAN_Baudrate
    {
        _5Kbps = 0x0000FFBF,
        _10Kbps = 0x00001C31,
        _20Kbps = 0x1c18,
        _50Kbps = 0x1c09,
        _100Kbps = 0x1c04,
        _125Kbps = 0x1c03,
        _250Kbps = 0x1c01,
        _500Kbps = 0x1600,
        _1000Kbps = 0x1400
    }

    //CAN滤波模式
    public enum CAN_FilterMode
    {
        NoFiletr = 0,
        SingleFilter,
        DoubleFilter
    }

    //CAN通信口接口
    public interface ICAN
    {
        /// <summary>
        /// 读取缓冲区中的CAN数据帧数
        /// </summary>
        uint ReceiveCount { get; }

        /// <summary>
        /// CAN通道号
        /// </summary>
        uint Channel { get; }

        /// <summary>
        /// CAN通道打开时间
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// 打开CAN通道
        /// </summary>
        bool Start(Configation conf);

        /// <summary>
        /// 关闭CAN通道
        /// </summary>
        bool Stop();

        /// <summary>
        /// 发送CAN数据帧
        /// </summary>
        bool Send(DataFrame sendData);

        /// <summary>
        /// 读取接收的CAN数据帧
        /// </summary>
        DataFrame Receive();

        /// <summary>
        /// 清除接收缓冲区数据
        /// </summary>
        void ClearReceive();

        /// <summary>
        /// 读取错误信息
        /// </summary>
        /// <remarks>
        /// 若在异步接收还未完成前调用该函数,将阻塞调用线程!
        /// 一般在异步接收回调函数中调用该函数.
        /// </remarks>
        /// <returns>一帧CAN数据</returns>
        ErrorCode GetErrorCode();

        /// <summary>
        /// 开始异步接收
        /// </summary>
        IAsyncResult BeginReceive(AsyncCallback requestCallBack);

        /// <summary>
        /// 停止异步接收并反回接收到的数据帧
        /// </summary>
        DataFrame EndReceive(IAsyncResult ar);

        /// <summary>
        /// 成功发送一帧数据事件
        /// </summary>
        event EventHandler<DataFrame> EventSendHander;

        /// <summary>
        /// 接收到一帧数据事件
        /// </summary>
        event EventHandler<DataFrame> EventReceiveHander;

        /// <summary>
        /// CAN通道打开事件
        /// </summary>
        event EventHandler EventStart;

        /// <summary>
        /// CAN通道关闭事件
        /// </summary>
        event EventHandler EventStop;
    }

    //CAN发送类型枚举
    public enum CAN_SendType
    {
        /// <summary>
        /// 正常发送
        /// </summary>
        Nomal = 0,
        /// <summary>
        /// 单次发送
        /// </summary>
        SingleSend,
        /// <summary>
        /// 自发自收
        /// </summary>
        SendAndReceive,
        /// <summary>
        /// 单次自发自收
        /// </summary>
        SingleSendAndReceive,
    }
}
