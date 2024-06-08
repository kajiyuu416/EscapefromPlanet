using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Camera))]
//カメラ操作、Cinemachineによるカメラ切り替え
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float distanceToPlayerM;    // カメラとプレイヤーとの距離[m]
    [SerializeField] float slideDistanceM;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
    [SerializeField] float heightM;            // 注視点の高さ[m]
    [SerializeField] CinemachineVirtualCamera subcamera1;
    [SerializeField] CinemachineVirtualCamera subcamera2;
    [SerializeField] liberate_potential first_Event;
    [SerializeField] liberate_potential second_Event;
    [SerializeField] SkinnedMeshRenderer normalBody;
    [SerializeField] Material defaultBodyMaterial;
    [SerializeField] Material transmissionBodyMaterial;
    private PlayerController playerController;
    public static float rotationSensitivity = 75.0f;// 感度
    private void Awake()
    {
        if (target == null)
        {
            Debug.LogError("ターゲットが設定されていない");
            Application.Quit();
        }
        transform.position = target.position;
        playerController = FindObjectOfType<PlayerController>();
    }
    private void FixedUpdate()
    {
        ChangeCamera();
        CameraMove();
    }
    private void CameraMove()
    {
        var rotX = playerController.Duplicate_cameraInputVal.x * Time.deltaTime * rotationSensitivity;
        var rotY = playerController.Duplicate_cameraInputVal.y * Time.deltaTime * rotationSensitivity;
        var lookAt = target.position + Vector3.up * heightM;
        float off_upper_limit = 0.5f;
        float off_lower_limit = -0.7f;
        float on_upper_limit = -0.7f;
        float on_lower_limit = 0.5f;

        if(playerController.Duplicate_isDead || GameManager.pauseflag)
        {
            rotX = 0;
            rotY = 0;
        }
        // 左右回転
        if(GameManager2.camera_Flip_left_and_right)
        {
            transform.RotateAround(lookAt, Vector3.down, rotX);
        }
        else
        {
            transform.RotateAround(lookAt, Vector3.up, rotX);
        }
        //上昇下降の制限
        // カメラがプレイヤーの真上や真下にあるときにそれ以上回転させないようにする
        if(!GameManager2.camera_Upside_down)
        {
            if(transform.forward.y > off_upper_limit && rotY < 0)
            {
                rotY = 0;
                normalBody.material = transmissionBodyMaterial;
            }

            if(transform.forward.y < off_lower_limit && rotY > 0)
            {
                rotY = 0;
                normalBody.material = transmissionBodyMaterial;
            }
        }
        else
        {
            if(transform.forward.y < on_upper_limit && rotY < 0)
            {
                rotY = 0;
                normalBody.material = transmissionBodyMaterial;
            }

            if(transform.forward.y > on_lower_limit && rotY > 0)
            {
                rotY = 0;
                normalBody.material = transmissionBodyMaterial;
            }
        }

        // 上下回転
        if(GameManager2.camera_Upside_down)
        {
            transform.RotateAround(lookAt, -transform.right, rotY);
        }
        else
        {
            transform.RotateAround(lookAt, transform.right, rotY);
        }

        if(rotY != 0)
        {
            normalBody.material = defaultBodyMaterial;
        }
        // カメラとプレイヤーとの間の距離を調整
        transform.position = lookAt - transform.forward * distanceToPlayerM;

        // カメラを横にずらして中央を開ける
        transform.position = transform.position + transform.right * slideDistanceM;

        var current_GP = Gamepad.current;
        var camera_Reset = current_GP.rightStickButton;

        if(PlayerController.Interval_InputButtondown(camera_Reset, Const.CO.const_Float_List[0]) &&!GameManager.pauseflag)
        {
            normalBody.material = defaultBodyMaterial;
            transform.rotation = Quaternion.Lerp(target.rotation, transform.rotation, 3.0f * Time.deltaTime);
            Debug.Log("カメラリセット");
        }
    }

    private void ChangeCamera()
    {
        if(first_Event.actionFlag)
        {
            subcamera1.Priority = Const.CO.const_Int_List[0];
        }
        if(second_Event.actionFlag)
        {
            subcamera2.Priority = Const.CO.const_Int_List[0];
        }

        if(!first_Event.actionFlag)
        {

            subcamera1.Priority = 0;
        }
        if(!second_Event.actionFlag)
        {
            subcamera2.Priority = 0;
        }
    }

}