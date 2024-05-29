using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class UILoginPopup : UI
{
    [SerializeField] private Button _googleLogin;
    [SerializeField] private Button _guestLogin;

    private LoginType _lastLoginType;

    private void Awake()
    {
        _lastLoginType = (LoginType)PlayerPrefs.GetInt(PlayerPrefsKey.LAST_LOGIN, 0);

        _googleLogin.onClick.AddListener(async () =>
        {
            await GoogleLoginManager.SignInWithGoogle();

            CustomSceneManager.LoadSceneAsync(SceneName.Main).Forget();
        });

        _guestLogin.onClick.AddListener(() =>
        {

        });
    }

    private void Start()
    {
        if (_lastLoginType == LoginType.Guest)
            _guestLogin.onClick.Invoke();
        else if (_lastLoginType == LoginType.Google)
            _googleLogin.onClick.Invoke();
    }
}
