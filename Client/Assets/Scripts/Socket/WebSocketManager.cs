using Behaviour;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WebSocketManager : SingletonBehaviour<WebSocketManager>
{
    private class Packet
    {
        public string apiName;
        public string data;
    }

    private static WebSocket _socket;
    private static Dictionary<string, string> _packetQ = new();

    private void Start()
    {
        _socket = new WebSocket("ws://localhost:8000");
        _socket.Connect();
        
        _socket.OnMessage += (sender, e) =>
        {
            //Debug.Log($"{((WebSocket)sender).Url} : {e.Data}");
            var packet = JsonUtility.FromJson<Packet>(e.Data);

            _packetQ[packet.apiName] = packet.data;
        };
    }

    private void Update()
    {
        if (_socket == null)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            _CallTest().Forget();
    }

    private async UniTask _CallTest()
    {
        var result = await Api_RenewalHighestScore.Send("1234", 55555);
    }

    public static async UniTask<Res> Send<Req, Res>(string apiName, Req request)
    {
        // 이미 응답 대기 중인 패킷이 있는데 또 보내려하면?
        if (!_packetQ.ContainsKey(apiName))
            _packetQ.Add(apiName, null);

        _socket.Send(JsonUtility.ToJson(new Packet()
        {
            apiName = apiName,
            data = JsonUtility.ToJson(request),
        }));

        await UniTask.WaitUntil(() => _packetQ[apiName] != null);

        var response = JsonUtility.FromJson<Res>(_packetQ[apiName]);

        _packetQ[apiName] = null;

        return response;
    }
}
