using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Video;

//OPScene,DemoScene（映像シーン）にて使用
//映像を任意でスキップする処理、映像終了後MainSceneに移行する処理
//Sceneを事前に読み込み、切り替えたいタイミングでフラグを返すことでスムーズにシーンを切り替えられるように実装
public class ChangeScene : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] FadeInOut fadeinout;
    [SerializeField] string sceneName;
    private GameManager2 gamemanager2;
    private AsyncOperation asyncLoad;
    private const string scenename = "OPScene";
    private void Start()
    {
        gamemanager2 = FindObjectOfType<GameManager2>();
        videoPlayer.loopPointReached += LoopPointReached;
        StartCoroutine("Skip");
    }
    IEnumerator Skip()
    {
        var current_GP = Gamepad.current;
        var SkipButton = current_GP.buttonEast;
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        bool waitFlag = true;
        while(waitFlag)
        {
            yield return null;
            waitFlag = !SkipButton.wasPressedThisFrame;
        }
        yield return fadeinout.FadeOut();
        asyncLoad.allowSceneActivation = true;
        if(SceneManager.GetActiveScene().name == scenename)
        {
            gamemanager2.Duplicate_firstLoadFlag = true;
        }
        yield return asyncLoad;
    }
    public void LoopPointReached(VideoPlayer vp)
    {
        // 動画再生完了時の処理
        if(SceneManager.GetActiveScene().name == scenename)
        {
            gamemanager2.Duplicate_firstLoadFlag = true;
        }
        asyncLoad.allowSceneActivation = true;
    }
}
