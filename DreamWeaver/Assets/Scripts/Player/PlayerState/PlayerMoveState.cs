using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        // player.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();
        if (StateMachine.currentState != player.MoveState)
            return;
        if (xInput == 0 || player.IsWallChecked())
            StateMachine.ChangeState(player.IdleState);
        player.SetVelocity(xInput * player.moveSpeed, -10);
    }
}
