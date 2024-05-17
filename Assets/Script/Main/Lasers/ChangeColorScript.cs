using UnityEngine;
using UnityEngine.UI;
//Playerとレーザーの距離に応じて危険を知らせるマークの表示及びレーザーとの距離に応じてマークの色を変更する処理。
public class ChangeColorScript : MonoBehaviour
{
    [SerializeField] float _changeColorThreshold = 10f;
    [SerializeField] Color _startColor = Color.white;
    [SerializeField] Color _endColor = Color.red;
    [SerializeField] Image WI;
    private Transform _target;
    private string tagname;
    private bool restriction;

    private void Start()
    {
        var go = GameObject.FindGameObjectWithTag("Player");

        if (go)
            _target = go.transform;
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
            if (!_target) return;
            float distance = Vector3.Distance(transform.position, _target.position);

            if (distance < _changeColorThreshold)
            {
                Color c = (_endColor * (_changeColorThreshold - distance) + _startColor * distance) / _changeColorThreshold;
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