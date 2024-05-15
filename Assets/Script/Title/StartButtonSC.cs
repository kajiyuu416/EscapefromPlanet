using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class StartButtonSC : MonoBehaviour
{
    public Button startButton;
    [SerializeField] GameObject StartButton;

    void Start()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(() => {
            GameManager2.GameStart();
        });
    }
     void Update()
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
