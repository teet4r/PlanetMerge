using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayPopup : UI, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Text _bestScoreText;
    [SerializeField] private Text _curScoreText;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsKey.BEST_SCORE))
            PlayerPrefs.SetInt(PlayerPrefsKey.BEST_SCORE, 0);

        _bestScoreText.text = PlayerPrefs.GetInt(PlayerPrefsKey.BEST_SCORE).ToString();

        _pauseButton.onClick.AddListener(() =>
        {
            UIManager.Show<UISettingPopup>();
            SFX.Play(Sfx.Button);
        });

        PlayScene.Instance.Score
            .Subscribe(score => _curScoreText.text = score.ToString())
            .AddTo(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayScene.Instance.TouchDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayScene.Instance.TouchUp();
    }
}
