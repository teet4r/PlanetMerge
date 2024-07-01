using Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverLine : SceneSingletonBehaviour<GameoverLine>, ICollidable
{
    public bool PauseUpdate;

    [SerializeField] private float _targetHeight;
    [Min(0f)][SerializeField] private float _diff;
    [Range(0f, 1f)][SerializeField] private float _lerpT;

    private Transform _tr;
    private SpriteRenderer _renderer;
    private Vector2 _t = new();

    private float _originHeight;
    private Color _originColor;
    private int _touchingCnt;
    private float _deadtime;

    protected override void Awake()
    {
        base.Awake();

        _tr = GetComponent<Transform>();
        _renderer = GetComponent<SpriteRenderer>();

        _originHeight = _tr.position.y;
        _t.x = _tr.position.x;
        _originColor = _renderer.material.color;
    }

    private void OnEnable()
    {
        PauseUpdate = false;

        _targetHeight = _originHeight;
        _renderer.material.color = _originColor;
        _touchingCnt = 0;
        _deadtime = 0f;
    }

    private void FixedUpdate()
    {
        if (PauseUpdate)
            return;

        _t.y = Mathf.Lerp(_tr.position.y, _targetHeight, _lerpT);
        _tr.position = _t;

        if (_touchingCnt > 0)
        {
            _deadtime += Time.fixedDeltaTime;

            if (_deadtime >= 6f)
            {
                PauseUpdate = true;
                PlayScene.Instance.Gameover();
            }
            else if (_deadtime >= 1f)
                _renderer.material.color = Color.Lerp(_originColor, Color.red, (_deadtime - 1f) / 5f);
        }
        else
        {
            _deadtime = Mathf.Max(_deadtime - Time.fixedDeltaTime, 0f);
            _renderer.material.color = Color.Lerp(_originColor, Color.red, (_deadtime - 1f) / 5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collidable = collision.gameObject.GetComponent<ICollidable>();

        switch (collidable)
        {
            case Planet:
                ++_touchingCnt;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var collidable = collision.gameObject.GetComponent<ICollidable>();

        switch (collidable)
        {
            case Planet:
                --_touchingCnt;
                break;
        }
    }

    public void LineDown() => _targetHeight -= _diff;
    public void LineUp() => _targetHeight += _diff;
}
