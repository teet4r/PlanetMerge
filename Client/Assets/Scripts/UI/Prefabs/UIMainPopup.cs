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
    [SerializeField] private Text _bestScoreText;
    [SerializeField] private Text _versionText;

    private void Awake()
    {
        _playButton.onClick.AddListener(() =>
        {
            SFX.PlayButtonClick();
            CustomSceneManager.LoadSceneAsync(SceneName.Play).Forget();
        });

        _howToButton.onClick.AddListener(() =>
        {
            SFX.PlayButtonClick();
            UIManager.Show<UIHowToPopup>();
        });

        _settingButton.onClick.AddListener(() =>
        {
            SFX.PlayButtonClick();
            UIManager.Show<UISettingPopup>().Bind();
        });

        _exitButton.onClick.AddListener(() =>
        {
            SFX.PlayButtonClick();
            UIManager.Show<UIExitPopup>();
        });

        _versionText.text = $"ver.{Application.version}";
    }

    private void OnEnable()
    {
        _bestScoreText.text = $"최고 점수 : {PlayerPrefs.GetInt(PlayerPrefsKey.HIGHEST_SCORE, 0).Comma()}";
    }
}