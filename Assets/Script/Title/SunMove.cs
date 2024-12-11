using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//能力開放イベント終了時に太陽の位置を更新し徐々に近寄ってきている風に見せる
//最後のフラグが返った時に太陽が近づいていてくる処理
public class SunMove : MonoBehaviour
{
    private Transform player;
    public float movespeed;
    public float rotx = 0;
    public float roty = 0;
    public float rotz = 0;
    private Vector3 FirstPos = new Vector3(1000, 500, 50);
    private Vector3 SecondPos = new Vector3(500, 300, 50);
    private Vector3 ThirdPos = new Vector3(220, 120, 50);
    public bool MPF = false;
    private bool firstPos;
    private bool secondPos;
    private bool thirdPos;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    private void Update()
    {
        transform.Rotate(new Vector3(rotx, roty, rotz));

        if (GameManager2.additionPlayerActionGetFlag && !GameManager2.floatPowerGetFlag && !firstPos)
        {
            firstPos = true;
            transform.position = FirstPos;
            

        }
        if (GameManager2.additionPlayerActionGetFlag && GameManager2.floatPowerGetFlag && !GameManager2.ALF && !secondPos)
        {
            secondPos = true;
            transform.position = SecondPos;
            

        }
        if (GameManager2.additionPlayerActionGetFlag && GameManager2.floatPowerGetFlag && GameManager2.ALF&& !thirdPos)
        {
           thirdPos = true;
           transform.position = ThirdPos;
        }
        if (MPF)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, movespeed * Time.deltaTime);
        }

    }
}
