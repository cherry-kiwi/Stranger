using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class CutScene02 : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Shooting();
    }

    void Shooting()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("isIdle", true);
        }
    }
}