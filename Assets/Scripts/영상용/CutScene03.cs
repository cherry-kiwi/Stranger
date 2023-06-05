using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class CutScene03 : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Attacking();
    }

    void Attacking()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("isAttack", true);
        }
    }
}
