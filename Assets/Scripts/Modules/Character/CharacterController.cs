using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public enum MainCameraState
    {
        NONE,
        NORMAL,
        FIGHT,
    }

    #region 组件
    private Transform normalCamera;
    private Transform fightCamera;
    private Transform camera;
    private CharacterStateManager characterStateManager;
    private Rigidbody rigidbody;
    #endregion
    #region 属性
    [Header("移动速度")]
    public float MoveSpeed;
    [Header("鼠标水平方向移动速度")]
    public float MouseXRotateSpeed;
    [Header("鼠标垂直方向移动速度")]
    public float MouseYRotateSpeed;
    [Header("换弹速度")]
    public float ReloadingSpeed;
    [Header("跳跃力量")]
    public float JumpForce;
    [Header("是否拥有快速弹夹")]
    public bool HaveFirstBox;
    [Header("镜头状态")]
    public MainCameraState CurCameraState;
    [Header("是否在地上")]
    public bool OnGround;
    [Header("死亡状态")]
    private bool isDead;
    private float cameraSpeed;
    private bool isInit;
    #endregion

    #region Unity 内置生命周期函数
    private void Awake()
    {
       
    }

    private void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        JumpControlRigi();
    }

    private void Update()
    {
        MovingControl();
        MouseMovingControl();
    }

    private void LateUpdate()
    {
        if(characterStateManager == null)
        {
            return;
        }
        // 开镜切换
        if(characterStateManager.curState != CharacterStateManager.CharacterState.RUNNING)
        {
            if (Input.GetAxis(InputSetting.OpenMirrorKey) > 0)
            {
                CameraChange(MainCameraState.FIGHT);
            }
            else
            {
                CameraChange(MainCameraState.NORMAL);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision == null || collision.gameObject == null)
        {
            return;
        }
        if(collision.gameObject.tag == TagDefine.TagGround)
        {
            OnGround = true;
        }
    }

    #endregion

    #region 玩家控制

    // 移动控制
    private void MovingControl()
    {
        if(transform == null)
        {
            return;
        }
        if(isDead == true)
        {
            return;
        }
        transform.Translate(Vector3.right * MoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);
        transform.Translate(Vector3.forward * MoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
    }

    // 玩家跳跃控制 by rigidbody
    private void JumpControlRigi()
    {
        if(rigidbody == null)
        {
            return;
        }
        if(OnGround == true && Input.GetAxis(InputSetting.JumpKey) > 0)
        {
            OnGround = false;
            rigidbody.AddForce(JumpForce * Vector3.up, ForceMode.Impulse);
        }
    }

    // 鼠标移动控制
    private void MouseMovingControl()
    {
        if(transform == null)
        {
            return;
        }
        if(isDead == true)
        {
            return;
        }
        transform.eulerAngles += new Vector3(0, MouseXRotateSpeed * Input.GetAxis("Mouse X") * Time.deltaTime, 0);
        transform.eulerAngles += new Vector3(-1 * MouseYRotateSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime, 0, 0);
    }

    private void Init()
    {
        if(isInit == true)
        {
            return;
        }
        if(transform == null)
        {
            return;
        }
        camera = transform.Find("Camera");
        normalCamera = transform.Find("NormalCameraPos");
        fightCamera = transform.Find("FightCameraPos");
        CurCameraState = MainCameraState.NORMAL;
        characterStateManager = transform.GetComponentInChildren<CharacterStateManager>();
        rigidbody = transform.GetComponent<Rigidbody>();
        cameraSpeed = 30;
        isDead = false;
        isInit = true;
    }

    // 镜头切换， 普通视角 <-> 战斗视角
    private void CameraChange(MainCameraState cameraState)
    {
        switch (cameraState)
        {
            case MainCameraState.NORMAL: Camera2Normal(); break;
            case MainCameraState.FIGHT: Camera2Fight(); break;
        }
    }

    private void Camera2Normal()
    {
        if (camera == null || normalCamera == null || fightCamera == null)
        {
            return;
        }
        if(CurCameraState == MainCameraState.NORMAL)
        {
            return;
        }
        if(CurCameraState == MainCameraState.FIGHT)
        {
            camera.position = Vector3.Lerp(camera.position, normalCamera.position, cameraSpeed * Time.deltaTime);
            if((camera.position - normalCamera.position).magnitude < 0.01f)
            {
                camera.position = normalCamera.position;
                CurCameraState = MainCameraState.NORMAL;
            }
            return;
        }
        camera.position = normalCamera.position;
        CurCameraState = MainCameraState.NORMAL;
    }

    private void Camera2Fight()
    {
        if (camera == null || normalCamera == null || fightCamera == null)
        {
            return;
        }
        if(CurCameraState == MainCameraState.FIGHT)
        {
            return;
        }
        if (CurCameraState == MainCameraState.NORMAL)
        {
            camera.position = Vector3.Lerp(camera.position, fightCamera.position, cameraSpeed * Time.deltaTime);
            if ((camera.position - fightCamera.position).magnitude < 0.01f)
            {
                camera.position = fightCamera.position;
                CurCameraState = MainCameraState.FIGHT;
            }
            return;
        }
        camera.position = fightCamera.position;
        CurCameraState = MainCameraState.FIGHT;
    }

    #endregion

}
