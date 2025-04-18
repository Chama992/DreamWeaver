using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    private int currentjumpBufferFrameCount;
    private int currentjumpGraceFrameCount;
    private bool jumpGraceFlag;
    private float jumpGraceTimer;
    public PlayerAirState(PlayerEntityController playerEntity, PlayerStateMachine _playerStateMachine, string _animBoolName, PlayerInputCheck _playerInputCheck) : base(playerEntity, _playerStateMachine, _animBoolName, _playerInputCheck)
    {
    }

    public override void Enter()
    {
        base.Enter();
        currentjumpBufferFrameCount = 0;
        if (jumpGraceFlag)
        {
            currentjumpGraceFrameCount = playerEntity.jumpGraceFrameCount;
            jumpGraceFlag = false;
            jumpGraceTimer = 0;
        }
        else
        {
            currentjumpGraceFrameCount = 0;
        }
    }

    public override void Exit()
    {
        base.Exit();
        currentjumpBufferFrameCount = 0;
        currentjumpGraceFrameCount = 0;
        jumpGraceFlag = false;
    }
    
    public override void Update()
    {
        base.Update();
        playerEntity.SetVelocity(xInput * playerEntity.airMoveSpeed, rb.velocity.y);
        JumpBufferCheck();
        JumpGraceCheck();
    }

    private void JumpBufferCheck()
    {
        if (currentjumpBufferFrameCount > 0)
        {
            currentjumpBufferFrameCount--;
        }
        if (playerInputCheck.PlayerInput.Jump.WasPerformedThisFrame())
        {
            currentjumpBufferFrameCount = playerEntity.jumpBufferFrameCount;
        }
        if (playerEntity.isOnGround)
        {
            if ( currentjumpBufferFrameCount==0)
            {
                StateMachine.ChangeState(playerEntity.IdleState);
            }
            else
            {
                StateMachine.ChangeState(playerEntity.JumpState);
            }
        }
    }

    private void JumpGraceCheck()
    {
        if (currentjumpGraceFrameCount > 0)
        {
            currentjumpGraceFrameCount--;
        }
        if (playerInputCheck.PlayerInput.Jump.WasPerformedThisFrame() && currentjumpGraceFrameCount > 0)
        {
            StateMachine.ChangeState(playerEntity.JumpState);
        }
    }
    public void JumpGraceStart()
    {
        jumpGraceFlag = true;
    }
}
