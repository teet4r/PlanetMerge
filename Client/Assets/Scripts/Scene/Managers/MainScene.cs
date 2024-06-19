using Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : SceneSingletonBehaviour<MainScene>
{
    private void Start()
    {
        UIManager.Show<UIMainPopup>();

        BGM.Play(Bgm.AmongStars);
    }
}
