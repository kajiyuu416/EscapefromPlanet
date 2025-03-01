using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

//アクション追加�Aジャンプボタンを押している長さでプレイヤーが上昇する処理
//対象のボタンを押している秒数を取得しフラグを返す
public class FloatPowerSC : MonoBehaviour
{
    [SerializeField] GameObject eventObj;
    [SerializeField] TextMeshProUGUI text;
    private PlayerController playerController;
    private AdditionPlayerAction additionPlayerAction;
    private bool isFloat;
    private bool isFloatFlag;
    private bool aerial_Rotation;
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
                aerial_Rotation = false;
                isFloatFlag = false;
            }
        }
    }

    private void Update()
    {
        if(GameManager2.floatPowerGetFlag)
        {
            OnPushKey();
            if(eventObj.activeSelf)
            {
                eventObj.SetActive(false);
            }

            if(isFloat)
            {
                text.text = "下降・・・Xボタンを離す";
            }
            else if(!isFloat)
            {
                text.text = "上昇・・・ダッシュ中 Xボタン長押し";
            }

            if(playerController.Duplicate_isDead)
            {
                isFloat = false;
                isFloatFlag = false;
                return;
            }


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
        const float limittime = 2.0f;
        const float floatPower = 3.0f;
        //ボタンの入力チェック
        //対応したボタンが押されたときと離されたとき
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
        //特定のボタンを押している間タイムカウントを行い
        //カウントを超えるまでフラグを返す
        if(isFloatFlag)
        {
            time += Time.deltaTime;
            isFloat = true;
            playerController.Duplicate_isJump = false;
            PushTime = time / limittime;
            //タイムカウントが上限値を超えたとき
            if(isDownFlag && time >= limittime)
            {
                aerial_Rotation = true;
                isFloatFlag = false;
                isDownFlag = false;
                isFloat = false;
            }
        }
        //上昇中ボタンを離した時のコールバック
        if(Float.wasReleasedThisFrame　&&isFloat)
        {
            aerial_Rotation = true;
            isFloatFlag = false;
            isDownFlag = false;
            isFloat = false;
            timeReset();
        }

        //Player上昇
        if(isFloat)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, floatPower, rigidbody.velocity.z);
            additionPlayerAction.Duplicate_isjumpOver = false;
        }
        //Playerがポーズ中はカウントを行わないよう制御
        if(ispose)
        {
            isFloatFlag = false;
            isDownFlag = false;
            isFloat = false;
            timeReset();
        }

        animator.SetBool("Aerial_Rotation", aerial_Rotation);
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
    public bool Duplicate_Aerial_Rotation
    {
        get
        {
            return aerial_Rotation;
        }
    }
}
