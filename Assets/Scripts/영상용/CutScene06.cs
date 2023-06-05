using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene06 : MonoBehaviour
{
    public GameObject door1;
    public GameObject door2;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            door1.SetActive(false);
            door2.SetActive(false);
            explosion.SetActive(true);
        }
    }
}
