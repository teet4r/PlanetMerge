using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Api_RenewalHighestScore
{
    public class Request
    {
		public string userId;
		public long score;
    }

    public class Response
    {
		public long highestScore;
    }

    public static async UniTask<Response> Send(string userId, long score)
    {
        var result = await WebSocketManager.Send<Request, Response>(
            "RenewalHighestScore",
            new Request()
            {
				userId = userId,
				score = score,
            }
        );

        return result;
    }
}
