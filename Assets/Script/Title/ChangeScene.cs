using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Video;

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
        // “®‰æÄ¶Š®—¹‚Ìˆ—
        SceneManager.LoadScene("MainScene");
        GameManager2.instance.firstLoadFlag = true;
    }

 

}
