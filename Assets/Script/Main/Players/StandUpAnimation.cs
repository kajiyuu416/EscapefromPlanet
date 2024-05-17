using UnityEngine;
public class StandUpAnimation : MonoBehaviour
{
    //プレイヤーの起き上がりモーション再生
    public Animator animator;
    private bool isStand;
    private void Start()
    {
        isStand = true;
        animator.SetBool("StandUp", isStand);
        Invoke("endanimation",3f);
    }
    private void endanimation()
    {
        animator.SetBool("StandUp",false);
    }
 
}
