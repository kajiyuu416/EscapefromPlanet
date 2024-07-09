using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

//�E�o���u�̃��b�N�����C�x���g
public class EscapeEventFlagSC : MonoBehaviour
{
    [SerializeField] FadeInOut fadeinout;
    [SerializeField] TextMeshProUGUI actionPop;
    [SerializeField] TextMeshProUGUI actiontmessage;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] BoxCollider boxCollider;
    private const string text1 = "LB�{�^�����͂ŃA�N�V�����s��";
    private const string text2 = "�ً}�E�o���u�̃��b�N���������ꂽ";
    private float waitsecondTime = 3.0f;
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
                actionPop.text = "";
                GameManager.pauseflag = true;
                fadeinout.fadeOutFlag = true;
                boxCollider.enabled = false;
                meshRenderer.enabled = false;
                StartCoroutine(SetALF());
        }

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            actionPop.text = text1;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            actionPop.text = "";
        }
    }

    IEnumerator SetALF()
    {
        yield return new WaitForSeconds(waitsecondTime);
        fadeinout.fadeInFlag = true;
        GameManager.pauseflag = false;
        GameManager2.ALF = true;
        SoundManager.Instance.SettingPlaySE6();
        SoundManager.Instance.Startbgm2();
        MessageIndication();
    }
    private void MessageIndication()
    {
        actiontmessage.text = text2;
        StartCoroutine(SetText());
    }
    IEnumerator SetText()
    {
        yield return new WaitForSeconds(waitsecondTime);
        actiontmessage.text = "";
    }
}


