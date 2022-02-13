using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DelegateDefine
{
    // 消息方法
    public delegate void MessageFunc(MSGID pbMsg, object pbMsgObj);
    // 事件方法
    //public delegate void EventFunc(ClientPeer client);
}