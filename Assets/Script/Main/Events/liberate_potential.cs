using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

//�R���C�_�[�ɐڐG���Ă����Ԃœ���̃{�^�����͂��������ۂ�
//�C�x���g�𔭐������A�v���C���[�֔\�͕t�^�̃t���O��Ԃ�
public class liberate_potential: MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ActionPop;
    [SerializeField] TextMeshProUGUI Actiontmessage;
    [SerializeField] TextMeshProUGUI SkipText;
    [SerializeField] SpawnEffect SE;
    [SerializeField] GameObject AE;
    private MeshRenderer meshRenderer;
    public bool actionFlag;

    private void Update()
    {
        OnClose();
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
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        actionFlag = true;
        ActionPop.text = "";
        SkipText.text = "B�{�^���ŃX�L�b�v";
        SE.enabled = true;
    }
    private void OnClose()
    {
            var current_GP = Gamepad.current;
            var close = current_GP.buttonEast;
            if(actionFlag && close.wasPressedThisFrame && !GameManager2.AGF)
            {
                GameManager2.AGF = true;
                GameManager.pauseflag = false;
                actionFlag = false;
                AE.SetActive(false);
                SkipText.text = "";
                ActionPop.text = "";
            }
            else if(actionFlag && close.wasPressedThisFrame && GameManager2.AGF && !GameManager2.FGF)
            {
                GameManager2.FGF = true;
                GameManager.pauseflag = false;
                actionFlag = false;
                AE.SetActive(false);
                SkipText.text = "";
                ActionPop.text = "";
            }
    }
    private void GiveOverJump()
    {
        if(!GameManager2.AGF)
        {
            AE.SetActive(true);
        }
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE11();
    }
    private void GiveFloat()
    {
        if(!GameManager2.FGF)
        {
            AE.SetActive(true);
        }
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE11();
    }
}

