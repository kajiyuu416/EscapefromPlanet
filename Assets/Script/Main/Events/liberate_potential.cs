using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Assertions.Must;

//コライダーに接触している状態で特定のボタン入力があった際に
//イベントを発生させ、プレイヤーへ能力付与のフラグを返す
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
        if (collision.CompareTag("Player") && Check.wasPressedThisFrame && !GameManager2.additionPlayerActionGetFlag)
        {
            if (!actionFlag)
            {
                OnCheck();
                Invoke("GiveOverJump",Const.CO.const_Float_List[1]);
            }
        }
        if(collision.CompareTag("Player") && Check.wasPressedThisFrame && GameManager2.additionPlayerActionGetFlag && !GameManager2.floatPowerGetFlag)
        {
            if (!actionFlag)
            {
                OnCheck();
                Invoke("GiveFloat", Const.CO.const_Float_List[1]);
            }
        }
        if(collision.CompareTag("Player"))
        {
            ActionPop.text = "LBボタン入力でアクション行う";
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActionPop.text = "LBボタン入力でアクション行う";
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
        SkipText.text = "Bボタンでスキップ";
    }
    //フラグを返す、UIの非表示
    private void OnClose()
    {
        var current_GP = Gamepad.current;
        var close = current_GP.buttonEast;
        if(actionFlag && close.wasPressedThisFrame && !GameManager2.additionPlayerActionGetFlag)
        {
            GameManager2.additionPlayerActionGetFlag = true;
            AE.SetActive(false);
            StartCoroutine(GameManager.Standbytime());
            not_applicable();
            actionFlag = false;
            SkipText.text = "";
            ActionPop.text = "";
        }
        else if(actionFlag && close.wasPressedThisFrame && GameManager2.additionPlayerActionGetFlag && !GameManager2.floatPowerGetFlag)
        {
            GameManager2.floatPowerGetFlag = true;
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
        if(!GameManager2.additionPlayerActionGetFlag)
        {
            SkipText.text = "";
            AE.SetActive(true);
        }
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE11();
    }
    private void GiveFloat()
    {
        if(!GameManager2.floatPowerGetFlag)
        {
            SkipText.text = "";
            AE.SetActive(true);
        }
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE11();
    }
    //特定のフラグが返っている時、リスト内のコライダー、メッシュを非表示にする
    private void not_applicable()
    {
        if(GameManager2.additionPlayerActionGetFlag)
        {
            BoxColList[0].enabled = false;
            MeshRenList[0].enabled = false;
        }

        if(GameManager2.floatPowerGetFlag)
        {
            BoxColList[Const.CO.const_Int_List[0]].enabled = false;
            MeshRenList[Const.CO.const_Int_List[0]].enabled = false;
        }
    }
}

