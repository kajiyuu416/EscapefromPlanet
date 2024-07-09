using System.Collections;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.InputSystem;

//条件を満たし、コライダーに接触した場合、カメラの位置切り替えとテキスト表示、BGM変更、タイマー表示を行う
//イベントシーンのスキップ実装
public class EscapeEventSC : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera subcamera3;
    [SerializeField] SunMove sunmove;
    [SerializeField] TextMeshProUGUI skipText;
    [SerializeField] TextMeshProUGUI actiontmessage;
    [SerializeField] GameObject timer;
    private const string text1 = "※太陽が宇宙船に接近中※";
    private const string text2 = "緊急脱出装置を目指してください";
    private const string text3 = "Bボタンでスキップ";
    private bool escapeEvent;
    private bool eventskip;
    private float waitsecond_shortTime = 3.0f;
    private float waitsecond_longTime = 4.0f;
    private void Update()
    {
        EventSkip();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && GameManager2.ALF && GameManager2.additionPlayerActionGetFlag && GameManager2.floatPowerGetFlag)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            subcamera3.Priority = 1;
            GameManager.instance.playerui.SetActive(false);
            GameManager.pauseflag = true;
            SoundManager.Instance.StopAudio();
            SoundManager.Instance.SettingPlaySE14();
            MessageIndication();
        }
    }
    private void MessageIndication()
    {
        escapeEvent = true;
        actiontmessage.text = "";
        if(!eventskip)
        {
            StartCoroutine(SetText1());
        }
    }
    private IEnumerator SetText1()
    {
        yield return new WaitForSeconds(waitsecond_shortTime);
        if(!eventskip)
        {
            actiontmessage.text = text1;
            StartCoroutine(SetText2());
        }
    }
    private IEnumerator SetText2()
    {
        yield return new WaitForSeconds(waitsecond_longTime);
        if(!eventskip)
        {
            actiontmessage.text = text2;
            StartCoroutine(SetAction());
        }
    }
    private IEnumerator SetAction()
    {
        yield return new WaitForSeconds(waitsecond_longTime);
        if(!eventskip)
        {
            EventEnd();
        }
    }
    private void EventEnd()
    {
        actiontmessage.text = "";
        skipText.text = "";
        subcamera3.Priority = 0;
        SoundManager.Instance.StopAudio();
        SoundManager.Instance.Startbgm3();
        GameManager.pauseflag = false;
        GameManager.instance.playerui.SetActive(true);
        escapeEvent = false;
        sunmove.MPF = true;
        timer.SetActive(true);
    }
    private void EventSkip()
    {
        var current_GP = Gamepad.current;
        var skip = current_GP.buttonEast;

        if(escapeEvent && !eventskip)
        {
            skipText.text = text3;
            if(skip.wasPressedThisFrame)
            {
                eventskip = true;
                EventEnd();
            }
        }
    }
}
