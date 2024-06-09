using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    Transform playerTransform;
    Rigidbody rb;
    [SerializeField] GameObject camera;

    [SerializeField] float moveSpd = 5f;
    private Vector3 moveDirection;
    private bool isMoving;

    void Start()
    {
        playerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 이동 방향 계산
        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0; // 카메라의 y축 회전을 제거하여 수평 방향으로 설정
        cameraForward = cameraForward.normalized;

        Vector3 moveDir = cameraForward * Input.GetAxis("Vertical") + camera.transform.right * Input.GetAxis("Horizontal");

        // 이동 방향이 0이 아닌지 확인
        isMoving = moveDir != Vector3.zero;

        // 이동 방향을 저장
        moveDirection = moveDir;

        // 플레이어 회전 설정
        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void FixedUpdate()
    {
        // 물리 기반 이동 처리
        if (isMoving)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpd * Time.fixedDeltaTime);
        }
        else
        {
            // 이동을 멈추는 로직
            rb.velocity = Vector3.zero;
        }
    }
}