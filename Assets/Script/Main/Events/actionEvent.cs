using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.InputSystem;

//�R���C�_�[�ɐڐG���Ă����Ԃœ���̃{�^�����͂��������ۂ�
//�C�x���g�𔭐������A�f���𗬂������B
public class actionEvent : MonoBehaviour
{
    [SerializeField] GameObject ActionPoint;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] BoxCollider boxCollider;
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
            ActionPoint.SetActive(false);
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        var current_GP = Gamepad.current;
        var Check = current_GP.leftShoulder;
        if (collision.CompareTag("Player") && Check.wasPressedThisFrame &&!actionFlag)
        {
            actionFlag = true;
            GameManager.pauseflag = true;
            GameManager.instance.playerui.SetActive(false);
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

    private void MessageIndication()
    {
        ActionPop.text = "���̃��b�N�����ɐ���";
        StartCoroutine("SetText");
    }
    IEnumerator SetText()
    {
        yield return new WaitForSeconds(Const.CO.const_Float_List[2]);
        ActionPop.text = "";
    }
    // ����Đ��������̏���
    public void LoopPointReached(VideoPlayer vp)
    {
        MessageIndication();
        moveiSC.SetActive(false);
        GameManager.pauseflag = false;
        GameManager.instance.playerui.SetActive(true);
        SoundManager.Instance.Startbgm2();
        SoundManager.Instance.SettingPlaySE6();
    }
}
