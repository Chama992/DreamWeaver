using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerEntityController playerEntity, PlayerStateMachine _playerStateMachine, string _animBoolName, PlayerInputCheck _playerInputCheck) : base(playerEntity, _playerStateMachine, _animBoolName, _playerInputCheck)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if (StateMachine.currentState != playerEntity.IdleState)
            return;
        //这么多主要用于判断临近墙不能走
        if (xInput != 0)
        {
            // near wall cant move
            if (playerEntity.IsWallChecked())
            {
                if (xInput != playerEntity.facingDir)
                    StateMachine.ChangeState(playerEntity.MoveState);
            }
            else
                StateMachine.ChangeState(playerEntity.MoveState);
        }
    }
}
