using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject GameOverTextobj;
    [SerializeField] GameObject GameOverBackground;
    [SerializeField] GameObject PauseTextobj;
    [SerializeField] GameObject PauseBackground;
    [SerializeField] GameObject Idle;
    [SerializeField] GameObject MC;
    [SerializeField] GameObject SettingButton;
    [SerializeField] GameObject warningimage;
    [SerializeField] GameObject GameClearTextobj;
    [SerializeField] GameObject restrictionImage1;
    [SerializeField] GameObject restrictionImage2;
    [SerializeField] public GameObject ActionUI;
    [SerializeField] public GameObject ActionUI1;
    [SerializeField] public GameObject ActionUI2;
    [SerializeField] AdditionPlayerAction AA;
    [SerializeField] FloatPowerSC FP;
    [SerializeField] Text GOCount;
    public static GameManager instance;
    public static bool pauseflag;
    public static int count = 0;
    private bool RestartFlag;
    private bool GameOverFlag;
    private bool GameClearFlag;
    private bool SettingOPflag;
    private bool stbflag;

    //ゲームオーバー時の条件チェック
    //UI表示のON、OFFチェック
    private void Update()
    {
        if(GameManager2.connect)
        {
            Pause();
            var current_GP = Gamepad.current;
            var Cansel = current_GP.buttonEast;
            if(GameClearFlag)
            {
                if(Cansel.wasPressedThisFrame)
                {
                    SceneManager.LoadScene("title");
                    GameClearFlag = false;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            EventSystem.current.SetSelectedGameObject(SettingButton);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(GameManager2.UIon_off_button)
        {
           ActionUI.SetActive(true);
        }
        else if(!GameManager2.UIon_off_button)
        {
           ActionUI.SetActive(false);
        }

        if (!GameOverFlag && RestartFlag)
        {
            GameOver();
            GameOverFlag = true;
        }
        GivePower();
    }

    //ゲームオーバー回数のカウント
    private void Awake()
    {
        Application.targetFrameRate = 60;
        pauseflag = true;
        GameOverFlag = false;
        GOCount.text = count.ToString();
        Invoke("Standbytime",Const.CO.Const_Float_List[3]);

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
            PauseTextobj.SetActive(true);
            PauseBackground.SetActive(true);
            warningimage.SetActive(false);
            Idle.GetComponent<Animator>().speed = 0;
            EventSystem.current.SetSelectedGameObject(SettingButton);
          　SoundManager.Instance.SettingPlaySE4();
        }
        if (Cansel.wasPressedThisFrame && SettingOPflag)
        {
            pauseflag = false;
            SettingOPflag = false;
            PauseTextobj.SetActive(false);
            PauseBackground.SetActive(false);
            Idle.GetComponent<Animator>().speed = Const.CO.Const_Float_List[0];
        }

    }
    //static bool型のflagが返っているかチェック
    private void GivePower()
    {
        if (GameManager2.AGF)
        {
            AdditionPlayerAction.AdditionPlayerActionFlag_OverJump = true;
            Destroy(restrictionImage1);
        }
        if (GameManager2.FGF)
        {
            FloatPowerSC.AdditionPlayerActionFlag_Float = true;
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
        GameOverTextobj.SetActive(true);
        GameOverBackground.SetActive(true);
        warningimage.SetActive(false);
        ActionUI1.SetActive(false);
        ActionUI2.SetActive(false);
        pauseflag = true;
        SoundManager.Instance.StopAudio();
        SoundManager.Instance.SettingPlaySE2();
        SoundManager.Instance.SettingPlaySE9();
        count++;
        Invoke("ReStartThiScene", Const.CO.Const_Float_List[3]);
    }
    public void RestartFlagOn()
    {
        RestartFlag = true;
    }
    //リスタート時に呼ばれる関数
    private void ReStartThiScene()
    {
        Scene ThisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(ThisScene.name);
        SoundManager.Instance.StopAudio();
    }
    //タイトル画面に戻る際呼ばれる関数
    private void BacktoTitle()
    {
        ActionUI1.SetActive(false);
        ActionUI2.SetActive(false);
        SceneManager.LoadScene("title");
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE();

    }
   　//リスタート時のプレイヤーの起き上がりアニメーションが終了するまでのプレイヤーの動き制御
    private void Standbytime()
    {
        stbflag = true;
        pauseflag = false;
    }

}
