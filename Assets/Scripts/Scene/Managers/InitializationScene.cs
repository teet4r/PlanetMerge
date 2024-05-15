using Behaviour;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializationScene : SceneSingletonBehaviour<InitializationScene>
{
    protected override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        CustomSceneManager.LoadSceneAsync(SceneName.Main).Forget();
    }
}
