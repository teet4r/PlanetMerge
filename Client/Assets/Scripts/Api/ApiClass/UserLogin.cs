using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserLogin
{
    public static async UniTask<Api_UserLogin.Response> Send()
    {
        var result = await Api_UserLogin.Send(GoogleLoginManager.UserId);

        return result;
    }
}
