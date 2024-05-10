using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExitPopup : UI
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _noButton;
    [SerializeField] private Button _yesButton;

    private void Awake()
    {
        _closeButton.onClick.AddListener(() =>
        {
            Hide();
            SFX.Play(Sfx.Button);
        });

        _noButton.onClick.AddListener(() =>
        {
            Hide();
            SFX.Play(Sfx.Button);
        });

        _yesButton.onClick.AddListener(() =>
        {
            Application.Quit();
            SFX.Play(Sfx.Button);
        });
    }
}
