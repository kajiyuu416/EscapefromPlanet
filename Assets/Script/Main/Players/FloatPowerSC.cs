using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

//�A�N�V�����ǉ��A�W�����v�{�^���������Ă��钷���Ńv���C���[���㏸���鏈��
//�Ώۂ̃{�^���������Ă���b�����擾���t���O��Ԃ�
public class FloatPowerSC : MonoBehaviour
{
    [SerializeField] GameObject EventObj1;
    [SerializeField] GameObject EventObj2;
    [SerializeField] TextMeshProUGUI text;
    private PlayerController playerController;
    private AdditionPlayerAction additionPlayerAction;
    private bool isFloat;
    private bool isFloatFlag;
    private bool Aerial_Rotation;
    private bool isDownFlag;
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
                Aerial_Rotation = false;
            }
        }
    }

    private void Update()
    {
        if(GameManager2.FGF)
        {
            OnPushKey();
            if(EventObj1.activeSelf || EventObj2.activeSelf)
            {
                EventObj1.SetActive(false);
                EventObj2.SetActive(false);
            }

            if(isFloat)
            {
                text.text = "���~�E�E�EX�{�^���𗣂�";
            }
            else if(!isFloat)
            {
                text.text = "�㏸�E�E�E�_�b�V���� X�{�^��������";
            }

            if(playerController.isDead)
            {
                isFloat = false;
                isFloatFlag = false;
                return;
            }


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
        const float Holdtime = 0.5f;
        const float limittime = 3.0f;
        const float floatPower = 3.0f;
        //�{�^���̓��̓`�F�b�N
        //�Ή������{�^���������ꂽ�Ƃ��Ɨ����ꂽ�Ƃ�
        if(isground  && isrun && !isjump && !isoverJump && !ispose)
        {
            if(Float.wasPressedThisFrame)
            {
                isFloatFlag = true;
                isDownFlag = true;
                timeReset();
            }

            if(Float.wasReleasedThisFrame)
            {
                isFloatFlag = false;
                isDownFlag = false;
                timeReset(); 
            }
        }
        //����̃{�^���������Ă���ԃ^�C���J�E���g���s��
        //�J�E���g�𒴂���܂Ńt���O��Ԃ�
        if(isFloatFlag)
        {
            time += Time.deltaTime;
            PushTime = time / Holdtime;
            if(time >= Holdtime)
            {
                isFloat = true;
                playerController.Duplicate_isJump = false;
            }

            //�^�C���J�E���g������l�𒴂����Ƃ�
            PushTime = time / limittime;
            if(isDownFlag && time >= limittime)
            {
                Aerial_Rotation = true;
                isFloatFlag = false;
                isDownFlag = false;
                isFloat = false;
            }
        }
        //�㏸���{�^���𗣂������̃R�[���o�b�N
        if(Float.wasReleasedThisFrame�@&&isFloat)
        {
            Aerial_Rotation = true;
            isFloatFlag = false;
            isDownFlag = false;
            isFloat = false;
            timeReset();
        }

        //Player�㏸
        if(isFloat)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, floatPower, rigidbody.velocity.z);
            additionPlayerAction.Duplicate_isjumpOver = false;
        }
        //Player���|�[�Y���̓J�E���g���s��Ȃ��悤����
        if(ispose)
        {
            isFloatFlag = false;
            isDownFlag = false;
            isFloat = false;
            timeReset();
        }

        animator.SetBool("Aerial_Rotation", Aerial_Rotation);
        animator.SetBool("floating", isFloat);

        void timeReset()
        {
            time = 0f;
            PushTime = 0f;
        }

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
