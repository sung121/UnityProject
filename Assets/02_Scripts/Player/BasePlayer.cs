using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BasePlayer : MonoBehaviour
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

    private Animator animator;

    [SerializeField] private float currentHp = 100;
    [SerializeField] private float maxHp = 100;
    [SerializeField] public float currentPower = 10f;


    private readonly int hashRun = Animator.StringToHash("Run");
    private readonly int hashJump = Animator.StringToHash("Jump");
    private readonly int hashFall = Animator.StringToHash("Fall");
    private readonly int hashPunch = Animator.StringToHash("Punch");

    IPlayerState playerState;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Walkable"))
        {
            jumped = false;
            isFalling = false;
            animator.SetBool(hashJump, false);
            animator.SetBool(hashFall, false);

        }
    }

    float previousInputVer = 0;
    float previousInputHor = 0;

    float currentInputVer = 0;
    float currentInputHor = 0;

    float threshold = 0.9f;

    bool isLeftMouseClicked = Input.GetMouseButtonDown(0);
    bool isRightMouseClicked = Input.GetMouseButtonDown(0);

    void Update()
    {
        HandleInput();
        Move();
    }

    protected virtual void HandleInput()
    {
        currentInputHor = Input.GetAxis("Horizontal");
        currentInputVer = Input.GetAxis("Vertical");


        if (Input.GetKeyUp(KeyCode.W)
           || Input.GetKeyUp(KeyCode.A)
           || Input.GetKeyUp(KeyCode.S)
           || Input.GetKeyUp(KeyCode.D))
        {
            isMoving = false;
        }

        // 이동 방향 계산



    }

    protected virtual void Move()
    {
        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0; // 카메라의 y축 회전을 제거하여 수평 방향으로 설정
        cameraForward = cameraForward.normalized;

        CaculateOverThresold();

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

    void CaculateOverThresold()
    {
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
    }

}
