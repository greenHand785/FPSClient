using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class UIInfoSetting
{
    /// <summary>
    /// 界面名称
    /// </summary>
    private string panelName;
    /// <summary>
    /// 界面层级
    /// </summary>
    private int layer;
    /// <summary>
    /// 延迟删除时间，default = 3
    /// </summary>
    private int delayDesTime;

    public string PanelName { get => panelName; set => panelName = value; }
    public int Layer { get => layer; set => layer = value; }
    public int DelayDesTime { get => delayDesTime; set => delayDesTime = value; }
}

