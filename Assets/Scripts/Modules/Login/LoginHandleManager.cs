using Server.Ygy.Game.Pb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginHandleManager : INetHandleManager
{
    #region 单例
    private static LoginHandleManager instance;
    public static LoginHandleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LoginHandleManager();
            }
            return instance;
        }
    }

    private LoginHandleManager()
    {

    }
    #endregion
    #region 消息注册
    public void RegisterPBMsg()
    {
        SocketMessageManger.Instance.RegisterPBMsg(MSGID.MSGID_LOGIN_RESPONSE, OnLoginResponse);
        SocketMessageManger.Instance.RegisterPBMsg(MSGID.MSGID_REGISTER_RESPONSE, OnRegisterResponse);
        SocketMessageManger.Instance.RegisterPBMsg(MSGID.MSGID_AFFIRM_LOGIN_RESPONSE, OnAffirmLoginResponse);
    }

    public void RemovePBMsg()
    {
        SocketMessageManger.Instance.RemovePBMsg(MSGID.MSGID_LOGIN_RESPONSE);
        SocketMessageManger.Instance.RemovePBMsg(MSGID.MSGID_REGISTER_RESPONSE);
        SocketMessageManger.Instance.RemovePBMsg(MSGID.MSGID_AFFIRM_LOGIN_RESPONSE);
    }
    #endregion
    #region 消息定义

    // 登陆回复
    private void OnLoginResponse(MSGID msgId, object pbMsg)
    {
        PBMsgLoginResponse pb = pbMsg as PBMsgLoginResponse;
        if (pb == null)
        {
            return;
        }
        LoginManager.Instance.OnLoginResponse(pb);
    }

    // 注册回复
    private void OnRegisterResponse(MSGID msgId, object pbMsg)
    {
        PBMsgRegisterResponse pb = pbMsg as PBMsgRegisterResponse;
        if (pb == null)
        {
            return;
        }
        LoginManager.Instance.OnRegisterResponse(pb);
    }

    // 确认登陆回复
    private void OnAffirmLoginResponse(MSGID msgId, object pbMsg)
    {
        PBMsgAffirmLoginResponse pb = pbMsg as PBMsgAffirmLoginResponse;
        if (pb == null)
        {
            return;
        }
        LoginManager.Instance.OnAffirmLoginResponse(pb);
    }

    // 登陆请求
    public void OnLoginRequest(PBMsgLoginRequest pbMsg)
    {
        if(pbMsg == null)
        {
            return;
        }
        if(ClientInternetManger.Instance.client == null)
        {
            return;
        }
        ClientInternetManger.Instance.client.Send(MSGID.MSGID_LOGIN_REQUEST, pbMsg);
    }
    // 注册请求
    public void OnRegisterRequest(PBMsgRegisterRequest pbMsg)
    {
        if (pbMsg == null)
        {
            return;
        }
        if (ClientInternetManger.Instance.client == null)
        {
            return;
        }
        ClientInternetManger.Instance.client.Send(MSGID.MSGID_REGISTER_REQUEST, pbMsg);
    }
    // 确认登陆请求
    public void OnAfrimLoginRequest(PBMsgAffirmLoginRequest pbMsg)
    {
        if (pbMsg == null)
        {
            return;
        }
        if (ClientInternetManger.Instance.client == null)
        {
            return;
        }
        ClientInternetManger.Instance.client.Send(MSGID.MSGID_AFFIRM_LOGIN_REQUEST, pbMsg);
    }
    #endregion
}
