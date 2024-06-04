using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Api_Heartbeat
{
    [System.Serializable]
    public class Request
    {
    }

    [System.Serializable]
    public class Response
    {
		public bool success;
    }

    public static async UniTask<Response> Send()
    {
        var result = await WebSocketManager.Send<Request, Response>(
            "Heartbeat",
            new Request()
            {

            }
        );

        return result;
    }
}