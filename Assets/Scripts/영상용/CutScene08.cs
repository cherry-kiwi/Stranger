using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutScene08 : MonoBehaviour
{
    public Slider slider;
    public GameObject slider2;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            slider2.SetActive(true);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            slider.value -= (float)Time.deltaTime;
        }
    }
}
