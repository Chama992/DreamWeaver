using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    private bool quitFlag = false;
    public PlayerMoveState(PlayerEntityController playerEntity, PlayerStateMachine _playerStateMachine, string _animBoolName, PlayerInputCheck _playerInputCheck) : base(playerEntity, _playerStateMachine, _animBoolName, _playerInputCheck)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        quitFlag = false;
        // player.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();
        if (StateMachine.currentState != playerEntity.MoveState)
        {
            return;
        }
        if (playerEntity.IsWallChecked())
        {
            StateMachine.ChangeState(playerEntity.IdleState);
        }
        if (playerEntity.enAbleAcceleraton)
        {
            VelocityChangeWithAcce();
        }
        else
        {
            VelocityChangeSimple();
        }
    }
    /// <summary>
    /// 不开启加速度测试时的速度判定
    /// </summary>
    private void VelocityChangeSimple()
    {
        if (xInput == 0)
        {
            StateMachine.ChangeState(playerEntity.IdleState);
        }
        playerEntity.SetVelocity(xInput * playerEntity.maxSpeed, 0);
    }
    /// <summary>
    /// 开启加速度测试时的速度判定
    /// </summary>
    private void VelocityChangeWithAcce()
    {    
        if (xInput == 0)
        {
            quitFlag = true;
        }
        if (quitFlag)
        {
            VelocityMoveTowards(rb.velocity.x>0? 1:-1, Mathf.Abs(rb.velocity.x), 0, -playerEntity.moveDecelerate);
            if (Mathf.Approximately(rb.velocity.x, 0))
            {
                StateMachine.ChangeState(playerEntity.IdleState);
            }
        }
        else
        {
            if (xInput * rb.velocity.x >= 0)
            {
                VelocityMoveTowards(xInput,Mathf.Abs(rb.velocity.x),playerEntity.maxSpeed,playerEntity.moveAcceleration);
            }
            else
            {
                VelocityMoveTowards(rb.velocity.x>0? 1:-1,Mathf.Abs(rb.velocity.x),0,-playerEntity.moveDecelerate);
            }
        }
    }

    /// <summary>
    /// 速度根据加速度值变化
    /// </summary>
    /// <param name="dir">当前移动方向，默认当前移动速度方向和目标速度方向相同</param>
    /// <param name="current">当前速度绝对值</param>
    /// <param name="target">当前目标速度绝对值</param>
    /// <param name="delta">加速度 带正负</param>
    private void VelocityMoveTowards(float dir,float currentVelocity, float targetVelocity, float delta)
    {
        currentVelocity = Mathf.Abs(currentVelocity);
        targetVelocity = Mathf.Abs(targetVelocity);
        if (delta > 0 && targetVelocity < currentVelocity)
        {
            return;
        }
        if (delta < 0 && targetVelocity > currentVelocity)
        {
            return;
        }
        if (Mathf.Approximately(currentVelocity, targetVelocity))
        {
            return;
        }
        float acce = delta * Time.deltaTime;
        if (delta > 0) //加速
        {
            currentVelocity = Mathf.MoveTowards(currentVelocity,targetVelocity,acce);
            
        }
        else//减速
        {
            currentVelocity = currentVelocity + acce > targetVelocity ?  currentVelocity + acce : targetVelocity;
        }
        playerEntity.SetVelocity(dir * currentVelocity, 0);

    }
}
