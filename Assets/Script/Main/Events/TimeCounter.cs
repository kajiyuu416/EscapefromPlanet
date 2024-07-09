using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

//イベントに合わせてタイマーを表示
public class TimeCounter : MonoBehaviour
{
    [SerializeField] GameObject idle;
    [SerializeField] GameObject explosionEfe;
    [SerializeField] CinemachineVirtualCamera subcamera3;
    private FadeInOut fadeinout;
    private int countdownMinutes = 2;
    private const int minute = 60;
    private float countdownSeconds;
    private Text timeText;
    private bool TimeUp;

    private void Start()
    {
        timeText = GetComponent<Text>();
        fadeinout = FindObjectOfType<FadeInOut>();
        countdownSeconds = countdownMinutes * minute;
    }
    void Update()
    {
        countdownSeconds -= Time.deltaTime;
        var span = new TimeSpan(0, 0, (int)countdownSeconds);
        timeText.text = span.ToString(@"mm\:ss");

        if (countdownSeconds <= 0 &&!TimeUp)
        {  // 0秒になったときの処理
            TimeUp= true;
            subcamera3.Priority = 1;
            timeText.enabled = false;
            explosionEfe.SetActive(true);
            idle.SetActive(false);
            GameManager.instance.playerui.SetActive(false);
            GameManager.pauseflag = true;
            SoundManager.Instance.StopAudio();
            SoundManager.Instance.Startbgm4();
            StartCoroutine(GameOver());
        }
        if (fadeinout.Duplicate_fadeOutEnd && TimeUp)
        {
            TimeUp = false;
            Scene ThisScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(ThisScene.name);
        }
    }
     IEnumerator GameOver()
    {
        yield return new WaitForSeconds(5);
        fadeinout.fadeOutFlag = true;
    }

}