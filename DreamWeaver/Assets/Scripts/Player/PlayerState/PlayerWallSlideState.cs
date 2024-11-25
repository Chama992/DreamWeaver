using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StateMachine.ChangeState(player.WallJumpState);
            return; //avoid this frame interupted by following check
        }
        // if (xInput != 0 && player.facingDir != xInput) //BUG: 墙上会翻面
        //     StateMachine.ChangeState(player.IdleState);
        if (yInput < 0)
            rb.velocity = new Vector2(0, rb.velocity.y * player.wallSlideYFastSpeedCoefficient);
        /*else if (yInput > 0)
            rb.velocity = new Vector2(0, rb.velocity.y * player.wallSlideYSlowSpeedCoefficient); // 向上爬判定 可以没有*/
        else
            rb.velocity = new Vector2(0, rb.velocity.y * player.wallSlideYSlowSpeedCoefficient);
        if (player.IsGroundChecked())
            StateMachine.ChangeState(player.IdleState);
        if(!player.IsWallChecked())
            StateMachine.ChangeState(player.IdleState);

    }
}
