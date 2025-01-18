using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class selectcharacterSC : MonoBehaviour
{
    public Button selectButton;
    [SerializeField] GameObject selectImage;
    [SerializeField] GameObject settingImage;
    [SerializeField] GameObject selectModeButton;
    [SerializeField] GameObject startButton;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(startButton);
        selectButton = GetComponent<Button>();
        selectButton.onClick.AddListener(() =>
        {
            selectImage.SetActive(true);
            EventSystem.current.SetSelectedGameObject(selectModeButton);
        });

        if(selectImage.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(selectModeButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(startButton);
        }
    }
    private void Update()
    {

        if(GameManager2.connect)
        {
            var current_GP = Gamepad.current;
            var Cansel = current_GP.buttonEast;


            if(Cansel.wasPressedThisFrame)
            {
                if(selectImage.activeSelf)
                {
                    selectImage.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(startButton);
                }
                else if(settingImage.activeSelf)
                {
                    selectImage.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(startButton);
                }
            }
        }
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if(selectImage.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(selectModeButton);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(startButton);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
