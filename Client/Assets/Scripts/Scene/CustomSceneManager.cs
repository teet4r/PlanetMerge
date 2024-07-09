using Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

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
    public static async UniTask LoadSceneAsync(SceneName sceneName)
    {
        await SceneManager.LoadSceneAsync((int)SceneName.Empty);
        await UniTask.DelayFrame(1);
        
        UIManager.HideAll();
        UIManager.ClearAll();
        ObjectPoolManager.HideAll();
        ObjectPoolManager.ClearAll();

        await SceneManager.LoadSceneAsync((int)sceneName);
        await UniTask.DelayFrame(1);
    }
}