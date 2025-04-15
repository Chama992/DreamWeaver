using UnityEngine;


public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(PlayerEntityController playerEntity, PlayerStateMachine _playerStateMachine, string _animBoolName, PlayerInputCheck _playerInputCheck) : base(playerEntity, _playerStateMachine, _animBoolName, _playerInputCheck)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
