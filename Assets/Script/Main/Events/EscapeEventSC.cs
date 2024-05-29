using System.Collections;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.InputSystem;

//�����𖞂����A�R���C�_�[�ɐڐG�����ꍇ�A�J�����̈ʒu�؂�ւ��ƃe�L�X�g�\���ABGM�ύX�A�^�C�}�[�\�����s��
//�C�x���g�V�[���̃X�L�b�v����
public class EscapeEventSC : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera subcamera3;
    [SerializeField] SunMove SM;
    [SerializeField] TextMeshProUGUI SkipText;
    [SerializeField] TextMeshProUGUI Actiontmessage;
    [SerializeField] GameObject Timer;
    private bool EscapeEvent;
    private bool Eventskip;
    private void Update()
    {
        EventSkip();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && GameManager2.ALF && GameManager2.AGF && GameManager2.FGF)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            subcamera3.Priority = Const.CO.Const_Int_List[0];
            GameManager.instance.ActionUI1.SetActive(false);
            GameManager.instance.ActionUI2.SetActive(false);
            GameManager.pauseflag = true;
            SoundManager.Instance.StopAudio();
            SoundManager.Instance.SettingPlaySE14();
            MessageIndication();
        }
    }
    private void MessageIndication()
    {
        EscapeEvent = true;
        Actiontmessage.text = "";
        if(!Eventskip)
        {
            StartCoroutine("SetText1");
        }
    }
    private IEnumerator SetText1()
    {
        yield return new WaitForSeconds(Const.CO.Const_Float_List[4]);
        if(!Eventskip)
        {
            Actiontmessage.text = "�����z���F���D�ɐڋߒ���";
            StartCoroutine("SetText2");
        }
    }
    private IEnumerator SetText2()
    {
        yield return new WaitForSeconds(Const.CO.Const_Float_List[4]);
        if(!Eventskip)
        {
            Actiontmessage.text = "�ً}�E�o���u��ڎw���Ă�������";
            StartCoroutine("SetAction");
        }
    }
    private IEnumerator SetAction()
    {
        yield return new WaitForSeconds(Const.CO.Const_Float_List[4]);
        if(!Eventskip)
        {
            EventEnd();
        }
    }
    private void EventEnd()
    {
        Actiontmessage.text = "";
        SkipText.text = "";
        subcamera3.Priority = 0;
        SoundManager.Instance.StopAudio();
        SoundManager.Instance.Startbgm3();
        GameManager.pauseflag = false;
        GameManager.instance.ActionUI1.SetActive(true);
        GameManager.instance.ActionUI2.SetActive(true);
        EscapeEvent = false;
        SM.MPF = true;
        Timer.SetActive(true);
    }
    private void EventSkip()
    {
        var current_GP = Gamepad.current;
        var Skip = current_GP.buttonEast;

        if(EscapeEvent && !Eventskip)
        {
            SkipText.text = "B�{�^���ŃX�L�b�v";
            if(Skip.wasPressedThisFrame)
            {
                Eventskip = true;
                EventEnd();
            }
        }
    }
}
