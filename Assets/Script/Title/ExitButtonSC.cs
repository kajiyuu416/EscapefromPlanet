using UnityEngine;
using UnityEngine.UI;

public class ExitButtonSC : MonoBehaviour
{
    //ExitButtonが押されたときにゲームを終了する処理
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
