using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Api_RenewalHighestScore
{
    [System.Serializable]
    public class Request
    {
		public string uid;
		public long score;
    }

    [System.Serializable]
    public class Response
    {
		public long highestScore;
		public bool success;
    }

    public static async UniTask<Response> Send(string uid, long score)
    {
        var result = await WebSocketManager.Send<Request, Response>(
            "RenewalHighestScore",
            new Request()
            {
				uid = uid,
				score = score,
            }
        );

        return result;
    }
}