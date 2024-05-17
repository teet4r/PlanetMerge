using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingPopup : UI
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _goMainButton;

    private void Awake()
    {
        _closeButton.onClick.AddListener(() =>
        {
            Hide();
            SFX.Play(Sfx.Button);
        });

        _goMainButton.onClick.AddListener(() =>
        {
            SFX.Play(Sfx.Button);
            CustomSceneManager.LoadSceneAsync(SceneName.Main).Forget();
        });
    }
}
