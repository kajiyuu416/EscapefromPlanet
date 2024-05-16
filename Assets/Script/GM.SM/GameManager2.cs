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

    //�Q�[���X�^�[�g���̃I�u�W�F�N�g��\��
    //���X�ɉ�ʂ����邭�Ȃ鏈��
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

    //�R���g���[���[�̐ڑ��m�F
    //�}�E�X�̔�\���A���݃V�[���̕ۑ�
  �@//�ݒ��ʂ̕\����\��
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
    //�ݒ���J�����Ƃ�
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
    //�ݒ������Ƃ�
    public void SettingCl()
    {
        Debug.Log("Settingcl");
        SettingBG.SetActive(false);
        BGMSlinder.SetActive(false);
        SESlinder.SetActive(false);
        UIButton.gameObject.SetActive(false);
        SettingFlag = false;
    }
    //�ݒ��ʂ�ON�EOFF�؂�ւ�
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
