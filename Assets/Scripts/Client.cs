using UnityEngine;
using JuhaKurisu.PopoTools.ByteSerializer;
using System.Linq;
using SuperSimpleTcp;

namespace JuhaKurisu.NgrokSpeedTest
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private string url;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float speed;

        SimpleTcpClient client;


        private void Start()
        {
            client = new SimpleTcpClient(url);

            client.Events.Connected += (sender, e) =>
            {
                Debug.Log("connected");
            };

            client.Events.DataReceived += (sender, e) =>
            {
                Debug.Log("DataReceived");
            };

            client.Events.Disconnected += (sender, e) =>
            {
                Debug.Log("Disconnected");
            };

            client.Connect();

            client.Send("Hello!!!");
        }

        private void OnDestroy()
        {
            client?.Disconnect();
            client?.Dispose();
        }

        private void SendData()
        {
            DataWriter writer = new DataWriter();
            writer.Append(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            client.Send(writer.bytes.ToArray());
            //            webSocket.Send(writer.bytes.ToArray());
        }
    }
}