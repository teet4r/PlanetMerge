using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayPopup : UI, IPointerDownHandler, IPointerUpHandler
{
    [Header("---------- 참조 ----------")]
    [Space(15)]
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Text _curScoreText;
    [SerializeField] private CustomToggle _boomItemToggle;
    [SerializeField] private CustomButton _upgradeItemButton;
    [SerializeField] private CustomButton _downgradeItemButton;

    [Header("---------- 인스펙터 에디터 ----------")]
    [Space(15)]
    [SerializeField] private Color _boom아이템쓸때적용할색깔;

    private Camera _mainCamera;
    private bool _boomItemMode;
    private bool _availableBoomItem;
    private bool _availableUpgradeItem;
    private bool _availableDowngradeItem;

    private void Start()
    {
        _pauseButton.onClick.AddListener(() =>
        {
            UIManager.Show<UIPausePopup>();
            SFX.PlayButtonClick();
        });

        _boomItemToggle.AddListener(isOn =>
        {
            if (!_availableBoomItem)
            {
                _boomItemMode = false;
                _boomItemToggle.SetSprite(SpriteName.Close_X);
            }
            else
            {
                _boomItemMode = isOn;
                _boomItemToggle.SetSprite(isOn ? SpriteName.Close_X : SpriteName.Boom);
            }

            PlayScene.Instance?.Planets.ForEach(planet => planet.SetColor(isOn ? _boom아이템쓸때적용할색깔 : Color.white));
        });

        _upgradeItemButton.AddListener(() =>
        {
            if (!_availableUpgradeItem)
                return;

            _availableUpgradeItem = false;
            PlayScene.Instance.LastPlanet?.LevelUp();
        });

        _downgradeItemButton.AddListener(() =>
        {
            if (!_availableDowngradeItem)
                return;

            _availableDowngradeItem = false;
            PlayScene.Instance.LastPlanet?.LevelDown();
        });
    }

    public void Bind()
    {
        _mainCamera = Camera.main;

        _boomItemToggle.Interactable = true;
        _upgradeItemButton.Interactable = true;
        _downgradeItemButton.Interactable = true;

        _availableBoomItem = true;
        _availableUpgradeItem = true;
        _availableDowngradeItem = true;

        Observable.EveryFixedUpdate().Subscribe(t =>
        {
            if (PlayScene.Instance.LastPlanet == null)
            {
                _boomItemToggle.Interactable = false;
                _upgradeItemButton.Interactable = false;
                _downgradeItemButton.Interactable = false;
            }
            else
            {
                _boomItemToggle.Interactable = _availableBoomItem;

                var level = PlayScene.Instance.LastPlanet.Level;

                _upgradeItemButton.SetSprite(_availableUpgradeItem ? SpriteName.Upgrade : SpriteName.Close_X);
                _upgradeItemButton.Interactable = _availableUpgradeItem && level < C.PLANET_MAX_LEVEL;

                _downgradeItemButton.SetSprite(_availableDowngradeItem ? SpriteName.Downgrade : SpriteName.Close_X);
                _downgradeItemButton.Interactable = _availableDowngradeItem && level > 0;
            }
        }).AddTo(disposablesOnHide);

        PlayScene.Instance.Score
            .Subscribe(score => _curScoreText.text = score.ToString())
            .AddTo(disposablesOnHide);
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
        if (collider == null || !collider.TryGetComponent(out Planet planet))
            return;

        planet.Hide(Vector3.up * 100);
        _availableBoomItem = false;
        _boomItemToggle.IsOn = false;
        _boomItemToggle.Interactable = false;
    }
}
