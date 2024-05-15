using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMove : MonoBehaviour
{
    // ���S�_
    [SerializeField] private Vector3 _center = Vector3.zero;
    // ��]��
    [SerializeField] private Vector3 _axis = Vector3.up;
    // �~�^������
    [SerializeField] private float _period = 2;
    public float movespeed;
    public float rotx = 0;
    public float roty = 0;
    public float rotz = 0;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(rotx, roty, rotz));
        // ���S�_center�̎�����A��axis�ŁAperiod�����ŉ~�^��
        transform.RotateAround(
            _center,
            _axis,
            360 / _period * Time.deltaTime
        );

    }
}
