using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMove : MonoBehaviour
{
    [SerializeField] Vector3 center = Vector3.zero;
    [SerializeField] Vector3 axis = Vector3.up;
    [SerializeField] float period;
    public float movespeed;
    public float rotx = 0;
    public float roty = 0;
    public float rotz = 0;
    // 中心点centerの周りを、軸axisで、period周期で円運動
    private void Update()
    {
        transform.Rotate(new Vector3(rotx, roty, rotz));
        transform.RotateAround(center,axis, 360 / period * Time.deltaTime);

    }
}
