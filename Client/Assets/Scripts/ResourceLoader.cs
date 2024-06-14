using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceLoader
{
    private static Dictionary<System.Type, Dictionary<string, Object>> _cache = new();

    public static Sprite LoadSprite(SpriteName spriteName)
    {
        return _Load<Sprite>("Sprites", spriteName.ToString());
    }

    private static T _Load<T>(string folderPath, string name) where T : Object
    {
        var type = typeof(T);

        if (!_cache.TryGetValue(type, out var dic))
            _cache.Add(type, dic = new());
        if (!dic.TryGetValue(name, out var obj))
            dic.Add(name, obj = Resources.Load<T>($"{folderPath}/{name}"));

        return obj as T;
    }
}
