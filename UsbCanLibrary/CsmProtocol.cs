using System;
using System.Collections.Generic;
using System.Text;

namespace CsmProtocl
{
    public abstract class CsmDataFrame
    {
        private FrameDirection _direction = FrameDirection.Send;
        private FrameFeature _feature = FrameFeature.independ;
        private ushort _address = 0x1F;
        private FrameType _type = FrameType.OneIndependFrame;
        private FramePrority _prority = FramePrority.Low;
        private byte[] _data = new byte[8];

        /// <summary>
        /// 构造函数
        /// </summary>
        public CsmDataFrame(ushort address, byte[] data, FrameDirection direction = FrameDirection.Send,
            FrameFeature feature = FrameFeature.independ, FrameType type = FrameType.OneIndependFrame,
            FramePrority prority = FramePrority.Low)
        {
            try
            {
                Address = address;
                Data = data;
                _direction = direction;
                _feature = feature;
                _type = type;
                _prority = prority;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString(), "错误:",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public CsmDataFrame(CAN.DataFrame CanDataFram)
        {
            Address = (ushort)(CanDataFram.ID >> 3 & 0x1F);
            if ((CanDataFram.ID & 0x07) != 1)
            {
                this._type = (FrameType)(CanDataFram.ID & 0x07);
            }
            else
            {
                throw new Exception(string.Format("帧类型错误!\nID:{0}\nData:{1}",
                    CanDataFram.ID.ToString(),BitConverter.ToString(CanDataFram.Date)));
            }
            Data = CanDataFram.Date;
            _prority = (FramePrority)(CanDataFram.ID>>8 & 0x1);
            _feature = (FrameFeature)(CanDataFram.ID>>9 &0x1);
            _direction = (FrameDirection)(CanDataFram.ID>>10 &0x1);
        }

        /// <summary>
        /// 分机地址
        /// </summary>
        public ushort Address
        {
            get
            {
                return this._address;
            }
            set
            {
                if (value > 0x1f | value < 0)
                {
                    //System.Windows.Forms.MessageBox.Show(string.Format("地址{0}超范围\n分机地址只能为0-31", value), "错误:",
                    //                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    throw new Exception(string.Format("地址{0}超范围\n分机地址只能为0-31", value));
                }
                else
                {
                    this._address = value;
                }
            }
        }

        /// <summary>
        /// 接收或发送的数据
        /// </summary>
        public byte[] Data
        {
            get
            {
                return this._data;
            }
            set
            {
                if (value.Length > 8 | value == null)
                {
                    //System.Windows.Forms.MessageBox.Show("数据只能是长度小于8的非空字节数组", "错误:",
                    //    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    throw new Exception("数据只能是长度小于8的非空字节数组");
                }
                else
                {
                    this._data = value;
                }
            }
        }

        /// <summary>
        /// 帧传送方向
        /// </summary>
        public FrameDirection Direction
        {
            get
            {
                return this._direction;
            }
            //set
            //{
            //}
        }

        /// <summary>
        /// 帧特性
        /// </summary>
        public FrameFeature Feature
        {
            get
            {
                return this._feature;
            }
            //set
            //{
            //}
        }

        /// <summary>
        /// 帧优先级
        /// </summary>
        public FramePrority Prority
        {
            get
            {
                return this._prority;
            }
            //set
            //{
            //}
        }

        /// <summary>
        /// 帧类型
        /// </summary>
        public FrameType Type
        {
            get
            {
                return this._type;
            }
            //set
            //{
            //}
        }

        /// <summary>
        /// 数据长度
        /// </summary>
        public byte Length
        {
            get
            {
                return (byte)Data.Length;
            }
        }

        /// <summary>
        /// 将CsmDatFram转为DataFrame
        /// </summary>
        public CAN.DataFrame ToDataFrame()
        {
            int id = (int)this.Direction<<10+(int)this.Feature<<9+(int)this.Prority<<8+
                this.Address<<3+(int)Type;
            CAN.DataFrame temp = new CAN.DataFrame((uint)id, this.Data, (byte)this.Data.Length);
            return temp;
        }

        public enum FrameDirection
        {
            /// <summary>
            /// 站机-&gt;采集机
            /// </summary>
            Send = 0,        //站机->采集机
            /// <summary>
            /// 采集机-&gt;站机
            /// </summary>
            receive         //采集机->站机
        }

        public enum FrameFeature
        {
            /// <summary>
            /// 自主帧
            /// </summary>
            independ = 0,    //自主帧
            /// <summary>
            /// 应答帧
            /// </summary>
            answer          //应答帧
        }

        public enum FrameType
        {
            /// <summary>
            /// 自主单帧
            /// </summary>
            OneIndependFrame = 4,   //自主单帧
            /// <summary>
            /// 应答单帧
            /// </summary>
            OneAnswerFrame = 0,      //应答单帧
            /// <summary>
            /// 非结束多帧
            /// </summary>
            HaveMoreFrame = 3,      //非结束多帧
            /// <summary>
            /// 结束多帧
            /// </summary>
            EndMoreFrame = 1         //结束多帧
        }
        public enum FramePrority
        {
            /// <summary>
            /// 高优先级
            /// </summary>
            Hight = 0,
            /// <summary>
            /// 低优先级
            /// </summary>
            Low = 1,
        }
    }
}
