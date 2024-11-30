using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region PlayerStates
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    // public PlayerDashState DashState { get; private set; }
    // public PlayerWallSlideState WallSlideState { get; private set; }
    // public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerHookState HookState { get; private set; }
    #endregion
    [Header("Move Info")]
    [SerializeField] public float moveSpeed;
    [Header("JumpFall Info")]
    [SerializeField] public float jumpForce;
    [SerializeField] public float airMoveSpeed;
    private Piece currentPiece;
    #region Components
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public CapsuleCollider2D Cc2 { get; private set; }
    public DistanceJoint2D DistanceJoint2D { get; private set; }
    public LineRenderer LineRenderer { get; private set; }
    [Header("Node Info")]
    public PlayerNodeControl PlayerNodeControl;
    #endregion
    [Header("CollisionCheck Info")]
    [SerializeField] protected Transform groundCheck1;
    [SerializeField] protected Transform groundCheck2;
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
    private List<KeyCode> propKeys = new List<KeyCode>() { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6 };
    #endregion

    public bool canGrap = true;
    public bool canBuild = true;
    public Action<Vector2,Vector2> CrossDoor;
    private void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        Cc2 = GetComponent<CapsuleCollider2D>();
        DistanceJoint2D = GetComponent<DistanceJoint2D>();
        DistanceJoint2D.enabled = false;
        LineRenderer = GetComponent<LineRenderer>();
        canGrap = true;
        //use statemachine,player can switch to any state
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        AirState = new PlayerAirState(this, StateMachine, "Jump");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        // DashState = new PlayerDashState(this, StateMachine, "Dash");
        // WallSlideState = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        // WallJumpState = new PlayerWallJumpState(this, StateMachine, "Jump");
        DeadState = new PlayerDeadState(this, StateMachine, "Dead");
        HookState = new PlayerHookState(this, StateMachine, "Jump");
        Props.Initialize(this);
    }
    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }
    private void Update()
    {
        if (GameController.instance.isAnimating||!GameController.instance.isGaming)
        {
            Rb.velocity = Vector2.zero;
            return;
        }

        StateMachine.currentState.Update();
        // CheckDashActive();
        UsePropDetect();
        
        LinkNodeChekc();

    }

    private void LinkNodeChekc()
    {
        RaycastHit2D hit = IsPieceChecked();
        if (hit)
        {
            currentPiece = hit.transform.gameObject.GetComponentInParent<Piece>();
        }
        if (currentPiece != null)
        {
            if(!GameController.instance.isPausing&&currentPiece.node != null&&Input.GetKeyDown(KeyCode.S)&&(transform.position-currentPiece.node.position).magnitude<GameController.instance.interactRatio)
            {
                PlayerNodeControl.LinkNode(currentPiece.gameObject.GetInstanceID(), currentPiece);
            }
        }
    }
    #region Collision
    public RaycastHit2D IsPieceChecked() => Physics2D.Raycast(groundCheck1.position, Vector2.down, groundCheckDistance, whatIsGround);

    public bool IsGroundChecked()
    {
        bool a = Physics2D.Raycast(groundCheck1.position, Vector2.down, groundCheckDistance, whatIsGround);
        bool b = Physics2D.Raycast(groundCheck2.position, Vector2.down, groundCheckDistance, whatIsGround);
        if ( a || b)
            return true;
        return false;
    }
    public bool IsWallChecked() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck1.position, new Vector3(groundCheck1.position.x, groundCheck1.position.y - groundCheckDistance));
        Gizmos.DrawLine(groundCheck2.position, new Vector3(groundCheck2.position.x, groundCheck2.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
    }
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.GetComponent<Piece>()!=null)
    //     {
    //         currentPiece = other.GetComponent<Piece>();
    //     }
    // }
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
    #region Prop
    private void UsePropDetect()
    {
        if (!GameController.instance.isGaming && GameController.instance.isAnimating && GameController.instance.isPausing)
        {
            return;
        }
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
        string key = _key.ToString().Substring(_key.ToString().Length - 1, 1);
        int keyInt = int.Parse(key);
        return keyInt;
        // int propIndex = int.TryParse(_key.ToString());
    }

    #endregion
    public void AnimationTrigger() => this.StateMachine.currentState.AnimationFinishTrigger();

}
