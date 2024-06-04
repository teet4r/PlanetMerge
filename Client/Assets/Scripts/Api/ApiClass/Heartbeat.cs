using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Heartbeat
{
    public static async UniTask<Api_Heartbeat.Response> Send()
    {
        var result = await Api_Heartbeat.Send();

        return result;
    }
}
