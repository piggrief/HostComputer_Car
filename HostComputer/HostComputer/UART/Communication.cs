using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PigCommunication
{
    class Communication
    {
        public byte[] BagBeginning = new byte[2] { 0x55, 0xAA };//包头
        /// <summary>
        /// 功能类型号
        /// </summary>
        public enum FunctionType
        {
            CameraSend = 0,
        }

        
    }
}
