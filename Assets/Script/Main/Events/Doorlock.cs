using UnityEngine;
using TMPro;
public class Doorlock : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI DoorlockText;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            DoorlockText.text = "        ～扉は緊急ロック中～ \n"+ 
                "コンソールルームにて解除可能";
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
