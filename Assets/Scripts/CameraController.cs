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
        mouseY += Input.GetAxis("Mouse Y") * mouseSpeed; //���콺 Y��(���Ʒ�)

        mouseY = Mathf.Clamp(mouseY, -90, 90); //Mathf.Clamp(����, �����ּҰ�, �ִ밪)
        //Mathf.Clamp�� �ؼ��ϸ� �Ʒ��� ����
        //if (mouseY >= 90)
        //{
        //    mouseY = 90;
        //}
        //else if (mouseY <= -90)
        //{
        //    mouseY = -90;
        //}

        transform.localEulerAngles = new Vector3(mouseY, 0, 0); //���� ȸ���� ����
        //mouseY += �� �� �� ������ ���� +�� ���ͼ� -mouseY�� �־���� ���Ʒ� ���� �۵�
    }
}
