using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Camera))]
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
    GameObject targetObj;
    Vector3 targetPos;
    void Start()
    {
        if (Target == null)
        {
            Debug.LogError("ターゲットが設定されていない");
            Application.Quit();
        }
        targetObj = GameObject.Find("Idle"); 
        targetPos = targetObj.transform.position;
    }
    void Update()
    {
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;
    }
    void FixedUpdate()
    {
        if(aE1.actionFlag)
        {
            subcamera1.Priority = 11;
        }
        if(aE3.actionFlag)
        {
            subcamera2.Priority = 11;
        }
        if (PC.isDead || GameManager.pauseflag)
        {
            RotationSensitivity = 0f;
        }
        else
        {
            RotationSensitivity = 100.0f;
        }

        if (!aE1.actionFlag)
        {
            
            subcamera1.Priority = 9;
        }
        if(!aE3.actionFlag)
        {
            subcamera2.Priority = 9;
        }
        var rotX = PC.CameraInputVal.x * Time.deltaTime * RotationSensitivity;
        var rotY = PC.CameraInputVal.y * Time.deltaTime * RotationSensitivity;
        var lookAt = Target.position + Vector3.up * HeightM;

        // 回転
        transform.RotateAround(lookAt, Vector3.up, rotX);
        // カメラがプレイヤーの真上や真下にあるときにそれ以上回転させないようにする
        if (transform.forward.y > 0.3f && rotY < 0)
        {
            rotY = 0;
        }
        if (transform.forward.y < -0.8f && rotY > 0)
        {
            rotY = 0;
        }
        transform.RotateAround(lookAt, transform.right, rotY);

        // カメラとプレイヤーとの間の距離を調整
        transform.position = lookAt - transform.forward * DistanceToPlayerM;

        // 注視点の設定
        transform.LookAt(lookAt);

        // カメラを横にずらして中央を開ける
        transform.position = transform.position + transform.right * SlideDistanceM;
    }

   
}