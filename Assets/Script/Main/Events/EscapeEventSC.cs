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
    private bool escapeEvent;
    private bool eventskip;
    private void Update()
    {
        EventSkip();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && GameManager2.ALF && GameManager2.additionPlayerActionGetFlag && GameManager2.floatPowerGetFlag)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            subcamera3.Priority = Const.CO.const_Int_List[0];
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
            StartCoroutine("SetText1");
        }
    }
    private IEnumerator SetText1()
    {
        yield return new WaitForSeconds(Const.CO.const_Float_List[2]);
        if(!eventskip)
        {
            actiontmessage.text = "※太陽が宇宙船に接近中※";
            StartCoroutine("SetText2");
        }
    }
    private IEnumerator SetText2()
    {
        yield return new WaitForSeconds(Const.CO.const_Float_List[3]);
        if(!eventskip)
        {
            actiontmessage.text = "緊急脱出装置を目指してください";
            StartCoroutine("SetAction");
        }
    }
    private IEnumerator SetAction()
    {
        yield return new WaitForSeconds(Const.CO.const_Float_List[3]);
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
        var Skip = current_GP.buttonEast;

        if(escapeEvent && !eventskip)
        {
            skipText.text = "Bボタンでスキップ";
            if(Skip.wasPressedThisFrame)
            {
                eventskip = true;
                EventEnd();
            }
        }
    }
}
