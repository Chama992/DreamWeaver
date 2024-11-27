using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region PlayerStates
    public PlayerStateMachine StateMachine { get; private set;  }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAirState AirState { get; private set;}
    public PlayerJumpState JumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerHookState HookState { get; private set; }
    #endregion
    [Header("Move Info")]
    [SerializeField] public float moveSpeed;
    [Header("JumpFall Info")]
    [SerializeField] public float jumpForce;
    [SerializeField] public float airMoveSpeed;
    [Header("WallSlide Info")]
    [SerializeField] public float wallSlideYSlowSpeedCoefficient;
    [SerializeField] public float wallSlideYFastSpeedCoefficient;
    [SerializeField] public float wallJumpXMoveSpeed;
    [SerializeField] public float wallJumpDuration;
    [SerializeField] public float wallJumpForce;
    [Header("Dash Info")]
    [SerializeField] public float dashDuration;
    [SerializeField] public float dashSpeed;
    [SerializeField] private float dashCoolDown;
    private float dashUsageTimer;
    public float dashDir { get; private set; }
    #region Components
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    #endregion
    [Header("CollisionCheck Info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    #region FacingDir
    public int facingDir { get; private set; } = 1;
    public bool facingRight { get; private set; } = true;
    #endregion
    #region Props
    public PlayerProps Props { get; private set; } = new PlayerProps();
    private List<KeyCode> propKeys = new List<KeyCode>() { KeyCode.Alpha1 ,KeyCode.Alpha2,KeyCode.Alpha3,KeyCode.Alpha4,KeyCode.Alpha5,KeyCode.Alpha6 };
    #endregion

    #region Hook
    public float hookSpeed;
    #endregion
    private  void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        //use statemachine,player can switch to any state
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        AirState = new PlayerAirState(this, StateMachine,"Jump");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        DashState = new PlayerDashState(this, StateMachine, "Dash");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, "Jump");
        DeadState = new PlayerDeadState(this, StateMachine, "Dead");
        HookState = new PlayerHookState(this, StateMachine, "Jump");
        Props.Initialize(this);
    }
    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }
    private  void Update()
    {
        StateMachine.currentState.Update();
        CheckDashActive();
        UsePropDetect();
    }
    #region Collision
    public  bool IsGroundChecked() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public  bool IsWallChecked() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    private  void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
    }
    #endregion
    #region Dash
    private void CheckDashActive()
    {
        dashUsageTimer -= Time.deltaTime;
        if (this.IsWallChecked()) 
            return;
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCoolDown;
            // ????????????????
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
                dashDir = facingDir;
            if (dashDir != facingDir)
                Flip();
            StateMachine.ChangeState(this.DashState);
        }
    }
    #endregion
    #region Flip
    public void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    private  void FlipControl(float _x)
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

    #region Prop
    private void UsePropDetect()
    {
        if (Input.anyKeyDown)
        {
            for (int i = 0; i < propKeys.Count; i++)
            {
                if (Input.GetKeyDown(propKeys[i]))
                {
                    Props.UsePropByIndex(CheckPropToUse(propKeys[i]));
                    break;
                }
            }
        }
    }

    private int CheckPropToUse(KeyCode _key)
    {
        string key = _key.ToString().Substring(_key.ToString().Length - 1,1);
        int keyInt = int.Parse(key);
        return keyInt;
        // int propIndex = int.TryParse(_key.ToString());
    }

    #endregion
    public void AnimationTrigger() => this.StateMachine.currentState.AnimationFinishTrigger();
    
}
