using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

//StartButton�������ꂽ��Q�[�����J�n���鏈��
//�I������^�[�Q�b�g���O��Ȃ��悤�Ƀ{�^���̓��́A�}�E�X�̃N���b�N���͂�����ꍇ
//�^�[�Q�b�g��StartButton�ɍ��킹��
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
