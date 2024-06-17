using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UI : MonoBehaviour
{
    public RectTransform RectTr => _rectTr;
    private RectTransform _rectTr;

    protected Action onHide;
    protected List<IDisposable> disposablesOnHide = new();

    public void Initialize()
    {
        _rectTr = GetComponent<RectTransform>();
    }

    public virtual void Show() => gameObject.SetActive(true);
    public virtual void Hide()
    {
        for (int i = 0; i < disposablesOnHide.Count; ++i)
            disposablesOnHide[i]?.Dispose();
        disposablesOnHide.Clear();

        onHide?.Invoke();

        gameObject.SetActive(false);
    }
}
