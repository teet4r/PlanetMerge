using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(SFX))]
public class GenerateSfxNameEnums : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StringBuilder enums = new();

        if (GUILayout.Button("Generate Sfx Name Enums"))
        {
            var sfxs = Resources.LoadAll("AudioClips/Sfx/");

            for (int i = 0; i < sfxs.Length; ++i)
                enums.AppendLine($"\t{sfxs[i].name},");

            StringBuilder script = new();

            script.AppendLine("public enum Sfx")
                .AppendLine("{")
                .Append(enums)
                .AppendLine("}");

            StreamWriter sw = new(File.Create($"{Application.dataPath}/Scripts/Enums/Sfx.cs"));

            sw.Write(script.ToString());
            sw.Close();

            AssetDatabase.Refresh();

            Debug.Log("Generate Sfx Name Enums!");
        }
    }
}
#endif