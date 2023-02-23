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
        [SerializeField] private Vector2 input;

        WebSocket webSocket;


        private async void Start()
        {
            webSocket = new WebSocket(url);

            webSocket.OnOpen += () =>
            {
                SendData();
                Debug.Log("open");
            };
            webSocket.OnMessage += (e) =>
            {
                ReadData(e.ToArray());
                SendData();
            };
            webSocket.OnError += (e) =>
                Debug.Log($"error {string.Join("", e)}");
            webSocket.OnClose += (bytes) =>
                Debug.Log("close");

            await webSocket.Connect();
        }

        private void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            webSocket?.DispatchMessageQueue();
#endif
            playerTransform.position += (Vector3)input.normalized * speed * Time.deltaTime;
        }

        private void OnDestroy()
        {
            webSocket?.Close();
        }

        private void ReadData(byte[] bytes)
        {
            DataReader reader = new DataReader(bytes);
            input = reader.ReadVector2();
        }

        private void SendData()
        {
            DataWriter writer = new DataWriter();
            writer.Append(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            webSocket?.Send(writer.bytes.ToArray());
        }
    }
}