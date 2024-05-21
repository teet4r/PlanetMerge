using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class UILoginPopup : UI
{
    [SerializeField] private Button _googleLogin;
    [SerializeField] private Button _guestLogin;

    private void Awake()
    {
        _googleLogin.onClick.AddListener(async () =>
        {
            await GoogleLoginManager.Instance.SignInWithGoogle();

            CustomSceneManager.LoadSceneAsync(SceneName.Main).Forget();
        });

        _guestLogin.onClick.AddListener(() =>
        {

        });
    }
}
