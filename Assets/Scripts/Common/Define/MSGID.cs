using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//public class MSGID
//{
//    //// 登陸功能
//    //public static int MSGID_LOGIN_REQUEST = 0x00010011; // 登陆请求
//    //public static int MSGID_LOGIN_RESPONSE = 0x00010012; // 登陆回复
//    //public static int MSGID_REGISTER_REQUEST = 0x00010021; // 注册请求
//    //public static int MSGID_REGISTER_RESPONSE = 0x00010022; // 注册回复
//    //public static int MSGID_AFFIRM_LOGIN_REQUEST = 0x00010031; // 確認登陸請求
//    //public static int MSGID_AFFIRM_LOGIN_RESPONSE = 0x00010032; // 確認登陸回復

//    //// 聊天功能
//    //public static int MSGID_CHAT_SEND_REQUEST = 0x00020011; // 发送消息请求
//    //public static int MSGID_CHAT_SEND_RESPONSE = 0x00020012; // 发送消息回复
//    //public static int MSGID_CHAT_SEND_NOTIFY = 0x00020010; // 发送消息通知
//    //public static int MSGID_CHAT_SEND_FRIEND_REQUEST = 0x00020021; // 发送好友请求
//    //public static int MSGID_CHAT_SEND_FRIEND_RESPONSE = 0x00020022; // 发送好友回复
//    //public static int MSGID_CHAT_SEND_FRIEND_NOTIFY = 0x00020020; // 好友添加通知
//    //public static int MSGID_CHAT_FRIEND_DATA_REQUEST = 0x00020031; // 好友列表请求
//    //public static int MSGID_CHAT_FRIEND_DATA_RESPONSE = 0x00020032; // 好友列表回复


//}
public enum MSGID
{
    // 登陸功能
    MSGID_LOGIN_REQUEST = 0x00010011,// 登陆请求
    MSGID_LOGIN_RESPONSE = 0x00010012, // 登陆回复
    MSGID_REGISTER_REQUEST = 0x00010021, // 注册请求
    MSGID_REGISTER_RESPONSE = 0x00010022,// 注册回复
    MSGID_AFFIRM_LOGIN_REQUEST = 0x00010031, // 確認登陸請求
    MSGID_AFFIRM_LOGIN_RESPONSE = 0x00010032, // 確認登陸回復


    // 聊天功能
    MSGID_CHAT_SEND_REQUEST = 0x00020011, // 发送消息请求
    MSGID_CHAT_SEND_RESPONSE = 0x00020012, // 发送消息回复
    MSGID_CHAT_SEND_NOTIFY = 0x00020010, // 发送消息通知
    MSGID_CHAT_SEND_FRIEND_REQUEST = 0x00020021, // 发送好友请求
    MSGID_CHAT_SEND_FRIEND_RESPONSE = 0x00020022, // 发送好友回复
    MSGID_CHAT_SEND_FRIEND_NOTIFY = 0x00020020, // 好友添加通知
    MSGID_CHAT_FRIEND_DATA_REQUEST = 0x00020031, // 好友列表请求
    MSGID_CHAT_FRIEND_DATA_RESPONSE = 0x00020032, // 好友列表回复
}

