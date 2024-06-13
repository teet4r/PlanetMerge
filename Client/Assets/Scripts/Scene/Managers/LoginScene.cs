using Behaviour;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class LoginScene : SceneSingletonBehaviour<LoginScene>
{
    private LoginType _lastLoginType;

    private void Start()
    {
        _AutoLogin().Forget();
    }

    private async UniTask _AutoLogin()
    {
        _lastLoginType = (LoginType)PlayerPrefs.GetInt(PlayerPrefsKey.LAST_LOGIN, 0);
        if (_lastLoginType == LoginType.None ||
            _lastLoginType == LoginType.Guest ||
            !await LoginProcess(_lastLoginType))
        {
            UIManager.Show<UILoginPopup>();
        }
    }

    public async UniTask<bool> LoginProcess(LoginType loginType)
    {
        if (loginType == LoginType.Guest)
        {
            User.LoginType = LoginType.Guest;
            User.UpdateData(new UserData()
            {
                uid = "",
                highestScore = long.Parse(PlayerPrefs.GetString(PlayerPrefsKey.HIGHEST_SCORE, "0")),
            });
            PlayerPrefs.SetInt(PlayerPrefsKey.LAST_LOGIN, (int)LoginType.Guest);
            CustomSceneManager.LoadSceneAsync(SceneName.Main).Forget();
            return true;
        }

        if (!await GoogleLoginManager.SignInWithGoogleClient()) // client validation
            return false;
        
        await WebSocketManager.Open();

        if (!(await UserLogin.Send()).success) // server validation
            return false;

        CustomSceneManager.LoadSceneAsync(SceneName.Main).Forget();

        return true;
    }
}
