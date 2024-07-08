using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ApplicationExtensions
{
    // 어플리케이션 재시작
    public static void Restart()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        var endings = new string[] { "exe", "x86", "x86_64", "app" };
        var executablePath = Application.dataPath + "/..";
        var files = System.IO.Directory.GetFiles(executablePath);

        for (int i = 0; i < files.Length; ++i)
            for (int j = 0; j < endings.Length; ++j)
                if (files[i].ToLower().EndsWith($".{endings[j]}"))
                {
                    System.Diagnostics.Process.Start($"{executablePath}{files[i]}");
                    Application.Quit();
                    return;
                }
#endif
    }
}
