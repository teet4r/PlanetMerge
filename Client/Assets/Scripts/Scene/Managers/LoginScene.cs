using Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : SceneSingletonBehaviour<LoginScene>
{
    private void Start()
    {
        UIManager.Show<UILoginPopup>();
    }
}
