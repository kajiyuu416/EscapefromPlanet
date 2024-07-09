using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.InputSystem;

//コライダーにプレイヤーが接触している状態で、特定のボタン入力を検知すると
//プレイヤー、UIを非表示、カメラの位置を変更、転送エフェクトの表示
public class transferEvent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI actionPop;
    [SerializeField] GameObject idleMeshs;
    [SerializeField] GameObject transEfe;
    [SerializeField] GameObject timer;
    [SerializeField] FadeInOut fadeinout;
    public CinemachineVirtualCamera subcamera5;
    public CinemachineVirtualCamera subcamera6;
    private const string text1 = "転送装置のロックが解除されていません";
    private const string text2 = "LBボタン入力でアクション行う";
    private const float waitsecondTime = 3.0f;
    private void OnTriggerStay(Collider collision)
    {
        var current_GP = Gamepad.current;
        var Check = current_GP.leftShoulder;

        if (collision.CompareTag("Player") && Check.wasPressedThisFrame && GameManager2.floatPowerGetFlag)
        {
            subcamera5.Priority = 1;
            idleMeshs.SetActive(false);
            transEfe.SetActive(true);
            timer.SetActive(false);
            gameObject.GetComponent<BoxCollider>().enabled = false;
            GameManager.instance.playerui.SetActive(false);
            SoundManager.Instance.StopAudio();
            SoundManager.Instance.SettingPlaySE12();
            GameManager.pauseflag = true;
            actionPop.text = "";
            StartCoroutine(FOtrue());
        }
        else
        {
            actionPop.text = text1;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "idle")
        {
            actionPop.text = text2;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "idle")
        {
            actionPop.text = ""; 
        }
    }
    IEnumerator FOtrue()
    {
        yield return new WaitForSeconds(waitsecondTime);
        fadeinout.fadeOutFlag = true;
        subcamera5.Priority = 0;
        StartCoroutine(CameraSwitch());
    }
    IEnumerator CameraSwitch()
    {
        yield return new WaitForSeconds(waitsecondTime);
        subcamera6.Priority = 1;
        StartCoroutine(FItrue());
    }
    IEnumerator FItrue()
    {
        yield return new WaitForSeconds(waitsecondTime);
        fadeinout.fadeInFlag = true;
        GameManager.instance.GameClear();
    }


}


