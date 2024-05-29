using Behaviour;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
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
        _socket = new WebSocket("ws://0.tcp.jp.ngrok.io:10570");
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
    
    private void _Start()
    {
        StartCoroutine(GetRequest("http://localhost:8000/"));
    }

    IEnumerator GetRequest(string uri)
    {
        WWWForm form = new WWWForm();
        form.AddField("1234", "55555555");

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            Debug.Log(webRequest.downloadHandler.text);
            //switch (webRequest.result)
            //{
            //    case UnityWebRequest.Result.ConnectionError:
            //    case UnityWebRequest.Result.DataProcessingError:
            //        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
            //        break;
            //    case UnityWebRequest.Result.ProtocolError:
            //        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
            //        break;
            //    case UnityWebRequest.Result.Success:
            //        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            //        jsonText = webRequest.downloadHandler.text;
            //        NewsListData[] newsListDatas = JsonConvert.DeserializeObject<NewsListData[]>(jsonText);
            //        StartCoroutine(CoLoadImageTexture(newsListDatas[0].img_url));
            //        title.text = newsListDatas[0].title;
            //        contents.text = newsListDatas[0].contents;
            //        break;
            //}
        }
    }
}
