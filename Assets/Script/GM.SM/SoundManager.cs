using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

//BGM、SE管理
//オーディオミキサーを使用してボリューム管理
public class SoundManager : MonoBehaviour
{ 
    [SerializeField] AudioClip SelectSe;
    [SerializeField] AudioClip HitSe;
    [SerializeField] AudioClip LandingSe;
    [SerializeField] AudioClip SlidingSe;
    [SerializeField] AudioClip PoseSe;
    [SerializeField] AudioClip RespawnSe;
    [SerializeField] AudioClip ActionSelectSe;
    [SerializeField] AudioClip UnlockSe;
    [SerializeField] AudioClip ScreamSe;
    [SerializeField] AudioClip SpinSE;
    [SerializeField] AudioClip GrantSE;
    [SerializeField] AudioClip ExtinctionSE;
    [SerializeField] AudioClip keyboardinputSE;
    [SerializeField] AudioClip warningSE;
    [SerializeField] AudioClip GameClearSE;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider BgmSlinder;
    [SerializeField] Slider SeSlinder;

    GameObject bgmObj;
    GameObject SeObj;

    AudioSource bgm1AudioSource;
    AudioSource bgm2AudioSource;
    AudioSource bgm3AudioSource;
    AudioSource bgm4AudioSource;
    AudioSource SelectSeAudioSource;
    AudioSource HitSeAudioSource;
    AudioSource LandingSeAudioSource;

    public static SoundManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        bgmObj = transform.GetChild(0).gameObject;
        SeObj = transform.GetChild(1).gameObject;
        bgm1AudioSource = bgmObj.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        bgm2AudioSource = bgmObj.transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        bgm3AudioSource = bgmObj.transform.GetChild(2).gameObject.GetComponent<AudioSource>();
        bgm4AudioSource = bgmObj.transform.GetChild(3).gameObject.GetComponent<AudioSource>();
        SelectSeAudioSource = SeObj.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        HitSeAudioSource = SeObj.transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        LandingSeAudioSource = SeObj.transform.GetChild(2).gameObject.GetComponent<AudioSource>();

        SetBGMVolume(BgmSlinder.value);
        SetSEVolume(SeSlinder.value);
        BgmSlinder.onValueChanged.AddListener(SetBGMVolume);
        SeSlinder.onValueChanged.AddListener(SetSEVolume);
        Startbgm1();
    }
    //ボリュームの下限値、上昇値の設定
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Clamp(Mathf.Log10(volume) * 60f, -80f, 0f));
    }
    public void SetSEVolume(float volume)
    {
        audioMixer.SetFloat("SE", Mathf.Clamp(Mathf.Log10(volume) * 60f, -80f, 0f));
    }
    public void StopAudio()
    {
        bgm1AudioSource.Stop();
        bgm2AudioSource.Stop();
        bgm3AudioSource.Stop();
        bgm4AudioSource.Stop();
        SelectSeAudioSource.Stop();
    }
    public void Startbgm1()
    {
        bgm1AudioSource.Play();
    }
    public void Startbgm2()
    {
        bgm2AudioSource.Play();
    }
    public void Startbgm3()
    {
        bgm3AudioSource.Play();
    }
    public void Startbgm4()
    {
        bgm4AudioSource.Play();
    }
    public void SettingPlaySE()
    {
        SelectSeAudioSource.PlayOneShot(SelectSe);
    }
    public void SettingPlaySE2()
    {
        SelectSeAudioSource.PlayOneShot(HitSe);
    }
    public void SettingPlaySE3()
    {
        SelectSeAudioSource.PlayOneShot(LandingSe);
    }
    public void SettingPlaySE4()
    {
        SelectSeAudioSource.PlayOneShot(PoseSe);
    }
    public void SettingPlaySE5()
    {
        SelectSeAudioSource.PlayOneShot(ActionSelectSe);
    }
    public void SettingPlaySE6()
    {
        SelectSeAudioSource.PlayOneShot(UnlockSe);
    }
    public void SettingPlaySE7()
    {
        SelectSeAudioSource.PlayOneShot(RespawnSe);
    }
    public void SettingPlaySE8()
    {
        SelectSeAudioSource.PlayOneShot(SlidingSe);
    }
    public void SettingPlaySE9()
    {
        SelectSeAudioSource.PlayOneShot(ScreamSe);
    }
    public void SettingPlaySE10()
    {
        SelectSeAudioSource.PlayOneShot(SpinSE);
    }
    public void SettingPlaySE11()
    {
        SelectSeAudioSource.PlayOneShot(GrantSE);
    }
    public void SettingPlaySE12()
    {
        SelectSeAudioSource.PlayOneShot(ExtinctionSE);
    }
    public void SettingPlaySE13()
    {
        SelectSeAudioSource.PlayOneShot(keyboardinputSE);
    }
    public void SettingPlaySE14()
    {
        SelectSeAudioSource.PlayOneShot(warningSE);
    }   
    public void SettingPlaySE15()
    {
        SelectSeAudioSource.PlayOneShot(GameClearSE);
    }
}
