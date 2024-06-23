using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerCtrl : MonoBehaviour
{
    Transform playerTransform;
    Rigidbody rb;
    [SerializeField] Camera camera;

    [SerializeField] float moveSpd = 5f;
    [SerializeField] float jumpSpd = 10f;
    private Vector3 moveDirection;
    private bool isMoving = false;
    private bool isJumping = false;
    private bool jumped = false;
    private bool isFalling = false;
    private bool isAttacking = false;

    private State animState = State.IDLE;
    private Animator animator;

    [SerializeField] private float currentHp = 100;
    [SerializeField] private float maxHp = 100;
    [SerializeField] public float currentPower = 10f;


    private readonly int hashRun = Animator.StringToHash("Run");
    private readonly int hashJump = Animator.StringToHash("Jump");
    private readonly int hashFall = Animator.StringToHash("Fall");
    private readonly int hashPunch = Animator.StringToHash("Punch");

    public enum State
    {
        IDLE,
        RUN,
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

    void FixedUpdate()
    {

        if (isAttacking)
        {
            return;
        }

        if (rb.velocity.y < -1)
        {
            isJumping = false;
            isFalling = true;
            animator.SetBool(hashRun, false);
            Debug.Log("떨어지는 중");
        }
        // 물리 기반 이동 처리

        if (isMoving)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpd * Time.fixedDeltaTime);
        }
    }

    void Update()
    {

        ExecuteStateAction();
        CheckState();
        Attack();
        if (isAttacking)
        {
            ExecuteStateAction();
            CheckState();
            return;
        }
        Jump();
        if (isJumping || isFalling)
        {
            if (Input.GetKeyUp(KeyCode.W) ||
                Input.GetKeyUp(KeyCode.A) ||
                Input.GetKeyUp(KeyCode.S) ||
                Input.GetKeyUp(KeyCode.D)
                )
            {
                animator.SetBool(hashRun, false);
            }
            ExecuteStateAction();
            CheckState();
            return;
        }
        Move();
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Walkable"))
        {
            jumped = false;
            isFalling = false;
            animator.SetBool(hashJump, false);
            animator.SetBool(hashFall, false);
            InitMoveInput();
        }
    }

    private void InitMoveInput()
    {
        previousInputHor = 0;
        previousInputVer = 0;
        currentInputHor = 0;
        currentInputVer = 0;
    }

    private void OnCollisionStay(Collision collision)
    {
    }

    float previousInputVer = 0;
    float previousInputHor = 0;

    float currentInputVer = 0;
    float currentInputHor = 0;

    float threshold = 0.9f;
    void Move()
    {

        if (Input.GetKeyUp(KeyCode.W)
            || Input.GetKeyUp(KeyCode.A)
            || Input.GetKeyUp(KeyCode.S)
            || Input.GetKeyUp(KeyCode.D))
        {
            isMoving = false;
        }

        // 이동 방향 계산

        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0; // 카메라의 y축 회전을 제거하여 수평 방향으로 설정
        cameraForward = cameraForward.normalized;


        currentInputHor = Input.GetAxis("Horizontal");
        currentInputVer = Input.GetAxis("Vertical");

        //Debug.Log("CurrentInputVer: " + currentInputVer);
        //Debug.Log("CurrentInputHor: " + currentInputHor);
        // 현재 입력값이 0 &&
        // 현재 입력값이 이전 입력값과의 차이가 0.1보다 클 경우(즉 입력값 변화가 클 경우)
        if (currentInputHor == 0 && Mathf.Abs(currentInputHor - previousInputHor) > threshold)
        {
            // 이전 입력 값 유지
            currentInputHor = previousInputHor;
        }
        else
        {
            previousInputHor = currentInputHor;
        }


        if (currentInputVer == 0 && Mathf.Abs(currentInputVer - previousInputVer) > threshold)
        {
            // 이전 입력 값 유지
            currentInputVer = previousInputVer;
        }
        else
        {
            previousInputVer = currentInputVer;
        }


        Vector3 moveVer = cameraForward * currentInputVer;
        Vector3 moveHor = camera.transform.right * currentInputHor;

        Vector3 moveDir = (moveVer + moveHor).normalized;


        if (moveDir == Vector3.zero)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        // 이동 방향을 저장
        moveDirection = moveDir;

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
            rb.AddRelativeForce(Vector3.up * jumpSpd * 37);
            isJumping = true;
            jumped = true;
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
            isMoving = false;
            isJumping = false;
            InitMoveInput();
        }
    }
    void ExecuteStateAction()
    {
        AnimatorStateInfo animStateInfo;

        switch (animState)
        {
            case State.IDLE:
                animator.SetBool(hashRun, false);
                animator.SetBool(hashJump, false);

                animator.SetBool(hashFall, false);
                break;

            case State.RUN:

                animator.SetBool(hashRun, true);
                animator.SetBool(hashJump, false);

                animator.SetBool(hashFall, false);
                break;

            case State.JUMP:

                animator.SetBool(hashJump, true);

                animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (!(animStateInfo.IsName("Jump")))
                {
                    return;
                }

                if (animStateInfo.normalizedTime >= 0.9f)
                {
                    isJumping = false;
                    animator.SetBool(hashJump, false);
                }

                break;
            case State.FALL:
                animator.SetBool(hashFall, true);

                break;

            case State.ATTACK:
                animator.SetBool(hashPunch, true);
                animator.SetBool(hashRun, false);

                animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (!(animStateInfo.IsName("PunchLeft") || animStateInfo.IsName("PunchRight")))
                {
                    return;
                }

                if (animStateInfo.normalizedTime >= 0.9f)
                {
                    isAttacking = false;
                    animator.SetBool(hashPunch, false);
                }


                break;
            default:
                break;
        }
    }

    void CheckState()
    {
        if (isAttacking)
        {
            animState = State.ATTACK;
            Debug.Log("Attack Animation");
        }
        else if (isFalling)
        {
            animState = State.FALL;
            Debug.Log("Fall Animation");
        }
        else if (isJumping)
        {
            animState = State.JUMP;
            Debug.Log("Jump Animation");
        }
        else if (isMoving)
        {
            animState = State.RUN;
            Debug.Log("Run Animation");
        }
        else
        {
            animState = State.IDLE;
            Debug.Log("Idle Animation");
        }
    }

}