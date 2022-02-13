using Server.Ygy.Game.Pb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class PBCodingTool
    {
        public delegate T DeserializeTmp<T>(Stream stream);
        public delegate void SerializeTmp<T>(Stream stream, T instance);

        // 序列化pb协议
        public static void PBSerialize(MSGID msgId, Stream stream, object value)
        {
            switch (msgId)
            {
                case MSGID.MSGID_LOGIN_REQUEST:
                    {
                        PBMsgLoginRequest.Serialize(stream, value as PBMsgLoginRequest);
                    }; break;
                case MSGID.MSGID_LOGIN_RESPONSE:
                    {
                        PBMsgLoginResponse.Serialize(stream, value as PBMsgLoginResponse);
                    }; break;
                case MSGID.MSGID_REGISTER_REQUEST:
                    {
                        PBMsgRegisterRequest.Serialize(stream, value as PBMsgRegisterRequest);
                    }; break;
                case MSGID.MSGID_REGISTER_RESPONSE:
                    {
                        PBMsgRegisterResponse.Serialize(stream, value as PBMsgRegisterResponse);
                    }; break;
                case MSGID.MSGID_AFFIRM_LOGIN_REQUEST:
                    {
                        PBMsgAffirmLoginRequest.Serialize(stream, value as PBMsgAffirmLoginRequest);
                    }; break;
                case MSGID.MSGID_AFFIRM_LOGIN_RESPONSE:
                    {
                        PBMsgAffirmLoginResponse.Serialize(stream, value as PBMsgAffirmLoginResponse);
                    }; break;


                case MSGID.MSGID_CHAT_SEND_REQUEST:
                    {
                        PBMsgChatSendRequest.Serialize(stream, value as PBMsgChatSendRequest);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_RESPONSE:
                    {
                        PBMsgChatSendResponse.Serialize(stream, value as PBMsgChatSendResponse);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_NOTIFY:
                    {
                        PBMsgChatSendNotify.Serialize(stream, value as PBMsgChatSendNotify);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_FRIEND_REQUEST:
                    {
                        PBMsgChatSendFriendRequest.Serialize(stream, value as PBMsgChatSendFriendRequest);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_FRIEND_RESPONSE:
                    {
                        PBMsgChatSendFriendResponse.Serialize(stream, value as PBMsgChatSendFriendResponse);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_FRIEND_NOTIFY:
                    {
                        PBMsgChatSendFriendNotify.Serialize(stream, value as PBMsgChatSendFriendNotify);
                    }; break;
                case MSGID.MSGID_CHAT_FRIEND_DATA_REQUEST:
                    {
                        PBMsgChatFriendDataRequest.Serialize(stream, value as PBMsgChatFriendDataRequest);
                    }; break;
                case MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE:
                    {
                        PBMsgChatFriendDataResponse.Serialize(stream, value as PBMsgChatFriendDataResponse);
                    }; break;
            }

        }

        // 反序列化pb协议
        public static object PBDeSerialize(MSGID msgId, Stream stream)
        {
            object value = null;
            switch (msgId)
            {
                case MSGID.MSGID_LOGIN_REQUEST: {
                        value = PBMsgLoginRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_LOGIN_RESPONSE: {
                        value = PBMsgLoginResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_REGISTER_REQUEST: {
                        value = PBMsgRegisterRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_REGISTER_RESPONSE: {
                        value = PBMsgRegisterResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_AFFIRM_LOGIN_REQUEST: {
                        value = PBMsgAffirmLoginRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_AFFIRM_LOGIN_RESPONSE: {
                        value = PBMsgAffirmLoginResponse.Deserialize(stream);
                    }; break;


                case MSGID.MSGID_CHAT_SEND_REQUEST: {
                        value = PBMsgChatSendRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_RESPONSE: {
                        value = PBMsgChatSendResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_NOTIFY: {
                        value = PBMsgChatSendNotify.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_FRIEND_REQUEST: {
                        value = PBMsgChatSendFriendRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_FRIEND_RESPONSE: {
                        value = PBMsgChatSendFriendResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_FRIEND_NOTIFY: {
                        value = PBMsgChatSendFriendNotify.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_FRIEND_DATA_REQUEST: {
                        value = PBMsgChatFriendDataRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE: {
                        value = PBMsgChatFriendDataResponse.Deserialize(stream);
                    }; break;
            }
            return value;
        }
    }
