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
        _socket = new WebSocket("ws://0.tcp.jp.ngrok.io:10726");
        _socket.Connect();
        
        _socket.OnMessage += (sender, e) =>
        {
            //Debug.Log($"{((WebSocket)sender).Url} : {e.Data}");
            var packet = JsonUtility.FromJson<Packet>(e.Data);

            _packetQ[packet.apiName] = packet.data;
        };
    }

    public static async UniTask<Res> Send<Req, Res>(string apiName, Req request)
    {
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
