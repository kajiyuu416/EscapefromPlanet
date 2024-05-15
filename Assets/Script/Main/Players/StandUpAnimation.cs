using UnityEngine;
public class StandUpAnimation : MonoBehaviour
{
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
