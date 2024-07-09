using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class FadeInOut : MonoBehaviour
{
    public bool fadeInFlag;
    public bool fadeOutFlag;
    private Image blackScreen;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private bool fadeOutEnd;
    //フラグが返るとフェードイン、フェードアウトの処理
    private void Start()
    {
        StartCoroutine(FadeIn());
    }
    private void Update()
    {
        FadeIn_OutFlag();
    }
    public IEnumerator FadeIn()
    {
        blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        var color = blackScreen.color;
        yield return new WaitForSeconds(1.0f);

        while (color.a >= 0)
        {
            color.a -= 0.1f;
            blackScreen.color = color;

            yield return null;
        }
        fadeInFlag = false;
        fadeIn = false;
        fadeOutEnd = false;
    }
    public IEnumerator FadeOut()
    {
        blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        blackScreen.gameObject.SetActive(true);
        var color = blackScreen.color;

        while (color.a <= 1.0f)
        {
            color.a += 0.1f;
            blackScreen.color = color;

            yield return null;
        }
        fadeOutFlag = false;
        fadeOut = false;
        fadeOutEnd = true;
    }

    public void FadeIn_OutFlag()
    {
        if(fadeInFlag)
        {
            if(!fadeIn)
            {
                StartCoroutine(FadeIn());
                fadeIn = true;
            }
        }

        if(fadeOutFlag)
        {
            if(!fadeOut)
            {
                StartCoroutine(FadeOut());
                fadeOut = true;
            }
        }
    }

    public bool Duplicate_fadeOutEnd
    {
        get
        {
            return fadeOutEnd;
        }

        set
        {
            fadeOutEnd = value;
        }
    }

}
