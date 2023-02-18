using UnityEngine;
using JuhaKurisu.PopoTools.ByteSerializer;
using System.Linq;
using NativeWebSocket;

namespace JuhaKurisu.NgrokSpeedTest
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private string url;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float speed;

        WebSocket webSocket;


        private async void Start()
        {
            webSocket = new WebSocket(url);

            webSocket.OnOpen += () =>
                Debug.Log("open");
            webSocket.OnMessage += (e) =>
                Debug.Log("message");
            webSocket.OnError += (e) =>
                Debug.Log("error");
            webSocket.OnClose += (bytes) =>
                Debug.Log("close");

            // Keep sending messages at every 0.3s
            InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

            // waiting for messages
            await webSocket.Connect();
        }

        private void OnDestroy()
        {
            webSocket.Close();
        }

        private void SendData()
        {
            DataWriter writer = new DataWriter();
            writer.Append(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            webSocket.Send(writer.bytes.ToArray());
        }
    }
}