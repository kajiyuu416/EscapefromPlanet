using UnityEngine;
using System.Collections;
using TMPro;
using Cinemachine;
public class Doorlock : MonoBehaviour
{
    //コライダーと接触時テキスト表示、カメラワーク変更

    [SerializeField] TextMeshProUGUI doorlockText;
    [SerializeField] CinemachineVirtualCamera subcamera7;
    private const string text1 = "        ～扉は緊急ロック中～ \n" +
                "コンソールルームにて解除可能";
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            doorlockText.text = text1;
            subcamera7.Priority = 1;
            GameManager.pauseflag = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(SetText1());
        }
    }
    private IEnumerator SetText1()
    {
        yield return new WaitForSeconds(5.0f);
        subcamera7.Priority = 0;
        GameManager.pauseflag = false;
        doorlockText.text = "";
        StartCoroutine(SetText2());
    }
    private IEnumerator SetText2()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}
