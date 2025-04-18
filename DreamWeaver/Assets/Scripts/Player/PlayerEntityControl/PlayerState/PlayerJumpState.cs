using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    private float jumpStartHeight;
    private float jumpLerpSpeed;
    public PlayerJumpState(PlayerEntityController playerEntity, PlayerStateMachine _playerStateMachine, string _animBoolName, PlayerInputCheck _playerInputCheck) : base(playerEntity, _playerStateMachine, _animBoolName, _playerInputCheck)
    {
        jumpLerpSpeed = playerEntity.jumpLerpSpeed;
    }
    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(rb.velocity.x, playerEntity.jumpFirstVelo);//给一个瞬间的初速度
        jumpStartHeight = playerEntity.transform.position.y;
    }

    public override void Exit()
    {
        base.Exit();
        jumpStartHeight = 0;
    }

    public override void Update()
    {
        base.Update();
        if (rb.velocity.y < 0)
            StateMachine.ChangeState(playerEntity.AirState);
        float yvelocity = rb.velocity.y;
        yvelocity = JumpHold(yvelocity);
        if (rb.transform.position.y - jumpStartHeight < playerEntity.jumpMaxHeight * 0.9f)
        {
            yvelocity = JumpHold(yvelocity);
        }
        else
        {
            yvelocity = JumpLerp2Zero(yvelocity);
        }
        playerEntity.SetVelocity(xInput * playerEntity.airMoveSpeed, yvelocity);
    }
    
    private float JumpLerp2Zero(float yvelocity)
    {
        if (rb.transform.position.y - jumpStartHeight > playerEntity.jumpMaxHeight)
        {
            return 0;
        }
        Debug.Log(Mathf.Lerp(yvelocity,0,jumpLerpSpeed * Time.deltaTime));
        return Mathf.Lerp(yvelocity,0,jumpLerpSpeed * Time.deltaTime);
    }

    private float JumpHold(float yvelocity)
    {
        if (rb.velocity.y < playerEntity.jumpMaxVelo && playerInputCheck.PlayerInput.Jump.IsInProgress())
        {
            yvelocity += playerEntity.jumpAcceleration * Time.deltaTime;
            yvelocity = yvelocity > playerEntity.jumpMaxVelo? playerEntity.jumpMaxVelo : yvelocity;
        }
        else
        {
            yvelocity = rb.velocity.y;
        }
        return yvelocity;
    }
}
