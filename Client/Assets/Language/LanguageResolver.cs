using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using ExcelDataReader;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(LanguageResolver))]
public class LanguageWorkerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Implement Conversion"))
        {
            StringBuilder lines = new();
            List<string> languages = new();
            LanguageResolver.TranslationContainer translationContainer = new();

            using (var stream = File.Open($"{Application.dataPath}/Language/LanguagePack.xlsx", FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var table = reader.AsDataSet().Tables[0];
                    var rows = table.Rows;
                    var columnCount = table.Columns.Count;

                    for (int j = 1; j < columnCount; ++j)
                        languages.Add(rows[0][j].ToString());

                    translationContainer.pairs = new LanguageResolver.TranslationPair[rows.Count - 1];

                    for (int i = 1; i < rows.Count; ++i)
                    {
                        var translatedSentences = new string[columnCount - 1];

                        for (int j = 1; j < columnCount; ++j)
                            translatedSentences[j - 1] = rows[i][j].ToString();

                        translationContainer.pairs[i - 1] = new LanguageResolver.TranslationPair()
                        {
                            key = rows[i][0].ToString(),
                            translations = translatedSentences,
                        };
                    }
                }
            }

            for (int i = 0; i < languages.Count; ++i)
                lines.AppendLine($"\t{languages[i]},");

            StringBuilder script = new();

            script.AppendLine("public enum Language")
                .AppendLine("{")
                .Append(lines)
                .AppendLine("}");

            using (StreamWriter swForEnum = new(File.Create($"{Application.dataPath}/Language/Language.cs")))
            {
                swForEnum.Write(script.ToString());
                swForEnum.Close();
            }
            using (StreamWriter swForJson = new(File.Create($"{Application.dataPath}/StreamingAssets/LanguagePack.json")))
            {
                swForJson.Write(JsonUtility.ToJson(translationContainer, true));
                swForJson.Close();
            }

            AssetDatabase.Refresh();

            Debug.Log("Conversion Complete!");
        }
    }
}
#endif

public class LanguageResolver : MonoBehaviour
{
    [System.Serializable]
    public class TranslationPair
    {
        public string key;
        public string[] translations;
    }

    [System.Serializable]
    public class TranslationContainer
    {
        public TranslationPair[] pairs;
    }
}