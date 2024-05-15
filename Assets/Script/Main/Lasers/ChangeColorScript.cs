using UnityEngine;
using UnityEngine.UI;

public class ChangeColorScript : MonoBehaviour
{
    [SerializeField] float _changeColorThreshold = 10f;
    [SerializeField] Color _startColor = Color.white;
    [SerializeField] Color _endColor = Color.red;

    [SerializeField] Image WI;
    Transform _target;
    string tagname;
    bool restriction;

    void Start()
    {
        var go = GameObject.FindGameObjectWithTag("Player");

        if (go)
            _target = go.transform;
        else
            Debug.LogWarning("�^�[�Q�b�g�ƂȂ� Player �^�O�̂����I�u�W�F�N�g���V�[����ɑ��݂��܂���");
        WI.enabled = false;
        tagname = "warningarea";
    }

    //Player�ƃ��[�U�[�̋����ɉ����Ċ댯��m�点��}�[�N�̕\���y�у}�[�N�̐F��ύX���鏈���B
     void OnTriggerStay(Collider collision)
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