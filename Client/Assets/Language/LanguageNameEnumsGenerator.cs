using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using ExcelDataReader;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(LanguageNameEnumsGenerator))]
public class GenerateLanguageNameEnums : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StringBuilder lines = new();
        List<string> languages = new();

        if (GUILayout.Button("Generate Language Name Enums"))
        {
            using (var stream = File.Open($"{Application.dataPath}/Language/LanguagePack.xlsx", FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var table = reader.AsDataSet().Tables[0];
                    var rows = table.Rows;
                    var columnCount = table.Columns.Count;

                    for (int j = 1; j < columnCount; ++j)
                        languages.Add(rows[0][j].ToString());
                }
            }

            for (int i = 0; i < languages.Count; ++i)
                lines.AppendLine($"\t{languages[i]},");

            StringBuilder script = new();

            script.AppendLine("public enum Language")
                .AppendLine("{")
                .Append(lines)
                .AppendLine("}");

            StreamWriter sw = new(File.Create($"{Application.dataPath}/Scripts/Enums/Language.cs"));

            sw.Write(script.ToString());
            sw.Close();

            AssetDatabase.Refresh();

            Debug.Log("Generate Language Name Enums!");
        }
    }
}
#endif

public class LanguageNameEnumsGenerator : MonoBehaviour
{

}