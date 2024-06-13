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

    private void Awake()
    {
        _pauseButton.onClick.AddListener(() =>
        {
            UIManager.Show<UIPausePopup>();
            SFX.Play(Sfx.Button);
        });
    }

    public void Bind()
    {
        PlayScene.Instance.Score
            .Subscribe(score => _curScoreText.text = score.ToString())
            .AddTo(disposables);
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
