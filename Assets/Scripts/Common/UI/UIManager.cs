using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingleModules<UIManager>
{
    private string componentName;
    private string uiPanelPath;
    private Dictionary<string, PanelInfo> panelDic;
    private LimitedStack<GameObject> limitedStack;
    public UIManager()
    {
        componentName = "UIManager";
        uiPanelPath = "UI/panel/";
        panelDic = new Dictionary<string, PanelInfo>();
        limitedStack = new LimitedStack<GameObject>(10);
        GameObject obj = GameObject.Find(componentName);
        if(obj == null)
        {
            obj = new GameObject();
            obj.name = componentName;
        }
    }

    /// <summary>
    /// 开启或关闭页面(无回退)
    /// </summary>
    /// <param name="panelName"></param>
    /// <param name="state"></param>
    /// <param name="param"></param>
    public void ShowPanel(string panelName, bool state, params object[] param)
    {
        if(panelDic == null)
        {
            return;
        }
        PanelInfo panelInfo = null;
        panelDic.TryGetValue(panelName, out panelInfo);
        if (panelInfo == null)
        {
            GameObject panel = InstantiateUIPanel(panelName);
            if(panel == null)
            {
                return;
            }
            PanelInfo newPanelInfo = new PanelInfo(panelName, panel, param);
            panelDic.Add(panelName, newPanelInfo);
            panel.SetActive(state);
            if(state == false)
            {
                limitedStack.Push(panel);
            }
            return;
        }
        GameObject p = panelInfo.Panel;
        if(p == null)
        {
            return;
        }
        panelInfo.Param = param;
        p.SetActive(state);
        limitedStack.Push(panelInfo.Panel);
    }

    // 实例化UI界面
    private GameObject InstantiateUIPanel(string panelName)
    {
        GameObject panel = GameObject.Find(panelName);
        if (panel == null)
        {
            panel = Instantiate(Resources.Load<GameObject>(uiPanelPath + panelName));
        }
        if (panel == null)
        {
            Debug.LogError(panelName + "不存在");
            return null;
        }
        // 设置界面层级
        SetPanelLayer(panel, panelName);

        return panel;
    }

    // 移除界面
    private void DestoryUIPanel(string panelName)
    {


    }

    // 设置界面层级
    private void SetPanelLayer(GameObject panel, string panelName)
    {
        if(panel == null)
        {
            return;
        }
        UIInfoSetting uIInfoSetting = UIInfoSettingManager.Instance.GetUIInfo(panelName);
        if(uIInfoSetting == null)
        {
            SetTransformLayer(panel.transform, 6);
            return;
        }
        SetTransformLayer(panel.transform, uIInfoSetting.Layer);
    }

    /// <summary>
    /// 设置tran的层级
    /// </summary>
    /// <param name="son"></param>
    /// <param name="father"></param>
    private void SetTransformLayer(Transform tran, int layer)
    {
        if(tran == null)
        {
            return;
        }
        GameObject canvas = UIInfoSettingManager.Instance.GetCanvasGameObject(layer);
        if (canvas == null)
        {
            Debug.LogError("UIManager.cs in Line 103 Error: no canvas gameobject");
            return;
        }
        tran.SetParent(canvas.transform);
        if (tran.GetComponent<RectTransform>() == null)
        {
            return;
        }
        tran.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        tran.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// 获得该页面传递的参数
    /// </summary>
    /// <param name="panelId"></param>
    public object[] GetPanelParamObj(string panelName)
    {
        if(panelDic == null)
        {
            return null;
        }
        PanelInfo panelInfo = panelDic[panelName];
        if(panelInfo == null)
        {
            return null;
        }
        return panelInfo.Param;
    }

    /// <summary>
    /// 关闭本页面并回退到上一个开启的页面
    /// </summary>
    /// <param name="panelName"></param>
    public void BackspaceClosePanel(string panelName)
    {
        if(panelDic == null || limitedStack == null)
        {
            return;
        }
        PanelInfo panelInfo = panelDic[panelName];
        if(panelInfo == null)
        {
            return;
        }
        GameObject panel = limitedStack.Pop();
        if(panel == null)
        {
            return;
        }
        panel.SetActive(true);
        if(panelInfo.Panel == null)
        {
            return;
        }
        panelInfo.Panel.SetActive(false);
    }
}
