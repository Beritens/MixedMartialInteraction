using System.Net.Sockets;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LocalNetworking
{
    [RequireComponent(typeof(Server))]
    public class NetworkManager : MonoBehaviour
    {
        #region nested classes

        [System.Serializable]
        public class Message
        {
            public string _opCode, _msg;

            public Message(string opCode, string msg)
            {
                _opCode = opCode;
                _msg = msg;
            }
        }

        #endregion
        #region member variables

        protected TCPServer _tcpServer;
        protected TCPClient _tcpClient;
        protected bool _started = false;
        protected bool isHost;

        #endregion

        /// <summary>
        /// Starts the server and binds the event listeners
        /// </summary>
        public void Init()
        {
            _tcpServer = GetComponent<TCPServer>();
            _tcpClient = GetComponent<TCPClient>();
            _tcpClient.OnData += OnData;
            _tcpServer.OnData += OnData;
            // _server.OnConnection += OnConnect;
            // _server.OnData += OnData;
            // _server.OnClientDisconnect += OnClientDisconnect;
            // _server.OnServerShutdown += OnServerShutdown;
        }

        /// <summary>
        /// Unbinds the event listeners
        /// </summary>
        public void Shutdown()
        {
            // _server.OnConnection -= OnConnect;
            // _server.OnData -= OnData;
            // _server.OnClientDisconnect -= OnClientDisconnect;
            // _server.OnServerShutdown -= OnServerShutdown;
        }

        /// <summary>
        /// Starts the Networking process as a Host
        /// </summary>
        public void Host()
        {
            if (!_started)
            {
                isHost = true;
                _started = true;
                _tcpServer.Host();
                //_server.Host();
            }
        }

        /// <summary>
        /// Starts the Networking process as a Client
        /// </summary>
        public void Join(string ip)
        {
            if (!_started)
            {
                _started = true;
                _tcpClient.Join(ip);
            }
        }

        #region events

        /// <summary>
        /// Called when a client connects to the server
        /// </summary>
        /// <param name="connection"></param>
        public virtual void OnConnect(Socket connection)
        {
        }

        /// <summary>
        /// Called whenever a packet of data is received from the server
        /// </summary>
        /// <param name="message"></param>
        public virtual void OnData(Message message)
        {
        }

        /// <summary>
        /// called whenever a client is disconnected
        /// </summary>
        public virtual void OnClientDisconnect(Socket connection)
        {
        }

        /// <summary>
        /// Called when the server closes the connection with the clients
        /// </summary>
        public virtual void OnServerShutdown()
        {
            Shutdown();
        }

        #endregion
    }
}