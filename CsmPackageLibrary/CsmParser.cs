using System;
using System.Collections.Generic;
using System.Text;
//using CAN;

namespace CsmProtocol
{
    #region 暂不用
    /// <summary>
    /// 方便Can接口发送的的Csm数据包类
    /// </summary>
    //public class CsmCan
    //{
    //    int _id;
    //    byte[] _data;

    //    public int Id { get { return this._id; } }
    //    public byte[] Data { get { return this._data; } }

    //    public CsmCan(int id, byte[] data)
    //    {
    //        this._id = id;
    //        this._data = data;
    //    }

    //    /// <summary>
    //    /// 显示的将CsmCan数据类型转换为CsmData数据类型
    //    /// </summary>
    //    /// <param name="csmCan">需要转换的数据类型</param>
    //    /// <returns>转换结果的数据类型</returns>
    //    public static explicit operator CsmData(CsmCan csmCan)
    //    {
    //        int type = csmCan.Id & ~0xF8;
    //        int address = (csmCan.Id & 0xF8) >> 3;
    //        int? index = null;
    //        int? count = null;
    //        byte[] data = new byte[8];

    //        if (Enum.IsDefined(typeof(CsmPackageType), type))
    //        {
    //            switch ((CsmPackageType)(type & ~0x100))
    //            {
    //                case (CsmPackageType.MoreAnswerHigh):
    //                    {
    //                        index = Convert.ToInt32(csmCan.Data[0]);
    //                        data = new byte[csmCan.Data.Length - 1];
    //                        Array.Copy(csmCan.Data, 1, data, 0, data.Length);
    //                        break;
    //                    }
    //                case (CsmPackageType.EndMoreAnswerHigh):
    //                    {
    //                        index = Convert.ToInt32(csmCan.Data[0]);
    //                        count = Convert.ToInt32(csmCan.Data[2]) << 8 + Convert.ToInt32(csmCan.Data[1]);
    //                        data = new byte[csmCan.Data.Length - 3];
    //                        Array.Copy(csmCan.Data, 3, data, 0, data.Length);
    //                        break;
    //                    }
    //                default:
    //                    {
    //                        data = csmCan.Data;
    //                        break;
    //                    }
    //            }
    //            return new CsmData(address, data, (CsmPackageType)type, index, count);
    //        }
    //        else
    //        {
    //            System.Windows.Forms.MessageBox.Show(string.Format("一个非法的Csm数据包：\nId:{0}\nData:{1}",
    //                csmCan.Id, BitConverter.ToString(csmCan.Data)), "错误：");
    //            return null;
    //        }
    //    }
    //}
    #endregion

    /// <summary>
    /// 由Can数据帧解析出来的初始Csm包类
    /// </summary>
    public class CsmDataFrame
    {
        private int _address;
        private int? _count = null;
        private int? _index = null;
        private CsmPackageType _type;
        private byte[] _data;

        /// <summary>
        /// Csm数据包类型字典
        /// </summary>
        public static readonly Dictionary<int, string> CsmPackageTypeDict = new Dictionary<int, string>{
            {(int)CsmPackageType.Command,"命令包"},{(int)CsmPackageType.IndepandHigh,"高优级先级自主包"},{(int)CsmPackageType.IndependLow,"低优先级自主包"},
            {(int)CsmPackageType.SingleAnswerHigh,"高优先级单帧应答包"},{(int)CsmPackageType.SingleAnswerLow,"低优先级单帧应答包"},
            {(int)CsmPackageType.MoreAnswerHigh,"高优先级多帧应答包"},{(int)CsmPackageType.MoreAnswerLow,"低优先级多帧应答包"},
            {(int)CsmPackageType.EndMoreAnswerHigh,"高优先级多帧应答结束包"},{(int)CsmPackageType.EndMoreAnswerLow,"低优先级多帧应答结束包"}
        };

        /// <summary>
        /// 构造函数
        /// </summary>
        public CsmDataFrame(int address, byte[] data, CsmPackageType type, int? index = null, int? count = null)
        {
            this._type = type;
            this._address = address;
            switch ((int)type & ~0x100)
            {
                case ((int)CsmPackageType.MoreAnswerHigh):
                    {
                        this._index = index;
                        break;
                    }
                case ((int)CsmPackageType.EndMoreAnswerHigh):
                    {
                        this._index = index;
                        this._count = count;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            this._data = data;
        }

        /// <summary>
        /// 由CAN的ID和Data的构造函数
        /// </summary>
        public CsmDataFrame(uint id, byte[] data)
        {

            int temp = (int)(id & ~0x1F8);
            if (Enum.IsDefined(typeof(CsmPackageType), temp))   //判断id是不是Csm定义的数据包
            {
                this._address = (int)(id & 0xF8)>>3;
                this._type = (CsmPackageType)(id & ~0xF8);
                switch (temp)
                {
                    case ((int)CsmPackageType.MoreAnswerHigh):
                        {
                            this._index = data[0];
                            this._count = null;
                            this._data = new byte[data.Length - 1];
                            Array.Copy(data, 1, this._data, 0, this._data.Length);
                            break;
                        }
                    case ((int)CsmPackageType.EndMoreAnswerHigh):
                        {
                            this._index = data[0];
                            this._count = (data[2]<<8)+data[1];
                            this._data = new byte[data.Length - 3];
                            Array.Copy(data, 3, this._data, 0, this._data.Length);
                            break;
                        }
                    default:
                        {
                            this._index = null;
                            this._count = null;
                            this._data = data;
                            break;
                        }
                }
            }
            else
            {
                throw new Exception("这不一个Csm数据包");
            }
        }

        /// <summary>
        /// 跟据Can的ID解析是什么Csm包
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string ParserIdToString(uint id)
        {
            int temp = (int)(id & ~0xF8);
            if (Enum.IsDefined(typeof(CsmPackageType), temp))
                return string.Format("分机号：{0}\t\t数据包类型：{1}", (id & 0x8F) >> 3, CsmPackageTypeDict[temp]);
            else
                return "非定义的Csm数据帧格式";
        }

        /// <summary>
        /// 数据包类型
        /// </summary>
        public CsmPackageType Type
        {
            get
            {
                return this._type;
            }
        }

        /// <summary>
        /// 采集机分机号
        /// </summary>
        public int Address
        {
            get
            {
                return this._address;
            }
        }

        /// <summary>
        /// 多帧序号
        /// </summary>
        public int? Index
        {
            get
            {
                return this._index;
            }
        }

        /// <summary>
        /// 多帧数据长度
        /// </summary>
        public int? Count
        {
            get
            {
                return this._count;
            }
        }

        /// <summary>
        /// 数据内容
        /// </summary>
        public byte[] Data
        {
            get
            {
                return this._data;
            }
        }

        #region 暂不用
        /// <summary>
        /// 显示的转换将DataFrame类型转换为CsmData类型
        /// </summary>
        /// <param name="dataFrame"></param>
        /// <returns></returns>
        //public static explicit operator CsmData(DataFrame dataFrame)
        //{
        //    return new CsmData(dataFrame.ID, dataFrame.Date);
        //}
        /// <summary>
        /// 显示的将CsmData数据类型转为CsmCan类型
        /// </summary>
        /// <param name="data">待转换的CsmData数据类型</param>
        /// <returns>显示转换后的CsmCan数据类型</returns>
        //public static explicit operator CsmCan(CsmData data)
        //{
        //    byte[] dat;
        //    switch ((CsmPackageType)((int)data.Type&~0x100))
        //    {
        //        case CsmPackageType.MoreAnswerHigh:
        //            {
        //                dat = new byte[data.Data.Length + 1];
        //                dat[0] = Convert.ToByte(data.Index);
        //                Array.Copy(data.Data, 0, dat, 1, data.Data.Length);
        //                break;
        //            }
        //        case CsmPackageType.EndMoreAnswerHigh:
        //            {
        //                dat = new byte[data.Data.Length + 3];
        //                dat[0] = Convert.ToByte(data.Index);
        //                dat[1] = Convert.ToByte(data.Count & 0xFF);
        //                dat[2] = Convert.ToByte(data.Count >> 8);
        //                Array.Copy(data.Data, 0, dat, 3, data.Data.Length);
        //                break;
        //            }
        //        default:
        //            {
        //                dat = data.Data;
        //                break;
        //            }
        //    }

        //    return new CsmCan((int)data.Type | (data.Address<<3), dat);
        //}
        #endregion

        /// <summary>
        /// 获取CsmData对应的CanDataFrame的ID
        /// </summary>
        /// <returns></returns>
        public int GetId()
        {
            return (int)this.Type | Address << 3;
        }

        /// <summary>
        /// 获取CsmData对应的CanDataFrame的Data
        /// </summary>
        /// <returns></returns>
        public byte[] GetData()
        {
            byte[] dat;

            switch ((int)this.Type & ~0x100)
            {
                case (int)CsmPackageType.MoreAnswerHigh:
                    {
                        dat = new byte[this.Data.Length + 1];
                        dat[0] = Convert.ToByte(this.Index);
                        Array.Copy(this.Data, 0, dat, 1, this.Data.Length);
                        break;
                    }
                case (int)CsmPackageType.EndMoreAnswerHigh:
                    {
                        dat = new byte[this.Data.Length + 3];
                        dat[0] = Convert.ToByte(this.Index);
                        dat[1] = Convert.ToByte(this.Count & 0xFF);
                        dat[2] = Convert.ToByte(this.Count >> 8);
                        Array.Copy(this.Data, 0, dat, 3, this.Data.Length);
                        break;
                    }
                default:
                        {
                            dat = this.Data;
                            break;
                        }
            }

            return dat;
        }


    }

    /// <summary>
    /// Csm上层应用解析所需的数据类
    /// </summary>
    public class CsmData:EventArgs
    {
        private int _add = 0;
        private byte[] _data = null;

        public CsmData(int addr,byte[] data)
        {
            this._add = addr;
            this._data = data;
        }

        /// <summary>
        /// 分机号
        /// </summary>
        public int Address
        {
            get
            {
                return this._add;
            }
        }

        /// <summary>
        /// 组合后的数据
        /// </summary>
        public byte[] Data
        {
            get
            {
                return this._data;
            }
        }

    }

    /// <summary>
    /// Csm数据包类型枚举类型
    /// </summary>
    public enum CsmPackageType
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
}
