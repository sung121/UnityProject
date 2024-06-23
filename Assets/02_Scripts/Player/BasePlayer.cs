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

        // �̵� ���� ���



    }

    protected virtual void Move()
    {
        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0; // ī�޶��� y�� ȸ���� �����Ͽ� ���� �������� ����
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

        // �̵� ������ ����
        moveDirection = moveDir;

        // �÷��̾� ȸ�� ����
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
        // ���� �Է°��� 0 &&
        // ���� �Է°��� ���� �Է°����� ���̰� 0.1���� Ŭ ���(�� �Է°� ��ȭ�� Ŭ ���)

        if (currentInputHor == 0 && Mathf.Abs(currentInputHor - previousInputHor) > threshold)
        {
            // ���� �Է� �� ����
            currentInputHor = previousInputHor;
        }
        else
        {
            previousInputHor = currentInputHor;
        }


        if (currentInputVer == 0 && Mathf.Abs(currentInputVer - previousInputVer) > threshold)
        {
            // ���� �Է� �� ����
            currentInputVer = previousInputVer;
        }
        else
        {
            previousInputVer = currentInputVer;
        }
    }

}
