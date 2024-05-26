using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

//�A�N�V�����ǉ��A�W�����v�{�^���������Ă��钷���Ńv���C���[���㏸���鏈��
//�Ώۂ̃{�^���������Ă���b�����擾���t���O��Ԃ�
public class FloatPowerSC : MonoBehaviour
{
    [SerializeField] GameObject EventObj1;
    [SerializeField] GameObject EventObj2;
    private PlayerController playerController;
    private AdditionPlayerAction additionPlayerAction;
    private bool isFloat;
    private bool isFloatFlag;
    private bool SmallInputFloat;
    private bool isDownFlag;
    public static bool AdditionPlayerActionFlag_Float = false;
    private float time;
    private  new Rigidbody rigidbody;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        playerController = FindObjectOfType<PlayerController>();
        additionPlayerAction = FindObjectOfType<AdditionPlayerAction>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        bool isground = playerController.Duplicate_isgroundFlag;

        if(!isground)
        {
            if(collision.CompareTag("ground"))
            {
                SmallInputFloat = false;
            }
        }
    }
    private void FixedUpdate()
    {
        if(AdditionPlayerActionFlag_Float)
        {
            OnPushKey();
            if(playerController.isDead)
            {
                isFloat = false;
                isFloatFlag = false;
                return;
            }
        }
    }
    private void Update()
    {
        if(GameManager2.FGF)
        {
            EventObj1.SetActive(false);
            EventObj2.SetActive(false);
        }
    }
    //����̃{�^���������������̎擾
    //�����������ɉ����ăv���C���[���㏸
    public void OnPushKey()
    {
        var current_GP = Gamepad.current;
        var Float = current_GP.buttonWest;
        bool isground = playerController.Duplicate_isgroundFlag;
        bool isrun = playerController.Duplicate_isRun;
        bool ispose = playerController.Duplicate_ChangePose;
        bool isjump = playerController.Duplicate_isJump;
        bool isoverJump = additionPlayerAction.Duplicate_isjumpOver;
        float PushTime = 0.0f;
        const float Purposetime = 1.0f;
        const float limittime = 5.0f;
        const float floatPower = 3.0f;


        //Player���n�ʂƐڐG���A�����Ă����Ԃœ���̃{�^������������
        if(isground  && isrun && !isjump && !isoverJump)
        {
            if(Float.wasPressedThisFrame)
            {
                isFloatFlag = true;
                isDownFlag = true;
                time = 0f;
                PushTime = 0f;
            }
        }

        //����̃{�^���������Ă���ԃ^�C���J�E���g���s��
        //�J�E���g�𒴂���܂Ńt���O��Ԃ�
        if(isFloatFlag && !ispose)
        {
            time += Time.deltaTime;
            PushTime = time / Purposetime;
            if(time >= Purposetime)
            {
                isFloat = true;
                playerController.Duplicate_isJump = false;
            }
        }

        //����̃{�^���������Ă���ԃ^�C���J�E���g���s��
        //�J�E���g�𒴂���܂Ńt���O��Ԃ�
        //�^�C���J�E���g������l�𒴂����Ƃ�
        if(isDownFlag)
        {
            time += Time.deltaTime;
            PushTime = time / limittime;
            if(time >= limittime)
            {
                SmallInputFloat = true;
                isFloatFlag = false;
                isDownFlag = false;
                isFloat = false;
                Debug.Log("bbb");
            }
        }

        //Player�㏸
        if(isFloat)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, floatPower, rigidbody.velocity.z);
            additionPlayerAction.Duplicate_isjumpOver = false;
            //�㏸���{�^���𗣂������̃R�[���o�b�N
            if(Float.wasReleasedThisFrame)
            {
                SmallInputFloat = true;
                isFloatFlag = false;
                isDownFlag = false;
                isFloat = false;
                time = 0f;
                PushTime = 0f;
                Debug.Log("ccc");
            }
        }


        animator.SetBool("smallfloat", SmallInputFloat);
        animator.SetBool("floating", isFloat);
    }
    public void Spin()
    {
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE10();
    }
    public bool Duplicate_isFloat
    {
        get
        {
            return isFloat;
        }
        set
        {
            isFloat = value;
        }
    }
    public bool Duplicate_isFloatFlag
    {
        get
        {
            return isFloatFlag;
        }
        set
        {
            isFloatFlag = value;
        }
    }
}
