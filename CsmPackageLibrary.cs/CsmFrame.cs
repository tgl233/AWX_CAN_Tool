using System;
using System.Collections.Generic;
using System.Text;

namespace CsmProtocl
{
    /// <summary>
    /// 命令包数据
    /// </summary>
    public class CsmFrame : CsmDataFrame
    {
        public CsmFrame(ushort addr, byte[] dat) :
            base(addr, dat, FrameDirection.Send, FrameFeature.independ,
             FrameType.OneIndependFrame, FramePrority.Hight)
        {

        }
    }

    /// <summary>
    /// 自主应答包数据
    /// </summary>
    public class CsmIndependAnswerFrame : CsmDataFrame
    {
        public CsmIndependAnswerFrame(ushort addr, byte[] dat, FramePrority pro) :
            base(addr, dat, FrameDirection.receive, FrameFeature.answer,
               FrameType.OneIndependFrame, pro)
        {

        }

        public new ushort Address { get { return base.Address; } }

        public new byte[] Data { get { return base.Data; } }
    }

    /// <summary>
    /// 多帧应答包数据
    /// </summary>
    public class CsmMoreAnswerFrame : CsmDataFrame
    {
        public CsmMoreAnswerFrame(ushort addr, byte[] dat, FramePrority pro) :
            base(addr, dat, FrameDirection.receive, FrameFeature.answer,
               FrameType.HaveMoreFrame, pro)
        {
        }

        public new ushort Address { get { return base.Address; } }

        public int Index { get { return base.Data[0]; } }

        public new byte[] Data
        {
            get
            {
                byte[] temp = new byte[this.Length - 1];
                Array.Copy(base.Data, 1, temp, 0, this.Length - 1);
                return temp;
            }
        }
    }

    ///<summary>
    ///多帧应答包结束包数据
    ///</summary>
    public class CsmEndMoreAnswerFrame : CsmDataFrame
    {
        public CsmEndMoreAnswerFrame(ushort addr, byte[] dat, FramePrority pro) :
            base(addr, dat, FrameDirection.receive, FrameFeature.answer,
               FrameType.EndMoreFrame, pro)
        {
        }

        public new ushort Address { get { return base.Address; } }
        public int Index { get { return base.Data[0]; } }
        public int Count { get { return base.Data[2] << 8 + base.Data[1]; } }
        public new byte[] Data
        {
            get
            {
                byte[] temp = new byte[base.Length - 3];
                Array.Copy(base.Data, 3, temp, 0, base.Length - 3);
                return temp;
            }
        }
    }
}
