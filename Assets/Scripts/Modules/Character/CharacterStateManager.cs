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

    #region ���
    private Animator animator;
    private CharacterController character;

    #endregion
    #region ����
    public CharacterState curState;
    // ����״̬����
    private float runAddSpeed;
    private float minRunValue;
    // ����״̬����
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
    // ����״̬�¼�
    private Dictionary<CharacterState, StateEventInfo<int>> inStateDic;
    //// ����״̬�¼�
    //private Dictionary<CharacterState, UnityAction<int>> enterStateDic;
    // �˳�״̬�¼�
    private Dictionary<CharacterState, StateEventInfo<int>> outStateDic;

    #endregion

    #region Unity �����������ں���
    private void Awake()
    {
        
    }

    private void Start()
    {
        Init();
    }
    private void Update()
    {
        // ��������
        AnimatorControl();
    }

    #endregion

    #region ��ҿ���
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

    // ��ӽ���״̬�¼�
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

    // �Ƴ�����״̬�¼�
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

    // ����˳�״̬�¼�
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

    // �Ƴ��˳�״̬�¼�
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
        // ״̬����
        // ���
        animator.SetBool("StartShoot", Input.GetAxis(InputSetting.ShootKey) > 0);
        // ����
        animator.SetBool("HaveFirstBox", character.HaveFirstBox == true);
        animator.SetBool("StartReloading", Input.GetKeyDown(InputSetting.ReloadKey) == true);
        // ����
        curRunningValue = Input.GetAxis(InputSetting.ForwardOrBackKey) * runAddSpeed;
        animator.SetFloat("StartRunning", curRunningValue);
        // ��ȡ������ֵ
        startReloading = animator.GetBool("StartReloading");
        // �¼�ִ���ж�
        // �����¼�
        ExecuteEvent(CharacterState.RUNNING, 0, curRunningValue >= minRunValue);
        ExecuteEvent(CharacterState.RELOADING, 0, startReloading == true);
        ExecuteEvent(CharacterState.RELOADING_F, 0, character.HaveFirstBox == true && startReloading == true);
        ExecuteEvent(CharacterState.START, 0, animator.GetCurrentAnimatorStateInfo(0).IsName("FirstGot_5") == true);
        ExecuteEvent(CharacterState.IDLE, 0, animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_2") == true);
        ExecuteEvent(CharacterState.SHOOTING, 0, animator.GetBool("StartShoot") == true);
        //ExecuteEvent(CharacterState.JUMP, 0, haveFirstBox == true && startReloading == true);
        //ExecuteEvent(CharacterState.WALKING, 0, haveFirstBox == true && startReloading == true);
    }

    // ִ���¼�
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
