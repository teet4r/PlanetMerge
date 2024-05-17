using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public abstract class ParticleObject : PoolObject
{
    protected ParticleSystem _particleSystem;

    public override void Initialize()
    {
        base.Initialize();

        _particleSystem = GetComponent<ParticleSystem>();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _particleSystem.Clear();
    }

    public async UniTask Play()
    {
        _particleSystem.Play();

        await UniTask.WaitUntil(() => _particleSystem.isStopped, cancellationToken : DisableCancellationToken);

        Pool();
    }
}
