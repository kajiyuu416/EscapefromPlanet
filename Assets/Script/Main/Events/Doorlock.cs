using UnityEngine;
using System.Collections;
using TMPro;
using Cinemachine;
public class Doorlock : MonoBehaviour
{
    //�R���C�_�[�ƐڐG���e�L�X�g�\��
    [SerializeField] TextMeshProUGUI DoorlockText;
    [SerializeField] CinemachineVirtualCamera subcamera7;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            DoorlockText.text = "        �`���ً͋}���b�N���` \n"+ 
                "�R���\�[�����[���ɂĉ����\";
            subcamera7.Priority = Const.CO.Const_Int_List[0];
            GameManager.pauseflag = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine("SetAction1");
        }
    }
    private IEnumerator SetAction1()
    {
        yield return new WaitForSeconds(Const.CO.Const_Float_List[4]);
        subcamera7.Priority = 0;
        GameManager.pauseflag = false;
        DoorlockText.text = "";
        StartCoroutine("SetAction2");
    }
    private IEnumerator SetAction2()
    {
        yield return new WaitForSeconds(Const.CO.Const_Float_List[4]);
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}
