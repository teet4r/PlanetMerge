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
        _originColor = _spriteRenderer.material.color;
    }

    private void OnDisable()
    {
        _spriteRenderer.material.color = _originColor;
    }

    public void Color(Color endValue, float duration)
    {
        _tweenerCore?.Kill();
        _tweenerCore = _spriteRenderer.material.DOColor(endValue, duration);
    }

    private Color _temp = new();
    public void Color(Color target, float curValue, float maxValue)
    {
        _temp.r = _spriteRenderer.material.color.r + (target.r - _originColor.r) * (curValue / maxValue);
        _temp.g = _spriteRenderer.material.color.g + (target.g - _originColor.g) * (curValue / maxValue);
        _temp.b = _spriteRenderer.material.color.b + (target.b - _originColor.b) * (curValue / maxValue);
        _temp.a = _originColor.a;

        _spriteRenderer.material.color = _temp;
    }
}