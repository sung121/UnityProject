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
        // �̵� ���� ���
        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0; // ī�޶��� y�� ȸ���� �����Ͽ� ���� �������� ����
        cameraForward = cameraForward.normalized;

        Vector3 moveDir = cameraForward * Input.GetAxis("Vertical") + camera.transform.right * Input.GetAxis("Horizontal");

        // �̵� ������ 0�� �ƴ��� Ȯ��
        isMoving = moveDir != Vector3.zero;

        // �̵� ������ ����
        moveDirection = moveDir;

        // �÷��̾� ȸ�� ����
        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void FixedUpdate()
    {
        // ���� ��� �̵� ó��
        if (isMoving)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpd * Time.fixedDeltaTime);
        }
        else
        {
            // �̵��� ���ߴ� ����
            rb.velocity = Vector3.zero;
        }
    }
}