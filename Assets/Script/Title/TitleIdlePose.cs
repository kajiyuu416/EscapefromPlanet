using UnityEngine;
public class TitleIdlePose : MonoBehaviour
{
    //インスペクターでセレクトしたポーズへ変更
    [SerializeField] Animator animator;
    [SerializeField] int selectNum;
    private bool crouchPose;
    private bool layingPose;
    private bool dancePose;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (selectNum == 0)
        {
            crouchPose = true;
            layingPose = false;
            dancePose = false;

        }
        if (selectNum == Const.CO.const_Int_List[0])
        {

            crouchPose = false;
            layingPose = true;
            dancePose = false;
        }
        if (selectNum == Const.CO.const_Int_List[1])
        {
            crouchPose = false;
            layingPose = false;
            dancePose = true;
        }
        animator.SetBool("Crouch", crouchPose);
        animator.SetBool("Laying", layingPose);
        animator.SetBool("dance", dancePose);
    }
}
