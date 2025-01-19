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
    public int selectNum;
    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject player;
    [SerializeField] PlayerSelectData playerSelectData;
    [SerializeField] PlanetMove planetMove;

    private void Start()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(() =>
        {
            playerSelectData.playerSelectNumber = selectNum;
            GameManager2.GameStart();
 
            EventSystem.current.SetSelectedGameObject(null);
        });
    }

    public void OpenInformationButton(BaseEventData eventData)
    {
        player.transform.rotation = Quaternion.Euler(0, 180, 0);
        planetMove.roty = 0.5f;
    }
    public void CloseInformationButton(BaseEventData eventData)
    {
        player.transform.rotation = Quaternion.Euler(0, 180, 0);
        planetMove.roty = 0;
    }
}
