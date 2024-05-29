using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Api_UserLogin
{
    public class Request
    {
		public string userId;
    }

    public class Response
    {
    }

    public static async UniTask<Response> Send(string userId)
    {
        var result = await WebSocketManager.Send<Request, Response>(
            "UserLogin",
            new Request()
            {
				userId = userId,
            }
        );

        return result;
    }
}
