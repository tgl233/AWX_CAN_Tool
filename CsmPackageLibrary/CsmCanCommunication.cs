using System;
using System.Collections.Generic;
using System.Text;
using CAN;

namespace CsmProtocol
{
    public class CsmUsbCan
    {
        private ICAN _device = null;
        private List<CsmData> _receiveBuffer = new List<CsmData>();
        private bool _isRuning = false;
        private bool _receiveing = false;

        public CsmUsbCan(ICAN device)
        {
            this._device = device;
        }

        //public ICAN Device
        //{
        //    get
        //    {
        //        return this._device;
        //    }
        //}

        //public List<CsmData> CsmDataList
        //{
        //    get
        //    {
        //        return this._receiveBuffer;
        //    }
        //}

        public bool Runing { get { return this._isRuning; } }

        public int ReceiveCount { get { return this._receiveBuffer.Count; } }

        //public void StartReceive()
        //{
        //    if (!this._isRuning)
        //    {
        //        System.Windows.Forms.MessageBox.Show("对应的Can设备未开启,开始接收前请先开启", "CsmUsbCan.StartReceive()");
        //        return;
        //    }
        //    //如果对应的Can设备已打开
        //    while (this._isRuning)
        //    {
        //        DataFrame candata = this.Device.Receive();
        //        CsmDataFrame csmframe = new CsmDataFrame(candata.ID, candata.Date);
        //        switch (candata.ID & ~0x1f8)
        //        {
        //            //如果是多帧应答包
        //            case (int)CsmPackageType.MoreAnswerHigh:
        //                {
        //                    List<byte> dat = new List<byte>(csmframe.Data);
        //                    //循环读取多帧应答包，直到收到多帧结束包
        //                    while (true)
        //                    {
        //                        uint lastid = candata.ID;
        //                        candata = this.Device.Receive();
        //                        csmframe = new CsmDataFrame(candata.ID, candata.Date);
        //                        //如果新取出来的帧，仍是多帧应答包
        //                        if (candata.ID == lastid)
        //                        {
        //                            dat.AddRange(csmframe.Data);    //追加数据
        //                            lastid = candata.ID;
        //                            continue;
        //                        }
        //                        //如果新取出来的帧，是多帧应答包的结束包
        //                        if (candata.ID == lastid - 1)
        //                        {
        //                            dat.AddRange(csmframe.Data);
        //                            if (csmframe.Count == dat.Count)    //验证多帧应答包完整性，结束包中的Count应跟上面组合的数据长度相等
        //                            {
        //                                CsmData csmdat = new CsmData(csmframe.Address, dat);
        //                                this._receiveBuffer.Add(csmdat);  //追加一条CsmData对象到接收缓存中
        //                            }
        //                            else
        //                            {
        //                                System.Diagnostics.Debug.WriteLine("多帧应答包的数据总数跟结束包中的总数不符!", "CsmCanComunication.StartReceive");
        //                            }

        //                            break;
        //                        }
        //                        System.Diagnostics.Debug.WriteLine("多帧数据包中插入了非多帧应答包", "CsmCanComunication.StartReceive");
        //                        break;
        //                    }
        //                    break;
        //                }
        //            //非多帧应答包
        //            default:
        //                {
        //                    List<byte> dat = new List<byte>(csmframe.Data);
        //                    CsmData csmdat = new CsmData(csmframe.Address, dat);
        //                    this._receiveBuffer.Add(csmdat);
        //                    break;
        //                }
        //        }
        //    }
        //}

        public CsmData GetData()
        {
            if (this._isRuning && this.ReceiveCount != 0)
            {
                CsmData temp = this._receiveBuffer[0];
                this._receiveBuffer.RemoveAt(0);
                return temp;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("接收缓存中暂无数据！", "CsmUsbCan.GetData()");
                return null;
            }
        }

        public bool Open(Configation cfg)
        {
            if (this._device != null)
            {
                bool res = this._device.Start(cfg);
                if (res)
                {
                    this._isRuning = true;
                    this.LoopReceive();
                }
                return res;
            }
            else
            {
                return false;
            }
        }

        public bool Close()
        {
            bool res = _device.Stop();
            if (res)
                this._isRuning = false;
            return res;
        }

        private CsmData Receive()
        {
            if (!this._isRuning)
            {
                System.Windows.Forms.MessageBox.Show("对应的Can设备未开启,开始接收前请先开启", "CsmUsbCan.StartReceive()");
                return null;
            }
            //如果对应的Can设备已打开
            while (this._isRuning)
            {
                DataFrame candata = this._device.Receive();
                CsmDataFrame csmframe = new CsmDataFrame(candata.ID, candata.Date);
                switch (candata.ID & ~0x1f8)
                {
                    //如果是多帧应答包
                    case (int)CsmPackageType.MoreAnswerHigh:
                        {
                            //创建一个存数据的列表
                            List<byte> dat = new List<byte>(csmframe.Data);
                            //循环读取多帧应答包，直到收到多帧结束包
                            while (true)
                            {
                                uint lastid = candata.ID;
                                candata = this._device.Receive();
                                csmframe = new CsmDataFrame(candata.ID, candata.Date);
                                //如果新取出来的帧，仍是多帧应答包
                                if (candata.ID == lastid)
                                {
                                    dat.AddRange(csmframe.Data);    //追加数据
                                    lastid = candata.ID;
                                    continue;
                                }
                                //如果新取出来的帧，是多帧应答包的结束包
                                if (candata.ID == lastid - 1)
                                {
                                    dat.AddRange(csmframe.Data);
                                    if (csmframe.Count == dat.Count)    //验证多帧应答包完整性，结束包中的Count应跟上面组合的数据长度相等
                                    {
                                        CsmData csmdat = new CsmData(csmframe.Address, dat.ToArray());
                                        //this._receiveBuffer.Add(csmdat);  //追加一条CsmData对象到接收缓存中
                                        return csmdat;
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine("多帧应答包的数据总数跟结束包中的总数不符!", "CsmCanComunication.StartReceive");
                                        return null;
                                    }
                                }
                                System.Diagnostics.Debug.WriteLine("多帧数据包中插入了非多帧应答包", "CsmCanComunication.StartReceive");
                                return null;
                            }
                        }
                    //非多帧应答包
                    default:
                        {
                            List<byte> dat = new List<byte>(csmframe.Data);
                            CsmData csmdat = new CsmData(csmframe.Address, dat.ToArray());
                            //this._receiveBuffer.Add(csmdat);  //追加一条CsmData对象到接收缓存中
                            return csmdat;
                        }
                }
            }
            return null;
        }

        private delegate CsmData receiveDelegate();

        private IAsyncResult BeginReceive(AsyncCallback callback)
        {
            receiveDelegate rev = new receiveDelegate(Receive);
            return rev.BeginInvoke(callback, rev);
        }

        private void EndReceive(IAsyncResult ar)
        {
            receiveDelegate rev = (receiveDelegate)ar.AsyncState;
            CsmData temp = rev.EndInvoke(ar);
            this._receiveing = false;
            if (temp != null)
            {
                //事件处理接收数据，就不把接收的数据存到缓冲区
                if (EventReceive != null)
                    EventReceive(this, temp);
                else
                    this._receiveBuffer.Add(temp);//把接收到的一个数据放到缓存列表中
            }
            if (this._isRuning)
            {
                BeginReceive(new AsyncCallback(EndReceive)); //重启异步接收
            }
        }

        public void LoopReceive()
        {
            if (this._isRuning && !this._receiveing) //设备已打开且，异步接收未工作
            {
                this._receiveing = true;
                BeginReceive(new AsyncCallback(EndReceive));//开始异步接收
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("开始循环接收前，请先打开设备", "CsmUsbCan");
            }
        }

        public bool Send(int address, byte[] data)
        {
            if (this._isRuning)
            {
                if (data.Length <= 8)
                {
                    if (data[0] == 0x01)    //命令包
                    {
                        return this._device.Send(new DataFrame((uint)((address << 3) | (int)CsmPackageType.Command), data, (byte)data.Length));
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void ClearnBuffer()
        {
            this._receiveBuffer.Clear();
        }

        event EventHandler<CsmData> EventReceive;

    }
}
