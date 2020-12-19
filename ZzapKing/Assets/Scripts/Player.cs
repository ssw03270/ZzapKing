using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int direction;      // 0 = stay, 1 = right, -1 = left       플레이어의 점프 방향 설정
    private float ySpeed;       // 점프 y 스피드
    private float xSpeed;       // 점프 x 스피드
    private float moveSpeed;    // 플레이어 이동 속도
    private float inputTime;    // 플레이어 점프 키 입력 시간
    private float cameraSize;   // 카메라 사이즈
    private bool isJumped;      // 공중에 있는지 확인
    private bool isCharged;     // 차징하는 중인지 확인

    public Camera cameraPlayer;
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        direction = 0;
        ySpeed = 6f;
        xSpeed = 8f;
        moveSpeed = 3f;
        inputTime = 1f;
        cameraSize = 7f;

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

    void Jump()     // 플레이어 점프 함수
    {
        if (Input.GetKey(KeyCode.Space) && !isJumped)       // 차징 시작
        {
            animator.SetInteger("State", 1);                // 차징 애니메이션
            isCharged = true;
            inputTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space) && !isJumped)     // 차징 끝, 점프 시작
        {
            animator.SetInteger("State", 0);                // 기본 애니메이션
            rigidbody2D.AddForce(new Vector2(direction * xSpeed, inputTime * ySpeed), 
                ForceMode2D.Impulse);                       // 점프 힘 가함
            inputTime = 1.5f;
            direction = 0;
            isJumped = true;
            isCharged = false;
        }
        if(inputTime >= 3)                                  // 입력 시간 3초 (1.5초) 제한
        {
            inputTime = 3;
        }
    }

    void Move()     // 플레이어 이동 함수
    {
        if (!isJumped && !isCharged)                    // 공중 아니고 차징 중이지 않으면
        {
            Vector3 moveVelocity = Vector3.zero;
            if (Input.GetKey(KeyCode.RightArrow))       // 오른쪽 이동
            {
                moveVelocity = Vector3.right;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))   // 왼쪽 이동
            {
                moveVelocity = Vector3.left;
            }
            transform.position += moveVelocity * moveSpeed * Time.deltaTime;
        }
        
    }
    void Dire()     // 플레이어 점프 방향 설정 함수
    {
        if (isCharged)                                  // 차징 중일 때만 실행
        {
            if (Input.GetKey(KeyCode.RightArrow))       // 오른쪽 방향키 입력
            {
                direction = 1;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))   // 왼쪽 방향키 입력
            {
                direction = -1;
            }
        }
    }

    void CameraMove()       // 카메라 이동
    {
        for(float i = -1 * cameraSize; i < 1000; i += 2 * cameraSize)                   // 플레이어 위치 확인 및 카메라 위치 설정
        {
            if(transform.position.y < i)
            {
                cameraPlayer.transform.position = new Vector3(0, i - cameraSize, -10);
                break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Tilemap")      // 타일맵과 충돌
        {
            isJumped = false;                           // 공중에 떠있지 않음
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.name == "Tilemap")      // 타일맵에서 벗어남
        {
            isJumped = true;                            // 공중에 떠있음
        }
    }
}
