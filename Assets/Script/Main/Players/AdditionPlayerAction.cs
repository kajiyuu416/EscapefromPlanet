using UnityEngine;
using UnityEngine.InputSystem;
//アクション追加①大ジャンプとスライディングの追加
public class AdditionPlayerAction : MonoBehaviour
{
    [SerializeField] GameObject EventObj1;
    [SerializeField] GameObject EventObj2;
    private PlayerController playerController;
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
    }
    private void Update()
    {
        Additional_Actions();
    }
    private void Additional_Actions()
    {
        if(AdditionPlayerActionFlag_OverJump && !playerController.isDead)
        {
            var current_GP = Gamepad.current;
            var JumpOver = current_GP.buttonNorth;
            var RunningSlide = current_GP.buttonEast;
            bool isrun = playerController.Duplicate_isRun;
            bool isground = playerController.Duplicate_isgroundFlag;
            bool isjump = playerController.Duplicate_isJump;

            if(isrun && isground)
            {
                if(JumpOver.wasPressedThisFrame &&!isjump)
                {
                    isjumpOver = true;
                    rigidbody.AddForce(transform.up * JumpOverPower, ForceMode.Impulse);
                }

                if(PlayerController.Interval_InputButtondown(RunningSlide, Const.CO.Const_Float_List[0]))
                {
                    rigidbody.AddForce(transform.forward * slidingPower, ForceMode.Impulse);
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
