using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RenewalHighestScore
{
    public static async UniTask<Api_RenewalHighestScore.Response> Send(long score)
    {
        var result = await Api_RenewalHighestScore.Send(User.UserId, score);
        if (!result.success)
            return result;

        User.HighestScore = result.highestScore;

        return result;
    }
}
