using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static DelegateDefine;


/// <summary>
/// 客户端消息管理中心
/// </summary>
public class SocketMessageManger
{
    private static SocketMessageManger instance;

    public static SocketMessageManger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SocketMessageManger();
            }
            return instance;
        }
    }

    private SocketMessageManger()
    {
        DataList = new Dictionary<MSGID, MessageFunc>();
    }

    private Dictionary<MSGID, MessageFunc> DataList;

    /// <summary>
    /// 分发消息
    /// </summary>
    /// <param name="client"></param>
    /// <param name="msg"></param>
    public void HandOutPBMsg(Client client, MSGID msgID, object pbMsg)
    {
        if (DataList == null)
        {
            return;
        }
        if (DataList.ContainsKey(msgID) == false)
        {
            return;
        }
        MessageFunc func = DataList[(MSGID)msgID];
        func(msgID, pbMsg);
    }

    /// <summary>
    /// 注册消息
    /// </summary>
    /// <param name="msgID"></param>
    /// <param name="func"></param>
    public void RegisterPBMsg(MSGID msgID, MessageFunc func)
    {
        if (DataList == null)
        {
            return;
        }
        if (func == null)
        {
            return;
        }
        if (DataList.ContainsKey(msgID) == true)
        {
            DataList[msgID] = func;
        }
        else
        {
            DataList.Add(msgID, func);
        }
    }

    /// <summary>
    /// 移除消息
    /// </summary>
    /// <param name="msgID"></param>
    public void RemovePBMsg(MSGID msgID)
    {
        if (DataList == null)
        {
            return;
        }
        if (DataList.ContainsKey(msgID) == false)
        {
            return;
        }
        DataList.Remove(msgID);
    }
}

