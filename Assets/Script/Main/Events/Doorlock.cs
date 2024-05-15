using UnityEngine;
using TMPro;
public class Doorlock : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI DoorlockText;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            DoorlockText.text = "        �`���ً͋}���b�N���` \n"+ 
                "�R���\�[�����[���ɂĉ����\";
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            DoorlockText.text = "";
        }
    }
}
