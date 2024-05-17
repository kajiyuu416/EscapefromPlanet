using UnityEngine;
using UnityEngine.UI;

public class ExitButtonSC : MonoBehaviour
{
    //ExitButton‚ª‰Ÿ‚³‚ê‚½‚Æ‚«‚ÉƒQ[ƒ€‚ğI—¹‚·‚éˆ—
    public Button exitButton;
    private void Start()
    {
        exitButton = GetComponent<Button>();
        exitButton.onClick.AddListener(() =>
        {
            GameManager2.EndGame();
        });
    }
}
