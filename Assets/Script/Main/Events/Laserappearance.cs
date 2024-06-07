using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


//コライダーがプレイヤーと接触時、カメラワークの変更
//インスペクターで指定したオブジェクトの表示
public class Laserappearance : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera subcamera4;
    [SerializeField] GameObject Lasers;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && GameManager2.ALF && GameManager2.additionPlayerActionGetFlag && GameManager2.floatPowerGetFlag)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            subcamera4.Priority = Const.CO.const_Int_List[0];
            Lasers.SetActive(true);
            GameManager.pauseflag = true;
            StartCoroutine("ViewpointBack");
        }
    }
    private IEnumerator ViewpointBack()
    {
        yield return new WaitForSeconds(Const.CO.const_Float_List[4]);
        subcamera4.Priority = 0;
        GameManager.pauseflag = false;
    }
}
