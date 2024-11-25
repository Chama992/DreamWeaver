using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        MySoundManager.PlayOneAudio("jump");
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);//给一个瞬间的力
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (rb.velocity.y < 0)
            StateMachine.ChangeState(player.AirState);
        if (xInput != 0)
            player.SetVelocity(xInput * player.airMoveSpeed, rb.velocity.y);
    }
}
