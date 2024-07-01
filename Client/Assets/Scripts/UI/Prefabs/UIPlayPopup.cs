using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayPopup : UI, IPointerDownHandler, IPointerUpHandler
{
    [Header("---------- ���� ----------")]
    [Space(15)]
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Text _curScoreText;
    [SerializeField] private Text _curComboText;
    [SerializeField] private CustomToggle _boomItemToggle;
    [SerializeField] private CustomButton _upgradeItemButton;
    [SerializeField] private CustomButton _downgradeItemButton;
    [SerializeField] private Animator _scoreAnimator;
    [SerializeField] private Animator _comboAnimator;

    [Header("---------- �ν����� ������ ----------")]
    [Space(15)]
    [SerializeField] private Color _boom�����۾��������һ���;

    private Camera _mainCamera;
    private bool _boomItemMode;
    private bool _availableBoomItem;
    private bool _availableUpgradeItem;
    private bool _availableDowngradeItem;

    private int _flinchTrigger = Animator.StringToHash("Flinch");
    private int _fadeoutTrigger = Animator.StringToHash("Fadeout");

    private void Start()
    {
        _pauseButton.onClick.AddListener(() =>
        {
            UIManager.Show<UIPausePopup>().Bind();
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

            var planets = FindObjectsOfType<Planet>();

            for (int i = 0; i < planets.Length; ++i)
                planets[i].SetColor(isOn ? _boom�����۾��������һ��� : Color.white);
        });

        _upgradeItemButton.AddListener(() =>
        {
            if (!_availableUpgradeItem)
                return;

            SFX.Play(Sfx.UpDowngrade);
            _availableUpgradeItem = false;
            PlayScene.Instance.LastPlanet?.LevelUp();
        });

        _downgradeItemButton.AddListener(() =>
        {
            if (!_availableDowngradeItem)
                return;

            SFX.Play(Sfx.UpDowngrade);
            _availableDowngradeItem = false;
            PlayScene.Instance.LastPlanet?.LevelDown();
        });

        onHide += () => AdManager.DestroyBannerView();

        Combo.OnCombo += () =>
        {
            _curComboText.text = $"{Combo.Count} Combo!";
            _comboAnimator.SetTrigger(_fadeoutTrigger);
        };
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
            .Subscribe(score =>
            {
                _curScoreText.text = score.Comma();
                if (score > 0)
                    _scoreAnimator.SetTrigger(_flinchTrigger);
            })
            .AddTo(disposablesOnHide);

        AdManager.LoadBannerAd();
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

        SFX.Play(Sfx.Boom);
        planet.Hide(Vector3.up * 100);
        _availableBoomItem = false;
        _boomItemToggle.IsOn = false;
        _boomItemToggle.Interactable = false;
    }
}
