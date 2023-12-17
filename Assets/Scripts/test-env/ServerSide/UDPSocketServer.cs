using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

[Serializable]
public class Data
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public class UdpSocketServer
{
    private UdpClient server;
    private RpcHandler rpcHandler;
    private RetransmissionHandler retransmissionHandler;
    private SerializationHandler serializationHandler; // Add this line


    private INetworkData lastReceivedData;

    public UdpSocketServer()
    {
        rpcHandler = new RpcHandler();
        rpcHandler.RegisterMethods(GetType());
        serializationHandler = new SerializationHandler(); // Initialize the SerializationHandler
    }

    public void StartServer(int port)
    {
        try
        {
            server = new UdpClient(port);
            Console.WriteLine("Server Started");

            while (true)
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] receivedBytes = server.Receive(ref remoteEndPoint);

                // Handle each client in a separate thread
                Thread clientThread = new Thread(() =>
                {
                    // Deserialize the received data
                    INetworkData receivedData = serializationHandler.Deserialize(receivedBytes);

                    // Check if the received data is the same as the last received data
                    if (receivedData.Equals(lastReceivedData))
                    {
                        // If the data is the same, skip processing
                        return;
                    }

                    // Store the received data
                    lastReceivedData = receivedData;

                    // Check the type of the received data and invoke the corresponding RPC method
                    if (receivedData is NetworkTransform)
                    {
                        rpcHandler.InvokeMethod("UpdateTransform", this, receivedData as NetworkTransform);
                    }
                    else if (receivedData is NetworkMessage)
                    {
                        rpcHandler.InvokeMethod("SendMessage", this, receivedData as NetworkMessage);
                    }
                });
                clientThread.Start();
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"SocketException: {ex}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex}");
        }
        finally
        {
            server?.Close();
        }
    }

    [Rpc]
    public void SomeRpcMethod()
    {
        // Implementation of an RPC method
    }
}

public class RpcHandler
{
    private Dictionary<string, MethodInfo> rpcMethods;

    public RpcHandler()
    {
        rpcMethods = new Dictionary<string, MethodInfo>();
    }

    public void RegisterMethods(Type type)
    {
        // Register all methods marked with the RpcAttribute
        foreach (var method in type.GetMethods())
        {
            if (Attribute.IsDefined(method, typeof(RpcAttribute)))
            {
                rpcMethods.Add(method.Name, method);
            }
        }
    }

    public void InvokeMethod(string methodName, object obj)
    {
        // Check if the received text is the name of an RPC method
        if (rpcMethods.ContainsKey(methodName))
        {
            // Call the RPC method
            rpcMethods[methodName].Invoke(obj, null);
        }
    }
}

public class RetransmissionHandler
{
    private Dictionary<int, byte[]> packets;
    private UdpClient server;
    private IPEndPoint remoteEndPoint;

    public RetransmissionHandler(UdpClient server, IPEndPoint remoteEndPoint)
    {
        this.server = server;
        this.remoteEndPoint = remoteEndPoint;
        packets = new Dictionary<int, byte[]>();
    }

    public void StorePacket(int sequenceNumber, byte[] packetData)
    {
        if (!packets.ContainsKey(sequenceNumber))
        {
            packets.Add(sequenceNumber, packetData);
        }
    }

    public void RetransmitPacket(int sequenceNumber)
    {
        if (packets.ContainsKey(sequenceNumber))
        {
            server.Send(packets[sequenceNumber], packets[sequenceNumber].Length, remoteEndPoint);
        }
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

// Attribute to mark RPC methods
public class RpcAttribute : Attribute
{
}
