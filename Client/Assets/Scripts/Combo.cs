using Behaviour;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Combo : SceneSingletonBehaviour<Combo>
{
    public static int Count => _count;
    public static event Action OnCombo;

    private const float _comboMaxIntervalMs = 2f;
    private static int _count;
    private static float _remainTime;

    private void OnEnable()
    {
        _count = 0;
        _remainTime = 0f;
        OnCombo = null;
    }

    private void FixedUpdate()
    {
        _remainTime = Mathf.Max(_remainTime - Time.fixedDeltaTime, 0f);

        if (_remainTime <= 0f)
            _count = 0;
    }

    public static void Add()
    {
        ++_count;

        if (_count > 1)
            OnCombo?.Invoke();

        _remainTime = _comboMaxIntervalMs;
    }
}
