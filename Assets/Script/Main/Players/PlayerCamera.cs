using UnityEngine;
using Cinemachine;
[RequireComponent(typeof(Camera))]
//カメラ操作、Cinemachineによるカメラ切り替え
public class PlayerCamera : MonoBehaviour
{
    public Transform Target;
    public float DistanceToPlayerM = 2.0f;    // カメラとプレイヤーとの距離[m]
    public float SlideDistanceM = 0.0f;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
    public float HeightM = 1.2f;            // 注視点の高さ[m]
    public float RotationSensitivity = 100.0f;// 感度
    public CinemachineVirtualCamera subcamera1;
    public CinemachineVirtualCamera subcamera2;
    [SerializeField] actionEvent1 aE1;
    [SerializeField] actionEvent1 aE3;
    [SerializeField] PlayerController PC;
    [SerializeField] GameObject targetObj;
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
        var rotX = PC.CameraInputVal.x * Time.deltaTime * RotationSensitivity;
        var rotY = PC.CameraInputVal.y * Time.deltaTime * RotationSensitivity;
        var lookAt = Target.position + Vector3.up * HeightM;

        // 回転
        if(GameManager2.Camera_Upside_down)
        {
            transform.RotateAround(lookAt, Vector3.down, rotX);
        }
        else
        {
            transform.RotateAround(lookAt, Vector3.up, rotX);
        }

        // カメラがプレイヤーの真上や真下にあるときにそれ以上回転させないようにする
        if (transform.forward.y > 0.3f && rotY < 0)
        {
            rotY = 0;
        }
        if (transform.forward.y < -0.8f && rotY > 0)
        {
            rotY = 0;
        }

        if(GameManager2.Camera_Flip_left_and_right)
        {
            transform.RotateAround(lookAt, -transform.right, rotY);
        }
        else
        {
            transform.RotateAround(lookAt, transform.right, rotY);
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
        if(PC.isDead || GameManager.pauseflag)
        {
            RotationSensitivity = 0f;
        }
        else
        {
            RotationSensitivity = 100.0f;
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