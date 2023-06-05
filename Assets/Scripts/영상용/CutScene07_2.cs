using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene07_2 : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.O))
        {
            animator.SetBool("isBurst", true);
        }
    }
}
