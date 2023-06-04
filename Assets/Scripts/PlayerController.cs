using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController character;

    public float Speed = 5;
    float mouseX;

    private void Awake()
    {
        //마우스 커서를 보이지 않게 설정, 현재 위치에 고정시킴
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        character = GetComponent<CharacterController>();
    }

    void Update()
    {
        PlayerMovement();
        mouseX += Input.GetAxis("Mouse X") * 10; //마우스 좌우
        transform.eulerAngles = new Vector3(0, mouseX, 0);
    }

    void PlayerMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); // 가로축
        float moveZ = Input.GetAxis("Vertical"); // 세로축

        Vector3 move = new Vector3(moveX, 0, moveZ);
        character.Move(transform.TransformDirection(move) * Time.deltaTime * 10);
    }
}