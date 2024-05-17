using Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager>
{
    protected static RectTransform RectTr => _rectTr;
    private static RectTransform _rectTr;

    private static Dictionary<Type, UI> _uiPool = new();

    protected override void Awake()
    {
        base.Awake();

        _rectTr = GetComponent<RectTransform>();
    }

    public static T Get<T>() where T : UI
    {
        var type = typeof(T);

        if (!_uiPool.TryGetValue(type, out UI ui))
        {
            var prefab = Resources.Load<T>($"Prefabs/UIs/{type}");

            prefab.gameObject.SetActive(false);
            ui = Instantiate(prefab, _rectTr);
            prefab.gameObject.SetActive(true);
            ui.Initialize();

            _uiPool.Add(type, ui);
        }

        return ui as T;
    }

    public static T Show<T>() where T : UI
    {
        var ui = Get<T>();

        ui.Show();

        return ui;
    }

    public static void HideAll()
    {
        foreach (var ui in _uiPool.Values)
            ui.Hide();
    }

    public static void ClearAll()
    {
        foreach (var ui in _uiPool.Values)
            Destroy(ui.gameObject);
        _uiPool.Clear();
    }
}
