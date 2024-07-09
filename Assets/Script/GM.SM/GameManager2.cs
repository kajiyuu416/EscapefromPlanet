using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class GameManager2 : MonoBehaviour
{
    [SerializeField] GameObject settingBG;
    [SerializeField] GameObject bgmSlinder;
    [SerializeField] GameObject seSlinder;
    [SerializeField] GameObject rotationSensitivitySlinder;
    [SerializeField] GameObject uiButton;
    [SerializeField] GameObject camera_Up_Down_FlipButton;
    [SerializeField] GameObject camera_left_and_right_FlipButton;
    [SerializeField] Text ui_on_off_text;
    [SerializeField] Text camera_Up_down_text;
    [SerializeField] Text camera_Flip_left_and_right_text;
    [SerializeField] Slider sensitivitySlinder;

    public static bool additionPlayerActionGetFlag;
    public static bool floatPowerGetFlag;
    public static bool ALF;
    public static bool connect;
    public static bool uion_off_button;
    public static bool camera_Upside_down;
    public static bool camera_Flip_left_and_right;
    public static GameManager2 instance;
    private bool firstLoadFlag;
    private bool settingFlag;
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
        uion_off_button = true;
        camera_Upside_down = false;
        camera_Flip_left_and_right = false;
        StartCoroutine(FadeIn());
        beforeScene = "title";
        countdown = timer;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SetRotationSensitivity(sensitivitySlinder.value);
        sensitivitySlinder.onValueChanged.AddListener(SetRotationSensitivity);
    }
    public void SetRotationSensitivity(float volume)
    {
        PlayerCamera.rotationSensitivity = sensitivitySlinder.value * 200.0f;
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

        if(settingFlag)
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
        settingBG.SetActive(true);
        bgmSlinder.SetActive(true);
        seSlinder.SetActive(true);
        rotationSensitivitySlinder.SetActive(true);
        uiButton.SetActive(true);
        camera_Up_Down_FlipButton.SetActive(true);
        camera_left_and_right_FlipButton.SetActive(true);
        settingFlag = true;
        EventSystem.current.SetSelectedGameObject(uiButton);
    }
    //設定を閉じたとき
    public void SettingCl()
    {
        settingBG.SetActive(false);
        bgmSlinder.SetActive(false);
        seSlinder.SetActive(false);
        rotationSensitivitySlinder.SetActive(false);
        uiButton.SetActive(false);
        camera_Up_Down_FlipButton.SetActive(false);
        camera_left_and_right_FlipButton.SetActive(false);
        settingFlag = false;
    }
    //設定画面のON・OFF切り替え
    public void Push_Button_UIChange()
    {
        uion_off_button = !uion_off_button;

        if (uion_off_button == true)
        {
            ui_on_off_text.text = "ON";
            ui_on_off_text.color = Color.red;
        }
        else if((uion_off_button == false))
        {
            ui_on_off_text.text = "OFF";
            ui_on_off_text.color = Color.blue;
        }
    }
    public void Push_Button_Camera_Up_Down_Change()
    {
        camera_Upside_down = !camera_Upside_down;

        if(camera_Upside_down == true)
        {
            camera_Up_down_text.text = "ON";
            camera_Up_down_text.color = Color.red;
        }
        else if((camera_Upside_down == false))
        {
            camera_Up_down_text.text = "OFF";
            camera_Up_down_text.color = Color.blue;
        }
    }
    public void Push_Button_Camera_Left_Right_Change()
    {
        camera_Flip_left_and_right = !camera_Flip_left_and_right;

        if(camera_Flip_left_and_right == true)
        {
            camera_Flip_left_and_right_text.text = "ON";
            camera_Flip_left_and_right_text.color = Color.red;
        }
        else if((camera_Flip_left_and_right == false))
        {
            camera_Flip_left_and_right_text.text = "OFF";
            camera_Flip_left_and_right_text.color = Color.blue;
        }
    }
    //徐々にフェードアウトしていき完全に暗くなるとシーンをロードする
    public IEnumerator FadeOut(string sceneName)
    {
        var color = blackScreen.color;
        while (color.a <= 1.0f)
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
        yield return new WaitForSeconds(1.0f);

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

            if(Cansel.wasPressedThisFrame && settingFlag)
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
