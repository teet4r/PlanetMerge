using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingPopup : UI
{
    [SerializeField] private Button _goToStore;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;

    private void Awake()
    {
        _bgmSlider.onValueChanged.AddListener(volume => BGM.Volume = volume);
        _sfxSlider.onValueChanged.AddListener(volume => SFX.Volume = volume);

        _closeButton.onClick.AddListener(() =>
        {
            Hide();
            SFX.PlayButtonClick();
        });

        _goToStore.onClick.AddListener(() =>
        {
            UIManager.Show<UIReadyPopup>().Bind();
            //Application.OpenURL("market://details?id=com.Company.ProductName");
        });
    }

    public void Bind()
    {
        _bgmSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKey.BGM_VALUE, 1f);
        _sfxSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKey.SFX_VALUE, 1f);
    }
}
