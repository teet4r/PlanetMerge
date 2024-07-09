using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Cysharp.Threading.Tasks;

public static class Translator
{
    public static Language CurLanguage => _curLan;
    public static string CurLocalLanguage => LocalLanguages[(int)_curLan];
    public static List<string> LocalLanguages = new(); // 한국어, English, 日本語, ... (LanguagePack.xlsx 2행)

    private static Dictionary<string, string> _dict = new();
    private static Language _curLan;

    public static void Initialize()
    {
        _curLan = (Language)PlayerPrefs.GetInt(PlayerPrefsKey.LANGUAGE, (int)Language.Korean);

        _ReadLanguageSheet();
    }

    private static void _ReadLanguageSheet()
    {
        string path = null;
        string json = null;
#if UNITY_EDITOR
        path = $"{Application.dataPath}/StreamingAssets/LanguagePack.json";
        json = File.ReadAllText(path);
#elif UNITY_ANDROID
        path = $"jar:file://{Application.dataPath}!/assets/LanguagePack.json";
        WWW reader = new WWW(path);
        while (!reader.isDone) { }
        json = reader.text;
#endif
        var translationContainer = JsonUtility.FromJson<LanguageResolver.TranslationContainer>(json);
        var translationPairs = translationContainer.pairs;
        var rows = translationPairs.Length;
        var cols = translationPairs[0].translations.Length;

        LocalLanguages.Clear();
        for (int j = 0; j < cols; ++j)
            LocalLanguages.Add(translationPairs[0].translations[j].ToString());

        var langIdx = (int)_curLan;

        _dict.Clear();
        for (int i = 1; i < rows; ++i)
            _dict.Add(translationPairs[i].key, translationPairs[i].translations[langIdx].ToString());
    }

    public static string Get(string korFormat, params object[] args)
        => string.Format(_dict[korFormat], args);

    public static void ChangeLanguage(Language newLanguage)
    {
        if (_curLan == newLanguage)
            return;

        PlayerPrefs.SetInt(PlayerPrefsKey.LANGUAGE, (int)newLanguage);
        CustomSceneManager.LoadSceneAsync(SceneName.Initialization).Forget();
    }
}
