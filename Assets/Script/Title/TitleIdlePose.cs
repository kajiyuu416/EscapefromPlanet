using UnityEngine;
public class TitleIdlePose : MonoBehaviour
{
    //インスペクターでセレクトしたポーズへ変更
    [SerializeField] Animator animator;
    [SerializeField] int selectNum;
    private int poseCount;
    private float time;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("poseNum",selectNum);
    }
    private void Update()
    {
        if(selectNum == 0)
        {
            time += Time.deltaTime;

            if(time > 5)
            {
                if(poseCount < 4)
                {
                    poseCount++;
                }
                else
                {
                    poseCount = 0;
                }
                
                time = 0;    
            }
        }
        animator.SetInteger("poseTimeCount", poseCount);
    }
}
