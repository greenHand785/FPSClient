using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    void Start()
    {
        Debug.Log("开始游戏游戏啦...");
        UIManager.Instance.ShowPanel(PanelDefine.Panel_LoginPanel, true);
        ClientInternetManger.Instance.StartConnection();
    }
}
