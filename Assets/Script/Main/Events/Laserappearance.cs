using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Laserappearance : MonoBehaviour
{
    public CinemachineVirtualCamera subcamera4;
    [SerializeField] GameObject Lasers;
    [SerializeField] GameObject Idle;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && GameManager2.ALF && GameManager2.AGF && GameManager2.FGF)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            subcamera4.Priority = 11;
            Lasers.SetActive(true);
            GameManager.pauseflag = true;
            StartCoroutine("ViewpointBack");
        }
    }
    private IEnumerator ViewpointBack()
    {
        yield return new WaitForSeconds(5.0f);
        subcamera4.Priority = 8;
        GameManager.pauseflag = false;
    }
}
