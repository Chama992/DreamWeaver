using Unity.VisualScripting;
using UnityEngine;

public class PlayerHookState : PlayerState
{
    private Vector2 hookPoint;
    private float hookDistance;
    private Collider2D other;
    private float hookSpeed;
    // public bool canHook;
    public PlayerHookState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // hookOriginPoint = player.transform.position;
        // hookDistance = Vector2.Distance(hookOriginPoint, hookPoint);
        player.SetVelocity(0,0);
        player.LineRenderer.SetPosition(0, hookPoint);
        player.LineRenderer.SetPosition(1, player.transform.position);
        player.DistanceJoint2D.connectedAnchor = hookPoint;
        player.DistanceJoint2D.enabled = true;
        player.LineRenderer.enabled = true;
    }
    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(1))
        { 
            player.LineRenderer.enabled = false;
            player.DistanceJoint2D.enabled = false;
            StateMachine.ChangeState(player.IdleState);
        }
        if (player.DistanceJoint2D.enabled)
        {
            player.LineRenderer.SetPosition(1, player.transform.position);
        }
        // player.SetVelocity((hookPoint - (Vector2)player.transform.position).normalized * hookSpeed);
        // Debug.Log((hookPoint - (Vector2)player.transform.position).normalized * player.hookSpeed);
        // if (Vector2.Distance(player.transform.position, hookPoint) <= player.DistanceJoint2D.distance)
        //     StateMachine.ChangeState(player.IdleState);
        if (player.Cc2.IsTouching(other))
            StateMachine.ChangeState(player.IdleState);
        if (player.IsGroundChecked() || player.IsWallChecked())
            StateMachine.ChangeState(player.IdleState);
    }
    public override void Exit()
    {
        base.Exit();
        // canHook = false;
        player.canGrap = true;
        player.DistanceJoint2D.enabled = false;
        player.LineRenderer.enabled = false;
        player.LineRenderer.positionCount = 0;
    }
    public void SetTarget(Vector2 _hookPoint, Collider2D _other,float _hookSpeed)
    {
        hookPoint = _hookPoint;
        other = _other;
        hookSpeed = _hookSpeed;
    }
    // public void CanHook()
    // {
    //     canHook = true;
    // }
}
