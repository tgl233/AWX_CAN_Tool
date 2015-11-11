using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CAN
{
    public class ACUSB_132B : UsbCanDevice
    {
        private UsbCan _can0;
        private UsbCan _can1;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">USB端口索引</param>
        public ACUSB_132B(uint index)
            : base(DeviceType.ACUSB_132B, index)
        {
            this._can0 = new UsbCan(this.Handle, 0);
            this._can1 = new UsbCan(this.Handle, 1);
        }

        /// <summary>
        /// CAN通道对象
        /// </summary>
        public ICAN CAN0 { get { return this._can0; } }

        /// <summary>
        /// CAN通道对象
        /// </summary>
        public ICAN CAN1 { get { return this._can1; } }

    }
}
