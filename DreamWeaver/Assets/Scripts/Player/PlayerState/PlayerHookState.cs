using UnityEngine;

public class PlayerHookState : PlayerState
{
    private Vector2 hookPoint;
    private Vector2 hookOriginPoint;
    private float hookDistance;
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
    }

    public override void Exit()
    {
        base.Exit();
        
    }
    public void SetTarget(Vector2 _hookPoint)
    {
        hookPoint = _hookPoint;
        player.SetVelocity((hookPoint - (Vector2)player.transform.position).normalized * player.hookSpeed);
        if (Vector2.Distance(hookPoint,player.transform.position) / hookDistance > 0.9f)
        {
            StateMachine.ChangeState(player.IdleState);
        }
    }
}
