using UnityEngine;
using UnityEngine.UI;

public class SettingButoonSC : MonoBehaviour
{
    //SettingButoon‚ª‰Ÿ‚³‚ê‚½‚Æ‚«İ’è‚ğŠJ‚­ˆ—
    public Button settingButton;
    private void Start()
    {
        settingButton = GetComponent<Button>();
        settingButton.onClick.AddListener(() => {
            GameManager2.Setting();
        });
    }
}
