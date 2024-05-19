using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
[RequireComponent(typeof(Camera))]
//カメラ操作、Cinemachineによるカメラ切り替え
public class PlayerCamera : MonoBehaviour
{
    public Transform Target;
    public float DistanceToPlayerM = 2.0f;    // カメラとプレイヤーとの距離[m]
    public float SlideDistanceM = 0.0f;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
    public float HeightM = 1.2f;            // 注視点の高さ[m]
    public CinemachineVirtualCamera subcamera1;
    public CinemachineVirtualCamera subcamera2;
    public static float RotationSensitivity = 75.0f;// 感度
    [SerializeField] actionEvent1 aE1;
    [SerializeField] actionEvent1 aE3;
    [SerializeField] PlayerController PC;
    [SerializeField] SkinnedMeshRenderer normalBody;
    [SerializeField] Material DefaultBodyMaterial;
    [SerializeField] Material TransmissionBodyMaterial;
    private void Awake()
    {
        if (Target == null)
        {
            Debug.LogError("ターゲットが設定されていない");
            Application.Quit();
        }
        transform.position = Target.position;
    }
    private void FixedUpdate()
    {
        ChangeCamera();
        CameraMove();
    }
    private void CameraMove()
    {
        var rotX = PC.CameraInputVal.x * Time.deltaTime * RotationSensitivity;
        var rotY = PC.CameraInputVal.y * Time.deltaTime * RotationSensitivity;
        var lookAt = Target.position + Vector3.up * HeightM;
        float off_upper_limit = 0.5f;
        float off_lower_limit = -0.7f;
        float on_upper_limit = -0.7f;
        float on_lower_limit = 0.5f;
        if(PC.isDead || GameManager.pauseflag)
        {
            rotX = 0;
            rotY = 0;
        }
        // 左右回転
        if(GameManager2.Camera_Flip_left_and_right)
        {
            transform.RotateAround(lookAt, Vector3.down, rotX);
        }
        else
        {
            transform.RotateAround(lookAt, Vector3.up, rotX);
        }
        //上昇下降の制限
        // カメラがプレイヤーの真上や真下にあるときにそれ以上回転させないようにする
        if(!GameManager2.Camera_Upside_down)
        {
            if(transform.forward.y > off_upper_limit && rotY < 0)
            {
                rotY = 0;
                normalBody.material = TransmissionBodyMaterial;
            }

            if(transform.forward.y < off_lower_limit && rotY > 0)
            {
                rotY = 0;
                normalBody.material = TransmissionBodyMaterial;
            }
        }
        else
        {
            if(transform.forward.y < on_upper_limit && rotY < 0)
            {
                rotY = 0;
                normalBody.material = TransmissionBodyMaterial;
            }

            if(transform.forward.y > on_lower_limit && rotY > 0)
            {
                rotY = 0;
                normalBody.material = TransmissionBodyMaterial;
            }
        }

        // 上下回転
        if(GameManager2.Camera_Upside_down)
        {
            transform.RotateAround(lookAt, -transform.right, rotY);
        }
        else
        {
            transform.RotateAround(lookAt, transform.right, rotY);
        }

        if(rotY != 0)
        {
            normalBody.material = DefaultBodyMaterial;
        }


        // カメラとプレイヤーとの間の距離を調整
        transform.position = lookAt - transform.forward * DistanceToPlayerM;

        // 注視点の設定
        transform.LookAt(lookAt);

        // カメラを横にずらして中央を開ける
        transform.position = transform.position + transform.right * SlideDistanceM;
    }

    private void ChangeCamera()
    {
        if(aE1.actionFlag)
        {
            subcamera1.Priority = 11;
        }
        if(aE3.actionFlag)
        {
            subcamera2.Priority = 11;
        }

        if(!aE1.actionFlag)
        {

            subcamera1.Priority = 9;
        }
        if(!aE3.actionFlag)
        {
            subcamera2.Priority = 9;
        }
    }

}