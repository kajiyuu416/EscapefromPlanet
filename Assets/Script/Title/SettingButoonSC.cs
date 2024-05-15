using UnityEngine;
using UnityEngine.UI;

public class SettingButoonSC : MonoBehaviour
{
    public Button settingButton;
    // Start is called before the first frame update
    void Start()
    {
        settingButton = GetComponent<Button>();
        settingButton.onClick.AddListener(() => {
            GameManager2.Setting();
        });
    }
}
