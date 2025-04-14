using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine StateMachine;
    protected Player player;
    private string animBoolName;
    protected float xInput;
    protected float yInput;
    protected Rigidbody2D rb;
    protected float stateTimer;
    protected bool animTriggerCalled;
    public bool stateActive;
    
    public PlayerState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName)
    { 
        this.StateMachine = _playerStateMachine;
        this.player = _player;
        this.animBoolName = _animBoolName;
        rb = _player.Rb;
    }

    public virtual void Enter()
    {
        player.Anim.SetBool(animBoolName, true);
        animTriggerCalled = false;
        stateActive = true;
        // Debug.Log($"enter the {StateMachine.currentState.GetType().Name} state");
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.Anim.SetFloat("yVelocity",  rb.velocity.y);
    }

    public virtual void Exit() 
    {
        player.Anim.SetBool(animBoolName, false);
        // Debug.Log($"exit the {StateMachine.currentState.GetType().Name} state");
        stateActive = false;
    }

    public void AnimationFinishTrigger()
    {
        animTriggerCalled = true;
    }
}   
