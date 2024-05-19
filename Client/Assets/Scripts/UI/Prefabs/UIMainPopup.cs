using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainPopup : UI
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _howToButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Text _a;

    private void Awake()
    {
        _playButton.onClick.AddListener(() =>
        {
            SFX.Play(Sfx.Button);
            CustomSceneManager.LoadSceneAsync(SceneName.Play).Forget();
        });

        _howToButton.onClick.AddListener(() =>
        {
            UIManager.Show<UIHowToPopup>();
            SFX.Play(Sfx.Button);
        });

        _settingButton.onClick.AddListener(() =>
        {
            UIManager.Show<UISettingPopup>();
            SFX.Play(Sfx.Button);
        });

        _exitButton.onClick.AddListener(() =>
        {
            UIManager.Show<UIExitPopup>();
            SFX.Play(Sfx.Button);
        });

        _loginButton.onClick.AddListener(() =>
        {
            GoogleTester.Instance.SignInWithGoogle();
        });
    }

    private void Update()
    {
            _a.text = GoogleTester.Instance.infoText;
    }
}
