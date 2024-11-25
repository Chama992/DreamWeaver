using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0,0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (StateMachine.currentState != player.IdleState)
            return;
        //��ô����Ҫ�����ж��ٽ�ǽ������
        if (xInput != 0)
        {
            // near wall cant move
            if (player.IsWallChecked())
            {
                if (xInput != player.facingDir)
                    StateMachine.ChangeState(player.MoveState);
            }
            else
                StateMachine.ChangeState(player.MoveState);
        }
    }
}
