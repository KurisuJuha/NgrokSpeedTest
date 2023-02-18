using UnityEngine;
using JuhaKurisu.PopoTools.ByteSerializer;
using System.Linq;
using WebSocketSharp;

namespace JuhaKurisu.NgrokSpeedTest
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private string url;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float speed;

        WebSocket webSocket;


        private void Start()
        {
            webSocket = new WebSocket(url);
            webSocket.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

            webSocket.OnOpen += (sender, e) =>
                Debug.Log("open");
            webSocket.OnMessage += (sender, e) =>
                Debug.Log("message");
            webSocket.OnError += (sender, e) =>
                Debug.Log("error");
            webSocket.OnClose += (sender, e) =>
                Debug.Log("close");

            webSocket.ConnectAsync();
            if (Input.GetKeyDown(KeyCode.A)) SendData();
        }

        private void OnDestroy()
        {
            webSocket.CloseAsync();
        }

        private void SendData()
        {
            DataWriter writer = new DataWriter();
            writer.Append(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            webSocket.Send(writer.bytes.ToArray());
        }
    }
}