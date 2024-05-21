using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIGameoverPopup : UI
{
    [SerializeField] private Button _goMainButton;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Text _scoreText;

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

    public void Bind(long score)
    {
        _scoreText.text = $"���� : {score}";
    }
}
