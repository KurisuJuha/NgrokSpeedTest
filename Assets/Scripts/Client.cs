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
        private bool transmittable;
        private Vector2 input;

        WebSocket webSocket;


        private void Start()
        {
            webSocket = new WebSocket(url);

            webSocket.OnOpen += (sender, e) =>
            {
                Debug.Log("open");
            };
            webSocket.OnMessage += (sender, e) =>
            {
                transmittable = true;
                ReadData(e.RawData);
            };
            webSocket.OnError += (sender, e) =>
                Debug.Log($"error: {e.Message}");
            webSocket.OnClose += (sender, e) =>
                Debug.Log("close");

            webSocket.Connect();
            SendData();
        }

        private void Update()
        {
            if (transmittable) SendData();
            playerTransform.position += (Vector3)input.normalized * Time.deltaTime * speed;
        }

        private void OnDestroy()
        {
            webSocket.CloseAsync();
        }

        private void ReadData(byte[] bytes)
        {
            DataReader reader = new DataReader(bytes);
            input = reader.ReadVector2();
        }

        private void SendData()
        {
            transmittable = false;
            DataWriter writer = new DataWriter();
            writer.Append(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            webSocket.Send(writer.bytes.ToArray());
        }
    }
}