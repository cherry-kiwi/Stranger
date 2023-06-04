using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSpeed = 10;
    float mouseY = 10;

    void Update()
    {
        mouseY += Input.GetAxis("Mouse Y") * mouseSpeed; //마우스 Y축(위아래)

        mouseY = Mathf.Clamp(mouseY, -90, 90); //Mathf.Clamp(변수, 제한최소값, 최대값)
        //Mathf.Clamp는 해석하면 아래와 같음
        //if (mouseY >= 90)
        //{
        //    mouseY = 90;
        //}
        //else if (mouseY <= -90)
        //{
        //    mouseY = -90;
        //}

        transform.localEulerAngles = new Vector3(mouseY, 0, 0); //로컬 회전값 조정
        //mouseY += 를 할 때 무조건 값이 +가 나와서 -mouseY로 넣어줘야 위아래 정상 작동
    }
}
