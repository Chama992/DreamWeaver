using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerEntityController : MonoBehaviour
{
    #region PlayerInput

    private PlayerInputCheck playerInputCheck;

    #endregion

    #region PlayerStates

    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerHookState HookState { get; private set; }

    #endregion

    [Header("Move Info")] [SerializeField] public bool enAbleAcceleraton;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float moveAcceleration;
    [SerializeField] public float moveDecelerate;

    [Header("JumpFall Info")] [SerializeField]
    public float jumpFirstVelo;

    [SerializeField] public float jumpMaxVelo;
    [SerializeField] public float jumpAcceleration;
    [SerializeField] public float jumpMaxHeight;
    [SerializeField] public float jumpLerpSpeed;
    [SerializeField] public int jumpBufferFrameCount;
    [SerializeField] public int jumpGraceFrameCount;
    [SerializeField] public float airMoveSpeed;

    #region Components

    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }

    #endregion

    [Header("CollisionCheck Info")] [Tooltip("wallcheck box collisiton up point")]
    public Transform groundCheckPoint;

    public float groundCheckWidth;
    public float groundCheckHeight;
    public bool isOnGround;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    #region FacingDir

    public int facingDir { get; private set; } = 1;
    public bool facingRight { get; private set; } = true;

    #endregion

    private void Awake()
    {
        playerInputCheck = new PlayerInputCheck();
        Anim = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, "Idle", playerInputCheck);
        MoveState = new PlayerMoveState(this, StateMachine, "Move", playerInputCheck);
        AirState = new PlayerAirState(this, StateMachine, "Jump", playerInputCheck);
        JumpState = new PlayerJumpState(this, StateMachine, "Jump", playerInputCheck);
        DeadState = new PlayerDeadState(this, StateMachine, "Dead", playerInputCheck);
        // HookState = new PlayerHookState(this, StateMachine, "Jump",playerInputCheck);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void OnEnable()
    {
        playerInputCheck.Enable();
    }

    private void OnDisable()
    {
        playerInputCheck.Disable();
    }

    private void Update()
    {
        StateMachine.currentState.Update();
        isOnGround = IsGroundChecked();
    }

    #region Collision

    public bool IsGroundChecked()
    {
        bool isBoxGround = Physics2D.BoxCast(groundCheckPoint.position,
            new Vector2(groundCheckWidth, groundCheckHeight), 0, Vector2.down, 0, whatIsGround);
        return isBoxGround;
    }

    public bool IsWallChecked() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    private void OnDrawGizmos()
    {
        GizmosTool.DrawGroundCheck(groundCheckPoint.position, groundCheckWidth, groundCheckHeight, Color.red);
        GizmosTool.DrawLine(new Vector3(transform.position.x - 0.3f, transform.position.y + this.jumpMaxHeight),
            new Vector3(transform.position.x + 0.3f, transform.position.y + this.jumpMaxHeight), Color.green);
    }

    #endregion

    #region Flip

    public void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipControl(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }

    #endregion

    #region Velocity

    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        Rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipControl(_xVelocity);
    }

    public virtual void SetVelocity(Vector2 velocity)
    {
        Rb.velocity = velocity;
        FlipControl(velocity.x);
    }

    #endregion

    public void AnimationTrigger() => this.StateMachine.currentState.AnimationFinishTrigger();
}