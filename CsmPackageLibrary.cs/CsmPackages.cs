using System;
using System.Collections.Generic;
using System.Text;
using CAN;

namespace CsmProtocl
{
    /// <summary>
    /// Csm数据包基类
    /// </summary>
    public abstract class CsmPackageBase
    {
        private CsmPackType _type;
        private int _address;
        private byte[] _data;
        private int _len;

        public CsmPackageBase(CsmPackType type, int addr, byte[] dat, int len)
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

        public CsmPackageBase(CsmPackType type, int addr, int index, byte[] dat, int len)
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

        public CsmPackageBase(CsmPackType type, int addr, int ind, int con, byte[] dat, int len)
        {
            if (ind > 255)
                throw new Exception("多帧应答包的索引不能大于255");
            if(con> 0xFFFF)
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
        /// 数据包类型
        /// </summary>
        public CsmPackType PackType
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
                return (int)PackType | Address;
            }
        }
    }

    public class CommandPackage : CsmPackageBase
    {
        /// <param name="address">采集机地址</param>
        /// <param name="data">数据内容</param>
        public CommandPackage(int address, byte[] data) :
            base(CsmPackType.Command, address, data, data.Length)
        {
            //具体实现在基类中
        }
    }

    public class IndependPackage : CsmPackageBase
    {
        public IndependPackage(PriorityType priority, int address, byte[] data) :
            base((CsmPackType)((int)priority | (int)(CsmPackType.IndepandHigh)),
               address, data, data.Length)
        {
            //throw new System.NotImplementedException();
        }
        /// <summary>
        /// 优先级
        /// </summary>
        public PriorityType Priority
        {
            get
            {
                return (PriorityType)((int)base.PackType& (int)CsmPackType.IndepandHigh);
            }
        }
    }

    public class SingleAnswerPackage : CsmPackageBase
    {
        /// <param name="priority">优先级</param>
        public SingleAnswerPackage(PriorityType priority, int address, byte[] data) :
            base((CsmPackType)((int)priority | (int)CsmPackType.SingleAnswerHigh),
               address, data, data.Length)
        {
        }
        /// <summary>
        /// 优先级
        /// </summary>
        public PriorityType Priority
        {
            get
            {
                return (PriorityType)((int)base.PackType&(int)CsmPackType.SingleAnswerHigh);
            }
        }
    }

    public class MoreAnswerPackage : CsmPackageBase
    {
        private byte[] _data = null;

        public MoreAnswerPackage(PriorityType prioriyt,int address, int index, byte[] data):
            base((CsmPackType)((int)prioriyt|(int)CsmPackType.MoreAnswerHigh),
            address,index,data,data.Length+1)
        {
            this._data = data;
        }
        /// <summary>
        /// 优先级
        /// </summary>
        public PriorityType Priority
        {
            get
            {
                return (PriorityType)((int)base.PackType & (int)PriorityType.Hight);
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
        public new  byte[] Data
        {
            get
            {
                return this._data;
            }
        }
    }

    public class EndMoreAnswerPackage : CsmPackageBase
    {
        private int _index =0;
        private int _count=0;
        private byte[] _data=null;

        public EndMoreAnswerPackage(PriorityType priority,int address,int index, int count,byte[] data):
            base((CsmPackType)((int)priority | (int)CsmPackType.EndMoreAnswerHigh),
            address,index,count,data,data.Length+3)
        {
            this._index = index;
            this._count = count;
            this._data = data;
        }
        /// <summary>
        /// 优先级
        /// </summary>
        public PriorityType Priority
        {
            get
            {
                return (PriorityType)((int)base.PackType & (int)PriorityType.Low);
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
        public new  byte[] Data
        {
            get
            {
                return this._data;
            }
        }
    }


    //Csm数据包类型枚举
    public enum CsmPackType
    {
        /// <summary>
        /// 命令包
        /// </summary>
        Command = 0x000,
        /// <summary>
        /// 高优先级自主应答包
        /// </summary>
        IndepandHigh = 0x404,
        /// <summary>
        /// 高优先级单帧应答包
        /// </summary>
        SingleAnswerHigh = 0x600,
        /// <summary>
        /// 高优先级多帧应答包
        /// </summary>
        MoreAnswerHigh = 0x603,
        /// <summary>
        /// 高优先级多帧应答结尾包
        /// </summary>
        EndMoreAnswerHigh = 0x602,
        /// <summary>
        /// 低优先级自主应答包
        /// </summary>
        IndependLow = 0x504,
        /// <summary>
        /// 低优先级单帧应答包
        /// </summary>
        SingleAnswerLow = 0x700,
        /// <summary>
        /// 低优先级多帧应答包
        /// </summary>
        MoreAnswerLow = 0x703,
        /// <summary>
        /// 低优先级多帧应答结尾包
        /// </summary>
        EndMoreAnswerLow = 0x702,
    }

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
