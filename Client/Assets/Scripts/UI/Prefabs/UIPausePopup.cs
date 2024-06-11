using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPausePopup : UI
{
    [SerializeField] private Button _goToGameButton;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _goToMainButton;

    private void Awake()
    {
        _goToGameButton.onClick.AddListener(() =>
        {
            Hide();
            SFX.Play(Sfx.Button);
        });

        _retryButton.onClick.AddListener(() =>
        {
            CustomSceneManager.LoadSceneAsync(SceneName.Play).Forget();
            SFX.Play(Sfx.Button);
        });

        _goToMainButton.onClick.AddListener(() =>
        {
            CustomSceneManager.LoadSceneAsync(SceneName.Main).Forget();
            SFX.Play(Sfx.Button);
        });
    }
}
