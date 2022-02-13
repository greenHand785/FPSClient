using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIInfoSettingManager : SingleModules<UIInfoSettingManager>
{
    private class CanvasInfo
    {
        private int layer;
        private string canvasName;
        private GameObject obj;
        private string configPath;

        public int Layer { get => layer; set => layer = value; }
        public string CanvasName { get => canvasName; set => canvasName = value; }
        public GameObject Obj { get => obj; set => obj = value; }
    }
    // ui页面设置字典
    private Dictionary<string, UIInfoSetting> uiInfoDic;

    // 层级对应字典
    private Dictionary<int, CanvasInfo> layerDic;
  
    public UIInfoSettingManager()
    {
        uiInfoDic = new Dictionary<string, UIInfoSetting>();
        layerDic = new Dictionary<int, CanvasInfo>();
        Init();
    }

    // 注册页面信息
    public void RegisterUIInfo(string panelName, UIInfoSetting info)
    {
        if(uiInfoDic == null)
        {
            return;
        }
        if(info == null)
        {
            return;
        }
        uiInfoDic.Add(panelName, info);
    }

    // 获得页面信息
    public UIInfoSetting GetUIInfo(string panelName)
    {
        if (uiInfoDic == null)
        {
            return null;
        }
        UIInfoSetting uIInfoSetting = null;
        uiInfoDic.TryGetValue(panelName, out uIInfoSetting);
        return uIInfoSetting;
    } 

    // 注册层级对应关系
    public void RegisterLayer(int layer, string canvasName)
    {
        if(layerDic == null)
        {
            return;
        }
        if(layerDic.ContainsKey(layer) == true)
        {
            layerDic[layer].CanvasName = canvasName;
            return;
        }
        CanvasInfo canvasInfo = new CanvasInfo();
        canvasInfo.Layer = layer;
        canvasInfo.CanvasName = canvasName;
        layerDic.Add(layer, canvasInfo);
    }

    // 获得对应canvas Name
    public string GetLayerCanvasName(int layer)
    {
        if (layerDic == null)
        {
            return null;
        }
        CanvasInfo a = null;
        layerDic.TryGetValue(layer, out a);
        if(a == null)
        {
            return null;
        }
        return a.CanvasName;
    }

    public GameObject GetCanvasGameObject(int layer)
    {
        if (layerDic == null)
        {
            return null;
        }
        CanvasInfo a = null;
        layerDic.TryGetValue(layer, out a);
        if (a == null)
        {
            return null;
        }
        if(a.Obj == null)
        {
            a.Obj = GameObject.Find(a.CanvasName);
            if(a.Obj == null)
            {
                // TODO 从资源中进行加载
            }
        }
        return a.Obj;
    }

    public void Init()
    {
        RegisterLayer(1, "Canvas1");
        RegisterLayer(2, "Canvas2");
        RegisterLayer(3, "Canvas3");
        RegisterLayer(4, "Canvas4");
        RegisterLayer(5, "Canvas5");
        RegisterLayer(6, "Canvas6");
    }
}



