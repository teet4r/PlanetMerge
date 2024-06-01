using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public static class Api_GoogleUserTest
{
    public class Request
    {
		public string firebaseData;
    }

    public class Response
    {
    }

    public static async UniTask<Response> Send(string firebaseData)
    {
        var result = await WebSocketManager.Send<Request, Response>(
            "GoogleUserTest",
            new Request()
            {
				firebaseData = firebaseData,
            }
        );

        return result;
    }
}