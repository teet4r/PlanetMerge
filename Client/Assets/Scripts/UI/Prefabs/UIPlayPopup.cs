using log4net.Core;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayPopup : UI, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Text _curScoreText;
    [SerializeField] private CustomToggle _boomItemToggle;
    [SerializeField] private CustomButton _upGradeItemButton;
    [SerializeField] private CustomButton _downGradeItemButton;

    private Camera _mainCamera;
    private bool _boomItemMode;

    private void OnEnable()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _pauseButton.onClick.AddListener(() =>
        {
            UIManager.Show<UIPausePopup>();
            SFX.Play(Sfx.Button);
        });

        _boomItemToggle.AddListener(isOn => _boomItemMode = isOn);
        _upGradeItemButton.AddListener(() => PlayScene.Instance.LastPlanet.Value?.LevelUp());
        _downGradeItemButton.AddListener(() => PlayScene.Instance.LastPlanet.Value?.LevelDown());
    }

    public void Bind()
    {
        PlayScene.Instance.LastPlanet.Subscribe(lastPlanet => 
        {
            if (lastPlanet == null)
            {
                _upGradeItemButton.interactable = false;
                _downGradeItemButton.interactable = false;
            }
            else
            {
                lastPlanet.Level.Subscribe(level =>
                {
                    _upGradeItemButton.interactable = level < C.PLANET_MAX_LEVEL;
                    _downGradeItemButton.interactable = level > 0;
                }).AddTo(lastPlanet.Disposables);
            }
        }).AddTo(disposables);

        PlayScene.Instance.Score
            .Subscribe(score => _curScoreText.text = score.ToString())
            .AddTo(disposables);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_boomItemMode)
            PlayScene.Instance.TouchDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_boomItemMode)
            PlayScene.Instance.TouchUp();
        else
            _BoomPlanet();
    }

    private void _BoomPlanet()
    {
        Vector2 touchPosition;

#if UNITY_EDITOR
        touchPosition = Input.mousePosition;
#else
        touchPosition = Input.touches[0].position;
#endif
        var collider = Physics2D.OverlapPoint(_mainCamera.ScreenToWorldPoint(touchPosition));
        if (collider == null)
            return;

        if (!collider.TryGetComponent(out Planet planet))
            return;

        planet.Hide(Vector3.up * 100);
        _boomItemToggle.IsOn = false;
    }
}
