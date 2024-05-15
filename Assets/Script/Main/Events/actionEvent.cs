using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.InputSystem;
public class actionEvent : MonoBehaviour
{
    [SerializeField] GameObject DoorFlag;
    [SerializeField] GameObject ActionPoint;
    [SerializeField] TextMeshProUGUI ActionPop;
    [SerializeField] GameObject moveiSC;
    [SerializeField] VideoPlayer videoPlayer;
    public static bool actionFlag = false;
    private void Start()
    {
        videoPlayer.loopPointReached += LoopPointReached;
    }
    private void Update()
    {
        if(actionFlag)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        var current_GP = Gamepad.current;
        var Check = current_GP.leftShoulder;
        if (collision.CompareTag("Player") && Check.wasPressedThisFrame &&!actionFlag)
        {
            actionFlag = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            GameManager.pauseflag = true;
            GameManager.instance.ActionUI1.SetActive(false);
            GameManager.instance.ActionUI2.SetActive(false);
            moveiSC.SetActive(true);
            ActionPop.text = "";
            SoundManager.Instance.StopAudio();
            SoundManager.Instance.SettingPlaySE5();
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("�A�N�V�����|�b�v�\��");
            ActionPop.text = "LB�{�^�����͂ŃA�N�V�����s��";
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("�A�N�V�����|�b�v��\��");
            ActionPop.text = "";
        }
    }

    private void MessageIndication()
    {
        ActionPop.text = "���̃��b�N�����ɐ���";
        StartCoroutine("SetText");
    }
    IEnumerator SetText()
    {
        yield return new WaitForSeconds(3.0f);
        ActionPop.text = "";
    }
    // ����Đ��������̏���
    public void LoopPointReached(VideoPlayer vp)
    {
        MessageIndication();
        moveiSC.SetActive(false);
        GameManager.pauseflag = false;
        GameManager.instance.ActionUI1.SetActive(true);
        GameManager.instance.ActionUI2.SetActive(true);
        SoundManager.Instance.Startbgm2();
        SoundManager.Instance.SettingPlaySE6();
    }
}
