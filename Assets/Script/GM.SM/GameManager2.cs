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

    //�Q�[���X�^�[�g���̃I�u�W�F�N�g��\��
    //���X�ɉ�ʂ����邭�Ȃ鏈��
    private void Start()
    {
        SettingBG.SetActive(false);
        BGMSlinder.SetActive(false);
        SESlinder.SetActive(false);
        RotationSensitivitySlinder.SetActive(false);
        UIButton.SetActive(false);
        Camera_Up_Down_FlipButton.SetActive(false);
        Camera_left_and_right_FlipButton.SetActive(false);
        UIon_off_button = true;
        Camera_Upside_down = false;
        Camera_Flip_left_and_right = false;
        StartCoroutine(FadeIn());
        beforeScene = "title";
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SetRotationSensitivity(SensitivitySlinder.value);
        SensitivitySlinder.onValueChanged.AddListener(SetRotationSensitivity);
    }
    public void SetRotationSensitivity(float volume)
    {
        PlayerCamera.RotationSensitivity = SensitivitySlinder.value * 150.0f;
    }
    //�R���g���[���[�̐ڑ��m�F
    //�}�E�X�̔�\���A���݃V�[���̕ۑ�
    //�ݒ��ʂ̕\����\��
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

    //�Q�[���X�^�[�g���̐ݒ�m�F�A����̃��[�h�̂�OP�V�[���ֈȍ~
    public static void GameStart()
    {
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
    //�ݒ���J�����Ƃ�
    public void SettingOp()
    {
        Debug.Log("Settingop");
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
    //�ݒ������Ƃ�
    public void SettingCl()
    {
        Debug.Log("Settingcl");
        SettingBG.SetActive(false);
        BGMSlinder.SetActive(false);
        SESlinder.SetActive(false);
        RotationSensitivitySlinder.SetActive(false);
        UIButton.SetActive(false);
        Camera_Up_Down_FlipButton.SetActive(false);
        Camera_left_and_right_FlipButton.SetActive(false);
        SettingFlag = false;
    }
    //�ݒ��ʂ�ON�EOFF�؂�ւ�
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
    //���X�Ƀt�F�[�h�A�E�g���Ă������S�ɈÂ��Ȃ�ƃV�[�������[�h����
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
    //���X�ɖ��邭�Ȃ鏈��
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
    //�R���g���[���[�̐ڑ��`�F�b�N
    private void GamePad_connection_Check()
    {
        var controllerNames = Input.GetJoystickNames();
        Debug.Log("�R���g���[���[�ڑ����" + connect);
        if(controllerNames[0] == "")
        {
            connect = false;
        }
        else
        {
            connect = true;
        }

    }
    //�V�[���ɉ����ĉ��y�̍Đ��A��~���s��
    public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
    {
        // Scene1����Scene2��
        if (beforeScene == "title" && nextScene.name == "OPScene")
        {
            SoundManager SM = SoundManager.Instance;
            SM.StopAudio();
        }
        //Scene2����Scene3��
        if (beforeScene == "OPScene" && nextScene.name == "MainScene")
        {
            SoundManager SM = SoundManager.Instance;
            SM.StopAudio();
            SM.Startbgm2();
        }
        // Scene3����Scene1��
        if (beforeScene == "MainScene" && nextScene.name == "title")
        {
            SoundManager SM = SoundManager.Instance;
            SM.StopAudio();
            SM.Startbgm1();
        }
        // Scene1����Scene3��
        if (beforeScene == "title" && nextScene.name == "MainScene")
        {
            SoundManager SM = SoundManager.Instance;
            SM.StopAudio();
            SM.Startbgm2();
        }
        // Scene3����Scene3��
        if (beforeScene == "MainScene" && nextScene.name == "MainScene")
        {
            SoundManager SM = SoundManager.Instance;
            SM.StopAudio();
            SM.Startbgm2();
        }

        //�J�ڌ�̃V�[�������u�P�O�̃V�[�����v�Ƃ��ĕێ�
        beforeScene = nextScene.name;
    }

}
