using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.InputSystem;

public class transferEvent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ActionPop;
    [SerializeField] GameObject IdleMeshs;
    [SerializeField] GameObject transEfe;
    [SerializeField] GameObject Timer;
    [SerializeField] FadeInOut FO;
    public CinemachineVirtualCamera subcamera5;
    public CinemachineVirtualCamera subcamera6;
    private void OnTriggerStay(Collider collision)
    {
        var current_GP = Gamepad.current;
        var Check = current_GP.leftShoulder;

        if (collision.CompareTag("Player") && Check.wasPressedThisFrame && GameManager2.FGF)
        {
            subcamera5.Priority = 11;
            IdleMeshs.SetActive(false);
            transEfe.SetActive(true);
            Timer.SetActive(false);
            gameObject.GetComponent<BoxCollider>().enabled = false;
            GameManager.instance.ActionUI1.SetActive(false);
            GameManager.instance.ActionUI2.SetActive(false);
            SoundManager.Instance.StopAudio();
            SoundManager.Instance.SettingPlaySE12();
            GameManager.pauseflag = true;
            ActionPop.text = "";
            StartCoroutine("FOtrue");
        }
        if (collision.CompareTag("Player") && Check.wasPressedThisFrame && !GameManager2.FGF)
        {
            ActionPop.text = "転送装置のロックが解除されていません";
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Idle")
        {
            ActionPop.text = "LBボタン入力でアクション行う";
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "Idle")
        {
            ActionPop.text = ""; 
        }
    }
    IEnumerator FOtrue()
    {
        yield return new WaitForSeconds(3.0f);
        FO.FadeOutFlag = true;
        subcamera5.Priority = 9;
        StartCoroutine("CameraSwitch");
    }
    IEnumerator FItrue()
    {
        yield return new WaitForSeconds(2.5f);
        FO.FadeInFlag = true;
        GameManager.instance.GameClear();
    }
    IEnumerator CameraSwitch()
    {
        yield return new WaitForSeconds(4.0f);
        subcamera6.Priority = 12;
        StartCoroutine("FItrue");
    }

}


