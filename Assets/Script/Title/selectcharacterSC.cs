using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class selectcharacterSC : MonoBehaviour
{
    public Button selectButton;
    [SerializeField] GameObject selectImage;
    [SerializeField] GameObject selectModeButton;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject[] SetGameObjects;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(startButton);
        selectButton = GetComponent<Button>();
        selectButton.onClick.AddListener(() =>
        {
            selectImage.SetActive(true);
            foreach( var setObjcts in SetGameObjects)
            {
                setObjcts.SetActive(true);

            }
            EventSystem.current.SetSelectedGameObject(selectModeButton);
            SoundManager SM = SoundManager.Instance;
            SM.SettingPlaySE();
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
                    foreach(var setObjcts in SetGameObjects)
                    {
                        setObjcts.SetActive(false);
                    }
                    SoundManager SM = SoundManager.Instance;
                    SM.SettingPlaySE();
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
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

        if(selectedObject == null)
        {
            EventSystem.current.SetSelectedGameObject(startButton);
        }

    }
}
