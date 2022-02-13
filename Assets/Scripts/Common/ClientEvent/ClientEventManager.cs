using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientEventManager : MonoBehaviour
{
    private class CalcuObj
    {
        private long endTime;

        private int eventType;

        private object[] paramList;

        public object[] ParamList
        {
            get
            {
                return paramList;
            }
        }

        public long EndTime
        {
            get
            {
                return endTime;
            }
        }

        public int EventType
        {
            get
            {
                return eventType;
            }
        }

        public CalcuObj(int eventType, long endTime, params object[] param)
        {
            this.endTime = endTime;
            this.eventType = eventType;
            this.paramList = param;
        }
    }

    private static ClientEventManager instance;

    private Dictionary<int, List<Action<object[]>>> funcDic;

    private List<CalcuObj> delayEventList;

    private string componentName;

    private ClientEventManager()
    {
        funcDic = new Dictionary<int, List<Action<object[]>>>();
        delayEventList = new List<CalcuObj>();
        componentName = "ClientEventManager";
        GameObject obj = GameObject.Find(componentName);
        if(obj == null)
        {
            obj = new GameObject();
            obj.name = componentName;
        }
    }

    public static ClientEventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ClientEventManager();
            }
            return instance;
        }
    }

    /// <summary>
    /// 执行事件
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="param">事件参数</param>
    private void ActiveEvent(int eventType, params object[] param)
    {
        if(funcDic == null)
        {
            Debug.LogError("funcDic对象不存在");
            return;
        }
        List<Action<object[]>> eventList = funcDic[eventType];
        if(eventList == null)
        {
            Debug.LogError("eventList对象不存在");
            return;
        }
        for(int i = 0; i < eventList.Count; i++)
        {
            Action<object[]> func = eventList[i];
            if(func == null)
            {
                continue;
            }
            func(param);
        }
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="action">事件</param>
    public void RegisterEvent(int eventType, Action<object[]> action)
    {
        if(funcDic.ContainsKey(eventType) == true)
        {
            List<Action<object[]>> list = funcDic[eventType];
            if(list == null)
            {
                return;
            }
            list.Add(action);
        }
        else
        {
            List<Action<object[]>> list = new List<Action<object[]>>();
            list.Add(action);
            funcDic.Add(eventType, list);
        }
    } 

    /// <summary>
    /// 执行事件
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="immediately">是否立即执行</param>
    /// <param name="delayTime">延迟时间:s</param>
    /// <param name="param">参数</param>
    public void PushEvent(int eventType, bool immediately, int delayTime, params object[] param)
    {
        if(immediately == true)
        {
            ActiveEvent(eventType, param);
            return;
        }
        long delayTimeL = delayTime * 1000;
        CalcuObj o = new CalcuObj(eventType, DateTime.Now.Ticks + delayTimeL, param);
        delayEventList.Add(o);
    }

    /// <summary>
    /// 执行事件
    /// </summary>
    /// <param name="eventType">事件类型</param>
    public void PushEvent(int eventType)
    {
        ActiveEvent(eventType);
    }

    private void Update()
    {
        for(int i = delayEventList.Count - 1; i >= 0; i--)
        {
            if(DateTime.Now.Ticks >= delayEventList[i].EndTime)
            {
                List<Action<object[]>> funcList = funcDic[delayEventList[i].EventType];
                if(funcList == null)
                {
                    delayEventList.RemoveAt(i);
                    continue;
                }
                foreach (var item in funcList)
                {
                    item(delayEventList[i].ParamList);
                }
                delayEventList.RemoveAt(i);
            }
        }
    }
}
