using UnityEngine;
using System.Collections;

public class ClientInternetManger: MonoBehaviour
{
    [Header("ClientSetting")]
    public Client client=null;
    public string IP;
    [Header("Name")]
    public string componentName;
    private int isPressedCount = 0;
    private bool isOpenCheck = false;
    private GameObject tips=null;
    private bool isPressHome = false;
    private static ClientInternetManger instance;
    public static ClientInternetManger Instance
    {
        get
        {
            return instance;
        }
    }

    public ClientInternetManger()
    {
        instance = this;
    }
    void Awake()
    {
        //在加载的时候不销毁
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        RegisterPBMsg();
        RegisterStart();
        RegisterUpdate();
    }

    // 注册消息
    private void RegisterPBMsg()
    {
        // LoginModuleManager.Instance.RegisterPBMsg()
        LoginHandleManager.Instance.RegisterPBMsg();
    }

    // 移除消息
    private void RemovePBMsg()
    {
        // LoginModuleManager.Instance.RemovePBMsg()
        LoginHandleManager.Instance.RemovePBMsg();

    }

    // 注册开始函数
    private void RegisterStart()
    {
        LoginManager.Instance.Start();

    }

    // 注册update函数
    private void RegisterUpdate()
    {
        LoginManager.Instance.Update();
    }

    // 注册destory
    private void RegisterDestory()
    {
        LoginManager.Instance.Destory();
    }

    /// <summary>
    /// 开始连接服务器
    /// </summary>
    public void StartConnection()
    {
        if (client == null)
        {
            client = new Client(8888, IP);
            //开始连接服务器
            client.StartConnect();
            //接收数据
            client.StartReceive();
        }
    }

    private void OnApplicationQuit()
    {
        
    }

    private void OnApplicationPause(bool pause)
    {
        //退出到桌面
        //关闭连接，退出游戏
        if (pause&&isPressHome)
        {
            if (client != null)
            {
                client.Disconnected();
                Debug.Log("关闭连接");
            }
            Application.Quit();
        }
    }

    private void Update()
    {
        //移动端退出游戏
        //按下退出键,弹出一个退出游戏按钮,
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Debug.Log("点击了回退按钮");
        //    isPressedCount++;
        //    if (!isOpenCheck)
        //    {
        //        if (tips == null)
        //        {
        //            tips = Instantiate(Resources.Load<GameObject>("Prefabers/Tips"));
        //            tips.transform.SetParent(GameObject.Find("Canvas").transform);
        //            tips.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,310,0);
        //            tips.GetComponent<RectTransform>().localScale = Vector3.one;
        //        }
        //        tips.SetActive(true);
        //        //tips.GetComponent<TipsText>().SetTipsText("再按一次退出游戏");
        //        isOpenCheck = true;
        //        StartCoroutine(CheckTime(3));
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.Home))
        //{
        //    isPressHome = true;
        //}
    }

    private IEnumerator CheckTime(float time)
    {
        for (;;)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                isOpenCheck = false;
                isPressedCount = 0;
                break;
            }
            if (isPressedCount >= 2)
            {
                if (client != null)
                {
                    client.Disconnected();
                    Debug.Log("关闭连接");
                }
                isPressedCount = 0;
                Debug.Log("退出游戏");
                Application.Quit();
            }
            yield return null;
        }
    }

    /// <summary>
    /// 在销毁前，需要断开连接
    /// </summary>
    private void OnDestroy()
    {
        if (client != null)
        {
            RemovePBMsg();
            client.Disconnected();
            Debug.Log("断开连接");
        }
    }
}
