using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

//アクション追加②ジャンプボタンを押している長さでプレイヤーが上昇する処理
//対象のボタンを押している秒数を取得しフラグを返す
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
    //特定のボタンを押した長さの取得
    //押した長さに応じてプレイヤーが上昇
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


        //Playerが地面と接触且つ、走っている状態で特定のボタンを押した時
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

        //特定のボタンを押している間タイムカウントを行い
        //カウントを超えるまでフラグを返す
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

        //特定のボタンを押している間タイムカウントを行い
        //カウントを超えるまでフラグを返す
        //タイムカウントが上限値を超えたとき
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

        //Player上昇
        if(isFloat)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, floatPower, rigidbody.velocity.z);
            additionPlayerAction.Duplicate_isjumpOver = false;
            //上昇中ボタンを離した時のコールバック
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
