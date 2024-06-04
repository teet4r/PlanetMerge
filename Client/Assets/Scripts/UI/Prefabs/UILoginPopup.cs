using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class UILoginPopup : UI
{
    [SerializeField] private Button _googleLogin;
    [SerializeField] private Button _guestLogin;

    private void Awake()
    {
        User.Initialize();

        _googleLogin.onClick.AddListener(() =>
        {
            LoginScene.Instance.LoginProcess(LoginType.Google).Forget();
        });

        _guestLogin.onClick.AddListener(() =>
        {
            LoginScene.Instance.LoginProcess(LoginType.Guest).Forget();
        }); 
    }
}
