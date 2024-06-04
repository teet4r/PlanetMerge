using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Api_UserLogin
{
    [System.Serializable]
    public class Request
    {
		public string uid;
    }

    [System.Serializable]
    public class Response
    {
		public UserData userData;
		public bool success;
    }

    public static async UniTask<Response> Send(string uid)
    {
        var result = await WebSocketManager.Send<Request, Response>(
            "UserLogin",
            new Request()
            {
				uid = uid,
            }
        );

        return result;
    }
}