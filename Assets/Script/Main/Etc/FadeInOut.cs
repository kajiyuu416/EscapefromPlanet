using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class FadeInOut : MonoBehaviour
{
    public bool FadeInFlag;
    public bool FadeOutFlag;
    public bool RSF;
    private Image blackScreen;
    private bool fadeIn = false;
    private bool fadeOut = false;
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
        yield return new WaitForSeconds(Const.CO.Const_Float_List[0]);

        while (color.a >= 0)
        {
            color.a -= 0.1f;
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

        while (color.a <= Const.CO.Const_Float_List[0])
        {
            color.a += 0.1f;
            blackScreen.color = color;

            yield return null;
        }
        FadeOutFlag = false;
        fadeOut = false;
        RSF = true;
    }

    private void FadeIn_OutFlag()
    {
        if(FadeInFlag)
        {
            if(!fadeIn)
            {
                StartCoroutine(FadeIn());
                fadeIn = true;
            }
        }

        if(FadeOutFlag)
        {
            if(!fadeOut)
            {
                StartCoroutine(FadeOut());
                fadeOut = true;
            }
        }
    }

}
