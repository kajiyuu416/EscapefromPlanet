using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

//プレイヤーの動作を管理
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
    public  bool ChangePose;
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
        //コントローラーが接続状態であれば、処理が回る
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
    //プレイヤーが地面に面しているかの確認、スポーン位置の更新、壁をすり抜けたときに強制的にリセットをかける（直近のリスポーン位置へ）
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
            CP = transform.position; // 現在位置を記憶
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
            transform.position = CP;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if(collision.CompareTag("ground"))
        {
            isgroundFlag = false;
            Posefalse();
            ChangePose = false;
        }
    }
    private void PlayerMove()//プレイヤーの移動及び歩きアニメーション処理
    {
        PlayerMove_input.x = moveInputVal.x;
        PlayerMove_input.z = moveInputVal.y;
        var current_GP = Gamepad.current;
        var Run = current_GP.rightShoulder;
        var speed = Run.isPressed ? Const.CO.Const_Float_List[1] : Const.CO.Const_Float_List[0];
        var jump = current_GP.buttonSouth;
        var velocity = new Vector3(PlayerMove_input.x, 0, PlayerMove_input.z).normalized;
        bool isfloat = floatPowerSC.Duplicate_isFloatFlag;
        bool isoverJump = additionPlayerAction.Duplicate_isjumpOver;
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(Const.CO.Const_Float_List[0], 0, Const.CO.Const_Float_List[0])).normalized;
        Vector3 moveForward = cameraForward * PlayerMove_input.z + Camera.main.transform.right * PlayerMove_input.x;
        rigidbody.velocity = moveForward * moveSpeed + new Vector3(0, rigidbody.velocity.y, 0);


        if(moveForward != Vector3.zero)
        {
            if(!GameManager.pauseflag)
            {
                Quaternion QL = Quaternion.LookRotation(moveForward);
                transform.rotation = Quaternion.Lerp(transform.rotation, QL, 10.0f * Time.deltaTime);
            }
        }
        //特定のフラグが返っていればプレイヤーの動作を制御する
        if(GameManager.pauseflag || ChangePose)
        {
            velocity = Vector3.zero;
            rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
            isRun = false;
        }

        if(!GameManager.pauseflag)
        {
            // Walk状態であればスピードの変更、UI表示の変更
            if(PlayerMove_input != Vector3.zero && isgroundFlag &&!isJump)
            {
                isWalk = true;
                isRun = false;
                moveSpeed = WalkSpeed;
                //Run状態であればスピードの変更、UI表示の変更
                if(Run.isPressed && !ChangePose)
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
                ChangePose = false;
                gameManager.PlayerActionUI1.SetActive(false);
                gameManager.PlayerActionUI2.SetActive(true);
            }
            else if(!isRun)
            {
                moveSpeed = WalkSpeed;
                gameManager.PlayerActionUI1.SetActive(true);
                gameManager.PlayerActionUI2.SetActive(false);
            }

            //ジャンプ処理
            if(Interval_InputButtondown(jump, 0.5f) &&isgroundFlag &&!isoverJump &&!isfloat)
            {
                isJump = true;
                isWalk = false;
                Posefalse();
                ChangePose = false;
                rigidbody.AddForce(transform.up * JumpPower, ForceMode.Impulse);
            }
            rigidbody.constraints = RigidbodyConstraints.None;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
        animator.SetBool("jump", isJump);
        animator.SetBool("walk", isWalk);
        animator.SetFloat("Speed", velocity.magnitude * speed, 0.1f, Time.deltaTime);
    }
    //ボタンの入力に応じてプレイヤーのポーズを変更
    private void ChangeIdlePose()
    {
        var current_GP = Gamepad.current;
        var Crouch = current_GP.buttonEast;
        var Laying = current_GP.buttonWest;
        var Dance = current_GP.buttonNorth;
        if (!isRun && !ChangePose && isgroundFlag &&!GameManager.pauseflag)
        {
            if(Crouch.isPressed)
            {
                CrouchPose = true;
            }
            else if(Laying.isPressed)
            {
                LayingPose = true;
            }
            else if(Dance.isPressed)
            {
                DancePose = true;
            }

            if(CrouchPose || LayingPose || DancePose)
            {
                ChangePose = true;
            }
        }

        if(Crouch.wasReleasedThisFrame || Laying.wasReleasedThisFrame || Dance.wasReleasedThisFrame)
        {
            Posefalse();
            ChangePose = false;
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
    //プレイヤーがレーザー接触時、自身を非表示にし子オブジェクトの表示を行う
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

    //ボタン入力のクールタイムを設定し、一定時間入力を制御する処理
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
    public bool Duplicate_ChangePose
    {
        get
        {
            return ChangePose;
        }
    }

}
