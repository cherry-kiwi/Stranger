using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    CharacterController character;
    Animator anim;

    Vector3 MoveDir;

    public float walkSpeed = 2;
    public float runSpeed = 3.5f;
    public float gravity;    // 캐릭터에게 작용하는 중력.
    float mouseX;

    int hp = 32;
    int dagmage = 2;
    int bullet = 60;

    private void Awake()
    {
        //마우스 커서를 보이지 않게 설정, 현재 위치에 고정시킴
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        gravity = 20.0f;

        MoveDir = Vector3.zero;
        character = GetComponent<CharacterController>();
    }

    void Update()
    {
        PlayerMovement();

        mouseX += Input.GetAxis("Mouse X") * 10; //마우스 좌우
        transform.eulerAngles = new Vector3(0, mouseX, 0);

        Shooting();
    }

    void PlayerMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); // 가로축
        float moveZ = Input.GetAxis("Vertical"); // 세로축
        Vector3 move = new Vector3(moveX, 0, moveZ);
        character.Move(transform.TransformDirection(move) * Time.deltaTime * runSpeed);

        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isWalk", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isWalkR", true);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("isWalkL", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("isWalkB", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }

        MoveDir.y -= gravity * Time.deltaTime;

        // 캐릭터 움직임.
        character.Move(MoveDir * Time.deltaTime);
    }

    void Shooting()
    {
        //좌클릭하면 총에서 파티클

    }
}
