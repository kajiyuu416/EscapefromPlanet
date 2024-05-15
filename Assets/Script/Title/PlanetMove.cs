using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMove : MonoBehaviour
{
    // ’†S“_
    [SerializeField] private Vector3 _center = Vector3.zero;
    // ‰ñ“]²
    [SerializeField] private Vector3 _axis = Vector3.up;
    // ‰~‰^“®üŠú
    [SerializeField] private float _period = 2;
    public float movespeed;
    public float rotx = 0;
    public float roty = 0;
    public float rotz = 0;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(rotx, roty, rotz));
        // ’†S“_center‚Ìü‚è‚ğA²axis‚ÅAperiodüŠú‚Å‰~‰^“®
        transform.RotateAround(
            _center,
            _axis,
            360 / _period * Time.deltaTime
        );

    }
}
