using UnityEngine;
using UnityEngine.UI;
//Playerとレーザーの距離に応じて危険を知らせるマークの表示及びレーザーとの距離に応じてマークの色を変更する処理。
public class ChangeColorScript : MonoBehaviour
{
    [SerializeField] Image WI;
    private float changeColorThreshold = 10.0f;
    private Color startColor = Color.white;
    private Color endColor = Color.red;
    private Transform target;
    private string tagname;
    private bool restriction;

    private void Start()
    {
        var go = GameObject.FindGameObjectWithTag("Player");

        if (go)
            target = go.transform;
        else
            Debug.LogWarning("ターゲットとなる Player タグのついたオブジェクトがシーン上に存在しません");
        WI.enabled = false;
        tagname = "warningarea";
    }
    private  void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.tag == tagname)
        {
            restriction = true;
            WI.enabled = true;
            if (!target) return;
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < changeColorThreshold)
            {
                Color c = (endColor * (changeColorThreshold - distance) + startColor * distance) / changeColorThreshold;
                WI.color = c;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(restriction)
        {
            WI.enabled = false;
            restriction = false;
        }
    
    }
}