using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UI : MonoBehaviour
{
    public RectTransform RectTr => _rectTr;
    private RectTransform _rectTr;

    protected List<IDisposable> disposables = new();

    public void Initialize()
    {
        _rectTr = GetComponent<RectTransform>();
    }

    public virtual void Show() => gameObject.SetActive(true);
    public virtual void Hide()
    {
        for (int i = 0; i < disposables.Count; ++i)
            disposables[i]?.Dispose();
        disposables.Clear();

        gameObject.SetActive(false);
    }
}
