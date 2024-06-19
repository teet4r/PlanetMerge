using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameoverPopup : UI
{
    [SerializeField] private Button _goMainButton;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _bestScoreText;
    [SerializeField] private GameObject _newBestScoreGroup;

    private void Awake()
    {
        _goMainButton.onClick.AddListener(() =>
        {
            SFX.PlayButtonClick();

            AdManager.ShowInterstitialAd(() => CustomSceneManager.LoadSceneAsync(SceneName.Main).Forget());
        });

        _retryButton.onClick.AddListener(() =>
        {
            SFX.PlayButtonClick();

            AdManager.ShowInterstitialAd(() => CustomSceneManager.LoadSceneAsync(SceneName.Play).Forget());
        });
    }

    public void Bind(long score, long prevHighestScore, long curHighestScore)
    {
        _scoreText.text = $"���� ���� : {score}";
        _bestScoreText.text = $"�ְ� ���� : {curHighestScore}";
        _newBestScoreGroup.SetActive(prevHighestScore < curHighestScore);
    }
}
