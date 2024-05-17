using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class CollidablePoolObject : PoolObject, ICollidable
{
    public Rigidbody2D Rigid => _rigid;
    public Collider2D Collider => _collider;

    private Rigidbody2D _rigid;
    private Collider2D _collider;

    public override void Initialize()
    {
        base.Initialize();

        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }
}
