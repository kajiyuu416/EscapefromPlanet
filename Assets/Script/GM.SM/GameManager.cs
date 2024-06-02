using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject GameOverobj;
    [SerializeField] GameObject Pauseobj;
    [SerializeField] GameObject Idle;
    [SerializeField] GameObject MC;
    [SerializeField] GameObject SettingButton;
    [SerializeField] GameObject warningimage;
    [SerializeField] GameObject GameClearTextobj;
    [SerializeField] GameObject restrictionImage1;
    [SerializeField] GameObject restrictionImage2;
    [SerializeField] GameObject UI;
    [SerializeField] public GameObject PlayerUI;
    [SerializeField] public GameObject PlayerActionUI1;
    [SerializeField] public GameObject PlayerActionUI2;
    [SerializeField] AdditionPlayerAction AA;
    [SerializeField] FloatPowerSC FP;
    [SerializeField] Text GOCount;
    public static GameManager instance;
    public static bool pauseflag;
    public static int count = 0;
    private bool RestartFlag;
    private bool GameOverFlag = false;
    private bool GameClearFlag = false;
    private bool SettingOPflag;
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
        GOCount.text = count.ToString();
        Invoke(nameof(Standby), Const.CO.Const_Float_List[3]);
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
            SettingOPflag = true;
            Pauseobj.SetActive(true);
            warningimage.SetActive(false);
            Idle.GetComponent<Animator>().speed = 0;
            EventSystem.current.SetSelectedGameObject(SettingButton);
          　SoundManager.Instance.SettingPlaySE4();
        }
        if (Cansel.wasPressedThisFrame && SettingOPflag )
        {
            SettingOPflag = false;
            Pauseobj.SetActive(false);
            Idle.GetComponent<Animator>().speed = Const.CO.Const_Float_List[0];
            StartCoroutine(Standbytime());
        }

    }
    //static bool型のflagが返っているかチェック
    private void GivePower()
    {
        if (GameManager2.AGF)
        {
            Destroy(restrictionImage1);
        }
        if (GameManager2.FGF)
        {
            Destroy(restrictionImage2);
        }
    }
    //ゲームクリア時に呼ばれる関数
    public  void GameClear()
    {
        GameClearFlag = true;
        GameClearTextobj.SetActive(true);
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE15();
    }
    //ゲームオーバー時に呼ばれる関数
    private void GameOver()
    {
        GameOverobj.SetActive(true);
        warningimage.SetActive(false);
        PlayerUI.SetActive(false);
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
        RestartFlag = true;
    }

    //タイトル画面に戻る際呼ばれる関数
    private void BacktoTitle()
    {
        PlayerUI.SetActive(false);
        SceneManager.LoadScene(0);
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE();
    }
    //特定のアクションが終了するまでのプレイヤー制御
    //5フレーム後にフラグを返す
    public static IEnumerator Standbytime()
    {
        for(var i = 0; i < Const.CO.Const_Int_List[4]; i++)
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
        yield return new WaitForSeconds(Const.CO.Const_Int_List[2]);
        SoundManager.Instance.StopAudio();
        yield return GameManager2.instance.FadeOut("MainScene");
    }
    //ゲームオーバー時の条件チェック
    //UI表示のON、OFFチェック
    private void UIcheck()
    {
        if(GameManager2.UIon_off_button && !UI.activeSelf)
        {
            UI.SetActive(true);
        }
        else if(!GameManager2.UIon_off_button && UI.activeSelf)
        {
            UI.SetActive(false);
        }
    }
    private void GameFlagcheck()
    {
        if(GameManager2.connect)
        {
            var current_GP = Gamepad.current;
            var Cansel = current_GP.buttonEast;
            if(GameClearFlag && Cansel.wasPressedThisFrame)
            {
                BacktoTitle();
                GameClearFlag = false;
            }
        }

        if(!GameOverFlag && RestartFlag)
        {
            GameOver();
            GameOverFlag = true;
        }

        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            EventSystem.current.SetSelectedGameObject(SettingButton);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}
