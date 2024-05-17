using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

//�E�o���u�̃��b�N�����C�x���g
public class EscapeEventFlagSC : MonoBehaviour
{
    [SerializeField] FadeInOut FO;
    [SerializeField] TextMeshProUGUI ActionPop;
    [SerializeField] TextMeshProUGUI Actiontmessage;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] BoxCollider boxCollider;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        if(GameManager2.ALF)
        {
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        var current_GP = Gamepad.current;
        var Check = current_GP.leftShoulder;

        if (collision.CompareTag("Player") && Check.wasPressedThisFrame && !GameManager2.ALF)
        {
                ActionPop.text = "";
                GameManager.pauseflag = true;
                FO.FadeOutFlag = true;
                boxCollider.enabled = false;
                meshRenderer.enabled = false;
                StartCoroutine("SetALF");
        }

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActionPop.text = "LB�{�^�����͂ŃA�N�V�����s��";
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActionPop.text = "";
        }
    }

    IEnumerator SetALF()
    {
        yield return new WaitForSeconds(3.0f);
        FO.FadeInFlag = true;
        GameManager.pauseflag = false;
        GameManager2.ALF = true;
        SoundManager.Instance.SettingPlaySE6();
        SoundManager.Instance.Startbgm2();
        MessageIndication();
    }
    private void MessageIndication()
    {
        Actiontmessage.text = "�ً}�E�o���u�̃��b�N���������ꂽ";
        StartCoroutine("SetText");

    }
    IEnumerator SetText()
    {
        yield return new WaitForSeconds(3.0f);
        Actiontmessage.text = "";
    }
}


