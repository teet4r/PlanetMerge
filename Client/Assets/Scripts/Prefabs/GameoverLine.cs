using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverLine : MonoBehaviour, ICollidable
{
    // Variables
    public bool PauseUpdate;

    // Get Properties
    public Vector3 Position => _tr.position;
    public int LineBonus => _lineBonus;

    [SerializeField] private float _targetHeight;
    private const float _DIFF = 0.4f;
    private const float _LERP_T = 0.05f;

    private Transform _tr;
    private SpriteRenderer _renderer;
    private Animation _animation;
    private AnimationState _fadeOutInState;
    private Vector2 _t = new();

    private float _originHeight;
    private Color _originColor;
    private int _touchingCnt;
    private float _deadtime;
    private int _lineBonus;

    private void Awake()
    {
        _tr = GetComponent<Transform>();
        _renderer = GetComponent<SpriteRenderer>();
        _animation = GetComponent<Animation>();

        _originHeight = _tr.position.y;
        _t.x = _tr.position.x;
        _originColor = _renderer.material.color;

        _fadeOutInState = _animation["FadeOutIn"];
    }

    private void OnEnable()
    {
        PauseUpdate = false;

        _targetHeight = _originHeight;
        _renderer.material.color = _originColor;
        _touchingCnt = 0;
        _deadtime = 0f;
        _lineBonus = 1;
    }

    private void FixedUpdate()
    {
        if (PauseUpdate)
            return;

        _t.y = Mathf.Lerp(_tr.position.y, _targetHeight, _LERP_T);
        _tr.position = _t;

        if (_touchingCnt > 0)
        {
            _deadtime = Mathf.Min(_deadtime + Time.fixedDeltaTime, 6f);

            if (_deadtime >= 6f)
            {
                PauseUpdate = true;
                PlayScene.Instance.Gameover();
            }
        }
        else
            _deadtime = Mathf.Max(_deadtime - Time.fixedDeltaTime, 0f);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        var collidable = collision.gameObject.GetComponent<ICollidable>();

        switch (collidable)
        {
            case Planet:
                if (_deadtime >= 1f && !_animation.isPlaying)
                {
                    _fadeOutInState.speed = _deadtime * 0.8f;
                    _animation.Play();
                }
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

    public void LineDown()
    {
        ++_lineBonus;
        _targetHeight -= _DIFF;
    }
}
