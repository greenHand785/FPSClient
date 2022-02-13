using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PanelBase : MonoBehaviour
{
    


    /// <summary>
    /// 为gameobject注册点击事件
    /// </summary>
    /// <param name="component"></param>
    /// <param name="callBack"></param>
    public void RegisterClickEvent(GameObject component, UnityAction<BaseEventData> callBack)
    {
        if(component == null)
        {
            return;
        }
        if(callBack == null)
        {
            Debug.LogError("NO FUNCTION");
            return;
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(callBack);
        EventTrigger eventTrigger = component.GetComponent<EventTrigger>();
        if(eventTrigger == null)
        {
            eventTrigger = component.AddComponent<EventTrigger>();
        }
        eventTrigger.triggers.Add(entry);
    }

    /// <summary>
    /// 通用的注册ui事件
    /// </summary>
    /// <param name="component"></param>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public void RegisterEvent(GameObject component, EventTriggerType eventType, UnityAction<BaseEventData> callBack)
    {
        if(component == null)
        {
            return;
        }
        if (callBack == null)
        {
            Debug.LogError("NO FUNCTION");
            return;
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(callBack);
        EventTrigger eventTrigger = component.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = component.AddComponent<EventTrigger>();
        }
        eventTrigger.triggers.Add(entry);
    }

    public  T GetComponent<T>(Transform transform, string path) where T : Component
    {
        if(transform == null)
        {
            return null;
        }
        T component = transform.GetComponent<T>();
        if (component != null)
        {
            return component;
        }
        if(path == "")
        {
            return null;
        }
        List<string> pathList = path.Split('/').ToList<string>();
        if(pathList.Count <= 0)
        {
            return null;
        }
        string first = pathList[0];
        pathList.RemoveAt(0);
        string newStr = string.Join("/", pathList);
        return GetComponent<T>(transform.Find(first), newStr);
    }

    public GameObject Find(Transform transform, string path)
    {
        if(transform == null)
        {
            return null;
        }
        if (path == "")
        {
            return transform.gameObject;
        }
        List<string> pathList = path.Split('/').ToList<string>();
        if (pathList.Count <= 0)
        {
            return null;
        }
        string first = pathList[0];
        pathList.RemoveAt(0);
        string newStr = string.Join("/", pathList);
        Transform target = transform.Find(first);
        if (target == null)
        {
            return null;
        }
        return Find(target, newStr);
    }
}
