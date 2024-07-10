using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using LocalNetworking;

public class MyNetworkManager : NetworkManager
{
    public event EventHandler<OnMessageReceivedEventArgs> OnMessageReceived;

    public class OnMessageReceivedEventArgs : EventArgs
    {
        public string opCode;
        public string message;
    }
    void Awake()
    {
        Init();
    }

    void OnDestroy()
    {
        Shutdown();
    }

    public void Send(string opCode, string message)
    {
        if (_started)
        {
            print("sending");
            if (isHost)
            {
               _tcpServer.Send(opCode, message); 
            }
            else
            {
                _tcpClient.Send(opCode, message);
            }
        }
    }

    public override void OnConnect(Socket connection)
    {
        base.OnConnect(connection);

        IPEndPoint endPoint = (IPEndPoint)connection.RemoteEndPoint;
        
        Debug.LogError("Client: "+ endPoint.Address.ToString() +" Connected to the Server!");
    }

    public override void OnData(Message message)
    {
        base.OnData(message);

        OnMessageReceived?.Invoke(this, new OnMessageReceivedEventArgs {opCode = message._opCode, message = message._msg});
    }

    public override void OnClientDisconnect(Socket connection)
    {
        base.OnClientDisconnect(connection);

        IPEndPoint ipEndpoint = (IPEndPoint)connection.RemoteEndPoint;

        Debug.LogError("Client disconnected from: " + ipEndpoint.Address.ToString());
    }

    public override void OnServerShutdown()
    {
        base.OnServerShutdown();

        Debug.LogError("Server shut down");
    }
}
