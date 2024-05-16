using UnityEngine;
public class TitleIdlePose : MonoBehaviour
{
    //インスペクターでセレクトしたポーズへ変更
    [SerializeField] Animator animator;
    [SerializeField] int SelectNum;
    private bool CrouchPose;
    private bool LayingPose;
    private bool DancePose;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (SelectNum == 0)
        {
            CrouchPose = true;
            LayingPose = false;
            DancePose = false;

        }
        if (SelectNum == 1)
        {

            CrouchPose = false;
            LayingPose = true;
            DancePose = false;
        }
        if (SelectNum == 2)
        {
            CrouchPose = false;
            LayingPose = false;
            DancePose = true;
        }
        animator.SetBool("Crouch", CrouchPose);
        animator.SetBool("Laying", LayingPose);
        animator.SetBool("dance", DancePose);
    }
}
