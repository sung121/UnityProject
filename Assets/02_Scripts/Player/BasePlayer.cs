using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasePlayer : MonoBehaviour
{
    public Transform PlayerTransform { get; set; }
    public Rigidbody rb { get; set; }
    public Animator animator { get; set; }
    public Vector3 moveDirection { get; set; }
    public bool isMoving { get; set; }
    private bool isJumping {get; set;}
    private bool jumped { get; set;}
    private bool isFalling { get; set;}
    private bool isAttacking { get; set;}

    [SerializeField] public GameObject camera;
    [SerializeField] public float moveSpd = 5f;
    [SerializeField] private float jumpSpd = 10f;
    [SerializeField] private float currentHp = 100;
    [SerializeField] private float maxHp = 100;
    [SerializeField] public float currentPower = 10f;

    private readonly int hashRun = Animator.StringToHash("Run");
    private readonly int hashJump = Animator.StringToHash("Jump");
    private readonly int hashFall = Animator.StringToHash("Fall");
    private readonly int hashPunch = Animator.StringToHash("Punch");

    public int HashRun => hashRun;
    public int HashJump => hashJump;
    public int HashFall => hashFall;
    public int HashPunch => hashPunch;

    private PlayerState currentState;

    public PlayerState IdleState { get; private set; }
    public PlayerState RunState { get; private set; }
    public PlayerState JumpState { get; private set; }
    public PlayerState FallState { get; private set; }
    public PlayerState AttackState { get; private set; }

    void Start()
    {
        PlayerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        IdleState = new IdleState();
        RunState = new RunState();
        JumpState = new JumpState();
        FallState = new FallState();
        AttackState = new AttackState();

        TransitionToState(IdleState);
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }

    void Update()
    {
        currentState.UpdateState();
        CheckState();
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnterState(this, collision);
    }

    public void TransitionToState(PlayerState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    private void HandleInput()
    {

    }

    void CheckState()
    {
        if (isAttacking)
        {
            TransitionToState(AttackState);
            Debug.Log("Attack Animation");
        }
        else if (isFalling)
        {
            TransitionToState(FallState);
            Debug.Log("Fall Animation");
        }
        else if (isJumping)
        {
            TransitionToState(JumpState);
            Debug.Log("Jump Animation");
        }
        else if (isMoving)
        {
            TransitionToState(AttackState);
            Debug.Log("Run Animation");
        }
        else
        {
            TransitionToState(IdleState);
            Debug.Log("Idle Animation");
        }
    }

}
