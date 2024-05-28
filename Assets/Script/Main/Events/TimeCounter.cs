using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

//イベントに合わせてタイマーを表示
public class TimeCounter : MonoBehaviour
{
    [SerializeField] GameManager GM;
    [SerializeField] FadeInOut FO;
    [SerializeField] GameObject Idle;
    [SerializeField] GameObject explosionEfe;
    [SerializeField] CinemachineVirtualCamera subcamera3;
    public int countdownMinutes = 5;
    private float countdownSeconds;
    private Text timeText;

    private bool TimeUp;

    private void Start()
    {
        timeText = GetComponent<Text>();
        countdownSeconds = countdownMinutes * 60;
    }
    void Update()
    {
        countdownSeconds -= Time.deltaTime;
        var span = new TimeSpan(0, 0, (int)countdownSeconds);
        timeText.text = span.ToString(@"mm\:ss");

        if (countdownSeconds <= 0 &&!TimeUp)
        {  // 0秒になったときの処理
            TimeUp= true;
            subcamera3.Priority = Const.CO.Const_Int_List[0];
            timeText.enabled = false;
            explosionEfe.SetActive(true);
            Idle.SetActive(false);
            GameManager.instance.ActionUI1.SetActive(false);
            GameManager.instance.ActionUI2.SetActive(false);
            GameManager.pauseflag = true;
            SoundManager.Instance.StopAudio();
            SoundManager.Instance.Startbgm4();
            StartCoroutine("GO");
        }
        if (FO.RSF&&TimeUp)
        {
            TimeUp = false;
            Scene ThisScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(ThisScene.name);
        }
    }
     IEnumerator GO()
    {
        yield return new WaitForSeconds(Const.CO.Const_Int_List[4]);
        FO.FadeOutFlag = true;
    }

}