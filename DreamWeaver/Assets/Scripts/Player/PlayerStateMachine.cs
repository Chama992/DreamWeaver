using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }
    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        //Debug.Log($"exit the {currentState.GetType().Name} state");
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
        //Debug.Log($"enter the {currentState.GetType().Name} state");
    }
}
