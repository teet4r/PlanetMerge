using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(CustomSceneManager))]
public class GenerateSceneNameEnums : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StringBuilder enums = new();

        if (GUILayout.Button("Generate Scene Name Enums"))
        {
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled)
                    continue;

                int lastSlash = scene.path.LastIndexOf('/');
                int period = scene.path.IndexOf('.', lastSlash);

                enums.AppendLine($"\t{scene.path.Substring(lastSlash + 1, period - (lastSlash + 1))},");
            }

            //StreamReader sr = new("ProjectSettings/EditorBuildSettings.asset");
            //string line;

            //while (!(line = sr.ReadLine()).Contains("m_Scenes"));

            //while (true)
            //{
            //    var enabled = sr.ReadLine();
            //    if (!enabled.Contains("enabled"))
            //        break;
            //    var isEnabled = enabled[enabled.Length - 1] == '1';

            //    var path = sr.ReadLine();
            //    if (isEnabled)
            //    {
            //        var tokens = path.Split('/');
            //        enums.AppendLine($"\t{tokens[tokens.Length - 1].Replace(".unity", ",")}");
            //    }

            //    var guid = sr.ReadLine();
            //}

            StringBuilder script = new();

            script.AppendLine("public enum SceneName")
                .AppendLine("{")
                .Append(enums)
                .AppendLine("}");

            StreamWriter sw = new(File.Create($"{Application.dataPath}/Scripts/Enums/SceneName.cs"));

            sw.Write(script.ToString());
            sw.Close();

            AssetDatabase.Refresh();

            Debug.Log("Generate Scene Name Enums!");
        }
    }
}
#endif