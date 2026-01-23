using System;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine<T> where T : System.Enum
{
    private T _currentState;
    private Dictionary<T, Action> _enterCallbacks = new();
    private Dictionary<T, Action> _exitCallbacks = new();
    
    public T CurrentState => _currentState;
    
    public void ChangeState(T newState)
    {
        if (_exitCallbacks.ContainsKey(_currentState))
            _exitCallbacks[_currentState]?.Invoke();
        
        _currentState = newState;
        
        if (_enterCallbacks.ContainsKey(_currentState))
            _enterCallbacks[_currentState]?.Invoke();
    }
    
    public void AddStateCallback(T state, Action onEnter, Action onExit = null)
    {
        _enterCallbacks[state] = onEnter;
        if (onExit != null)
            _exitCallbacks[state] = onExit;
    }
}