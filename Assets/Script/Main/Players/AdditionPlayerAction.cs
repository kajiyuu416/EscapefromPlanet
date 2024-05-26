using UnityEngine;
using UnityEngine.InputSystem;
//アクション追加①大ジャンプとスライディングの追加
public class AdditionPlayerAction : MonoBehaviour
{
    [SerializeField] GameObject EventObj1;
    [SerializeField] GameObject EventObj2;
    private PlayerController playerController;
    private FloatPowerSC floatPowerSC;
    private const float JumpOverPower = 200.0f;
    private const float slidingPower = 1000.0f;
    private bool isjumpOver;
    public static bool AdditionPlayerActionFlag_OverJump = false;
    private Animator animator;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
        floatPowerSC = FindObjectOfType<FloatPowerSC>();
    }
    private void Update()
    {
        Additional_Actions();
    }
    private void Additional_Actions()
    {
        //特定のフラグが帰っていた場合のみ処理
        if(AdditionPlayerActionFlag_OverJump && !playerController.isDead)
        {
            var current_GP = Gamepad.current;
            var JumpOver = current_GP.buttonNorth;
            var RunningSlide = current_GP.buttonEast;
            bool isrun = playerController.Duplicate_isRun;
            bool isground = playerController.Duplicate_isgroundFlag;
            bool isjump = playerController.Duplicate_isJump;
            bool isfloat = floatPowerSC.Duplicate_isFloatFlag;

            //Playerが地面と接触且つ、走っている状態で特定のボタンを押した時
            if(isrun && isground)
            {
                if(JumpOver.wasPressedThisFrame &&!isjump &&!isfloat)
                {
                    isjumpOver = true;
                    floatPowerSC.Duplicate_isFloat = false;
                    rigidbody.AddForce(transform.up * JumpOverPower, ForceMode.Impulse);
                }

                if(PlayerController.Interval_InputButtondown(RunningSlide, Const.CO.Const_Float_List[0]))
                {
                    rigidbody.AddForce(transform.forward * slidingPower, ForceMode.Impulse);
                    floatPowerSC.Duplicate_isFloat = false;
                    SoundManager SM = SoundManager.Instance;
                    animator.SetTrigger("RunningSlide");
                    SM.SettingPlaySE8();
                }
            }
            if(GameManager2.AGF)
            {
                EventObj1.SetActive(false);
                EventObj2.SetActive(false);
            }
            if(playerController.isDead)
            {
                isjumpOver = false;
            }
            animator.SetBool("Jumpover", isjumpOver);
        }
    }
    public void OverJumpmiss()
    {
        bool isground = playerController.Duplicate_isgroundFlag;
        isjumpOver = false;
        isground = false;
        animator.SetBool("Jumpover",false);
        Debug.Log("Jumpmiss");
    }
    public bool Duplicate_isjumpOver
    {
        get
        {
            return isjumpOver;
        }
        set
        {
            isjumpOver = value;
        }
    }
}
