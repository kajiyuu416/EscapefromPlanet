using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

//BGM、SE管理
//オーディオミキサーを使用してボリューム管理
public class SoundManager : MonoBehaviour
{ 
    [SerializeField] AudioClip selectSe;
    [SerializeField] AudioClip hitSe;
    [SerializeField] AudioClip landingSe;
    [SerializeField] AudioClip slidingSe;
    [SerializeField] AudioClip poseSe;
    [SerializeField] AudioClip respawnSe;
    [SerializeField] AudioClip actionSelectSe;
    [SerializeField] AudioClip unlockSe;
    [SerializeField] AudioClip screamSe;
    [SerializeField] AudioClip spinSE;
    [SerializeField] AudioClip grantSE;
    [SerializeField] AudioClip extinctionSE;
    [SerializeField] AudioClip keyboardinputSE;
    [SerializeField] AudioClip warningSE;
    [SerializeField] AudioClip gameClearSE;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider bgmSlinder;
    [SerializeField] Slider seSlinder;

    private GameObject bgmObj;
    private GameObject SeObj;

    private AudioSource bgm1AudioSource;
    private AudioSource bgm2AudioSource;
    private AudioSource bgm3AudioSource;
    private AudioSource bgm4AudioSource;
    private AudioSource SelectSeAudioSource;
    private AudioSource HitSeAudioSource;
    private AudioSource LandingSeAudioSource;

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

        SetBGMVolume(bgmSlinder.value);
        SetSEVolume(seSlinder.value);
        bgmSlinder.onValueChanged.AddListener(SetBGMVolume);
        seSlinder.onValueChanged.AddListener(SetSEVolume);
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
        SelectSeAudioSource.PlayOneShot(selectSe);
    }
    public void SettingPlaySE2()
    {
        SelectSeAudioSource.PlayOneShot(hitSe);
    }
    public void SettingPlaySE3()
    {
        SelectSeAudioSource.PlayOneShot(landingSe);
    }
    public void SettingPlaySE4()
    {
        SelectSeAudioSource.PlayOneShot(poseSe);
    }
    public void SettingPlaySE5()
    {
        SelectSeAudioSource.PlayOneShot(actionSelectSe);
    }
    public void SettingPlaySE6()
    {
        SelectSeAudioSource.PlayOneShot(unlockSe);
    }
    public void SettingPlaySE7()
    {
        SelectSeAudioSource.PlayOneShot(respawnSe);
    }
    public void SettingPlaySE8()
    {
        SelectSeAudioSource.PlayOneShot(slidingSe);
    }
    public void SettingPlaySE9()
    {
        SelectSeAudioSource.PlayOneShot(screamSe);
    }
    public void SettingPlaySE10()
    {
        SelectSeAudioSource.PlayOneShot(spinSE);
    }
    public void SettingPlaySE11()
    {
        SelectSeAudioSource.PlayOneShot(grantSE);
    }
    public void SettingPlaySE12()
    {
        SelectSeAudioSource.PlayOneShot(extinctionSE);
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
        SelectSeAudioSource.PlayOneShot(gameClearSE);
    }
}
