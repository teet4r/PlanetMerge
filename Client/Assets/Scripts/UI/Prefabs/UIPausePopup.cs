using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPausePopup : UI
{
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Button _goToGameButton;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _goToMainButton;

    private void Awake()
    {
        _bgmSlider.onValueChanged.AddListener(volume => BGM.Volume = volume);
        _sfxSlider.onValueChanged.AddListener(volume => SFX.Volume = volume);

        _goToGameButton.onClick.AddListener(() =>
        {
            SFX.PlayButtonClick();
            Hide();
        });

        _retryButton.onClick.AddListener(() =>
        {
            SFX.PlayButtonClick();
            UIManager.Get<UIRemindPopup>()
                .SetTitle("다시하기")
                .SetDescription("처음부터 다시 하시겠습니까?")
                .SetYesButton(() =>
                {
                    SFX.PlayButtonClick();
                    CustomSceneManager.LoadSceneAsync(SceneName.Play).Forget();
                })
                .SetNoButton(null)
                .Show();
        });

        _goToMainButton.onClick.AddListener(() =>
        {
            SFX.PlayButtonClick();
            UIManager.Get<UIRemindPopup>()
                .SetTitle("메인으로 가기")
                .SetDescription("메인으로 돌아가시겠습니까?")
                .SetYesButton(() =>
                {
                    SFX.PlayButtonClick();
                    CustomSceneManager.LoadSceneAsync(SceneName.Main).Forget();
                })
                .SetNoButton(null)
                .Show();
        });
    }

    public void Bind()
    {
        _bgmSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKey.BGM_VALUE, 1f);
        _sfxSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKey.SFX_VALUE, 1f);
    }
}
