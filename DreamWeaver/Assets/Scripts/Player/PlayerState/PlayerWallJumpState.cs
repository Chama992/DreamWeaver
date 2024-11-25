using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    private float jumpTime = 0.1f;
    private float jumpTimer = 0f;
    public PlayerWallJumpState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.wallJumpDuration;//墙上跳跃有一小段时间玩家无法控制移动
        player.SetVelocity(player.wallJumpXMoveSpeed * -player.facingDir, player.wallJumpForce);
        jumpTimer = jumpTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        jumpTimer -= Time.deltaTime;
        if (jumpTimer < 0)
        {
            if (xInput != 0)
                player.SetVelocity(xInput * player.airMoveSpeed, rb.velocity.y);
        }
        if (stateTimer < 0)
            StateMachine.ChangeState(player.AirState);
        if (player.IsGroundChecked())
            StateMachine.ChangeState(player.IdleState);
        base.Update();
    }
}
