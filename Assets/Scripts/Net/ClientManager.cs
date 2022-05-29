using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Common;
using UnityEngine;

public class ClientManager : BaseManager
{
    private const string IP = "127.0.0.1";
    private const int Port = 6688;
    private Socket _clientSocket;
    private Message msg = new Message();

    public ClientManager(GameFacade facade) : base(facade){ }
    public ClientManager(GameFacade facade, Socket clientSocket) : base(facade)
    {
        _clientSocket = clientSocket;
    }

    public override void OnInit()
    {
        _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            _clientSocket.Connect(IP, Port);
            Start();
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private void Start()
    {
        _clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack,null);
    }

    private void ReceiveCallBack(IAsyncResult ar)
    {
        int count = _clientSocket.EndReceive(ar);
        msg.ReadMessage(count, OnProcessDataCallback);
        Start();
    }

    private void OnProcessDataCallback(ActionCode actionCode, string data)
    {
        facade.HandleResponse(actionCode, data);
    }

    public override void OnDestroy()
    {
        try
        {
            _clientSocket.Close();
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        byte[] bytes = Message.PackData(requestCode, actionCode, data);
        _clientSocket.Send(bytes);
    }
}