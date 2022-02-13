using Server.Ygy.Game.Pb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginPanel : PanelBase
{
    private GameObject mask;
    private InputField accountInput;
    private InputField passwordInput;
    private GameObject loginBtn;

    private void InitParam()
    {
        mask = Find(transform, "Mask");
        accountInput = GetComponent<InputField>(transform, "InputVer/InputHor/AccountInput");
        passwordInput = GetComponent<InputField>(transform, "InputVer/InputHor (1)/PasswordInput");
        loginBtn = Find(transform, "LoginBtn");

        
        // 注册事件
        RegisterClickEvent(mask, OnClickMask);
        RegisterClickEvent(loginBtn, OnClickLoginBtn);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        InitParam();

    }


    // Update is called once per frame
    void Update()
    {
        
    }


    // 点击mask事件
    private void OnClickMask(BaseEventData data)
    {
        UIManager.Instance.ShowPanel(PanelDefine.Panel_LoginPanel, false);
    }

    // 点击登陆按钮
    private void OnClickLoginBtn(BaseEventData data)
    {
        PBMsgLoginRequest pBMsgLoginRequest = new PBMsgLoginRequest();
        pBMsgLoginRequest.Account = "123456";
        pBMsgLoginRequest.UserPassword = "12345";
        // TODO
        LoginHandleManager.Instance.OnLoginRequest(pBMsgLoginRequest);
    }
}
