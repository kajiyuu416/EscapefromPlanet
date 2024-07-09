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
    [SerializeField] GameObject idleBody;
    [SerializeField] Animator animator;
    private FloatPowerSC floatPowerSC;
    private AdditionPlayerAction additionPlayerAction;
    private const float walkSpeed = 3.0f;
    private const float runSpeed = 7.0f;
    private const float jumpPower = 140.0f;
    private float moveSpeed;
    private bool isWalk;
    private bool isRun;
    private bool isJump;
    private bool isgroundFlag;
    private bool changePose;
    private bool crouchPose;
    private bool layingPose;
    private bool dancePose;
    private bool isDead;
    private static bool preventContinuityInput;
    private static float buttonDownTime;
    private static float timer;
    public static PlayerController instance;
    private Child[] childParts;
    private TrailRenderer[] child_Trails;
    private new Rigidbody rigidbody;
    private Vector2 moveInputVal;
    private Vector2 cameraInputVal;
    private Vector3 playerMove_input;
    private static Vector3 checkPoint = new Vector3();
   

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        childParts = GetComponentsInChildren<Child>();
        child_Trails = GetComponentsInChildren<TrailRenderer>();
        floatPowerSC = FindObjectOfType<FloatPowerSC>();
        additionPlayerAction = FindObjectOfType<AdditionPlayerAction>();
    }

    private void Start()
    {
        if(checkPoint != Vector3.zero)
        {
            transform.position = checkPoint;
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
    //プレイヤーが地面に面しているかの確認、スポーン位置の更新、壁をすり抜けたときに強制的にリセットをかける（最後に通過したリスポーンポイントの位置へ）
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
            checkPoint = transform.position; // 現在位置を記憶
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
            transform.position = checkPoint;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if(collision.CompareTag("ground"))
        {
            isgroundFlag = false;
            Posefalse();
            changePose = false;
        }
    }
    private void PlayerMove()//プレイヤーの移動及び歩きアニメーション処理
    {
        playerMove_input.x = moveInputVal.x;
        playerMove_input.z = moveInputVal.y;
        var current_GP = Gamepad.current;
        var Run = current_GP.rightShoulder;
        var speed = Run.isPressed ? 2.0f : 1.0f;
        var jump = current_GP.buttonSouth;
        var velocity = new Vector3(playerMove_input.x, 0, playerMove_input.z).normalized;
        bool isfloat = floatPowerSC.Duplicate_isFloatFlag;
        bool isAerial_Rotation = floatPowerSC.Duplicate_Aerial_Rotation;
        bool isoverJump = additionPlayerAction.Duplicate_isjumpOver;
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1.0f, 0, 1.0f)).normalized;
        Vector3 moveForward = cameraForward * playerMove_input.z + Camera.main.transform.right * playerMove_input.x;
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
        if(GameManager.pauseflag || changePose)
        {
            velocity = Vector3.zero;
            rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
            isRun = false;
        }

        if(!GameManager.pauseflag)
        {
            // Walk状態であればスピードの変更、UI表示の変更
            if(playerMove_input != Vector3.zero && isgroundFlag &&!isJump)
            {
                isWalk = true;
                isRun = false;
                moveSpeed = walkSpeed;
                //Run状態であればスピードの変更、UI表示の変更
                if(Run.isPressed && !changePose)
                {
                    isRun = true;
                }
            }
            else if(playerMove_input == Vector3.zero && isgroundFlag)
            {
                isWalk = false;
                isRun = false;
            }

            if(isRun)
            {
                moveSpeed = runSpeed;
                changePose = false;
                gameManager.playeractionui1.SetActive(false);
                gameManager.playeractionui2.SetActive(true);
                if(isfloat || isAerial_Rotation)
                {
                    moveSpeed = walkSpeed;
                }
            }
            else if(!isRun)
            {
                moveSpeed = walkSpeed;
                gameManager.playeractionui1.SetActive(true);
                gameManager.playeractionui2.SetActive(false);
            }

            if(isJump||isoverJump)
            {
                foreach(var ct in child_Trails)
                {
                    ct.emitting = true;
                }
            }
            else if(!isJump || !isoverJump)
            {
                foreach(var ct in child_Trails)
                {
                    ct.emitting = false;
                }
            }


            //ジャンプ処理・0.5秒間入力を制限
            if(Interval_InputButtondown(jump, 0.5f) &&isgroundFlag &&!isoverJump &&!isfloat)
            {
                isJump = true;
                isWalk = false;
                Posefalse();
                changePose = false;
                rigidbody.AddForce(transform.up * jumpPower, ForceMode.Impulse);
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
        if (!isRun && !changePose && isgroundFlag &&!GameManager.pauseflag)
        {
            if(Crouch.isPressed)
            {
                crouchPose = true;
            }
            else if(Laying.isPressed)
            {
                layingPose = true;
            }
            else if(Dance.isPressed)
            {
                dancePose = true;
            }

            if(crouchPose || layingPose || dancePose)
            {
                changePose = true;
            }
        }

        if(Crouch.wasReleasedThisFrame || Laying.wasReleasedThisFrame || Dance.wasReleasedThisFrame)
        {
            Posefalse();
            changePose = false;
        }

        animator.SetBool("Crouch", crouchPose);
        animator.SetBool("Laying", layingPose);
        animator.SetBool("dance", dancePose);
    }
    private void Posefalse()
    {
        crouchPose = false;
        layingPose = false;
        dancePose = false;
    }
    //プレイヤーがレーザー接触時、自身を非表示にし子オブジェクトの表示を行う
    public void PlayerDeath()
    {
        foreach(var ct in child_Trails)
        {
            ct.emitting = false;
        }
        foreach (var p in childParts)
        {
            p.On();
        }
        idleBody.SetActive(false);
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
       cameraInputVal = var.Get<Vector2>();
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
            return changePose;
        }
    }   
    public bool Duplicate_isDead
    {
        get
        {
            return isDead;
        }
    }
    public Vector2 Duplicate_cameraInputVal
    {
        get
        {
            return cameraInputVal;
        }
    }

}
