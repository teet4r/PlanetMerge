using Behaviour;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using WebSocketSharp;

public class WebSocketManager : SingletonBehaviour<WebSocketManager>
{
    private static WebSocket _socket;
    private static Dictionary<string, string> _packetQ;
    private static HashSet<string> _exceptLoadingPopupApi;
    private static UILoadingPopup _loading;
    private static CancellationTokenSource _cancellationTokenSource;

    private void OnDestroy()
    {
        Close();
    }

    public static void Open()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = new();

        //_socket = new WebSocket("ws://localhost:8000");
        _socket = new WebSocket("ws://0.tcp.jp.ngrok.io:15502");
        _socket.Connect();

        _socket.OnMessage += (sender, e) =>
        {
            //Debug.Log($"{((WebSocket)sender).Url} : {e.Data}");
            var infos = e.Data.Split('/');

            _packetQ[infos[0]] = infos[1];
        };

        _packetQ = new();
        _exceptLoadingPopupApi = new()
        {
            "Heartbeat",
        };
    }

    public static void Close()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;

        _socket?.Close();
        _socket = null;

        _packetQ = null;
        _exceptLoadingPopupApi = null;
    }

    public static async UniTask<Res> Send<Req, Res>(string apiName, Req request)
    {
        if (!_exceptLoadingPopupApi.Contains(apiName))
            _loading = UIManager.Show<UILoadingPopup>();
        if (!_packetQ.ContainsKey(apiName))
            _packetQ.Add(apiName, null);

        _socket.Send($"{apiName}/{JsonUtility.ToJson(request)}");

        await UniTask.WaitUntil(() => _packetQ[apiName] != null);

        var response = JsonUtility.FromJson<Res>(_packetQ[apiName]);

        _packetQ[apiName] = null;
        if (!_exceptLoadingPopupApi.Contains(apiName))
            _loading.Hide();

        return response;
    }

    public static async UniTask StartHeartbeat()
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            await Heartbeat.Send();
            await UniTask.Delay(10000, cancellationToken: _cancellationTokenSource.Token);
        }
    }
}
