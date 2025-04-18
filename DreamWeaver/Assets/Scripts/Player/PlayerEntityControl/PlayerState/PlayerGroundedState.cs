using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �ڵ���ĳ���״̬
/// </summary>
public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(PlayerEntityController playerEntity, PlayerStateMachine _playerStateMachine, string _animBoolName, PlayerInputCheck _playerInputCheck) : base(playerEntity, _playerStateMachine, _animBoolName, _playerInputCheck)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
        playerEntity.SetVelocity(rb.velocity.x,0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!playerEntity.isOnGround) //掉落
        {
            playerEntity.AirState.JumpGraceStart();
            StateMachine.ChangeState(playerEntity.AirState);
        }

        if (playerInputCheck.PlayerInput.Jump.WasPerformedThisFrame() && playerEntity.isOnGround)
        {
            MySoundManager.PlayAudio("跳跃");
            StateMachine.ChangeState(playerEntity.JumpState);
        }
    }
}
