using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMove : MonoBehaviour
{
    // ’†S“_
    [SerializeField] private Vector3 _center = Vector3.zero;
    // ‰ñ“]²
    [SerializeField] private Vector3 _axis = Vector3.up;
    // ‰~‰^“®üŠú
    public bool MPF = false;
    [SerializeField] private float _period = 2;
    [SerializeField] Transform target;
    public float movespeed;
    public float rotx = 0;
    public float roty = 0;
    public float rotz = 0;

    private float fposx = 1000;
    private float fposy = 500;
    private float fposz = 50;

    private float sposx = 700;
    private float sposy = 300;
    private float sposz = 50;

    private float tposx = 350;
    private float tposy = 150;
    private float tposz = 50;

    bool a;
    bool b;
    bool c;

    void Update()
    {
        transform.Rotate(new Vector3(rotx, roty, rotz));
        // ’†S“_center‚Ìü‚è‚ğA²axis‚ÅAperiodüŠú‚Å‰~‰^“®
        transform.RotateAround(
            _center,
            _axis,
            360 / _period * Time.deltaTime
        );
        if (GameManager2.AGF && !GameManager2.FGF)
        {
            if (!a)
            {
                a = true;
                transform.position = new Vector3(fposx, fposy, fposz);
            }

        }
        if (GameManager2.AGF && GameManager2.FGF && !GameManager2.ALF)
        {
            if (!b)
            {
                b = true;
                transform.position = new Vector3(sposx, sposy, sposz);
            }

        }
        if (GameManager2.AGF && GameManager2.FGF && GameManager2.ALF)
        {
            if (!c)
            {
                c = true;
                transform.position = new Vector3(tposx, tposy, tposz);
            }
        }
        if (MPF)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, movespeed * Time.deltaTime);
        }

    }
}
