using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

//动态库需要的命名空间
using System.Runtime.InteropServices;

namespace CAN
{
    internal class Like
    {
        #region   CmdCan.dll文件中的结构体定义
        /// <summary>
        /// CAN错误码枚举
        /// </summary>
        //public enum CAN_ErrorCode
        //{
        //    /// <summary>
        //    /// 没有发现错误
        //    /// </summary>
        //    CAN_E_NOERROR = 0x0000,
        //    /// <summary>
        //    /// CAN控制器内部FIFO溢出
        //    /// </summary>
        //    CAN_E_OVERFLOW = 0x0001,
        //    /// <summary>
        //    /// CAN控制器错误报警
        //    /// </summary>
        //    CAN_E_ERRORALARM = 0x0002,
        //    /// <summary>
        //    /// CAN控制器消极错误
        //    /// </summary>
        //    CAN_E_PASSIVE = 0x0004,
        //    /// <summary>
        //    /// CAN控制器仲裁丢失
        //    /// </summary>
        //    CAN_E_LOSE = 0x0008,
        //    /// <summary>
        //    /// CAN控制器总线错误
        //    /// </summary>
        //    CAN_E_BUSERROR = 0x0010,
        //    /// <summary>
        //    /// 设备已经打开
        //    /// </summary>
        //    CAN_E_DEVICEOPENED = 0x0100,
        //    /// <summary>
        //    /// 打开设备错误
        //    /// </summary>
        //    CAN_E_DEVICEOPEN = 0x0200,
        //    /// <summary>
        //    /// 设备没有打开
        //    /// </summary>
        //    CAN_E_DEVICENOTOPEN = 0x0400,
        //    /// <summary>
        //    /// 缓冲区溢出
        //    /// </summary>
        //    CAN_E_BUFFEROVERFLOW = 0x0800,
        //    /// <summary>
        //    /// 此设备不存在
        //    /// </summary>
        //    CAN_E_DEVICENOTEXIST = 0x1000,
        //    /// <summary>
        //    /// 装载动态库失败
        //    /// </summary>
        //    CAN_E_LOADKERNELDLL = 0x2000,
        //    /// <summary>
        //    /// 执行命令失败错误码
        //    /// </summary>
        //    CAN_E_CMDFAILED = 0x4000,
        //    /// <summary>
        //    /// 内存不足
        //    /// </summary>
        //    CAN_E_BUFFERCREATE = 0x8000
        //}

        /// <summary>
        /// CAN数据帧类型
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CAN_DataFrame
        {
            /// <summary>
            /// 时间标识,对接收帧有效
            /// </summary>
            public uint uTimeFlag;
            /// <summary>
            /// 发送帧类型,0-正常发送;1-单次发送;2-自发自收;3-单次自发自收
            /// </summary>
            public byte nSendType;
            /// <summary>
            /// 是否是远程帧
            /// </summary>
            public byte bRemoteFlag;
            /// <summary>
            /// 是否是扩展帧
            /// </summary>
            public byte bExternFlag;
            /// <summary>
            ///  数据长度
            /// </summary>
            public byte nDataLen;
            /// <summary>
            /// 报文ID号
            /// </summary>
            public uint uID;
            /// <summary>
            /// 8字节报文数据
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
            public byte[] arryData;
        }

        /// <summary>
        /// CAN初始化配置参数的结构体数据类型
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CAN_InitConfig
        {
            /// <summary>
            /// 工作模(0表示正常模式,1表示只听模式)
            /// </summary>
            public byte bMode;
            /// <summary>
            /// 位定时参数模式(1表示SJA1000,0表示LPC21XX)
            /// </summary>
            public byte nBtrType;
            /// <summary>
            /// CAN位定时参数
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
            public byte[] dwBtr;
            /// <summary>
            /// 验收码
            /// </summary>			
            public uint dwAccCode;
            /// <summary>
            /// 屏蔽码
            /// </summary>
            public uint dwAccMask;
            /// <summary>
            /// 滤波方式(0表示未设置滤波功能,1表示双滤波,2表示单滤波)
            /// </summary>
            public byte nFilter;
            /// <summary>
            ///  预留字段
            /// </summary>
            public byte dwReserved;
        }

        /// <summary>
        /// CAN设备信息
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CAN_DeviceInformation
        {
            /// <summary>
            /// 硬件版本
            /// </summary>
            public ushort uHardWareVersion;
            /// <summary>
            /// 固件版本
            /// </summary>
            public ushort uFirmWareVersion;
            /// <summary>
            /// 驱动版本
            /// </summary>
            public ushort uDriverVersion;
            /// <summary>
            /// 接口库版本
            /// </summary>
            public ushort uInterfaceVersion;
            /// <summary>
            /// 中断号
            /// </summary>
            public ushort uInterruptNumber;
            /// <summary>
            /// 有几路CAN
            /// </summary>
            public byte bChannelNumber;
            /// <summary>
            /// 设备序列号
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
            public byte[] szSerialNumber;
            /// <summary>
            /// 硬件类型
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
            public byte[] szHardWareType;
            /// <summary>
            /// 设备描述
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
            public byte[] szDescription;
        }

        /// <summary>
        /// CAN错误信息
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CAN_ErrorInformation
        {
            /// <summary>
            /// 错误类型   
            /// </summary>
            public uint uErrorCode;
            /// <summary>
            /// 消极错误数据
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            public byte[] PassiveErrData;
            /// <summary>
            ///  仲裁错误数据
            /// </summary>
            public byte ArLostErrData;
        }
        #endregion

        #region CmdCan.dll文件中的API函数

        /// <summary>
        /// 打开设备,成功返回设备句柄，失败返回0
        /// </summary>
        /// <param name="dwType">设置类型,请根据具体设备从CAN_DeviceType枚举定中选择具体类型</param>
        /// <param name="dwIndex">设备的端口号，从0开始</param>
        /// <param name="pDescription">设备描述，用户描述设备信息</param>
        /// <returns>返回0表示打开失败，否则表示返回具体的设备句柄，以后全部通过此句柄操作设备</returns>
        [DllImport("CanCmd.dll", EntryPoint = "CAN_DeviceOpen", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint CAN_DeviceOpen(uint dwType, uint dwIndex, ref string pDescription);
        /// <summary>
        /// 关闭设备 成功返回CAN_RESULT_OK ，失败返回CAN_RESULT_ERROR
        /// </summary>
        /// <param name="dwDeviceHandle">要送闭的设备句柄，通个CAN_DeviceOpen获取</param>
        /// <returns>成功返回CAN_RESULT_OK ，失败返回CAN_RESULT_ERROR</returns>
        [DllImport("CanCmd.dll", EntryPoint = "CAN_DeviceClose", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint CAN_DeviceClose(uint dwDeviceHandle);
        /// <summary>
        /// 用于初始化并启动指定的CAN通道 \r\n成功返回CAN_RESULT_OK ，失败返回CAN_RESULT_ERROR
        /// </summary>
        /// <param name="dwDeviceHandle">设备句柄</param>
        /// <param name="dwChannel">设备通道号，索引从0开始</param>
        /// <param name="InitConfig">初始化参数结构</param>
        /// <returns>成功返回CAN_RESULT_OK ，失败返回CAN_RESULT_ERROR</returns>
        [DllImport("CanCmd.dll", EntryPoint = "CAN_ChannelStart", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint CAN_ChannelStart(uint dwDeviceHandle, uint dwChannel, ref CAN_InitConfig InitConfig);
        /// <summary>
        /// 用于停止指定的CAN通道 \r\n成功返回CAN_RESULT_OK ，失败返回CAN_RESULT_ERROR
        /// </summary>
        /// <param name="dwDeviceHandle">设备句柄</param>
        /// <param name="dwChannel">设备通道号，索引从0开始</param>
        /// <returns></returns>
        [DllImport("CanCmd.dll", EntryPoint = "CAN_ChannelStop", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint CAN_ChannelStop(uint dwDeviceHandle, uint dwChannel);
        /// <summary>
        /// 用于从指定CAN通道发送数据
        /// </summary>
        /// <param name="dwDeviceHandle">设备句柄</param>
        /// <param name="dwChannel">设备通道号，索引从0开始</param>
        /// <param name="pSend">预发送数据帧数组首指针</param>
        /// <param name="nCount">预发送的数据帧数组的长度</param>
        /// <returns></returns>
        [DllImport("CanCmd.dll", EntryPoint = "CAN_ChannelSend", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint CAN_ChannelSend(uint dwDeviceHandle, uint dwChannel, ref CAN_DataFrame pSend, ulong nCount = 1);
        /// <summary>
        /// 用于从指定CAN通道读取帧数据 返回时间读到的帧数，如果返回值为0xFFFFFFF,则表示读取数据失败，有错误发生，请调用CAN_GetErrorInfo获取错误码
        /// </summary>
        /// <param name="dwDeviceHandle">设备句柄</param>
        /// <param name="dwChannel">设备通道号，索引从0开始</param>
        /// <param name="pReceive">用来接收数据帧数据组缓冲区的首指针</param>
        /// <param name="nCount">用来接收数据帧缓冲区的数组长度</param>
        /// <param name="nWaitTime">等待超时时间，以毫秒为单位</param>
        /// <returns>返回时间读到的帧数，如果返回值为0xFFFFFFF,则表示读取数据失败，有错误发生，请调用CAN_GetErrorInfo获取错误码</returns>
        [DllImport("CanCmd.dll", EntryPoint = "CAN_ChannelReceive", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint CAN_ChannelReceive(uint dwDeviceHandle, uint dwChannel, ref CAN_DataFrame pReceive, uint nCount=1, int nWaitTime = -1);
        /// <summary>
        /// 用于获取底层接口库缓冲区中接收到但尚未被读取的帧数。
        /// </summary>
        /// <param name="dwDeviceHandle">设备句柄</param>
        /// <param name="dwChannel">设备通道号</param>
        /// <returns>缓冲区帧数</returns>
        [DllImport("CanCmd.dll", EntryPoint = "CAN_GetReceiveCount", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint CAN_GetReceiveCount(uint dwDeviceHandle, uint dwChannel);
        /// <summary>
        /// 用于清空底层接口缓冲区数据
        /// </summary>
        /// <param name="dwDeviceHandle">设备句柄</param>
        /// <param name="dwChannel">设备通道号</param>
        /// <returns></returns>
        [DllImport("CanCmd.dll", EntryPoint = "CAN_ClearReceiveBuffer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint CAN_ClearReceiveBuffer(uint dwDeviceHandle, uint dwChannel);
        /// <summary>
        /// 用于获取当前设备信息
        /// </summary>
        /// <param name="dwDeviceHandle">设备句柄</param>
        /// <param name="pInfo">用于存放设备信息的结构体</param>
        /// <returns></returns>
        [DllImport("CanCmd.dll", EntryPoint = "CAN_GetDeviceInfo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint CAN_GetDeviceInfo(uint dwDeviceHandle, ref CAN_DeviceInformation pInfo);
        /// <summary>
        /// 用于获取最后一次错误信息
        /// </summary>
        /// <param name="dwDeviceHandle">设备句柄</param>
        /// <param name="dwChannel">设备通道</param>
        /// <param name="pErr">用于存储错误信息的CAN_ErrorInformation结构体指指针</param>
        /// <returns></returns>
        [DllImport("CanCmd.dll", EntryPoint = "CAN_GetErrorInfo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint CAN_GetErrorInfo(uint dwDeviceHandle, uint dwChannel, ref CAN_ErrorInformation pErr);
        /// <summary>
        /// 向CAN控制器寄存器中写入数据
        /// </summary>
        /// <param name="dwDeviceHandle">设备句柄</param>
        /// <param name="dwChannel">设备通道</param>
        /// <param name="dwAddr">CAN寄存器的地址</param>
        /// <param name="pBuff">寄存器的数据值，小端结构。如pBuff[0]-[3]为0x78 0x56 0x34 0x12则写的内容为0x12345678</param>
        /// <param name="nLen">写入数据长度</param>
        /// <returns></returns>
        [DllImport("CanCmd.dll", EntryPoint = "CAN_ReadRegister", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        static extern uint CAN_ReadRegister(uint dwDeviceHandle, uint dwChannel, uint dwAddr, ref byte[] pBuff, uint nLen);
        /// <summary>
        /// 读取CAN控制器指定寄存器的数值
        /// </summary>
        /// <param name="dwDeviceHandle">设备句柄</param>
        /// <param name="dwChannel">设备通道</param>
        /// <param name="dwAddr">CAN寄存器的址</param>
        /// <param name="pBuff">存放读取所得的寄存器值的缓冲区</param>
        /// <param name="nLen">指定读取数据长度，默认为4个字节</param>
        /// <returns></returns>
        [DllImport("CanCmd.dll", EntryPoint = "CAN_WriteRegister", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        static extern uint CAN_WriteRegister(uint dwDeviceHandle, uint dwChannel, uint dwAddr, ref byte[] pBuff, uint nLen);

        #endregion
    }
}
