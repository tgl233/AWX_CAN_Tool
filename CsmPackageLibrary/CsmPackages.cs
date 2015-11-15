using System;
using System.Collections.Generic;
using System.Text;
using CAN;

namespace CsmProtocol
{
    /// <summary>
    /// Csm数据包基类
    /// </summary>
    public abstract class CsmPackageBase : EventArgs
    {
        private CsmPackageType _type;
        private int _address;
        private byte[] _data;
        private int _len;

        public static readonly Dictionary<int, string> CsmPackageTypeDict = new Dictionary<int, string>{
            {(int)CsmPackageType.Command,"命包"},{(int)CsmPackageType.IndepandHigh,"高级先级自主包"},{(int)CsmPackageType.IndependLow,"低优先级自主包"},
            {(int)CsmPackageType.SingleAnswerHigh,"高优先级单帧应答包"},{(int)CsmPackageType.SingleAnswerLow,"低优先级单帧应答包"},
            {(int)CsmPackageType.MoreAnswerHigh,"高优先级多帧应答包"},{(int)CsmPackageType.MoreAnswerLow,"高优先级多帧应答包"},
            {(int)CsmPackageType.EndMoreAnswerHigh,"高优先级多帧应答结束包"},{(int)CsmPackageType.EndMoreAnswerLow,"低优先级多帧应答结束包"}
        };

        /// <summary>
        /// 单帧构造函数
        /// </summary>
        public CsmPackageBase(CsmPackageType type, int addr, byte[] dat, int len)
        {
            if (addr > 0 || addr < 0)
                throw new Exception("采集分机地址只能是0-31");
            if (dat == null)
                throw new Exception("数据不以为空值");
            if (len > 8 || dat.Length > 8)
                throw new Exception("每帧数据长度能超过8字节");

            this._type = type;
            this._address = addr;
            this._data = dat;
            this._len = len;
        }

        /// <summary>
        /// 应答多帧构造函数
        /// </summary>
        public CsmPackageBase(CsmPackageType type, int addr, int index, byte[] dat, int len)
        {
            if (index > 255)
                throw new Exception("多帧应答包的索引不能大于255");
            if (addr > 0 || addr < 0)
                throw new Exception("采集分机地址只能是0-31");
            if (dat == null)
                throw new Exception("数据不以为空值");
            if (len > 8 || dat.Length > 7)
                throw new Exception("每个多帧应答包的实际数据长度能超过7字节");

            byte[] data = new byte[dat.Length + 1];
            Array.Copy(dat, 0, data, 1, dat.Length);
            data[0] = (byte)index;

            this._type = type;
            this._address = addr;
            this._data = data;
            this._len = len;
        }

        /// <summary>
        /// 应答多帧结束包构造函数
        /// </summary>
        public CsmPackageBase(CsmPackageType type, int addr, int ind, int con, byte[] dat, int len)
        {
            if (ind > 255)
                throw new Exception("多帧应答包的索引不能大于255");
            if (con > 0xFFFF)
                throw new Exception("多帧应答包的数据总数据不能大于0xFFFF");
            if (addr > 0 || addr < 0)
                throw new Exception("采集分机地址只能是0-31");
            if (dat == null)
                throw new Exception("数据不以为空值");
            if (len > 8 || dat.Length > 5)
                throw new Exception("每个多帧应答结尾包的实际数据长度能超过5字节");

            this._type = type;
            this._address = addr;
            this._len = len;
            byte[] data = new byte[dat.Length + 3];
            Array.Copy(dat, 0, data, 1, dat.Length);
            data[0] = (byte)ind;
            data[1] = (byte)(con & 0xFF);
            data[2] = (byte)(con >> 4);
        }

        /// <summary>
        /// 直接通过ID和Data构造
        /// </summary>
        public CsmPackageBase(int id, byte[] dat)
        {
            if (id > 0x7FF)
                throw new Exception("不是一个正确的Csm数据包");
            try
            {
                CsmPackageType type = (CsmPackageType)(id & ~0xF8);
                this._address = (id & 0xF8) >> 3;
                this._data = dat;
                this._len = dat.Length;
            }
            catch 
            {
                throw new Exception("不是一个正确的Csm数据包");
            }

        }

        /// <summary>
        /// 数据包类型
        /// </summary>
        public CsmPackageType PackageType
        {
            get
            {
                return this._type;
            }
        }

        /// <summary>
        /// 采集分机地址
        /// </summary>
        public int Address
        {
            get
            {
                return this._address;
            }

        }

        /// <summary>
        /// 数据长度
        /// </summary>
        public int Length
        {
            get
            {
                return this._len;
            }
        }

        /// <summary>
        /// 实际数据
        /// </summary>
        public byte[] Data
        {
            get
            {
                return this._data;
            }
        }

        /// <summary>
        /// 帧ID
        /// </summary>
        public int ID
        {
            get
            {
                return (int)PackageType | Address;
            }
        }

        /// <summary>
        /// 通过ID和Data解析为对应的Csm数据包
        /// </summary>
        /// <param name="id"></param>
        public void PackagePaser(int id, byte[] data)
        {

        }
    }

    /// <summary>
    /// 命令包
    /// </summary>
    public class CommandPackage : CsmPackageBase
    {
        public event System.EventHandler<CommandPackage> EventCommandPackage;
        /// <summary>
        /// 命令包构造函数
        /// </summary>
        /// <param name="address">采集机地址</param>
        /// <param name="data">数据内容</param>
        public CommandPackage(byte address, byte[] data) :
            base(CsmPackageType.Command, address, data, data.Length)
        {
            //具体实现在基类中
        }

        public CommandPackage(int id, byte[] data) :
            base(id, data)
        {
        }

    }

    /// <summary>
    /// 自主帧
    /// </summary>
    public class IndependPackage : CsmPackageBase
    {
        public event System.EventHandler<IndependPackage> EventIndependPackage;
        /// <summary>
        /// 应答自主帧构造函数
        /// </summary>
        public IndependPackage(PriorityType priority, int address, byte[] data) :
            base((CsmPackageType)((int)priority | (int)(CsmPackageType.IndepandHigh)),
               address, data, data.Length)
        {
            //throw new System.NotImplementedException();
        }

        public IndependPackage(int id, byte[] data)
            : base(id, data)
        {
        }
        /// <summary>
        /// 优先级
        /// </summary>
        public PriorityType Priority
        {
            get
            {
                return (PriorityType)((int)base.PackageType & (int)CsmPackageType.IndepandHigh);
            }
        }
    }

    /// <summary>
    /// 应答单帧
    /// </summary>
    public class SingleAnswerPackage : CsmPackageBase
    {
        /// <summary>
        /// 应答单帧构造函数
        /// </summary>
        /// <param name="priority">优先级</param>
        public SingleAnswerPackage(PriorityType priority, int address, byte[] data) :
            base((CsmPackageType)((int)priority | (int)CsmPackageType.SingleAnswerHigh),
               address, data, data.Length)
        {
        }
        public SingleAnswerPackage(int id, byte[] data):
            base(id,data)
        {
        }

        /// <summary>
        /// 优先级
        /// </summary>
        public PriorityType Priority
        {
            get
            {
                return (PriorityType)((int)base.PackageType & (int)CsmPackageType.SingleAnswerHigh);
            }
        }

        public event EventHandler<SingleAnswerPackage> EventSingleAnswer;
    }

    /// <summary>
    /// 应答多帧
    /// </summary>
    public class MoreAnswerPackage : CsmPackageBase
    {
        private byte[] _data = null;

        public event System.EventHandler<MoreAnswerPackage> EventMoreAnserPackage;

        /// <summary>
        /// 应答多帧构造函数
        /// </summary>
        public MoreAnswerPackage(PriorityType prioriyt, int address, int index, byte[] data) :
            base((CsmPackageType)((int)prioriyt | (int)CsmPackageType.MoreAnswerHigh),
              address, index, data, data.Length + 1)
        {
            this._data = data;
        }

        public MoreAnswerPackage(int id, byte[] data) :
            base(id, data)
        {
        }

        /// <summary>
        /// 优先级
        /// </summary>
        public PriorityType Priority
        {
            get
            {
                return (PriorityType)((int)base.PackageType & (int)PriorityType.Hight);
            }
        }

        /// <summary>
        /// 多帧数据中该帧的索引号
        /// </summary>
        public int Index
        {
            get
            {
                return base.Data[0];
            }
        }

        /// <summary>
        /// 实际数据
        /// </summary>
        public new byte[] Data
        {
            get
            {
                return this._data;
            }
        }
    }

    /// <summary>
    /// 应答多帧结束帧
    /// </summary>
    public class EndMoreAnswerPackage : CsmPackageBase
    {
        private int _index = 0;
        private int _count = 0;
        private byte[] _data = null;

        public event System.EventHandler<EndMoreAnswerPackage> EventEndMoreAnswerPackage;

        /// <summary>
        /// 应答多帧结束包构造函数
        /// </summary>
        public EndMoreAnswerPackage(PriorityType priority, int address, int index, int count, byte[] data) :
            base((CsmPackageType)((int)priority | (int)CsmPackageType.EndMoreAnswerHigh),
                address, index, count, data, data.Length + 3)
        {
            this._index = index;
            this._count = count;
            this._data = data;
        }

        public EndMoreAnswerPackage(int id, byte[] data) :
            base(id, data)
        {
        }
        /// <summary>
        /// 优先级
        /// </summary>
        public PriorityType Priority
        {
            get
            {
                return (PriorityType)((int)base.PackageType & (int)PriorityType.Low);
            }
        }

        /// <summary>
        /// 多帧数据的索引号
        /// </summary>
        public int Index
        {
            get
            {
                return this._index;
            }
        }

        /// <summary>
        /// 多帧数据的数据总数
        /// </summary>
        public int Count
        {
            get
            {
                return this._count;
            }
        }

        /// <summary>
        /// 该帧数据中实际的数据
        /// </summary>
        public new byte[] Data
        {
            get
            {
                return this._data;
            }
        }
    }

    ///// <summary>
    ///// Csm数据包类型枚举类型
    ///// </summary>
    //public enum CsmPackageType
    //{
    //    /// <summary>
    //    /// 命令包
    //    /// </summary>
    //    Command = 0x000,
    //    /// <summary>
    //    /// 高优先级自主应答包
    //    /// </summary>
    //    IndepandHigh = 0x404,
    //    /// <summary>
    //    /// 高优先级单帧应答包
    //    /// </summary>
    //    SingleAnswerHigh = 0x600,
    //    /// <summary>
    //    /// 高优先级多帧应答包
    //    /// </summary>
    //    MoreAnswerHigh = 0x603,
    //    /// <summary>
    //    /// 高优先级多帧应答结尾包
    //    /// </summary>
    //    EndMoreAnswerHigh = 0x602,
    //    /// <summary>
    //    /// 低优先级自主应答包
    //    /// </summary>
    //    IndependLow = 0x504,
    //    /// <summary>
    //    /// 低优先级单帧应答包
    //    /// </summary>
    //    SingleAnswerLow = 0x700,
    //    /// <summary>
    //    /// 低优先级多帧应答包
    //    /// </summary>
    //    MoreAnswerLow = 0x703,
    //    /// <summary>
    //    /// 低优先级多帧应答结尾包
    //    /// </summary>
    //    EndMoreAnswerLow = 0x702,
    //}

    /// <summary>
    /// Csm数据包的优先级
    /// </summary>
    public enum PriorityType
    {
        //ID中的第9位为优先级选择位
        /// <summary>
        /// 高优先级
        /// </summary>
        Hight = 0x000,
        /// <summary>
        /// 低优先级
        /// </summary>
        Low = 0x100,
    }
}
