using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Laserappearance : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera subcamera4;
    [SerializeField] GameObject Lasers;
    [SerializeField] GameObject Idle;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && GameManager2.ALF && GameManager2.AGF && GameManager2.FGF)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            subcamera4.Priority = Const.CO.Const_Int_List[0];
            Lasers.SetActive(true);
            GameManager.pauseflag = true;
            StartCoroutine("ViewpointBack");
        }
    }
    private IEnumerator ViewpointBack()
    {
        yield return new WaitForSeconds(Const.CO.Const_Float_List[4]);
        subcamera4.Priority = 0;
        GameManager.pauseflag = false;
    }
}
