using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Planet : CollidablePoolObject
{
    public int Level => _level;
    public readonly List<IDisposable> Disposables = new();

    [SerializeField] private ColorSprite _colorSprite;
    private bool _isAlive;
    private bool _isDrag;
    private bool _isMerging;
    private bool _isAttach;
    private int _level;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private Color _planetColor;

    private Camera _mainCamera;
    private float _deadtime = 0;

    public override void Initialize()
    {
        base.Initialize();

        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _planetColor = _spriteRenderer.color;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _mainCamera = Camera.main;

        _level = UnityEngine.Random.Range(0, PlayScene.Instance.MaxLevel);
        _spriteRenderer.color = _planetColor;
        _spriteRenderer.sprite = ResourceLoader.LoadSprite($"Level{_level}");
        _animator.SetInteger(AniParam.LEVEL, _level);
     
        _isAlive = true;
    }

    private void Update()
    {
        if (!_isDrag)
            return;

        var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition); //스크린 좌표를 월드 좌표로
                                                                            //x축 경계 설정
        float leftBorder = -2.9f + tr.localScale.x;
        float rightBorder = 2.9f - tr.localScale.x;

        if (mousePos.x < leftBorder)
            mousePos.x = leftBorder;
        else if (mousePos.x > rightBorder)
            mousePos.x = rightBorder;

        mousePos.y = 4.3f;
        mousePos.z = 0;
        //부드럽게 따라다니게 lerp(현재위치, 목표위치, 따라가는 강도
        tr.position = Vector3.Lerp(tr.position, mousePos, 0.2f);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _isAlive = false;
        _isDrag = false;
        _isMerging = false;
        _isAttach = false;

        tr.localPosition = Vector3.zero;
        tr.localRotation = Quaternion.identity;
        tr.localScale = Vector3.zero;

        Rigid.simulated = false;
        Rigid.velocity = Vector2.zero;
        Rigid.angularVelocity = 0;
        Collider.enabled = true;

        for (int i = 0; i < Disposables.Count; ++i)
            Disposables[i]?.Dispose();
        Disposables.Clear();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var collidable = collision.gameObject.GetComponent<ICollidable>();

        switch (collidable)
        {
            case GameoverLine:
                _deadtime += Time.deltaTime;
                if (_deadtime > 1f)
                    _colorSprite.Color(Color.red, _deadtime - 1f, 2f);
                if (_deadtime > 3f)
                    PlayScene.Instance.Gameover();
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var collidable = collision.gameObject.GetComponent<ICollidable>();

        switch (collidable)
        {
            case GameoverLine:
                _colorSprite.Color(Color.white, _deadtime);
                _deadtime = 0;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _Attach();

        var collidable = collision.gameObject.GetComponent<ICollidable>();

        switch (collidable)
        {
            case Planet:
                _Merge(collidable as Planet);
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var collidable = collision.gameObject.GetComponent<ICollidable>();

        switch (collidable)
        {
            case Planet:
                _Merge(collidable as Planet);
                break;
        }
    }

    private void _Merge(Planet other)
    {
        if (_level != other._level || _isMerging || other._isMerging)
            return;

        if (_level < 9)
        {
            //나와 상대 위치 가져오기
            float meX = tr.position.x;
            float meY = tr.position.y;
            float otherX = other.tr.position.x;
            float otherY = other.tr.position.y;
            //1. 내가 아래
            //2. 동일한 높이, 내가 오른쪽
            if (meY < otherY || (meY == otherY && meX > otherX))
            {
                //상대 숨기기
                other.Hide(tr.position);
                //나 레벨업
                LevelUp(needMergingTime: true);
            }
        }
        else // level >= 9
        {
            other.Hide(Vector3.up * 100);
            other._PlayEffect();
            Hide(Vector3.up * 100);
            _PlayEffect();
        }
    }



    public void Drag()
    {
        _isDrag = true;
    }

    public void Drop()
    {
        _isDrag = false;
        Rigid.simulated = true;
    }



    private void _Attach()
    {
        _AttachRoutine().Forget();
    }

    private async UniTask _AttachRoutine()
    {
        if (_isAttach)
            return;

        _isAttach = true;

        SFX.Play(Sfx.Attach);

        await UniTask.Delay(500, cancellationToken: DisableCancellationToken);

        _isAttach = false;
    }



    public void Hide(Vector3 targetPos)
    {
        if (!_isAlive)
            return;
        _isAlive = false;
        _isMerging = true;

        _HideRoutine(targetPos).Forget();
    }

    private async UniTask _HideRoutine(Vector3 targetPos)
    {
        Rigid.simulated = false;
        Collider.enabled = false;

        if (targetPos == Vector3.up * 100)
            _PlayEffect();

        int frameCount = 0;

        while (frameCount < 20)
        {
            frameCount++;

            if (targetPos != Vector3.up * 100)
                tr.position = Vector3.Lerp(tr.position, targetPos, 0.5f);
            else
                tr.localScale = Vector3.Lerp(tr.localScale, Vector3.zero, 0.05f);

            await UniTask.DelayFrame(1, cancellationToken : DisableCancellationToken);
        }

        PlayScene.Instance.Score.Value += (int)Mathf.Pow(2, _level);

        _isMerging = false;

        Pool();
    }



    public void LevelUp(bool needMergingTime = false)
    {
        _isMerging = true;

        _level = Mathf.Min(_level + 1, C.PLANET_MAX_LEVEL);

        _ChangeLevelRoutine(_level, needMergingTime).Forget();
    }

    public void LevelDown(bool needMergingTime = false)
    {
        _isMerging = true;

        _level = Mathf.Max(_level - 1, 0);

        _ChangeLevelRoutine(_level, needMergingTime).Forget();
    }

    private async UniTask _ChangeLevelRoutine(int level, bool needMergingTime)
    {
        Rigid.velocity = Vector2.zero;
        Rigid.angularVelocity = 0;

        if (needMergingTime)
            await UniTask.Delay(200, cancellationToken: DisableCancellationToken);

        _PlayEffect();
        SFX.Play(Sfx.LevelUp);

        _animator.SetInteger(AniParam.LEVEL, level);
        _spriteRenderer.sprite = ResourceLoader.LoadSprite($"Level{level}");

        await UniTask.Delay(300, cancellationToken: DisableCancellationToken);

        PlayScene.Instance.MaxLevel = Mathf.Max(level, PlayScene.Instance.MaxLevel);

        _isMerging = false;
    }



    private void _PlayEffect()
    {
        var effect = ObjectPoolManager.Release<MergeEffect>();

        effect.Tr.position = tr.position;
        effect.Tr.localScale = tr.localScale;

        effect.Activate();
        effect.Play().Forget();
    }



    public override void Pool() => ObjectPoolManager.Return(this);
}
