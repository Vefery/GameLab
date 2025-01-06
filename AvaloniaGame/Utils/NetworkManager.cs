using System;
using System.Net;
using AvaloniaGame.GameLogic;
using Lidgren.Network;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class NetworkManager
{
    private NetPeerConfiguration _config;
    private NetServer _server;
    private NetClient _client;
    public bool isServer;
    public bool clientConnectedToServer;
    public NetConnection connectedClient;

    public NetworkManager(string appIdentifier, bool isServer)
    {
        this.isServer = isServer;
        _config = new NetPeerConfiguration(appIdentifier);

        if (this.isServer)
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
        if (!isServer)
        {
            try
            {
                _client.Connect(host, port);
            }
            catch
            {
                Console.WriteLine("Не удалось подключиться к серверу");
            }
        }
    }

    public void SendMessage(string message)
    {
        Console.WriteLine("MSG: " + message);
        if (isServer && connectedClient != null)
        {
            // Отправка сообщения только подключенному клиенту
            var msg = _server.CreateMessage();
            msg.Write(message);
            connectedClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, 0);
        }
        else if (!isServer)
        {
            // Клиент отправляет сообщение на сервер
            var msg = _client.CreateMessage();
            msg.Write(message);
            _client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }
    }

    private void ProcessDataMsg(NetIncomingMessage incomingMsg)
    {
        string data = incomingMsg.ReadString();
        Console.WriteLine("Получено сообщение от сервера: " + data);

        // Парсинг сообщения
        string[] parts = data.Split(new[] { ": " }, 2, StringSplitOptions.None);
        if (parts.Length == 2)
        {
            string key = parts[0].Trim(); // Ключ (например, "Time", "Seed", "Win")
            string value = parts[1].Trim(); // Значение (строка после ": ")

            // Обработка в зависимости от ключа
            switch (key)
            {
                case "Connected":
                    Console.WriteLine("Клиент подключен: ");
                    MainLogic.networkManager.clientConnectedToServer = true;
                    MainLogic.timeGetted = true;
                    break;
                case "Time":
                    Console.WriteLine("Время: " + value);
                    MainLogic.timeString = value;
                    MainLogic.timeGetted = true;
                    break;
                case "Seed":
                    Console.WriteLine("Сид: " + value);
                    MainLogic.seedString = value;
                    MainLogic.seedGetted = true;
                    break;
                case "Winner":
                    Console.WriteLine("Победа: " + value);
                    if (isServer && value == "server")
                        Console.WriteLine("You won !!!");
                    else
                        Console.WriteLine("You lose :(");

                    break;
                default:
                    Console.WriteLine("Неизвестный ключ: " + key);
                    break;
            }
        }
        else
        {
            Console.WriteLine("Неверный формат сообщения: " + data);
        }
    }
    public void Update()
    {
        if (isServer)
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
                            if (connectedClient == null)
                            {
                                // Если нет подключенного клиента, сохраняем его
                                connectedClient = msg.SenderConnection;
                                Console.WriteLine("Клиент подключен: " + msg.SenderEndPoint);
                                SendMessage("Connected: ");
                                MainLogic.finishFlag = true;
                                MainLogic.CallUpdate(0);
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
                            connectedClient = null;
                            Console.WriteLine("Клиент отключен: " + msg.SenderEndPoint);
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        ProcessDataMsg(msg);
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
                        ProcessDataMsg(incomingMsg);
                        break;
                }
                _client.Recycle(incomingMsg);
            }
        }
    }

}