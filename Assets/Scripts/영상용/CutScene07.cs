using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene07 : MonoBehaviour
{
    Animator animator;
    public GameObject explosion;
    public GameObject human;
    public GameObject monster;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            explosion.SetActive(true);
            human.SetActive(true);
            monster.SetActive(false);
        }
        else if (Input.GetKey(KeyCode.I))
        {
            animator.SetBool("isShot", true);
        }
        else
        {
            animator.SetBool("Look Around", true);
        }
    }
}
