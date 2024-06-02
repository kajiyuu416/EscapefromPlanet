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
    [SerializeField] GameObject RotationSensitivitySlinder;
    [SerializeField] GameObject UIButton;
    [SerializeField] GameObject Camera_Up_Down_FlipButton;
    [SerializeField] GameObject Camera_left_and_right_FlipButton;
    [SerializeField] Text UI_on_off_text;
    [SerializeField] Text Camera_Up_down_text;
    [SerializeField] Text Camera_Flip_left_and_right_text;
    [SerializeField] Slider SensitivitySlinder;

    public static bool AGF;
    public static bool FGF;
    public static bool ALF;
    public static bool connect;
    public static bool UIon_off_button;
    public static bool Camera_Upside_down;
    public static bool Camera_Flip_left_and_right;
    public static GameManager2 instance;
    private bool firstLoadFlag;
    private bool SettingFlag;
    private bool loadDemoScene;
    private string beforeScene;
    private string nowSceneName = "title";
    private Image blackScreen;

    private const float timer = 20.0f;
    private float countdown;
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
        SettingCl();
        UIon_off_button = true;
        Camera_Upside_down = false;
        Camera_Flip_left_and_right = false;
        StartCoroutine(FadeIn());
        beforeScene = "title";
        countdown = timer;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SetRotationSensitivity(SensitivitySlinder.value);
        SensitivitySlinder.onValueChanged.AddListener(SetRotationSensitivity);
    }
    public void SetRotationSensitivity(float volume)
    {
        PlayerCamera.RotationSensitivity = SensitivitySlinder.value * 200.0f;
    }
    //コントローラーの接続確認
    //マウスの非表示、現在シーンの保存
    //設定画面の表示非表示
    //設定画面表示時は、DemoSceneに移行しないようにリターンし処理の制御
    private void Update()
    {
        GamePad_connection_Check();
        if(SceneManager.GetActiveScene().name != nowSceneName)
        {
            nowSceneName = SceneManager.GetActiveScene().name;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if(SettingFlag)
        {
            return;
        }

        if(beforeScene == "title")
        {
            countdown -= Time.deltaTime;

            if(countdown <= 0 &&!loadDemoScene)
            {
                countdown = 0;
                loadDemoScene = true;
                instance.StartCoroutine(instance.FadeOut("DemoScene"));
            }
        }
    }

    //ゲームスタート時の設定確認、初回のロードのみOPシーンへ以降
    public static void GameStart()
    {
        if (instance.firstLoadFlag == false)
        {
            instance.StartCoroutine(instance.FadeOut("OpScene"));
            SoundManager SM = SoundManager.Instance;
            SM.SettingPlaySE();
        }
        else
        {
            instance.StartCoroutine(instance.FadeOut("MainScene"));
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
        SettingBG.SetActive(true);
        BGMSlinder.SetActive(true);
        SESlinder.SetActive(true);
        RotationSensitivitySlinder.SetActive(true);
        UIButton.SetActive(true);
        Camera_Up_Down_FlipButton.SetActive(true);
        Camera_left_and_right_FlipButton.SetActive(true);
        SettingFlag = true;
        EventSystem.current.SetSelectedGameObject(UIButton);
    }
    //設定を閉じたとき
    public void SettingCl()
    {
        SettingBG.SetActive(false);
        BGMSlinder.SetActive(false);
        SESlinder.SetActive(false);
        RotationSensitivitySlinder.SetActive(false);
        UIButton.SetActive(false);
        Camera_Up_Down_FlipButton.SetActive(false);
        Camera_left_and_right_FlipButton.SetActive(false);
        SettingFlag = false;
    }
    //設定画面のON・OFF切り替え
    public void Push_Button_UIChange()
    {
        UIon_off_button = !UIon_off_button;

        if (UIon_off_button == true)
        {
            UI_on_off_text.text = "ON";
            UI_on_off_text.color = Color.red;
        }
        else if((UIon_off_button == false))
        {
            UI_on_off_text.text = "OFF";
            UI_on_off_text.color = Color.blue;
        }
    }
    public void Push_Button_Camera_Up_Down_Change()
    {
        Camera_Upside_down = !Camera_Upside_down;

        if(Camera_Upside_down == true)
        {
            Camera_Up_down_text.text = "ON";
            Camera_Up_down_text.color = Color.red;
        }
        else if((Camera_Upside_down == false))
        {
            Camera_Up_down_text.text = "OFF";
            Camera_Up_down_text.color = Color.blue;
        }
    }
    public void Push_Button_Camera_Left_Right_Change()
    {
        Camera_Flip_left_and_right = !Camera_Flip_left_and_right;

        if(Camera_Flip_left_and_right == true)
        {
            Camera_Flip_left_and_right_text.text = "ON";
            Camera_Flip_left_and_right_text.color = Color.red;
        }
        else if((Camera_Flip_left_and_right == false))
        {
            Camera_Flip_left_and_right_text.text = "OFF";
            Camera_Flip_left_and_right_text.color = Color.blue;
        }
    }
    //徐々にフェードアウトしていき完全に暗くなるとシーンをロードする
    public IEnumerator FadeOut(string sceneName)
    {
        var color = blackScreen.color;
        while (color.a <= Const.CO.Const_Float_List[0])
        {
            color.a += 0.1f; 
            blackScreen.color = color;

            yield return null;
        }
        SceneManager.LoadSceneAsync(sceneName).allowSceneActivation = true;
    }
    //徐々に明るくなる処理
    public IEnumerator FadeIn()
    {
        blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        var color = blackScreen.color;
        yield return new WaitForSeconds(Const.CO.Const_Float_List[0]);

        while (color.a >= 0)
        {
            color.a -= 0.1f;
            blackScreen.color = color;

            yield return null;
        }
    }
    //コントローラーの接続チェック
    private void GamePad_connection_Check()
    {
        var controllerNames = Input.GetJoystickNames();
        if(controllerNames[0] == "")
        {
            connect = false;
        }
        else 
        {
            connect = true;
        }

        if(connect)
        {
            var current_GP = Gamepad.current;
            var Cansel = current_GP.buttonEast;

            if(Cansel.wasPressedThisFrame && SettingFlag)
            {
                SettingCl();
            }
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
            countdown = timer;
            loadDemoScene = false;


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
        if(beforeScene == "title" && nextScene.name == "DemoScene")
        {
            SoundManager SM = SoundManager.Instance;
            SM.StopAudio();
        }
        if(beforeScene == "DemoScene" && nextScene.name == "title")
        {
            StartCoroutine(FadeIn());
            SoundManager SM = SoundManager.Instance;
            SM.StopAudio();
            SM.Startbgm1();
            countdown = timer;
            loadDemoScene = false;
        }
        //遷移後のシーン名を「１つ前のシーン名」として保持
        beforeScene = nextScene.name;
    }
    public bool Duplicate_firstLoadFlag
    {
        get
        {
            return firstLoadFlag;
        }
        set
        {
            firstLoadFlag = value;
        }
    }
}
