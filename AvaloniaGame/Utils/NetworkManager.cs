using System;
using System.Net;
using Lidgren.Network;

public class NetworkManager
{
    private NetPeerConfiguration _config;
    private NetServer _server;
    private NetClient _client;
    private bool _isServer;
    private NetConnection _connectedClient;
    public NetworkManager(string appIdentifier, bool isServer)
    {
        _isServer = isServer;
        _config = new NetPeerConfiguration(appIdentifier);

        if (_isServer)
        {
            _config.Port = 12345; // Порт для сервера
            _server = new NetServer(_config);
            _server.Start();
            Console.WriteLine("Сервер запущен на порту " + _config.Port);
        }
        else
        {
            _client = new NetClient(_config);
            _client.Start();
        }
    }

    public void Connect(string host, int port)
    {
        if (!_isServer)
        {
            _client.Connect(host, port);
            Console.WriteLine("Подключение к серверу " + host + ":" + port);
        }
    }

    public void SendMessage(string message)
    {
        if (_isServer && _connectedClient != null)
        {
            // Отправка сообщения только подключенному клиенту
            var msg = _server.CreateMessage();
            msg.Write(message);
            _connectedClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, 0);
        }
        else if (!_isServer)
        {
            // Клиент отправляет сообщение на сервер
            var msg = _client.CreateMessage();
            msg.Write(message);
            _client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }
    }

    public void Update()
    {
        if (_isServer)
        {
            // Обработка входящих сообщений на сервере
            NetIncomingMessage msg;
            while ((msg = _server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                        if (msg.SenderConnection.Status == NetConnectionStatus.Connected)
                        {
                            Console.WriteLine("Клиент подключен: " + msg.SenderEndPoint);
                            if (_connectedClient == null)
                            {
                                // Если нет подключенного клиента, сохраняем его
                                _connectedClient = msg.SenderConnection;
                                Console.WriteLine("Клиент подключен: " + msg.SenderEndPoint);
                            }
                            else
                            {
                                // Отклоняем новое подключение
                                Console.WriteLine("Попытка подключения нового клиента отклонена: " + msg.SenderEndPoint);
                                msg.SenderConnection.Disconnect("Только одно подключение разрешено.");
                            }
                            break;
                        }
                        else if (msg.SenderConnection.Status == NetConnectionStatus.Disconnected)
                        {
                            _connectedClient = null;
                            Console.WriteLine("Клиент отключен: " + msg.SenderEndPoint);
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        string data = msg.ReadString();
                        Console.WriteLine("Получено сообщение от клиента: " + data);
                        break;
                }
                _server.Recycle(msg);
            }
        }
        else
        {
            // Обработка входящих сообщений на клиенте
            NetIncomingMessage incomingMsg;
            while ((incomingMsg = _client.ReadMessage()) != null)
            {
                switch (incomingMsg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        string data = incomingMsg.ReadString();
                        Console.WriteLine("Получено сообщение от сервера: " + data);
                        break;
                }
                _client.Recycle(incomingMsg);
            }
        }
    }

}