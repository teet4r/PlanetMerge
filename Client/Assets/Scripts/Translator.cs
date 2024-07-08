using System.Collections.Generic;
using UnityEngine;
using ExcelDataReader;
using System.IO;

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
        using (var stream = File.Open($"{Application.dataPath}/Language/LanguagePack.xlsx", FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                var sheet = result.Tables[0];
                var rows = sheet.Rows;
                var columnCount = sheet.Columns.Count;

                LocalLanguages.Clear();
                for (int j = 1; j < columnCount; ++j)
                    LocalLanguages.Add(rows[1][j].ToString());

                var langIdx = (int)_curLan + 1;

                for (int i = 2; i < rows.Count; ++i)
                    _dict.Add(rows[i][0].ToString(), rows[i][langIdx].ToString());
            }
        }
    }

    public static string Get(string korFormat, params object[] args)
        => string.Format(_dict[korFormat], args);

    public static void ChangeLanguage(Language newLanguage)
    {
        if (_curLan == newLanguage)
            return;

        PlayerPrefs.SetInt(PlayerPrefsKey.LANGUAGE, (int)newLanguage);
    }
}
