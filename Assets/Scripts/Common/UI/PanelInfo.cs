using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PanelInfo
{
    private string panelName;
    
    private GameObject panel;
    private object[] param;

    public string PanelName
    {
        get
        {
            return panelName;
        }
        set
        {
            panelName = value;
        }
    }

    public GameObject Panel
    {
        get
        {
            return panel;
        }
        set
        {
            if(value == null)
            {
                return;
            }
            panel = value;
        }
    }

    public object[] Param
    {
        get
        {
            return param;
        }
        set
        {
            if(value == null)
            {
                return;
            }
            param = value;
        }
    }

    public PanelInfo(string panelName, GameObject gameObject, params object[] param)
    {
        PanelName = panelName;
        Panel = gameObject;
        Param = param;
    }
}
