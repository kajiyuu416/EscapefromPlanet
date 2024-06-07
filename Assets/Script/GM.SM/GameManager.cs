using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverobj;
    [SerializeField] GameObject pauseobj;
    [SerializeField] GameObject idle;
    [SerializeField] GameObject maincamera;
    [SerializeField] GameObject settingButton;
    [SerializeField] GameObject warningimage;
    [SerializeField] GameObject gameClearTextobj;
    [SerializeField] GameObject restrictionImage1;
    [SerializeField] GameObject restrictionImage2;
    [SerializeField] GameObject ui;
    [SerializeField] public GameObject playerui;
    [SerializeField] public GameObject playeractionui1;
    [SerializeField] public GameObject playeractionui2;
    [SerializeField] AdditionPlayerAction additionPlayerAction;
    [SerializeField] FloatPowerSC floatPowerSC;
    [SerializeField] Text gameOverCount;
    public static GameManager instance;
    public static bool pauseflag;
    public static int count = 0;
    private bool restartFlag;
    private bool gameOverFlag = false;
    private bool gameClearFlag = false;
    private bool settingOPflag;
    private bool stbflag;

    private void Update()
    {
        Pause();
        GivePower();
        UIcheck();
        GameFlagcheck();
    }

    //ゲームオーバー回数のカウント
    //フレームレート固定
    private void Awake()
    {
        Application.targetFrameRate = 60;
        pauseflag = true;
        gameOverCount.text = count.ToString();
        Invoke(nameof(Standby), Const.CO.const_Float_List[3]);
        if (instance == null)
        {
            instance = this;
        }
    }
    //pause画面に以降時のオブジェクト表示・非表示、プレイヤー動き制御
    private void Pause()
    {
        var current_GP = Gamepad.current;
        var Cansel = current_GP.buttonEast;
        var Setting = current_GP.startButton;

        if (Setting.wasPressedThisFrame && !pauseflag && stbflag)
        {
            pauseflag = true;
            settingOPflag = true;
            pauseobj.SetActive(true);
            warningimage.SetActive(false);
            idle.GetComponent<Animator>().speed = 0;
            EventSystem.current.SetSelectedGameObject(settingButton);
          　SoundManager.Instance.SettingPlaySE4();
        }
        if (Cansel.wasPressedThisFrame && settingOPflag )
        {
            settingOPflag = false;
            pauseobj.SetActive(false);
            idle.GetComponent<Animator>().speed = Const.CO.const_Float_List[0];
            StartCoroutine(Standbytime());
        }

    }
    //static bool型のflagが返っているかチェック
    private void GivePower()
    {
        if (GameManager2.additionPlayerActionGetFlag)
        {
            Destroy(restrictionImage1);
        }
        if (GameManager2.floatPowerGetFlag)
        {
            Destroy(restrictionImage2);
        }
    }
    //ゲームクリア時に呼ばれる関数
    public  void GameClear()
    {
        gameClearFlag = true;
        gameClearTextobj.SetActive(true);
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE15();
    }
    //ゲームオーバー時に呼ばれる関数
    private void GameOver()
    {
        gameOverobj.SetActive(true);
        warningimage.SetActive(false);
        playerui.SetActive(false);
        pauseflag = true;
        stbflag = false;
        SoundManager.Instance.StopAudio();
        SoundManager.Instance.SettingPlaySE2();
        SoundManager.Instance.SettingPlaySE9();
        StartCoroutine(ReStartThiScene());
        count++;
    }
    public void RestartFlagOn()
    {
        restartFlag = true;
    }

    //タイトル画面に戻る際呼ばれる関数
    private void BacktoTitle()
    {
        playerui.SetActive(false);
        SceneManager.LoadScene(0);
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE();
    }
    //特定のアクションが終了するまでのプレイヤー制御
    //5フレーム後にフラグを返す
    public static IEnumerator Standbytime()
    {
        for(var i = 0; i < Const.CO.const_Int_List[4]; i++)
        {
            yield return null;
        }
        pauseflag = false;
    }

   public void Standby()
    {
        stbflag = true;
        pauseflag = false;
    }

    //リスタート時に呼ばれる関数
    //ゲームオーバー後3秒後にリスタート時に呼ばれる関数
    private IEnumerator ReStartThiScene()
    {
        yield return new WaitForSeconds(Const.CO.const_Int_List[2]);
        SoundManager.Instance.StopAudio();
        yield return GameManager2.instance.FadeOut("MainScene");
    }
    //ゲームオーバー時の条件チェック
    //UI表示のON、OFFチェック
    private void UIcheck()
    {
        if(GameManager2.uion_off_button && !ui.activeSelf)
        {
            ui.SetActive(true);
        }
        else if(!GameManager2.uion_off_button && ui.activeSelf)
        {
            ui.SetActive(false);
        }
    }
    private void GameFlagcheck()
    {
        if(GameManager2.connect)
        {
            var current_GP = Gamepad.current;
            var Cansel = current_GP.buttonEast;
            if(gameClearFlag && Cansel.wasPressedThisFrame)
            {
                BacktoTitle();
                gameClearFlag = false;
            }
        }

        if(!gameOverFlag && restartFlag)
        {
            GameOver();
            gameOverFlag = true;
        }

        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            EventSystem.current.SetSelectedGameObject(settingButton);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}
