using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(SpriteNameEnumsGenerator))]
public class GenerateSpriteNameEnums : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StringBuilder enums = new();

        if (GUILayout.Button("Generate Sprite Name Enums"))
        {
            var sprites = Resources.LoadAll<Sprite>("Sprites/");

            for (int i = 0; i < sprites.Length; ++i)
                enums.AppendLine($"\t{sprites[i].name},");

            StringBuilder script = new();

            script.AppendLine("public enum SpriteName")
                .AppendLine("{")
                .Append(enums)
                .AppendLine("}");

            StreamWriter sw = new(File.Create($"{Application.dataPath}/Scripts/Enums/SpriteName.cs"));

            sw.Write(script.ToString());
            sw.Close();

            AssetDatabase.Refresh();

            Debug.Log("Generate Sprite Name Enums!");
        }
    }
}
#endif