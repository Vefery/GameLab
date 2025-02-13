using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Mathematics;
using Silk.NET.OpenGL;

using AvaloniaGame.Utils;
using AvaloniaGame.Views;
using Avalonia.Controls;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AvaloniaGame.ViewModels;

namespace AvaloniaGame.GameLogic
{
    public static class MainLogic
    {
        public static GL gl {set; private get;}
        public static event Action? OnFinished;
        public static List<GameObject> gameObjects = [];
        public static string seedString;
        public static bool seedGetted;
        public static string timeString;
        public static bool timeGetted;
        public static string winnerString;
        public static bool winnerGetted;
        public static bool isMultiplayer;
        public static string finishText;
        public static List<IRenderable> renderables
        {
            get
            {
                return gameObjects.OfType<IRenderable>().ToList();
            }
        }
        // public static KeyboardState keyboardState;
        // public static MouseState mouseState;
        public static bool finishFlag = false;
        public static int difficulty = 0;
        public static MainWindow mainWindow;
        public static NetworkManager networkManager;
        public static void InitializeScene()
        {
            gameObjects.Add(new Maze(gl));
        }
        public static Player InitializePlayer()
        {
            Player _player = new Player(gl, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1, 5, 1), 3, 4, 0.0f);
            mainWindow.KeyDown += _player.KeyDownHandler;
            mainWindow.KeyUp += _player.KeyUpHandler;
            mainWindow.PointerMoved += _player.PointerMovedHandler;
            gameObjects.Add(_player);
            return _player;
        }

        public static void StartSingleplayer()
        {
            finishFlag = true;
            isMultiplayer = false;
            mainWindow.StartTimer();
        }
        public static void StartMultiplayerAsHost()
        {
            networkManager = new NetworkManager("game", true);
            isMultiplayer = true;
            WaitSecondPlayerConnect();
            finishFlag = true;
            if (!networkManager.isServer)
            {
                WaitAnswer();
            }
            mainWindow.StartTimer();
        }
        public static void StartMultiplayerAsClient(string ipAdress)
        {
            networkManager = new NetworkManager("game", false);
            networkManager.serverIp = ipAdress;
            isMultiplayer = true;
            WaitSecondPlayerConnect();
            finishFlag = true;
            if (!networkManager.isServer)
            {
                WaitAnswer();
            }
            mainWindow.StartTimer();
        }
        public static async void WaitSecondPlayerConnect()
        {
            if (networkManager.isServer)
            {
                do
                {
                    networkManager.Update();
                    Console.WriteLine("Ждём подключение второго игрока");
                    //Thread.Sleep(1000);
                    await Task.Delay(1000);
                } while (networkManager.connectedClient == null);

                (mainWindow.DataContext as MainViewModel).IsWaiting = false;
                finishFlag = true;
            }
            else
            {
                do
                {
                    networkManager.Connect(networkManager.serverIp, 12345);
                    networkManager.Update();
                    Console.WriteLine("Ждём ответа от сервера");
                    //Thread.Sleep(1000);
                    await Task.Delay(1000);
                } while (networkManager.clientConnectedToServer == false);
            }
        }

        public static Player ReloadLevel()
        {
            Console.WriteLine("ReloadLevel");

            gameObjects.Clear();
            var player = InitializePlayer();
            InitializeScene();
            
            mainWindow.UpdateTimer();
            return player;
        }

        public static async void WaitAnswer()
        {

            if (isMultiplayer)
            {
                if (networkManager.isServer)
                {
                    networkManager.SendMessage("Time: " + mainWindow._timeElapsed.ToString(@"mm\:ss\.ff"));
                    networkManager.SendMessage("Seed: " + seedString);

                    while (!winnerGetted)
                    {
                        MainLogic.networkManager.Update();
                        Console.WriteLine("Ждём пока не узнаем кто выиграл");
                        //Thread.Sleep(1000);
                        await Task.Delay(1000);
                    }
                    (mainWindow.DataContext as MainViewModel).IsWaiting = false;
                    winnerGetted = false;
                }
                else
                {
                    while (!timeGetted || !seedGetted)
                    {
                        MainLogic.networkManager.Update();
                        Console.WriteLine("Ждём пока сервер даст время и сид");
                        //Thread.Sleep(1000);
                        await Task.Delay(1000);
                    }
                    MainLogic.finishFlag = true;
                    (mainWindow.DataContext as MainViewModel).IsWaiting = false;
                    timeGetted = false;
                    seedGetted = false;
                }
            }

        }

        public static void CallUpdate(float deltaTime)
        {
            if (finishFlag)
            {
                OnFinished?.Invoke();
                finishFlag = false;
                return;
            }
            var tmpCopy = new List<GameObject>(gameObjects);
            foreach (var gameObject in tmpCopy)
                gameObject.Update(deltaTime);
        }

        public static void OnCloseCleanUp(Object? sender, WindowClosingEventArgs e)
        {
            // По хорошему надо удалять временные файлы, но они используются libvlc и залочены
            //Directory.Delete("Temp", true);
        }

        public static T Register<T>(T gameObject, Vector3 position, Vector3 rotation)
        where T: GameObject
        {
            gameObject.position = position;
            gameObject.eulerRotation = rotation;
            gameObjects.Add(gameObject);
            gameObject.Start(gl);
            return gameObject;
        }

        public static T Register<T>(T gameObject, Vector3 position)
        where T: GameObject
        {
            Register(gameObject, position, Vector3.Zero);
            return gameObject;
        }
    }
}
