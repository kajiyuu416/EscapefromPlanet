using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

//StartButtonが押されたらゲームを開始する処理
//選択するターゲットが外れないようにボタンの入力、マウスのクリック入力がある場合
//ターゲットをStartButtonに合わせる
public class StartButtonSC : MonoBehaviour
{
    public Button startButton;
    [SerializeField] GameObject StartButton;

    private void Start()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(() =>
        {
            GameManager2.GameStart();
            EventSystem.current.SetSelectedGameObject(null);
        });
    }
    private void Update()
    {
        if(GameManager2.connect)
        {
            var current_GP = Gamepad.current;
            var Cansel = current_GP.buttonEast;
            if(Cansel.wasPressedThisFrame)
            {
                EventSystem.current.SetSelectedGameObject(StartButton);
            }
        }
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            EventSystem.current.SetSelectedGameObject(StartButton);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
}
