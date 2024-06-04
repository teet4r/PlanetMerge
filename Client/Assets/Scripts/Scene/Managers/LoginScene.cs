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
        WebSocketManager.Open();

        _AutoLogin().Forget();
    }

    private async UniTask _AutoLogin()
    {
        _lastLoginType = (LoginType)PlayerPrefs.GetInt(PlayerPrefsKey.LAST_LOGIN, 0);
        if (_lastLoginType != LoginType.None)
            if (await LoginProcess(_lastLoginType))
                return;

        UIManager.Show<UILoginPopup>();
    }

    public async UniTask<bool> LoginProcess(LoginType loginType)
    {
        if (loginType == LoginType.Guest)
        {
            PlayerPrefs.SetInt(PlayerPrefsKey.LAST_LOGIN, (int)LoginType.Guest);
            WebSocketManager.Close();
        }
        else if (loginType == LoginType.Google)
        {
            if (!await GoogleLoginManager.SignInWithGoogleClient()) // client validation
                return false;
            if (!(await UserLogin.Send()).success) // server validation
                return false;
            
            WebSocketManager.StartHeartbeat().Forget();
        }

        CustomSceneManager.LoadSceneAsync(SceneName.Main).Forget();

        return true;
    }
}
