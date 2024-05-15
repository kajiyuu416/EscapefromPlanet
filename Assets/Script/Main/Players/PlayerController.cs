using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] AdditionPlayerAction AP;
    [SerializeField] FloatPowerSC floatPowerSC;
    [SerializeField] GameObject IdleBody;
    [SerializeField] Animator animator;
    public float WalkSpeed = 3f;
    public float RunSpeed = 5f;
    public float JumpPower;
    private float moveSpeed;
    private bool isWalk;
    public bool isRun;
    public bool isJump;
    public bool isDead;
    public bool isgroundFlag;
    private bool CrouchPose;
    private bool LayingPose;
    private bool DancePose;
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

        if(GameManager2.connect)
        {
            PlayerMove();
            ChangeIdlePose();
        }

        if (isDead)
        {
            isJump = false;
            floatPowerSC.isFloat = false;
            AP.isjumpOver = false;
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
                AP.isjumpOver = false;
            }
        }

        if(collision.CompareTag("spawnpoint"))
        {
            CP = transform.position; // 現在位置を記憶
            Debug.Log("CheckPoint更新");
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
    private void PlayerMove()//プレイヤーの移動及び歩きアニメーション処理
    {
        PlayerMove_input.x = moveInputVal.x;
        PlayerMove_input.z = moveInputVal.y;
        var current_GP = Gamepad.current;
        var Run = current_GP.rightShoulder;
        var speed = Run.isPressed ? 2 : 1;
        var jump = current_GP.buttonSouth;
        var velocity = new Vector3(PlayerMove_input.x, 0, PlayerMove_input.z).normalized;
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * PlayerMove_input.z + Camera.main.transform.right * PlayerMove_input.x;
        Posefalse();
        rigidbody.velocity = moveForward * moveSpeed + new Vector3(0, rigidbody.velocity.y, 0);


        if(moveForward != Vector3.zero)
        {
            if(!GameManager.pauseflag)
            {
                Quaternion QL = Quaternion.LookRotation(moveForward);
                transform.rotation = Quaternion.Lerp(transform.rotation, QL, 15.0f * Time.deltaTime);
            }
        }

        if(GameManager.pauseflag)
        {
            velocity = Vector3.zero;
            rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
            isRun = false;
        }

        if(!GameManager.pauseflag)
        {
            if(PlayerMove_input != Vector3.zero && isgroundFlag &&!isJump)
            {
                isWalk = true;
                isRun = false;
                moveSpeed = WalkSpeed;
                gameManager.ActionUI2.SetActive(false);
                Posefalse();
                if(GameManager2.ActionUIFlag)
                {
                    gameManager.ActionUI1.SetActive(true);
                }

                if(Run.isPressed)
                {
                    isRun = true;
                    moveSpeed = RunSpeed;

                    if(GameManager2.ActionUIFlag)
                    {
                        gameManager.ActionUI1.SetActive(false);
                        gameManager.ActionUI2.SetActive(true);
                    }
                }
            }
            else if(PlayerMove_input == Vector3.zero && isgroundFlag)
            {
                isWalk = false;
                isRun = false;
                gameManager.ActionUI2.SetActive(false);
                if(GameManager2.ActionUIFlag)
                {
                    gameManager.ActionUI1.SetActive(true);
                }
            }
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
        animator.SetBool("floating", floatPowerSC.isFloat);
        animator.SetBool("Jumpover", AP.isjumpOver);
    }
    private void ChangeIdlePose()
    {
        var current_GP = Gamepad.current;
        var Crouch = current_GP.buttonEast;
        var Laying = current_GP.buttonWest;
        var Dance = current_GP.buttonNorth;
        if (!GameManager.pauseflag)
        {
            if (!isRun)
            {
                if (Crouch.wasPressedThisFrame)
                {
                    CrouchPose = true;
                    LayingPose = false;
                    DancePose = false;
                }
                else if (Laying.wasPressedThisFrame)
                {

                    CrouchPose = false;
                    LayingPose = true;
                    DancePose = false;
                }
                else if (Dance.wasPressedThisFrame)
                {
                    CrouchPose = false;
                    LayingPose = false;
                    DancePose = true;
                }
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
        AP.isjumpOver = false;
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE3();
        Debug.Log("着地したよ");
    }
    public void Jumpmiss()
    {
        isJump = false;
        isgroundFlag = false;
        animator.SetBool("jump", false);
        Debug.Log("Jumpmiss");
    }

    //ボタン入力のクールタイムを設定し、一定時間入力を制御する処理
    private static bool preventContinuityInput;
    private static float buttonDownTime;
    private static float timer;
    public static bool Interval_InputButtondown(ButtonControl input, float intervalSeconds)
    {
        timer = Time.time;

        if(input.wasPressedThisFrame && timer - buttonDownTime >= intervalSeconds)
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
}
