using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using UnityEngine;

[Serializable]
public class Data
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public class UdpClientHandler : MonoBehaviour
{
    private UdpClient client;
    private IPEndPoint remoteEndPoint;
    private SerializationHandler serializationHandler;
    private Thread receiveThread;

    private INetworkData lastReceivedData;

    void Start()
    {
        // Initialize the UDP client
        client = new UdpClient();

        // Set the server IP and port
        remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);

        // Initialize the SerializationHandler
        serializationHandler = new SerializationHandler();

        // Start receiving data in a separate thread
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void Update()
    {
        // Send data to the server
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Data data = new Data { Name = "SomeRpcMethod", Age = 25 };
            SendData(data);
        }
    }

    void SendData(Data data)
    {
        try
        {
            byte[] bytes = serializationHandler.Serialize(data);
            client.Send(bytes, bytes.Length, remoteEndPoint);
        }
        catch (SocketException ex)
        {
            Debug.Log($"SocketException: {ex}");
        }
    }

    void ReceiveData()
    {
        while (true)
        {
            try
            {
                // Receive data from the server
                byte[] receivedBytes = client.Receive(ref remoteEndPoint);

                // Deserialize the received data
                INetworkData receivedData = serializationHandler.Deserialize(receivedBytes);

                // Check if the received data is the same as the last received data
                if (receivedData.Equals(lastReceivedData))
                {
                    // If the data is the same, skip processing
                    continue;
                }

                // Store the received data
                lastReceivedData = receivedData;

                // Process the received data
                switch (receivedData.DataType)
                {
                    case "NetworkTransform":
                        NetworkTransform transformData = receivedData as NetworkTransform;
                        Debug.Log($"Received position: {transformData.Position}, rotation: {transformData.Rotation}");
                        break;
                    case "NetworkMessage":
                        NetworkMessage messageData = receivedData as NetworkMessage;
                        Debug.Log($"Received message: {messageData.Message}");
                        break;
                }
            }
            catch (SocketException ex)
            {
                Debug.Log($"SocketException: {ex}");
            }
        }
    
    }

    void OnDestroy()
    {
        // Close the client when the script is destroyed
        client?.Close();
        receiveThread?.Abort();
    }
}

public class SerializationHandler
{
    public byte[] Serialize(Data data)
    {
        using (var memoryStream = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, data);
            return memoryStream.ToArray();
        }
    }

    public Data Deserialize(byte[] data)
    {
        using (var memoryStream = new MemoryStream(data))
        {
            var formatter = new BinaryFormatter();
            return (Data)formatter.Deserialize(memoryStream);
        }
    }
}
