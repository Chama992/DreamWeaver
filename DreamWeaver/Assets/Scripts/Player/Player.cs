using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : Entity
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
    [SerializeField] private bool canDash = true;
    private float dashUsageTimer;
    public float dashDir { get; private set; }
    protected override void Awake()
    {
        base.Awake();
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
    }
    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
    }
    protected override void Update()
    {
        base.Update();
        StateMachine.currentState.Update();
        CheckDashActive();//???????????
    }
    private void OnDisable()
    {
    }
    
    #region Dash
    /// <summary>
    /// ???????????????
    /// </summary>
    private void CheckDashActive()
    {
        if (this.IsWallChecked()) //???????????
            return;
        dashUsageTimer -= Time.deltaTime;//??????
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0 && canDash) // ��??????????shift
        {
            dashUsageTimer = dashCoolDown;
            // ????????????????
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
                dashDir = facingDir;//????��???????
            if (dashDir != facingDir)
                Flip();
            StateMachine.ChangeState(this.DashState);
        }
    }
    #endregion
    public void AnimationTrigger() => this.StateMachine.currentState.AnimationFinishTrigger();
    
}
