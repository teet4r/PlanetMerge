using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    private void Start()
    {
        UIManager.Show<UIMainPopup>();
        BGM.Play(Bgm.BGM2);
    }
}
