using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int direction;      // 0 = stay, 1 = right, -1 = left       플레이어의 점프 방향 설정
    private float jumpSpeed;    // 점프 스피드
    private float moveSpeed;
    private float inputTime;    // 플레이어 점프 키 입력 시간
    private bool isJumped;
    private bool isCharged;

    Rigidbody2D rigidbody2D;

    public Camera cameraPlayer;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        direction = 0;
        jumpSpeed = 5f;
        moveSpeed = 3f;
        inputTime = 1.5f;
        isJumped = false;
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
        if (Input.GetKey(KeyCode.Space))
        {
            isCharged = true;
            inputTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space) && direction != 0 && !isJumped)
        {
            rigidbody2D.AddForce(new Vector2(direction * inputTime * 2, inputTime * jumpSpeed), ForceMode2D.Impulse);
            Debug.Log(inputTime);
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
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = 1;
        }else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = -1;
        }
    }

    void CameraMove()
    {
        if (!isJumped)
        {
            Vector3 targetPosition = new Vector3(0, transform.position.y + 5, -10);
            cameraPlayer.transform.position = Vector3.MoveTowards(cameraPlayer.transform.position, targetPosition, Time.deltaTime * 3);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Tilemap")
        {
            isJumped = false;
        }
    }
}
