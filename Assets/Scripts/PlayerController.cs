using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController character;
    Animator anim;

    Vector3 MoveDir;

    public float walkSpeed = 2;
    public float runSpeed = 3.5f;
    public float gravity;    // 캐릭터에게 작용하는 중력.
    float mouseX;
    float mouseSensitivity = 5;

    public float zeroMove = 0;
    float shotDelay = 0.6f;

    int hp = 32;
    int dagmage = 2;
    int bullet = 60;

    public GameObject gunParticle;

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
        if (zeroMove <= 0)
        {
            PlayerMovement();
        }
        else
        {
            zeroMove -= 1 * Time.deltaTime;
        }

        //카메라 조작
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity; //마우스 좌우
        transform.eulerAngles = new Vector3(0, mouseX, 0);

        Shooting();
    }

    void PlayerMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); // 가로축
        float moveZ = Input.GetAxis("Vertical"); // 세로축
        Vector3 move = new Vector3(moveX, 0, moveZ);

        if (!(move == Vector3.zero))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                character.Move(transform.TransformDirection(move) * Time.deltaTime * runSpeed);
                anim.SetBool("isRun", true);
                anim.SetBool("isWalk", false);
            }
            else
            {
                character.Move(transform.TransformDirection(move) * Time.deltaTime * walkSpeed);
                anim.SetBool("isWalk", true);
                anim.SetBool("isRun", false);
            }
        }
        else
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
        }

        MoveDir.y -= gravity * Time.deltaTime;

        // 캐릭터 움직임.
        character.Move(MoveDir * Time.deltaTime);
    }

    void Shooting()
    {
        //좌클릭하면 총에서 파티클
        if (Input.GetMouseButton(0))
        {
            zeroMove = shotDelay;
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
            anim.SetBool("isShot", true);

            gunParticle.SetActive(true);
        }
        else if (zeroMove <= 0)
        {
            anim.SetBool("isShot", false);

            gunParticle.SetActive(false);
        }
    }
}