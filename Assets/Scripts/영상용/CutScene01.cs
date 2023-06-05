using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class CutScene01 : MonoBehaviour
{
    Animator animator;
    public ParticleSystem particle;

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
            animator.SetBool("isShot", true);
            particle.Play();
        }
    }
}