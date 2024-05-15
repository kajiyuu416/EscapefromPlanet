using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FloatPowerSC : MonoBehaviour
{
    [SerializeField] PlayerController PC;
    [SerializeField] GameObject EventObj1;
    [SerializeField] GameObject EventObj2;
    public float PushTime;
    public float floatPower = 4.0f;
    public bool isFloat;
    public bool isFloatFlag;
    public static bool AdditionPlayerActionFlag = false;
    private bool SmallInputFloat;
    private bool isDownFlag;
    private float time;
    private float Purposetime = 1.0f;
    private float limittime = 3.5f;
    private  new Rigidbody rigidbody;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(!PC.isgroundFlag)
        {
            if(collision.CompareTag("ground"))
            {
                SmallInputFloat = false;
            }
        }
    }
    private void Update()
    {
        if(AdditionPlayerActionFlag)
        {
            OnPushKey();
            Timelength();
            if(PC.isDead)
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
    public void OnPushKey()
    {
        var current_GP = Gamepad.current;
        var Float = current_GP.buttonSouth;
        if(Float.wasPressedThisFrame &&PC.isJump)
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
        animator.SetBool("floating", isFloat);
    }
    public void Timelength()//キーが押された時間を取得し、フラグを返す
    {
        var current_GP = Gamepad.current;
        var Float = current_GP.buttonSouth;
        if (isFloatFlag)
        {
          time += Time.deltaTime;
          PushTime = time / Purposetime;
           if (time >= Purposetime)
           {
             isFloat = true;
             PC.isJump = false;
           }
        }

        if (isDownFlag)
        {
            time += Time.deltaTime;
            PushTime = time / limittime;
            if (time >= limittime)
            {
                isFloatFlag = false;
                isDownFlag = false;
                isFloat = false;
            }
        }
        if (isFloat)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x,floatPower, rigidbody.velocity.z );
            if (Float.wasReleasedThisFrame)
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
    }
    public void Spin()
    {
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE10();
    }
}
