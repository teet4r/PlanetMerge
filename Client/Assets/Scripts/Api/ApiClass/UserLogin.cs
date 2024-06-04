using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserLogin
{
    public static async UniTask<Api_UserLogin.Response> Send()
    {
        var result = await Api_UserLogin.Send(GoogleLoginManager.UserId);
        if (!result.success)
            return result;
        
        User.LoginType = LoginType.Google;
        PlayerPrefs.SetInt(PlayerPrefsKey.LAST_LOGIN, (int)LoginType.Google);
        User.UpdateData(result.userData);

        return result;
    }
}
