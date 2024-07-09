using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Assertions.Must;

//コライダーに接触している状態で特定のボタン入力があった際に
//イベントを発生させ、プレイヤーへ能力付与のフラグを返す
public class liberate_potential: MonoBehaviour
{
    [SerializeField] TextMeshProUGUI actionPop;
    [SerializeField] TextMeshProUGUI actiontmessage;
    [SerializeField] TextMeshProUGUI skipText;
    private const string text1 = "LBボタン入力でアクション行う";
    private const string text2 = "Bボタンでスキップ";
    [SerializeField] SpawnEffect spawneffect;
    [SerializeField] GameObject actionimage;
    [SerializeField] List<BoxCollider> boxColList;
    [SerializeField] List<MeshRenderer>meshRenList;
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
                Invoke("GiveOverJump",2.0f);
            }
        }
        if(collision.CompareTag("Player") && Check.wasPressedThisFrame && GameManager2.additionPlayerActionGetFlag && !GameManager2.floatPowerGetFlag)
        {
            if (!actionFlag)
            {
                OnCheck();
                Invoke("GiveFloat", 2.0f);
            }
        }
        if(collision.CompareTag("Player"))
        {
            actionPop.text = text1;
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            actionPop.text = text1;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            actionPop.text = "";
        }
    }
    private void OnCheck()
    {
        GameManager.pauseflag = true;
        SoundManager.Instance.SettingPlaySE12();
        spawneffect.enabled = true;
        actionFlag = true;
        actionPop.text = "";
        skipText.text = text2;
    }
    //フラグを返す、UIの非表示
    private void OnClose()
    {
        var current_GP = Gamepad.current;
        var close = current_GP.buttonEast;
        if(actionFlag && close.wasPressedThisFrame && !GameManager2.additionPlayerActionGetFlag)
        {
            GameManager2.additionPlayerActionGetFlag = true;
            actionimage.SetActive(false);
            StartCoroutine(GameManager.Standbytime());
            not_applicable();
            actionFlag = false;
            skipText.text = "";
            actionPop.text = "";
        }
        else if(actionFlag && close.wasPressedThisFrame && GameManager2.additionPlayerActionGetFlag && !GameManager2.floatPowerGetFlag)
        {
            GameManager2.floatPowerGetFlag = true;
            actionimage.SetActive(false);
            StartCoroutine(GameManager.Standbytime());
            not_applicable();
            actionFlag = false;
            skipText.text = "";
            actionPop.text = "";
        }
    }
    private void GiveOverJump()
    {
        if(!GameManager2.additionPlayerActionGetFlag)
        {
            skipText.text = "";
            actionimage.SetActive(true);
        }
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE11();
    }
    private void GiveFloat()
    {
        if(!GameManager2.floatPowerGetFlag)
        {
            skipText.text = "";
            actionimage.SetActive(true);
        }
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE11();
    }
    //特定のフラグが返っている時、リスト内のコライダー、メッシュを非表示にする
    private void not_applicable()
    {
        if(GameManager2.additionPlayerActionGetFlag)
        {
            boxColList[0].enabled = false;
            meshRenList[0].enabled = false;
        }

        if(GameManager2.floatPowerGetFlag)
        {
            boxColList[1].enabled = false;
            meshRenList[1].enabled = false;
        }
    }
}

