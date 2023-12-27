using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<T> where T : Enum {
    
    public T Current {
        get => _current;
        set {
            if (_current.Equals(value)) return;

            _onExited[_current]?.Invoke();
            _current = value;
            _onEntered[_current]?.Invoke();
            _nextState = _current;
        }
    }

    private T _current;
    private T _nextState;
    private float _transitionTime;

    private readonly Dictionary<T, Action> _onEntered = new();
    private readonly Dictionary<T, Action> _onStay = new();
    private readonly Dictionary<T, Action> _onExited = new();

    public Action OnEntered(T state) {
        if (!_onEntered.ContainsKey(state)) _onEntered[state] = default;
        return _onEntered[state];
    }
    public Action OnStay(T state) {
        if (!_onStay.ContainsKey(state)) _onStay[state] = default;
        return _onStay[state];
    }
    public Action OnExited(T state) {
        if (!_onExited.ContainsKey(state)) _onExited[state] = default;
        return _onExited[state];
    }

    public void OnStay() {
        if (!_onStay.ContainsKey(Current)) _onStay[Current] = default;
        _onStay[Current]?.Invoke();

        if (_nextState.Equals(Current)) return;
        _transitionTime -= Time.deltaTime;
        if (_transitionTime <= 0) {
            _transitionTime = 0;
            Current = _nextState;
        }
    }
    public void SetStateAfterTime(T nextState, float time) {
        _nextState = nextState;
        _transitionTime = time;
    }
}