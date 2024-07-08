using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public RectTransform RectTr => _rectTr;
    private RectTransform _rectTr;

    protected Action onHide;
    protected List<IDisposable> disposablesOnHide = new();

    public void Initialize()
    {
        _rectTr = GetComponent<RectTransform>();

        // "$$"이 있는 문구 번역
        var texts = _rectTr.GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; ++i)
            if (texts[i].text.StartsWith("$$"))
                texts[i].text = Translator.Get(texts[i].text);
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
