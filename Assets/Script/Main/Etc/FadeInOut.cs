using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class FadeInOut : MonoBehaviour
{
    Image blackScreen;
    public bool FadeInFlag;
    public bool FadeOutFlag;
    public bool RSF;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private void Start()
    {
        StartCoroutine(FadeIn());
    }
    private void Update()
    { 
        if (FadeInFlag)
        {
            if(!fadeIn)
            {
                StartCoroutine(FadeIn());
                fadeIn = true;
            }
         
        }
        if (FadeOutFlag)
        {
            if(!fadeOut)
            {
                StartCoroutine(FadeOut());
                fadeOut = true;
            }

        }
    }
    //フェードイン、フェードアウトの処理
    public IEnumerator FadeIn()
    {
        blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        var color = blackScreen.color;
        yield return new WaitForSeconds(1f);

        while (color.a >= 0)
        {
            color -= new Color(0, 0, 0, 0.01f);
            blackScreen.color = color;

            yield return null;
        }
        FadeInFlag = false;
        fadeIn = false;
        RSF = false;
    }
    public IEnumerator FadeOut()
    {
        blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        blackScreen.gameObject.SetActive(true);
        var color = blackScreen.color;

        while (color.a <= 1)
        {
            color += new Color(0, 0, 0, 0.01f);
            blackScreen.color = color;

            yield return null;
        }
        FadeOutFlag = false;
        fadeOut = false;
        RSF = true;
    }
}
