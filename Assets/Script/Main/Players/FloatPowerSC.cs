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
    private float PushTime;
    private float floatPower = 4.0f;
    private bool isFloat;
    private bool isFloatFlag;
    private bool SmallInputFloat;
    private bool isDownFlag;
    public static bool AdditionPlayerActionFlag_Float = false;
    private float time;
    private float Purposetime = 1.0f;
    private float limittime = 3.5f;
    private  new Rigidbody rigidbody;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        playerController = FindObjectOfType<PlayerController>();
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
    private void Update()
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
        var Float = current_GP.buttonSouth;
        bool isjump = playerController.Duplicate_isJump;
        if(Float.wasPressedThisFrame &&isjump)
        {
            isFloatFlag = true;
            isDownFlag = true;
            time = 0f;
            PushTime = 0f;
        }
        
        if (Float.wasReleasedThisFrame)
        {
            isFloatFlag = false;
            isDownFlag = false;
            SmallInputFloat = false;
            time = 0f;
            PushTime = 0f;
        }

        if(isFloatFlag)
        {
            time += Time.deltaTime;
            PushTime = time / Purposetime;
            if(time >= Purposetime)
            {
                isFloat = true;
                playerController.Duplicate_isJump = false;
            }
        }

        if(isDownFlag)
        {
            time += Time.deltaTime;
            PushTime = time / limittime;
            if(time >= limittime)
            {
                isFloatFlag = false;
                isDownFlag = false;
                isFloat = false;
            }
        }
        if(isFloat)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, floatPower, rigidbody.velocity.z);
            if(Float.wasReleasedThisFrame)
            {
                SmallInputFloat = true;
                isFloatFlag = false;
                isDownFlag = false;
                isFloat = false;
                time = 0f;
                PushTime = 0f;
            }
            animator.SetBool("smallfloat", SmallInputFloat);
        }
        SmallInputFloat = false;
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
}
