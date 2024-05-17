using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Video;

//OPシーン（映像シーン）にて使用、映像を任意でスキップする処理
//映像終了後MainSceneに移行する処理
public class ChangeScene : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] FadeInOut fadeinout;
    private void Start()
    {
        videoPlayer.loopPointReached += LoopPointReached;
        if (GameManager2.on_off_button)
        {
            GameManager2.ActionUIFlag = true;
        }
        if (GameManager2.on_off_button == false)
        {
            GameManager2.ActionUIFlag = false;
        }

    }
    private void Update()
    {
        var current_GP = Gamepad.current;
        var Cansel = current_GP.buttonEast;
            if (Cansel.wasPressedThisFrame)
            {
                fadeinout.FadeOutFlag = true;
                StartCoroutine("Skip");
            }
    }
    IEnumerator Skip()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("MainScene");
        GameManager2.instance.firstLoadFlag = true;
    }
    public void LoopPointReached(VideoPlayer vp)
    {
        // 動画再生完了時の処理
        SceneManager.LoadScene("MainScene");
        GameManager2.instance.firstLoadFlag = true;
    }

 

}
