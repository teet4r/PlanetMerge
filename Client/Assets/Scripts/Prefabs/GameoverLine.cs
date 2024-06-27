using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverLine : MonoBehaviour, ICollidable
{
    [SerializeField] private float _targetHeight;
    [Min(0f)][SerializeField] private float _diff;
    [Range(0f, 1f)][SerializeField] private float _lerpT;
    private Transform _tr;
    private Vector2 _t = new();
    private float _originHeight;

    private void Awake()
    {
        _tr = GetComponent<Transform>();

        _originHeight = _tr.position.y;
        _t.x = _tr.position.x;
    }

    private void OnEnable()
    {
        _targetHeight = _originHeight;
    }

    private void FixedUpdate()
    {
        _t.y = Mathf.Lerp(_tr.position.y, _targetHeight, _lerpT);
        _tr.position = _t;
    }

    public void LineDown() => _targetHeight -= _diff;
    public void LineUp() => _targetHeight += _diff;
}
