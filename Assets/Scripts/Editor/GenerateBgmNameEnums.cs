using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(BGM))]
public class GenerateBgmNameEnums : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StringBuilder enums = new();

        if (GUILayout.Button("Generate Bgm Name Enums"))
        {
            var bgms = Resources.LoadAll("AudioClips/Bgm/");

            for (int i = 0; i < bgms.Length; ++i)
                enums.AppendLine($"\t{bgms[i].name},");

            StringBuilder script = new();

            script.AppendLine("public enum Bgm")
                .AppendLine("{")
                .Append(enums)
                .AppendLine("}");

            StreamWriter sw = new(File.Create($"{Application.dataPath}/Scripts/Enums/Bgm.cs"));

            sw.Write(script.ToString());
            sw.Close();

            AssetDatabase.Refresh();

            Debug.Log("Generate Bgm Name Enums!");
        }
    }
}
#endif