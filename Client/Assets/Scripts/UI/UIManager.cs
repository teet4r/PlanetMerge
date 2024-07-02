using Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonBehaviour<UIManager>
{
    protected static RectTransform RectTr => _rectTr;
    private static RectTransform _rectTr;
    private static CanvasScaler _canvasScaler;

    private static Dictionary<Type, UI> _uiPool = new();

    private static float _heightRatio;
    private static float _widthRatio;

    protected override void Awake()
    {
        base.Awake();

        _rectTr = GetComponent<RectTransform>();
        _canvasScaler = GetComponent<CanvasScaler>();

        _heightRatio = Screen.height / _canvasScaler.referenceResolution.y;
        _widthRatio = Screen.width / _canvasScaler.referenceResolution.x;
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

    /// <summary>
    /// 절대 길이를 상대 길이로 변환
    /// </summary>
    /// <param name="height"></param>
    /// <returns></returns>
    public static float ToReactiveHeight(float height)
    {
        return height * _heightRatio;
    }

    public static float ToReactiveWidth(float width)
    {
        return width * _widthRatio;
    }

    public static Vector2 ToReactiveSize(Vector2 size)
    {
        size.y = ToReactiveHeight(size.y);
        size.x = ToReactiveWidth(size.x);

        return size;
    }
}
