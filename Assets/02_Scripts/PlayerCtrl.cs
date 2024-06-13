using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCtrl : MonoBehaviour
{
    Transform playerTransform;
    Rigidbody rb;
    [SerializeField] Camera camera;

    [SerializeField] float moveSpd = 5f;
    [SerializeField] float jumpSpd = 10f;
    private Vector3 moveDirection;
    private bool isMoving;
    private bool isjumping;
    private bool jumped;

    [SerializeField] float power = 10f;


    private State animState = State.IDLE;
    private Animator animator;

    private readonly int hashRun = Animator.StringToHash("Run");
    private readonly int hashJump = Animator.StringToHash("Jump");
    private readonly int hashFall = Animator.StringToHash("Fall");

    public enum State
    {
        IDLE,
        Run,
        JUMP,
        FALL,
        ATTACK,
        DIE
    }


    void Start()
    {
        playerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Move();
        Jump();
        Attack();
    }

    void FixedUpdate()
    {

        // 물리 기반 이동 처리
        if (isMoving)
        {
            //rb.AddRelativeForce(moveSpd * 100 * Time.fixedDeltaTime * Vector3.forward);
            //Debug.Log(moveDirection.magnitude);
            rb.MovePosition(rb.position + moveDirection * moveSpd * Time.fixedDeltaTime);
            animState = State.Run;
        }
        else
        {
            animState = State.IDLE;
        }

        if (isjumping)
        {
            animState = State.JUMP;
            rb.AddRelativeForce(Vector3.up * jumpSpd * 2000 * Time.fixedDeltaTime);
            isjumping = false;

        }

        ExecuteStateAction();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!jumped) 
        {
            return;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walkable"))
        {
            jumped = false;
            Debug.Log("Reached walkable Obj!");
        }
    }

    void Move()
    {
        // 이동 방향 계산
        
        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0; // 카메라의 y축 회전을 제거하여 수평 방향으로 설정
        cameraForward = cameraForward.normalized;

        Vector3 moveDir = (cameraForward * Input.GetAxis("Vertical") + camera.transform.right * Input.GetAxis("Horizontal")).normalized;
        

        // 이동 방향이 0이 아닌지 확인
        isMoving = moveDir != Vector3.zero;

        // 이동 방향을 저장
        moveDirection = moveDir;

        if(Input.GetKeyUp(KeyCode.W))
        {
            isMoving = false;
        }

        // 플레이어 회전 설정
        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, Time.deltaTime * 10f);
            return;
        }

    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !jumped)
        {
            jumped = true;
            isjumping = true;
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }
    void ReceiveInput()
    {
        
    }

    void ExecuteStateAction()
    {
        switch (animState)
        {
            case State.IDLE:
                animator.SetBool(hashRun, false);
                break;

            case State.Run:

                animator.SetBool(hashRun, true);
                break;

            case State.JUMP:

                animator.SetTrigger(hashJump);

                break;
            case State.FALL:
                animator.SetBool(hashJump, false);
                animator.SetBool(hashFall, true);

                break;
            default:
                break;
        }
    }



}