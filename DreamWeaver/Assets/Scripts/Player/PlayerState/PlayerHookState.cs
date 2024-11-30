using Unity.VisualScripting;
using UnityEngine;

public class PlayerHookState : PlayerState
{
    private Vector2 hookPoint;
    private Vector2 hookOriginPoint;
    private float hookDistance;
    private Collider2D other;
    public PlayerHookState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        hookOriginPoint = player.transform.position;
        hookDistance = Vector2.Distance(hookOriginPoint, hookPoint);
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity((hookPoint - (Vector2)player.transform.position).normalized * player.hookSpeed);
        // Debug.Log((hookPoint - (Vector2)player.transform.position).normalized * player.hookSpeed);
        if (player.Cc2.IsTouching(other))
            StateMachine.ChangeState(player.IdleState);
        if (player.IsGroundChecked() || player.IsWallChecked())
            StateMachine.ChangeState(player.IdleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
    public void SetTarget(Vector2 _hookPoint, Collider2D _other)
    {
        hookPoint = _hookPoint;
        other = _other;
    }
}
