using Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WebSocketManager : SingletonBehaviour<WebSocketManager>
{
    private WebSocket _webSocket;

    private void Start()
    {
        _webSocket = new WebSocket("ws://localhost:8000");
        _webSocket.Connect();

        _webSocket.OnMessage += (sender, e) =>
        {
            Debug.Log($"{((WebSocket)sender).Url} : {e.Data}");
        };
    }

    private void Update()
    {
        if (_webSocket == null)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _webSocket.Send("지금 보냄!");
            _webSocket.Send("1231543534563453");
        }
    }
}
