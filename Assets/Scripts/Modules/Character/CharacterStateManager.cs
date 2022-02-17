using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterStateManager : MonoBehaviour
{
    public enum CharacterState
    {
        NONE,
        START,
        IDLE,
        SHOOTING,
        RUNNING,
        JUMP,
        RELOADING,
        RELOADING_F,
        WALKING,
    }

    public class StateEventInfo<T>
    {
        private bool isExecute;
        private UnityAction<T> callBack;

        public bool IsExecute { get => isExecute; set => isExecute = value; }
        public UnityAction<T> CallBack { get => callBack; set => callBack = value; }
    }

    #region 组件
    private Animator animator;
    private CharacterController character;

    #endregion
    #region 属性
    public CharacterState curState;
    // 奔跑状态属性
    private float runAddSpeed;
    private float minRunValue;
    // 换弹状态属性
    private bool startReloading;
    public CharacterState CurState 
    { 
        get
        {
            return curState;
        } 
    }
    private bool isInit;
    private float curRunningValue;
    // 进入状态事件
    private Dictionary<CharacterState, StateEventInfo<int>> inStateDic;
    //// 进入状态事件
    //private Dictionary<CharacterState, UnityAction<int>> enterStateDic;
    // 退出状态事件
    private Dictionary<CharacterState, StateEventInfo<int>> outStateDic;

    #endregion

    #region Unity 内置生命周期函数
    private void Awake()
    {
        
    }

    private void Start()
    {
        Init();
    }
    private void Update()
    {
        // 动画控制
        AnimatorControl();
    }

    #endregion

    #region 玩家控制
    private void Init()
    {
        if(isInit == true)
        {
            return;
        }
        animator = transform.GetComponentInChildren<Animator>();
        character = transform.GetComponent<CharacterController>();
        curState = CharacterState.NONE;
        minRunValue = 3;
        runAddSpeed = 4;
        isInit = true;
    }

    // 添加进入状态事件
    public void AddInStateEvent(CharacterState state, UnityAction<int> callBack)
    {
        if(inStateDic == null)
        {
            return;
        }
        if(inStateDic.ContainsKey(state) == true)
        {
            if(inStateDic[state] == null)
            {
                return;
            }
            if(inStateDic[state].CallBack == null)
            {
                return;
            }
            inStateDic[state].CallBack += callBack;
            return;
        }
        StateEventInfo<int> stateEventInfo = new StateEventInfo<int>();
        stateEventInfo.IsExecute = false;
        stateEventInfo.CallBack = callBack;
        inStateDic.Add(state, stateEventInfo);
    }

    // 移除进入状态事件
    public void RemoveInStateEvent(CharacterState state, UnityAction<int> callBack)
    {
        if(inStateDic == null)
        {
            return;
        }
        if (inStateDic.ContainsKey(state) == false)
        {
            return;
        }
        if(inStateDic[state] == null || inStateDic[state].CallBack == null || inStateDic[state].CallBack.GetInvocationList() == null)
        {
            return;
        }
        if(inStateDic[state].CallBack.GetInvocationList().Length == 0)
        {
            inStateDic.Remove(state);
            return;
        }
        inStateDic[state].CallBack -= callBack;
    }

    // 添加退出状态事件
    public void AddOutStateEvent(CharacterState state, UnityAction<int> callBack)
    {
        if (outStateDic == null)
        {
            return;
        }
        if (outStateDic.ContainsKey(state) == true)
        {
            if (outStateDic[state] == null)
            {
                return;
            }
            if (outStateDic[state].CallBack == null)
            {
                return;
            }
            outStateDic[state].CallBack += callBack;
            return;
        }
        StateEventInfo<int> stateEventInfo = new StateEventInfo<int>();
        stateEventInfo.IsExecute = false;
        stateEventInfo.CallBack = callBack;
        outStateDic.Add(state, stateEventInfo);
    }

    // 移除退出状态事件
    public void RemoveOutStateEvent(CharacterState state, UnityAction<int> callBack)
    {
        if (outStateDic == null)
        {
            return;
        }
        if (outStateDic.ContainsKey(state) == false)
        {
            return;
        }
        if (outStateDic[state] == null || outStateDic[state].CallBack == null || outStateDic[state].CallBack.GetInvocationList() == null)
        {
            return;
        }
        if (outStateDic[state].CallBack.GetInvocationList().Length == 0)
        {
            outStateDic.Remove(state);
            return;
        }
        outStateDic[state].CallBack -= callBack;
    }

    private void AnimatorControl()
    {
        if (animator == null)
        {
            return;
        }
        // 状态控制
        // 射击
        animator.SetBool("StartShoot", Input.GetAxis(InputSetting.ShootKey) > 0);
        // 换弹
        animator.SetBool("HaveFirstBox", character.HaveFirstBox == true);
        animator.SetBool("StartReloading", Input.GetKeyDown(InputSetting.ReloadKey) == true);
        // 奔跑
        curRunningValue = Input.GetAxis(InputSetting.ForwardOrBackKey) * runAddSpeed;
        animator.SetFloat("StartRunning", curRunningValue);
        // 获取控制阙值
        startReloading = animator.GetBool("StartReloading");
        // 事件执行判断
        // 奔跑事件
        ExecuteEvent(CharacterState.RUNNING, 0, curRunningValue >= minRunValue);
        ExecuteEvent(CharacterState.RELOADING, 0, startReloading == true);
        ExecuteEvent(CharacterState.RELOADING_F, 0, character.HaveFirstBox == true && startReloading == true);
        ExecuteEvent(CharacterState.START, 0, animator.GetCurrentAnimatorStateInfo(0).IsName("FirstGot_5") == true);
        ExecuteEvent(CharacterState.IDLE, 0, animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_2") == true);
        ExecuteEvent(CharacterState.SHOOTING, 0, animator.GetBool("StartShoot") == true);
        //ExecuteEvent(CharacterState.JUMP, 0, haveFirstBox == true && startReloading == true);
        //ExecuteEvent(CharacterState.WALKING, 0, haveFirstBox == true && startReloading == true);
    }

    // 执行事件
    private void ExecuteEvent(CharacterState state, int arg, bool Enter)
    {
        StateEventInfo<int> stateEventInfo = null;
        if (Enter)
        {
            curState = state;
            if(inStateDic == null)
            {
                return;
            }
            inStateDic.TryGetValue(state, out stateEventInfo);
            if (stateEventInfo == null)
            {
                return;
            }
            if(stateEventInfo.IsExecute == true)
            {
                return;
            }
            if(stateEventInfo.CallBack == null)
            {
                return;
            }
            stateEventInfo.CallBack(arg);
            stateEventInfo.IsExecute = true;
            StateEventInfo<int> outStateEventInfo = null;
            if(outStateDic == null)
            {
                return;
            }
            outStateDic.TryGetValue(state, out outStateEventInfo);
            if(outStateEventInfo == null)
            {
                return;
            }
            outStateEventInfo.IsExecute = false;
            return;
        }
        if(outStateDic == null)
        {
            return;
        }
        outStateDic.TryGetValue(state, out stateEventInfo);
        if (stateEventInfo == null)
        {
            return;
        }
        if (stateEventInfo.IsExecute == true)
        {
            return;
        }
        if (stateEventInfo.CallBack == null)
        {
            return;
        }
        stateEventInfo.CallBack(arg);
        stateEventInfo.IsExecute = true;
        StateEventInfo<int> inStateEventInfo = null;
        if(inStateDic == null)
        {
            return;
        }
        inStateDic.TryGetValue(state, out inStateEventInfo);
        if (inStateEventInfo == null)
        {
            return;
        }
        inStateEventInfo.IsExecute = false;
    }
    #endregion



}
