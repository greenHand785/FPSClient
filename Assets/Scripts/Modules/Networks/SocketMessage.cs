using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.InterNet
{
    [Serializable]
    public class SocketMessage
    {
        //操作码
        public readonly int opCode;
        //子操作
        public readonly int subCode;
        //携带参数数据
        public readonly object value;
        public SocketMessage(int opcode, int subcode, object value)
        {
            this.opCode = opcode;
            this.subCode = subcode;
            this.value = value;
        }
    }
}
