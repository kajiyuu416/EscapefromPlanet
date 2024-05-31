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
    private AsyncOperation asyncLoad;
    private void Start()
    {
        videoPlayer.loopPointReached += LoopPointReached;
        StartCoroutine("Skip");
    }
    IEnumerator Skip()
    {
        var current_GP = Gamepad.current;
        var SkipButton = current_GP.buttonEast;
        asyncLoad = SceneManager.LoadSceneAsync("MainScene");
        asyncLoad.allowSceneActivation = false;
        bool waitFlag = true;
        while(waitFlag)
        {
            yield return null;
            waitFlag = !SkipButton.wasPressedThisFrame;
        }
        yield return fadeinout.FadeOut();
        asyncLoad.allowSceneActivation = true;
        GameManager2.instance.firstLoadFlag = true;
        yield return asyncLoad;
    }
    public void LoopPointReached(VideoPlayer vp)
    {
        // 動画再生完了時の処理
        GameManager2.instance.firstLoadFlag = true;
        asyncLoad.allowSceneActivation = true;
    }
}
