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

        // Translator.Get()을 통해서 번역할 수 없는 텍스트(고정 텍스트)는 따로 번역
        // 이름을 '$'으로 시작
        var texts = _rectTr.GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; ++i)
            if (texts[i].name.StartsWith('$'))
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
