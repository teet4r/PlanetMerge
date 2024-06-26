using System.Collections.Generic;
using UnityEngine;
using ExcelDataReader;
using System.IO;

public static class Translator
{
    public static Language CurLanguage => _curLan;

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
                var rows = result.Tables[(int)_curLan].Rows;

                for (int i = 0; i < rows.Count; ++i)
                    _dict.Add(rows[i][0].ToString(), rows[i][1].ToString());
            }
        }
    }

    public static string Get(string kor)
    {
        if (_curLan == Language.Korean)
            return kor;
        return _dict[kor];
    }

    public static void ChangeLanguage(Language newLanguage)
    {
        if (_curLan == newLanguage)
            return;

        PlayerPrefs.SetInt(PlayerPrefsKey.LANGUAGE, (int)newLanguage);
    }
}
