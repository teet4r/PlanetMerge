using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainPopup : UI
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _illustratedButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _playButton.onClick.AddListener(() =>
        {
            SFX.Play(Sfx.Button);
            CustomSceneManager.LoadSceneAsync(SceneName.Play).Forget();
        });

        _illustratedButton.onClick.AddListener(() =>
        {
            SFX.Play(Sfx.Button);
            UIManager.Show<UIReadyPopup>().Bind();
            //UIManager.Show<UIIllustratedPopup>();
        });

        _settingButton.onClick.AddListener(() =>
        {
            SFX.Play(Sfx.Button);
            UIManager.Show<UISettingPopup>();
        });

        _exitButton.onClick.AddListener(() =>
        {
            SFX.Play(Sfx.Button);
            UIManager.Show<UIExitPopup>();
        });
    }
}
