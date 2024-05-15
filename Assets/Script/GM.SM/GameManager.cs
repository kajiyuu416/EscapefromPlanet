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
    [SerializeField] GameObject restrictionImage;
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
    private bool GCF;
    private bool SettingOPflag;
    private bool stbflag;

    private void Update()
    {
        if(GameManager2.connect)
        {
            Pause();
            var current_GP = Gamepad.current;
            var Cansel = current_GP.buttonEast;
            if(GCF)
            {
                if(Cansel.wasPressedThisFrame)
                {
                    SceneManager.LoadScene("title");
                    GCF = false;
                }
            }
        }
        GivePower();
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            EventSystem.current.SetSelectedGameObject(SettingButton);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(GameManager2.ActionUIFlag)
        {
           ActionUI.SetActive(true);
        }
        else if(!GameManager2.ActionUIFlag)
        {
           ActionUI.SetActive(false);
        }

        if (!GameOverFlag && RestartFlag)
        {
            GameOver();
            GameOverFlag = true;
        }
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        pauseflag = true;
        GameOverFlag = false;
        GOCount.text = count.ToString();
        Invoke("Standbytime", 3.8f);

        if (instance == null)
        {
            instance = this;
        }
    }
    //pause画面に移動時の処理
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
            Idle.GetComponent<Animator>().speed = 0;
            EventSystem.current.SetSelectedGameObject(SettingButton);
            SoundManager SM = SoundManager.Instance;
            warningimage.SetActive(false);
            SM.SettingPlaySE4();
        }
        if (Cansel.wasPressedThisFrame && SettingOPflag)
        {
            pauseflag = false;
            SettingOPflag = false;
            PauseTextobj.SetActive(false);
            PauseBackground.SetActive(false);
            Idle.GetComponent<Animator>().speed = 1;
        }

    }
    private void GivePower()
    {
        if (GameManager2.AGF)
        {
            AdditionPlayerAction.AdditionPlayerActionFlag = true;
            Destroy(restrictionImage);
        }
        if (GameManager2.FGF)
        {
            FloatPowerSC.AdditionPlayerActionFlag = true;
        }
    }

    public  void GameClear()
    {
        GameClearTextobj.SetActive(true);
        GCF = true;
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE15();
    }
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
        Invoke("ReStartThiScene", 3.0f);
    }
    private void ReStartThiScene()
    {
        Scene ThisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(ThisScene.name);
        SoundManager.Instance.StopAudio();
    }

    private void BacktoTitle()
    {
        ActionUI1.SetActive(false);
        ActionUI2.SetActive(false);
        SceneManager.LoadScene("title");
        SoundManager SM = SoundManager.Instance;
        SM.SettingPlaySE();

    }
    private void Standbytime()
    {
        stbflag = true;
        pauseflag = false;
    }
    public void RestartFlagOn()
    {
        RestartFlag = true;
    }

}
