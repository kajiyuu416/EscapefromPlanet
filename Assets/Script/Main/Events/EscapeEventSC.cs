using System.Collections;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.InputSystem;

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
            subcamera3.Priority = 11;
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
        yield return new WaitForSeconds(3.0f);
        if(!Eventskip)
        {
            Actiontmessage.text = "※太陽が宇宙船に接近中※";
            StartCoroutine("SetText2");
        }
    }
    private IEnumerator SetText2()
    {
        yield return new WaitForSeconds(4.0f);
        if(!Eventskip)
        {
            Actiontmessage.text = "緊急脱出装置を目指してください";
            StartCoroutine("SetAction");
        }
    }
    private IEnumerator SetAction()
    {
        yield return new WaitForSeconds(3.0f);
        if(!Eventskip)
        {
            EventEnd();
        }
    }
    private void EventEnd()
    {
        Actiontmessage.text = "";
        SkipText.text = "";
        subcamera3.Priority = 9;
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
            SkipText.text = "Bボタンでスキップ";
            if(Skip.wasPressedThisFrame)
            {
                Eventskip = true;
                EventEnd();
                Debug.Log("EventSkip");
            }
        }
    }
}
