using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//能力開放イベント終了時に太陽の位置を更新し徐々に近寄ってきている風に見せる
//最後のフラグが返った時に太陽が近づいていてくる処理
public class SunMove : MonoBehaviour
{
    [SerializeField] Vector3 center = Vector3.zero;
    [SerializeField] Vector3 axis = Vector3.up;
    [SerializeField] float period = 2;
    [SerializeField] Transform target;
    public float movespeed;
    public float rotx = 0;
    public float roty = 0;
    public float rotz = 0;
    private Vector3 FirstPos = new Vector3(1000, 500, 50);
    private Vector3 SecondPos = new Vector3(700, 300, 50);
    private Vector3 ThirdPos = new Vector3(350, 150, 50);
    public bool MPF = false;

    private bool firstPos;
    private bool secondPos;
    private bool thirdPos;

    private void Update()
    {
        transform.Rotate(new Vector3(rotx, roty, rotz));
        // 中心点centerの周りを、軸axisで、period周期で円運動
        transform.RotateAround(center,axis,360 / period * Time.deltaTime);

        if (GameManager2.AGF && !GameManager2.FGF)
        {
            if (!firstPos)
            {
                firstPos = true;
                transform.position = FirstPos;
            }

        }
        if (GameManager2.AGF && GameManager2.FGF && !GameManager2.ALF)
        {
            if (!secondPos)
            {
                secondPos = true;
                transform.position = SecondPos;
            }

        }
        if (GameManager2.AGF && GameManager2.FGF && GameManager2.ALF)
        {
            if (!thirdPos)
            {
                thirdPos = true;
                transform.position = ThirdPos;
            }
        }
        if (MPF)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, movespeed * Time.deltaTime);
        }

    }
}
