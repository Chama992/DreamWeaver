using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    float animClipDuration;
    bool findFlag;
    public PlayerDashState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        MySoundManager.PlayOneAudio("Shift");
        if (!findFlag)
        {
            AnimationClip dashClip = (from clip in player.Anim.runtimeAnimatorController.animationClips
                                      where clip.name == "dash"
                                      select clip).First();
            animClipDuration = dashClip.length;
            findFlag = true;
        }
        player.Anim.SetFloat("DashDuration", animClipDuration / player.dashDuration);
        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        player.SetVelocity(0, rb.velocity.y);
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(player.dashSpeed * player.dashDir, 0);
        if (!player.IsGroundChecked() && player.IsWallChecked())
            StateMachine.ChangeState(player.WallSlideState);
        if (stateTimer < 0)
            StateMachine.ChangeState(player.IdleState);
    }
}
