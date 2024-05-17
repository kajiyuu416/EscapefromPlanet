using UnityEngine;
using UnityEngine.InputSystem;
//アクション追加①大ジャンプとスライディングの追加
public class AdditionPlayerAction : MonoBehaviour
{
    [SerializeField] PlayerController PC;
    [SerializeField] GameObject EventObj1;
    [SerializeField] GameObject EventObj2;
    public float JumpOverPower;
    public float slidingPower;
    public bool isjumpOver;
    public static bool AdditionPlayerActionFlag = false;
    public Animator animator;
    private new Rigidbody rigidbody;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        Additional_Actions();
    }
    private void Additional_Actions()
    {
        if(AdditionPlayerActionFlag &&!PC.isDead)
        {
            var current_GP = Gamepad.current;
            var JumpOver = current_GP.buttonSouth;
            var RunningSlide = current_GP.buttonEast;
            if(PC.isRun && PC.isgroundFlag)
            {
                if(JumpOver.wasPressedThisFrame)
                {
                    isjumpOver = true;
                    rigidbody.AddForce(transform.up * JumpOverPower, ForceMode.Impulse);
                    Debug.Log("OverJump");
                }

                if(PlayerController.Interval_InputButtondown(RunningSlide, 1.0f))
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
            if(PC.isDead)
            {
                isjumpOver = false;
            }
            animator.SetBool("Jumpover", isjumpOver);
        }
    }
    public void OverJumpmiss()
    {
        isjumpOver = false;
        PC.isgroundFlag = false;
        animator.SetBool("Jumpover",false);
        Debug.Log("Jumpmiss");
    }
}
