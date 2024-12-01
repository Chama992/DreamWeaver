using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ڵ���ĳ���״̬
/// </summary>
public class PlayerGroundedState : PlayerState
{
    private  readonly int _weaponIndex = Animator.StringToHash("WeaponIndex");

    public PlayerGroundedState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
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
        if (!player.IsGroundChecked())
            StateMachine.ChangeState(player.AirState);
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundChecked())
        {
            MySoundManager.PlayOneAudio("跳跃");
            StateMachine.ChangeState(player.JumpState);
        }
    }
}
