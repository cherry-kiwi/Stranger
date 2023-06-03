using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5;    
    float h, v;                     // 가로축과 세로축을 담을 변수를 전역변수로 생성. FixedUpdate에서 직접 생성하지 않은 이유는
                                    // 이후 다른 함수에서도 접근할 것이기 때문.

    void FixedUpdate()
    {
        h = Input.GetAxis("Horizontal"); // 가로축
        v = Input.GetAxis("Vertical"); // 세로축

        transform.position += new Vector3(h, 0, v) * Speed * Time.deltaTime;
    }
}