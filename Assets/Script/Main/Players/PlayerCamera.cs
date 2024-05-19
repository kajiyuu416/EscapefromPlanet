using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
[RequireComponent(typeof(Camera))]
//�J��������ACinemachine�ɂ��J�����؂�ւ�
public class PlayerCamera : MonoBehaviour
{
    public Transform Target;
    public float DistanceToPlayerM = 2.0f;    // �J�����ƃv���C���[�Ƃ̋���[m]
    public float SlideDistanceM = 0.0f;       // �J���������ɃX���C�h������G�v���X�̎��E�ցC�}�C�i�X�̎�����[m]
    public float HeightM = 1.2f;            // �����_�̍���[m]
    public CinemachineVirtualCamera subcamera1;
    public CinemachineVirtualCamera subcamera2;
    public static float RotationSensitivity = 75.0f;// ���x
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
            Debug.LogError("�^�[�Q�b�g���ݒ肳��Ă��Ȃ�");
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
        // ���E��]
        if(GameManager2.Camera_Flip_left_and_right)
        {
            transform.RotateAround(lookAt, Vector3.down, rotX);
        }
        else
        {
            transform.RotateAround(lookAt, Vector3.up, rotX);
        }
        //�㏸���~�̐���
        // �J�������v���C���[�̐^���^���ɂ���Ƃ��ɂ���ȏ��]�����Ȃ��悤�ɂ���
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

        // �㉺��]
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