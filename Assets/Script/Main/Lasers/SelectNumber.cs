using UnityEngine;
// LINQ �̎g�p
using System.Linq;

// ��������G�f�B�^�[��ł̂ݗL��
#if UNITY_EDITOR
using UnityEditor;

// �G�f�B�^�[�g���N���X
[CustomEditor(typeof(SelectNumber))]
public class selectnumberedEditor : Editor
{// Editor �N���X���p��
    // selectnumber �N���X�̕ϐ����������߂ɐ錾
    SelectNumber selectnumber;

    void OnEnable()
    {// �ŏ��Ɏ��s
        // selectnumber �N���X�� target ����
        selectnumber = (SelectNumber)target;
    }

    public override void OnInspectorGUI()
    {// Inspector �ɕ\��
        // ����ȍ~�̗v�f�Ɋւ��ăG�f�B�^�ɂ��ύX���L�^
        EditorGUI.BeginChangeCheck();

        // SerializedObject �̓��e���X�V
        serializedObject.Update();
        // Inspector �̃R���|�[�l���g�ɕ\�����鍀�ږ�
        var text = "List";
        // �c�[���`�b�v�̃e�L�X�g
        var tooltip = "Popup list \"Selectnum\" will be made out of this.";
        // �c�[���`�b�v���胉�x���̍쐬
        var label = new GUIContent(text, tooltip);
        // �z��̒��g���\������
        var includeChildren = true;
        // ���X�g���擾
        var property = serializedObject.FindProperty(nameof(selectnumber.list));
        // �z��t�B�[���h�̍쐬
        EditorGUILayout.PropertyField(property, label, includeChildren);
        // SerializedObject �̕ύX��K�p
        serializedObject.ApplyModifiedProperties();

        // �C���f�b�N�X�ԍ���t�^����f���Q�[�g
        System.Func<string, int, string> selector = (string name, int number) => $"{number}: \r{name}";
        // �h���b�v�_�E���̍��ڂɃC���f�b�N�X�ԍ���t�^
        var list = selectnumber.list.Select(selector).ToList();
        // �d�؂�
        var divider = string.Empty;
        // ���I�����
        var unselected = "(Unselected)";
        // ���X�g�Ɏd�؂��ǉ�
        list.Add(divider);
        // ���I����Ԃ�ǉ�
        list.Add(unselected);
        // Inspector �̃R���|�[�l���g�ɕ\�����鍀�ږ�
        text = "Selectnum";
        // �c�[���`�b�v�̃e�L�X�g
        tooltip = "Select one item.";
        // �c�[���`�b�v���胉�x���̍쐬
        label = new GUIContent(text, tooltip);
        // �����l�Ƃ��ĕ\�����鍀�ڂ̃C���f�b�N�X�ԍ�
        var selectedIndex = selectnumber.list.Length == 0 ? -1
            : selectnumber.index < 0 ? selectnumber.list.Length + 1
            : selectnumber.index;
        // �v���_�E�����j���[�ɓo�^���镶����z��
        var displayOptions = list.ToArray();
        // �v���_�E�����j���[�̍쐬
        var index = selectnumber.list.Length > 0 ? EditorGUILayout.Popup(label, selectedIndex, displayOptions)
            : selectedIndex;

        if (EditorGUI.EndChangeCheck())
        {// ����� Undo �ɓo�^
            // selectnumber �N���X�̕ύX���L�^
            var objectToUndo = selectnumber;
            // Undo ���j���[�ɕ\�����鍀�ږ�
            var name = "selectnumber";
            // �L�^����
            Undo.RecordObject(objectToUndo, name);
            // �C���f�b�N�X�ԍ���o�^
            selectnumber.index = index;
        }

        // ���I����Ԃ̃C���f�b�N�X�ԍ��� -1 �Ƃ���
        selectnumber.index = index > selectnumber.list.Length ? -1
            : index;

        if ((selectnumber.index != selectedIndex) && (selectnumber.index >= 0))
        {// �I���������ڂ̃C���f�b�N�X�ԍ��ƍ��ږ������O�ɏo��
            Debug.Log(selectnumber.index);
            Debug.Log(selectnumber.list[selectnumber.index]);
        }
    }
}
// �����܂ŃG�f�B�^�[��ł̂ݗL��
#endif

// ����I�u�W�F�N�g�ւ̕����ǉ����֎~
[DisallowMultipleComponent]
public class SelectNumber : MonoBehaviour

{// �G�f�B�^�[�g���̒��g
    // ���X�g
    public string[] list = { };
    // �����l�́g���I���h
    public int index = -1;

    //public int GetindexCount()
    //{
    //    return index;
    //}
}