using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;

//�v���C���[�̓�����Ǘ�
public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject IdleBody;
    [SerializeField] Animator animator;
    private FloatPowerSC floatPowerSC;
    private AdditionPlayerAction additionPlayerAction;
    private const float WalkSpeed = 3.0f;
    private const float RunSpeed = 7.0f;
    private const float JumpPower = 140.0f;
    private float moveSpeed;
    private bool isWalk;
    private bool isRun;
    private bool isJump;
    private bool isgroundFlag;
    private bool CrouchPose;
    private bool LayingPose;
    private bool DancePose;
    public bool isDead;
    private static bool preventContinuityInput;
    private static float buttonDownTime;
    private static float timer;
    public static PlayerController instance;
    private Child[] Parts;
    private new Rigidbody rigidbody;
    public Vector2 moveInputVal;
    public Vector2 CameraInputVal;
    private Vector3 PlayerMove_input;
    private static Vector3 CP = new Vector3();

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Parts = GetComponentsInChildren<Child>();
        floatPowerSC = FindObjectOfType<FloatPowerSC>();
        additionPlayerAction = FindObjectOfType<AdditionPlayerAction>();
    }

    private void Start()
    {
        if(CP != Vector3.zero)
        {
            transform.position = CP;
        }
    }
    private void Update()
    {
        //�R���g���[���[���ڑ���Ԃł���΁A���������
        if(GameManager2.connect)
        {
            PlayerMove();
            ChangeIdlePose();
        }

        if (isDead)
        {  
            isJump = false;
            floatPowerSC.Duplicate_isFloat = false;
            additionPlayerAction.Duplicate_isjumpOver = false;
            return;
        }
    }
    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
    }
    //�v���C���[���n�ʂɖʂ��Ă��邩�̊m�F�A�X�|�[���ʒu�̍X�V�A�ǂ����蔲�����Ƃ��ɋ����I�Ƀ��Z�b�g��������i���߂̃��X�|�[���ʒu�ցj
    private void OnTriggerEnter(Collider collision)
    {
        if (isDead)
        {
            return;
        }

        if(!isgroundFlag)
        {
            if(collision.CompareTag("ground"))
            {
                isJump = false;
                additionPlayerAction.Duplicate_isjumpOver = false;
            }
        }

        if(collision.CompareTag("spawnpoint"))
        {
            CP = transform.position; // ���݈ʒu���L��
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("ground"))
        {
            isgroundFlag = true;
        }

        if(collision.CompareTag("ResetArea"))
        {
            Scene ThisScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(ThisScene.name);
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if(collision.CompareTag("ground"))
        {
            isgroundFlag = false;
        }
    }
    private void PlayerMove()//�v���C���[�̈ړ��y�ѕ����A�j���[�V��������
    {
        PlayerMove_input.x = moveInputVal.x;
        PlayerMove_input.z = moveInputVal.y;
        var current_GP = Gamepad.current;
        var Run = current_GP.rightShoulder;
        var speed = Run.isPressed ? Const.CO.Const_Float_List[1] : Const.CO.Const_Float_List[0];
        var jump = current_GP.buttonSouth;
        var velocity = new Vector3(PlayerMove_input.x, 0, PlayerMove_input.z).normalized;
        bool isfloat = floatPowerSC.Duplicate_isFloat;
        bool isoverJump = additionPlayerAction.Duplicate_isjumpOver;
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(Const.CO.Const_Float_List[0], 0, Const.CO.Const_Float_List[0])).normalized;
        Vector3 moveForward = cameraForward * PlayerMove_input.z + Camera.main.transform.right * PlayerMove_input.x;
        rigidbody.velocity = moveForward * moveSpeed + new Vector3(0, rigidbody.velocity.y, 0);


        if(moveForward != Vector3.zero)
        {
            if(!GameManager.pauseflag)
            {
                Quaternion QL = Quaternion.LookRotation(moveForward);
                transform.rotation = Quaternion.Lerp(transform.rotation, QL, 15.0f * Time.deltaTime);
            }
        }
        //����̃t���O���Ԃ��Ă���΃v���C���[�̓���𐧌䂷��
        if(GameManager.pauseflag)
        {
            velocity = Vector3.zero;
            rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
            isRun = false;
        }

        if(!GameManager.pauseflag)
        {
            // Walk��Ԃł���΃X�s�[�h�̕ύX�AUI�\���̕ύX
            if(PlayerMove_input != Vector3.zero && isgroundFlag &&!isJump)
            {
                isWalk = true;
                isRun = false;
                moveSpeed = WalkSpeed;
                //Run��Ԃł���΃X�s�[�h�̕ύX�AUI�\���̕ύX
                if(Run.isPressed)
                {
                    isRun = true;
                }
            }
            else if(PlayerMove_input == Vector3.zero && isgroundFlag)
            {
                isWalk = false;
                isRun = false;
            }

            if(isRun)
            {
                moveSpeed = RunSpeed;
                gameManager.ActionUI1.SetActive(false);
                gameManager.ActionUI2.SetActive(true);
            }
            else if(!isRun)
            {
                gameManager.ActionUI1.SetActive(true);
                gameManager.ActionUI2.SetActive(false);
            }

            //�W�����v����
            if(Interval_InputButtondown(jump, 0.5f) && !isRun &&isgroundFlag)
            {
                isJump = true;
                isWalk = false;
                Posefalse();
                rigidbody.AddForce(transform.up * JumpPower, ForceMode.Impulse);
            }
            rigidbody.constraints = RigidbodyConstraints.None;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
        animator.SetBool("jump", isJump);
        animator.SetBool("walk", isWalk);
        animator.SetFloat("Speed", velocity.magnitude * speed, 0.1f, Time.deltaTime);
        animator.SetBool("floating", isfloat);
        animator.SetBool("Jumpover", isoverJump);
    }
    //�{�^���̓��͂ɉ����ăv���C���[�̃|�[�Y��ύX
    private void ChangeIdlePose()
    {
        var current_GP = Gamepad.current;
        var Crouch = current_GP.buttonEast;
        var Laying = current_GP.buttonWest;
        var Dance = current_GP.buttonNorth;
        if (!GameManager.pauseflag && !isRun)
        {
            if(Crouch.isPressed)
            {
                CrouchPose = true;
                LayingPose = false;
                DancePose = false;
            }
            else if(Laying.isPressed)
            {
                CrouchPose = false;
                LayingPose = true;
                DancePose = false;
            }
            else if(Dance.isPressed)
            {
                CrouchPose = false;
                LayingPose = false;
                DancePose = true;
            }

            if(Crouch.wasReleasedThisFrame || Laying.wasReleasedThisFrame || Dance.wasReleasedThisFrame)
            {
                Posefalse();
            }

        }
        animator.SetBool("Crouch", CrouchPose);
        animator.SetBool("Laying", LayingPose);
        animator.SetBool("dance", DancePose);
    }
    private void Posefalse()
    {
        CrouchPose = false;
        LayingPose = false;
        DancePose = false;
    }
    //�v���C���[�����[�U�[�ڐG���A���g���\���ɂ��q�I�u�W�F�N�g�̕\�����s��
    public void PlayerDeath()
    {
        IdleBody.SetActive(false);
        foreach (var p in Parts)
        {
            p.On();
        }
        gameManager.RestartFlagOn();
        isDead = true;
        animator.SetBool("Death", isDead);
    }

    public void landing()
    {
        additionPlayerAction.Duplicate_isjumpOver = false;
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE3();
    }
    public void Jumpmiss()
    {
        isJump = false;
        isgroundFlag = false;
        animator.SetBool("jump", false);
        Debug.Log("Jumpmiss");
    }

    //�{�^�����͂̃N�[���^�C����ݒ肵�A��莞�ԓ��͂𐧌䂷�鏈��
    public static bool Interval_InputButtondown(ButtonControl input, float IntervalSeconds)
    {
        timer = Time.time;
        if(input.wasPressedThisFrame && timer - buttonDownTime >= IntervalSeconds)
        {
            if(preventContinuityInput == false)
            {
                preventContinuityInput = true;
                buttonDownTime = Time.time;
                return true;
            }
            else if(preventContinuityInput)
            {
                preventContinuityInput = false;
                buttonDownTime = Time.time;
                return true;
            }
        }
        return false;
    }
    public void OnMove(InputValue var)
    {
        moveInputVal = var.Get<Vector2>();
    }
    public void OnCamera(InputValue var)
    {
       CameraInputVal = var.Get<Vector2>();
    }
    public bool Duplicate_isRun
    {
        get
        {
            return isRun;
        }
    }
    public bool Duplicate_isJump
    {
        get
        {
            return isJump;
        }
        set
        {
            isJump = value;
        }
    }
    public bool Duplicate_isgroundFlag
    {
        get
        {
            return isgroundFlag;
        }
        set
        {
            isgroundFlag = value;
        }
    }

}
