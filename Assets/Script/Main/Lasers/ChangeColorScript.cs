using UnityEngine;
using UnityEngine.UI;
//Player�ƃ��[�U�[�̋����ɉ����Ċ댯��m�点��}�[�N�̕\���y�у��[�U�[�Ƃ̋����ɉ����ă}�[�N�̐F��ύX���鏈���B
public class ChangeColorScript : MonoBehaviour
{
    [SerializeField] float _changeColorThreshold = 10f;
    [SerializeField] Color _startColor = Color.white;
    [SerializeField] Color _endColor = Color.red;
    [SerializeField] Image WI;
    private Transform _target;
    private string tagname;
    private bool restriction;

    private void Start()
    {
        var go = GameObject.FindGameObjectWithTag("Player");

        if (go)
            _target = go.transform;
        else
            Debug.LogWarning("�^�[�Q�b�g�ƂȂ� Player �^�O�̂����I�u�W�F�N�g���V�[����ɑ��݂��܂���");
        WI.enabled = false;
        tagname = "warningarea";
    }
    private  void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.tag == tagname)
        {
            restriction = true;
            WI.enabled = true;
            if (!_target) return;
            float distance = Vector3.Distance(transform.position, _target.position);

            if (distance < _changeColorThreshold)
            {
                Color c = (_endColor * (_changeColorThreshold - distance) + _startColor * distance) / _changeColorThreshold;
                WI.color = c;
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if(restriction)
        {
            WI.enabled = false;
            restriction = false;
        }
    
    }
}