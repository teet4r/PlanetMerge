using Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class FirstSceneLoader
{
    static FirstSceneLoader()
    {
        var firstScenePath = EditorBuildSettings.scenes[0].path;
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(firstScenePath);
        EditorSceneManager.playModeStartScene = sceneAsset;
    }
}
#endif

public class CustomSceneManager : SingletonBehaviour<CustomSceneManager>
{
    public static void LoadScene(SceneName sceneName)
    {
        SceneManager.LoadScene((int)sceneName);
    }
}