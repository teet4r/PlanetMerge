using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExitPopup : UI
{
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _cancelButton.onClick.AddListener(() =>
        {
            Hide();
            SFX.Play(Sfx.Button);
        });

        _exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
            SFX.Play(Sfx.Button);
        });
    }
}
