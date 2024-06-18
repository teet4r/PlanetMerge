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
            SFX.Play(Sfx.Button);
            UIManager.Get<UIRemindPopup>()
                .SetTitle("�ٽ��ϱ�")
                .SetDescription("ó������ �ٽ� �Ͻðڽ��ϱ�?")
                .SetYesButton(() =>
                {
                    SFX.Play(Sfx.Button);
                    CustomSceneManager.LoadSceneAsync(SceneName.Play).Forget();
                })
                .SetNoButton(null)
                .Show();
        });

        _goToMainButton.onClick.AddListener(() =>
        {
            SFX.Play(Sfx.Button);
            UIManager.Get<UIRemindPopup>()
                .SetTitle("Ȩ���� ����")
                .SetDescription("Ȩ���� ���ư��ðڽ��ϱ�?")
                .SetYesButton(() =>
                {
                    SFX.Play(Sfx.Button);
                    CustomSceneManager.LoadSceneAsync(SceneName.Main).Forget();
                })
                .SetNoButton(null)
                .Show();
        });
    }
}
