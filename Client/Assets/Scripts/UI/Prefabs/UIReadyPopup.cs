using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReadyPopup : UI
{
    [SerializeField] private CanvasGroup _canvasGroup;
    private TweenerCore<float, float, FloatOptions> _tweener;

    private void Awake()
    {
        onHide += () => _tweener?.Kill();
    }

    public void Bind()
    {
        _tweener?.Kill();
        _canvasGroup.alpha = 1f;
        _tweener = _canvasGroup.DOFade(0f, 1.5f).OnComplete(() => Hide());
    }
}
