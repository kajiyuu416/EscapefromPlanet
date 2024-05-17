using UnityEngine;
using Cinemachine;
[RequireComponent(typeof(Camera))]
//�J��������ACinemachine�ɂ��J�����؂�ւ�
public class PlayerCamera : MonoBehaviour
{
    public Transform Target;
    public float DistanceToPlayerM = 2.0f;    // �J�����ƃv���C���[�Ƃ̋���[m]
    public float SlideDistanceM = 0.0f;       // �J���������ɃX���C�h������G�v���X�̎��E�ցC�}�C�i�X�̎�����[m]
    public float HeightM = 1.2f;            // �����_�̍���[m]
    public float RotationSensitivity = 100.0f;// ���x
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
            Debug.LogError("�^�[�Q�b�g���ݒ肳��Ă��Ȃ�");
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

        // ��]
        if(GameManager2.Camera_Upside_down)
        {
            transform.RotateAround(lookAt, Vector3.down, rotX);
        }
        else
        {
            transform.RotateAround(lookAt, Vector3.up, rotX);
        }

        // �J�������v���C���[�̐^���^���ɂ���Ƃ��ɂ���ȏ��]�����Ȃ��悤�ɂ���
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
 

        // �J�����ƃv���C���[�Ƃ̊Ԃ̋����𒲐�
        transform.position = lookAt - transform.forward * DistanceToPlayerM;

        // �����_�̐ݒ�
        transform.LookAt(lookAt);

        // �J���������ɂ��炵�Ē������J����
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