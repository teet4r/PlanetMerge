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
            SFX.Play(Sfx.Button);
            CustomSceneManager.LoadSceneAsync(SceneName.Main).Forget();
        });

        _retryButton.onClick.AddListener(() =>
        {
            SFX.Play(Sfx.Button);
            CustomSceneManager.LoadSceneAsync(SceneName.Play).Forget();
        });
    }

    public void Bind(long score, long highestScore)
    {
        _scoreText.text = $"현재 점수 : {score}";
        _bestScoreText.text = $"최고 점수 : {highestScore}";
        _newBestScoreGroup.SetActive(score > highestScore);
    }
}
