using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int direction;      // 0 = stay, 1 = right, -1 = left       플레이어의 점프 방향 설정
    private float ySpeed;    // 점프 y 스피드
    private float xSpeed;    // 점프 x 스피드
    private float moveSpeed;
    private float inputTime;    // 플레이어 점프 키 입력 시간
    private bool isJumped;
    private bool isCharged;

    Rigidbody2D rigidbody2D;
    Animator animator;
    
    public Camera cameraPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        direction = 0;
        ySpeed = 6f;

        xSpeed = 8f;
        moveSpeed = 3f;
        inputTime = 1.5f;
        isJumped = true;
        isCharged = false;
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Dire();
        Move();
        CameraMove();
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && !isJumped)
        {
            animator.SetInteger("State", 1);
            isCharged = true;
            inputTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space) && !isJumped)
        {
            animator.SetInteger("State", 0);
            rigidbody2D.AddForce(new Vector2(direction * xSpeed, inputTime * ySpeed), ForceMode2D.Impulse);
            inputTime = 1.5f;
            direction = 0;
            isJumped = true;
            isCharged = false;
        }
        if(inputTime >= 3)
        {
            inputTime = 3;
        }
    }

    void Move()
    {
        if (!isJumped && !isCharged)
        {
            Vector3 moveVelocity = Vector3.zero;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveVelocity = Vector3.right;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveVelocity = Vector3.left;
            }
            transform.position += moveVelocity * moveSpeed * Time.deltaTime;
        }
        
    }
    void Dire()
    {
        if (isCharged)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                direction = 1;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                direction = -1;
            }
        }
    }

    void CameraMove()
    {
        for(int i = -7; i < 1000; i += 14)
        {
            if(transform.position.y < i)
            {
                cameraPlayer.transform.position = new Vector3(0, i - 7, -10);
                break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Tilemap")
        {
            isJumped = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.name == "Tilemap")
        {
            isJumped = true;
        }
    }
}
