using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationScene : MonoBehaviour
{
    private void Start()
    {
        CustomSceneManager.LoadSceneAsync(SceneName.Initialization).Forget();
    }
}
