using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool OpendoorFlag = false;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            OpendoorFlag = true;
        }
        animator.SetBool("character_nearby", OpendoorFlag);
    }

    private void OnTriggerExit(Collider collision)
    {

        if (collision.CompareTag("Player"))
        {
            OpendoorFlag = false;

        }
        animator.SetBool("character_nearby", OpendoorFlag);
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            OpendoorFlag = true;
        }
        animator.SetBool("character_nearby", OpendoorFlag);

    }
}
