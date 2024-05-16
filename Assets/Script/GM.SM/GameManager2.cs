using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameManager2 : MonoBehaviour
{
    [SerializeField] GameObject SettingBG;
    [SerializeField] GameObject BGMSlinder;
    [SerializeField] GameObject SESlinder;
    [SerializeField] GameObject UIButton;
    [SerializeField] Text on_off_text;
    public static bool AGF;
    public static bool FGF;
    public static bool ALF;
    public static bool connect;
    public static bool on_off_button;
    public static bool ActionUIFlag;
    public static GameManager2 instance;
    public bool firstLoadFlag;
    private string beforeScene;
    private string nowSceneName = "title";
    private Image blackScreen;
    private bool SettingFlag;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    //ゲームスタート時のオブジェクト非表示
    //徐々に画面が明るくなる処理
    private void Start()
    {
        SettingBG.SetActive(false);
        BGMSlinder.SetActive(false);
        SESlinder.SetActive(false);
        UIButton.gameObject.SetActive(false);
        on_off_button = true;
        StartCoroutine(FadeIn());
        beforeScene = "title";
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    //コントローラーの接続確認
    //マウスの非表示、現在シーンの保存
  　//設定画面の表示非表示
    private void Update()

    {
        GamePad_connection_Check();

        if(connect)
        {
            var current_GP = Gamepad.current;
            var Cansel = current_GP.buttonEast;

            if(Cansel.wasPressedThisFrame && SettingFlag)
            {
                SettingCl();
            }
        }

        if (SceneManager.GetActiveScene().name != nowSceneName)
        {
            nowSceneName = SceneManager.GetActiveScene().name;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    
    }

    //ゲームスタート時の設定確認、初回のロードのみOPシーンへ以降
    public static void GameStart()
    {
        if (GameManager2.on_off_button)
        {
            GameManager2.ActionUIFlag = true;
        }
        else
        { 
            GameManager2.ActionUIFlag = false;
        }

        if (instance.firstLoadFlag == false)
        {
            instance.StartCoroutine(instance.LoadScene("OpScene"));
            SoundManager SM = SoundManager.Instance;
            SM.SettingPlaySE();
        }
        else
        {
            instance.StartCoroutine(instance.LoadScene("MainScene"));
            SoundManager SM = SoundManager.Instance;
            SM.SettingPlaySE();
        }

    }
    public static void Setting()
    {
        instance.SettingOp();
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE();
    }

    public static void EndGame()
    {
        Application.Quit();
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE();
    }
    //設定を開いたとき
    public void SettingOp()
    {
        Debug.Log("Settingop");
        SettingBG.SetActive(true);
        BGMSlinder.SetActive(true);
        SESlinder.SetActive(true);
        UIButton.gameObject.SetActive(true);
        SettingFlag = true;
        EventSystem.current.SetSelectedGameObject(UIButton);
    }
    //設定を閉じたとき
    public void SettingCl()
    {
        Debug.Log("Settingcl");
        SettingBG.SetActive(false);
        BGMSlinder.SetActive(false);
        SESlinder.SetActive(false);
        UIButton.gameObject.SetActive(false);
        SettingFlag = false;
    }
    //設定画面のON・OFF切り替え
    public void Push_Button_Change()
    {
        on_off_button = !on_off_button;

        if (on_off_button == true)
        {
            on_off_text.text = "ON";
            ActionUIFlag = true;
        }
        else if((on_off_button == false))
        {
            on_off_text.text = "OFF";
            ActionUIFlag = false;
        }
    }
    //徐々にフェードアウトしていき完全に暗くなるとシーンをロードする
    public IEnumerator LoadScene(string sceneName)
    {
        var color = blackScreen.color;
        while (color.a <= 1)
        {
            color += new Color(0, 0, 0, 0.05f);
            blackScreen.color = color;

            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
    //徐々に明るくなる処理
    public IEnumerator FadeIn()
    {
        blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        var color = blackScreen.color;
        yield return new WaitForSeconds(1f);

        while (color.a >= 0)
        {
            color -= new Color(0, 0, 0, 0.05f);
            blackScreen.color = color;

            yield return null;
        }
    }
    //コントローラーの接続チェック
    private void GamePad_connection_Check()
    {
        var controllerNames = Input.GetJoystickNames();
        Debug.Log("コントローラー接続状態" + connect);
        if(controllerNames[0] == "")
        {
            connect = false;
        }
        else
        {
            connect = true;
        }

    }
    //シーンに応じて音楽の再生、停止を行う
    public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
    {
        // Scene1からScene2へ
        if (beforeScene == "title" && nextScene.name == "OPScene")
        {
            SoundManager SM = SoundManager.Instance;
            SM.StopAudio();
        }
        //Scene2からScene3へ
        if (beforeScene == "OPScene" && nextScene.name == "MainScene")
        {
            SoundManager SM = SoundManager.Instance;
            SM.StopAudio();
            SM.Startbgm2();
        }
        // Scene3からScene1へ
        if (beforeScene == "MainScene" && nextScene.name == "title")
        {
            SoundManager SM = SoundManager.Instance;
            SM.StopAudio();
            SM.Startbgm1();
        }
        // Scene1からScene3へ
        if (beforeScene == "title" && nextScene.name == "MainScene")
        {
            SoundManager SM = SoundManager.Instance;
            SM.StopAudio();
            SM.Startbgm2();
        }
        // Scene3からScene3へ
        if (beforeScene == "MainScene" && nextScene.name == "MainScene")
        {
            SoundManager SM = SoundManager.Instance;
            SM.StopAudio();
            SM.Startbgm2();
        }

        //遷移後のシーン名を「１つ前のシーン名」として保持
        beforeScene = nextScene.name;
    }

}
