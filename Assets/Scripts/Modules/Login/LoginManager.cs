using Server.Ygy.Game.Pb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManager :IModuleManager
{
    #region 单例
    private static LoginManager instance;
    public static LoginManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LoginManager();
            }
            return instance;
        }
    }
    private LoginManager()
    {

    }
    #endregion
    #region 生命周期函数定义

    public override void Start()
    {

    }

    public override void Update()
    {

    }

    public override void Destory()
    {

    }
    #endregion
    #region 消息回复

    // 登陆回复
    public void OnLoginResponse(PBMsgLoginResponse pbMsg)
    {
        if (pbMsg == null)
        {
            return;
        }

        // TODO
    }

    // 注册回复
    public void OnRegisterResponse(PBMsgRegisterResponse pbMsg)
    {
        if (pbMsg == null)
        {
            return;
        }
        // TODO
    }

    // 确认登陆回复
    public void OnAffirmLoginResponse(PBMsgAffirmLoginResponse pbMsg)
    {
        if (pbMsg == null)
        {
            return;
        }

        // TODO
    }
    #endregion
}
