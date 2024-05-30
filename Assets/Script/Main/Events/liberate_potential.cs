using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Assertions.Must;

//�R���C�_�[�ɐڐG���Ă����Ԃœ���̃{�^�����͂��������ۂ�
//�C�x���g�𔭐������A�v���C���[�֔\�͕t�^�̃t���O��Ԃ�
public class liberate_potential: MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ActionPop;
    [SerializeField] TextMeshProUGUI Actiontmessage;
    [SerializeField] TextMeshProUGUI SkipText;
    [SerializeField] SpawnEffect SE;
    [SerializeField] GameObject AE;
    [SerializeField] List<BoxCollider> BoxColList;
    [SerializeField] List<MeshRenderer>MeshRenList;
    public bool actionFlag;

    private void Update()
    {
        OnClose();
    }
    private void Awake()
    {
        not_applicable();
    }
    private void OnTriggerStay(Collider collision)
    {
        var current_GP = Gamepad.current;
        var Check = current_GP.leftShoulder;
        if (collision.CompareTag("Player") && Check.wasPressedThisFrame && !GameManager2.AGF)
        {
            if (!actionFlag)
            {
                OnCheck();
                Invoke("GiveOverJump",Const.CO.Const_Float_List[1]);
            }
        }
        if(collision.CompareTag("Player") && Check.wasPressedThisFrame && GameManager2.AGF && !GameManager2.FGF)
        {
            if (!actionFlag)
            {
                OnCheck();
                Invoke("GiveFloat", Const.CO.Const_Float_List[1]);
            }
        }
        if(collision.CompareTag("Player"))
        {
            ActionPop.text = "LB�{�^�����͂ŃA�N�V�����s��";
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
        if(collision.CompareTag("Player"))
        {
            ActionPop.text = "";
        }
    }
    private void OnCheck()
    {
        GameManager.pauseflag = true;
        SoundManager.Instance.SettingPlaySE12();
        SE.enabled = true;
        actionFlag = true;
        ActionPop.text = "";
        SkipText.text = "B�{�^���ŃX�L�b�v";
    }
    //�t���O��Ԃ��AUI�̔�\��
    private void OnClose()
    {
        var current_GP = Gamepad.current;
        var close = current_GP.buttonEast;
        if(actionFlag && close.wasPressedThisFrame && !GameManager2.AGF)
        {
            GameManager2.AGF = true;
            AE.SetActive(false);
            StartCoroutine(GameManager.Standbytime());
            not_applicable();
            actionFlag = false;
            SkipText.text = "";
            ActionPop.text = "";
        }
        else if(actionFlag && close.wasPressedThisFrame && GameManager2.AGF && !GameManager2.FGF)
        {
            GameManager2.FGF = true;
            AE.SetActive(false);
            StartCoroutine(GameManager.Standbytime());
            not_applicable();
            actionFlag = false;
            SkipText.text = "";
            ActionPop.text = "";
        }
    }
    private void GiveOverJump()
    {
        if(!GameManager2.AGF)
        {
            SkipText.text = "";
            AE.SetActive(true);
        }
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE11();
    }
    private void GiveFloat()
    {
        if(!GameManager2.FGF)
        {
            SkipText.text = "";
            AE.SetActive(true);
        }
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE11();
    }
    //����̃t���O���Ԃ��Ă��鎞�A���X�g���̃R���C�_�[�A���b�V�����\���ɂ���
    private void not_applicable()
    {
        if(GameManager2.AGF)
        {
            BoxColList[0].enabled = false;
            MeshRenList[0].enabled = false;
        }

        if(GameManager2.FGF)
        {
            BoxColList[Const.CO.Const_Int_List[0]].enabled = false;
            MeshRenList[Const.CO.Const_Int_List[0]].enabled = false;
        }
    }
}

