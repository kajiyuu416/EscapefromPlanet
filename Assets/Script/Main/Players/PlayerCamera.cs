using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Camera))]
//�J��������ACinemachine�ɂ��J�����؂�ւ�
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] float DistanceToPlayerM;    // �J�����ƃv���C���[�Ƃ̋���[m]
    [SerializeField] float SlideDistanceM;       // �J���������ɃX���C�h������G�v���X�̎��E�ցC�}�C�i�X�̎�����[m]
    [SerializeField] float HeightM;            // �����_�̍���[m]
    [SerializeField] CinemachineVirtualCamera subcamera1;
    [SerializeField] CinemachineVirtualCamera subcamera2;
    [SerializeField] liberate_potential First_Event;
    [SerializeField] liberate_potential Second_Event;
    [SerializeField] PlayerController PC;
    [SerializeField] SkinnedMeshRenderer normalBody;
    [SerializeField] Material DefaultBodyMaterial;
    [SerializeField] Material TransmissionBodyMaterial;
    public static float RotationSensitivity = 75.0f;// ���x
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
        if(GameManager2.camera_Flip_left_and_right)
        {
            transform.RotateAround(lookAt, Vector3.down, rotX);
        }
        else
        {
            transform.RotateAround(lookAt, Vector3.up, rotX);
        }
        //�㏸���~�̐���
        // �J�������v���C���[�̐^���^���ɂ���Ƃ��ɂ���ȏ��]�����Ȃ��悤�ɂ���
        if(!GameManager2.camera_Upside_down)
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
            normalBody.material = DefaultBodyMaterial;
        }
        // �J�����ƃv���C���[�Ƃ̊Ԃ̋����𒲐�
        transform.position = lookAt - transform.forward * DistanceToPlayerM;

        // �J���������ɂ��炵�Ē������J����
        transform.position = transform.position + transform.right * SlideDistanceM;

        var current_GP = Gamepad.current;
        var camera_Reset = current_GP.rightStickButton;

        if(PlayerController.Interval_InputButtondown(camera_Reset, Const.CO.const_Float_List[0]) &&!GameManager.pauseflag)
        {
            normalBody.material = DefaultBodyMaterial;
            transform.rotation = Quaternion.Lerp(Target.rotation, transform.rotation, 3.0f * Time.deltaTime);
            Debug.Log("�J�������Z�b�g");
        }
    }

    private void ChangeCamera()
    {
        if(First_Event.actionFlag)
        {
            subcamera1.Priority = Const.CO.const_Int_List[0];
        }
        if(Second_Event.actionFlag)
        {
            subcamera2.Priority = Const.CO.const_Int_List[0];
        }

        if(!First_Event.actionFlag)
        {

            subcamera1.Priority = 0;
        }
        if(!Second_Event.actionFlag)
        {
            subcamera2.Priority = 0;
        }
    }

}