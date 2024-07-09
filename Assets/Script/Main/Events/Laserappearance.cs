using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//コライダーがプレイヤーと接触時、カメラワークの変更
//インスペクターで指定したオブジェクトの表示
public class Laserappearance : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera subcamera4;
    [SerializeField] GameObject lasers;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && GameManager2.ALF && GameManager2.additionPlayerActionGetFlag && GameManager2.floatPowerGetFlag)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            subcamera4.Priority = 1;
            lasers.SetActive(true);
            GameManager.pauseflag = true;
            StartCoroutine(ViewpointBack());
        }
    }
    private IEnumerator ViewpointBack()
    {
        yield return new WaitForSeconds(5.0f);
        subcamera4.Priority = 0;
        GameManager.pauseflag = false;
    }
}
