using System.Collections.Generic;
using UnityEngine;
using ExcelDataReader;
using System.IO;

public static class Translator
{
    private static Dictionary<string, string> _dict = new();
    private static Language _curLan;

    public static void Initialize()
    {
        _curLan = (Language)PlayerPrefs.GetInt(PlayerPrefsKey.LANGUAGE, (int)Language.Korean);

        _ReadLanguageSheet();
    }

    private static void _ReadLanguageSheet()
    {
        if (_curLan == Language.Korean)
            return;

        using (var stream = File.Open($"{Application.dataPath}/Language/LanguagePack.xlsx", FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                var rows = result.Tables[0].Rows;
                var korIdx = (int)Language.Korean;
                var langIdx = (int)_curLan;

                for (int i = 1; i < rows.Count; ++i)
                    _dict.Add(rows[i][korIdx].ToString(), rows[i][langIdx].ToString());
            }
        }
    }

    public static string Get(string korFormat, params object[] args)
    {
        string format;

        if (_curLan == Language.Korean)
            format = korFormat;
        else
            format = _dict[korFormat];

        return string.Format(format, args);
    }

    public static void ChangeLanguage(Language newLanguage)
    {
        if (_curLan == newLanguage)
            return;

        PlayerPrefs.SetInt(PlayerPrefsKey.LANGUAGE, (int)newLanguage);
    }
}
