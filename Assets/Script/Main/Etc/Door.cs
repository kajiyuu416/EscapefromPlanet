using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool opendoorFlag = false;
    public Animator animator;

    //プレイヤーがコライダーの範囲内に扉が開き、範囲外に出ると扉が閉まる
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            opendoorFlag = true;
        }
        animator.SetBool("character_nearby", opendoorFlag);
    }

    private void OnTriggerExit(Collider collision)
    {

        if (collision.CompareTag("Player"))
        {
            opendoorFlag = false;

        }
        animator.SetBool("character_nearby", opendoorFlag);
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            opendoorFlag = true;
        }
        animator.SetBool("character_nearby", opendoorFlag);

    }
}
