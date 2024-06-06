using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class ColorSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Color _originColor;
    private TweenerCore<Color, Color, ColorOptions> _tweenerCore;

    private void OnEnable()
    {
        _originColor = _spriteRenderer.color;
    }

    private void OnDisable()
    {
        _spriteRenderer.color = _originColor;
    }

    public void Color(Color endValue, float duration)
    {
        _tweenerCore?.Kill();
        _tweenerCore = _spriteRenderer.material.DOColor(endValue, duration);
    }
}