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

    #region ���
    private Transform normalCamera;
    private Transform fightCamera;
    private Transform camera;
    private CharacterStateManager characterStateManager;
    private Rigidbody rigidbody;
    #endregion
    #region ����
    [Header("�ƶ��ٶ�")]
    public float MoveSpeed;
    [Header("���ˮƽ�����ƶ��ٶ�")]
    public float MouseXRotateSpeed;
    [Header("��괹ֱ�����ƶ��ٶ�")]
    public float MouseYRotateSpeed;
    [Header("�����ٶ�")]
    public float ReloadingSpeed;
    [Header("��Ծ����")]
    public float JumpForce;
    [Header("�Ƿ�ӵ�п��ٵ���")]
    public bool HaveFirstBox;
    [Header("��ͷ״̬")]
    public MainCameraState CurCameraState;
    [Header("�Ƿ��ڵ���")]
    public bool OnGround;
    [Header("����״̬")]
    private bool isDead;
    private float cameraSpeed;
    private bool isInit;
    #endregion

    #region Unity �����������ں���
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
        // �����л�
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

    #region ��ҿ���

    // �ƶ�����
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

    // �����Ծ���� by rigidbody
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

    // ����ƶ�����
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

    // ��ͷ�л��� ��ͨ�ӽ� <-> ս���ӽ�
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
