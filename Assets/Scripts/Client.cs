using UnityEngine;
using JuhaKurisu.PopoTools.ByteSerializer;
using JuhaKurisu.PopoTools.Extentions;
using System;
using System.Linq;
using System.Collections.Generic;
using NativeWebSocket;

namespace JuhaKurisu.NgrokSpeedTest
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private string url;
        [SerializeField] private int speed;
        [SerializeField] private Player playerPrefab;
        [SerializeField] private Dictionary<Guid, Player> players = new();

        private WebSocket webSocket;

        private async void Start()
        {
            DataWriter writer = new DataWriter();
            new TestInput().Serialize(writer);
            writer.bytes.Count.Inspect();
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
        }

        private void OnDestroy()
        {
            webSocket?.Close();
        }

        private void ReadData(byte[] bytes)
        {
            HashSet<Guid> currentPlayers = new();

            DataReader reader = new(bytes);
            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                System.Guid guid = new(reader.ReadBytes(16));
                TestInput testinput = new();
                testinput.Deserialize(reader);
                currentPlayers.Add(guid);
            }

            // いないプレイヤーを削除
            for (int i = currentPlayers.Count - 1; i > 0; i--)
            {
                Guid id = currentPlayers.ElementAt(i);
                GameObject.Destroy(players[id].gameObject);
                players.Remove(id);
                currentPlayers.Remove(id);
            }

            // 新しく追加されたプレイヤーを追加
            foreach (var id in currentPlayers)
            {

            }
        }

        private void SendData()
        {
            DataWriter writer = new();
            TestInput input = new()
            {
                right = Input.GetKey(KeyCode.D),
                left = Input.GetKey(KeyCode.A),
                up = Input.GetKey(KeyCode.W),
                down = Input.GetKey(KeyCode.S),
            };
            input.Serialize(writer);
            webSocket?.Send(writer.bytes.ToArray());
        }

        private void CreateNewPlayer()
        {

        }
    }
}