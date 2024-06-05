using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSpriteRenderer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Coroutine _coloringRoutine;
    private Color _originColor;

    private void OnEnable()
    {
        _originColor = _spriteRenderer.color;
    }

    private void OnDisable()
    {
        if (_coloringRoutine != null)
            StopCoroutine(_coloringRoutine);
        _coloringRoutine = null;
        _spriteRenderer.color = _originColor;
    }

    public void StartColoringRGB(Color targetColor, float time)
    {
        if (_coloringRoutine != null)
            StopCoroutine(_coloringRoutine);
        _coloringRoutine = StartCoroutine(_ColoringRoutineRGB(targetColor, time));
    }

    private IEnumerator _ColoringRoutineRGB(Color targetColor, float time)
    {
        Color c = new Color();

        var r = (targetColor.r - _spriteRenderer.color.r) / time;
        var g = (targetColor.g - _spriteRenderer.color.g) / time;
        var b = (targetColor.b - _spriteRenderer.color.b) / time;

        while (_spriteRenderer.color != targetColor)
        {
            c.r = r * Time.deltaTime;
            c.g = g * Time.deltaTime;
            c.b = b * Time.deltaTime;

            _spriteRenderer.color += c;
            Debug.Log(_spriteRenderer.color);

            yield return null;
        }
    }
}