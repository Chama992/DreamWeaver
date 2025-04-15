using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState
{
    protected PlayerInputCheck playerInputCheck;
    protected PlayerStateMachine StateMachine;
    protected PlayerEntityController playerEntity;
    private string animBoolName;
    protected float xInput;
    protected float yInput;
    protected Rigidbody2D rb;
    protected float stateTimer;
    protected bool animTriggerCalled;
    public bool stateActive;
    
    public PlayerState(PlayerEntityController playerEntity, PlayerStateMachine _playerStateMachine, string _animBoolName, PlayerInputCheck _playerInputCheck)
    { 
        this.StateMachine = _playerStateMachine;
        this.playerEntity = playerEntity;
        this.animBoolName = _animBoolName;
        rb = playerEntity.Rb;
        playerInputCheck = _playerInputCheck;
    }

    public virtual void Enter()
    {
        playerEntity.Anim.SetBool(animBoolName, true);
        animTriggerCalled = false;
        stateActive = true;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        xInput = playerInputCheck.PlayerInput.Move.ReadValue<Vector2>().x;
        yInput = playerInputCheck.PlayerInput.Move.ReadValue<Vector2>().y;
        playerEntity.Anim.SetFloat("yVelocity",  rb.velocity.y);
    }

    public virtual void Exit() 
    {
        playerEntity.Anim.SetBool(animBoolName, false);
        // Debug.Log($"exit the {StateMachine.currentState.GetType().Name} state");
        stateActive = false;
    }

    public void AnimationFinishTrigger()
    {
        animTriggerCalled = true;
    }
}   
