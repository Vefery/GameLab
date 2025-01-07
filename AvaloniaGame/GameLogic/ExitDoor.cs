using System.Linq;

using OpenTK.Mathematics;
using Silk.NET.OpenGL;

using AvaloniaGame.Utils;
using AvaloniaGame.Views;
using System;
using AvaloniaGame.ViewModels;

namespace AvaloniaGame.GameLogic
{
    public class ExitDoor : GameObject, IRenderable
    {
        public Mesh mesh { get; private set; }
        private GameObject player;
        private AudioObject AudioDoor = new AudioObject();
        public ExitDoor(GL gl) : base(gl)
        {
            mesh = new Mesh(gl, "avares://AvaloniaGame/Assets/" + "Models/ExitDoor.model", "avares://AvaloniaGame/Assets/" + "Textures/Exit.png");
            mesh.texId = 4;
            player = MainLogic.gameObjects.OfType<Player>().Single();
            AudioDoor.LoadAudio("Door");
        }

        static long ConvertToMilliseconds(string time)
        {
            // Разделение строки на части
            string[] parts = time.Split(new[] { ':', '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 3)
            {
                throw new FormatException("Неверный формат времени. Ожидается mm:ss.ff");
            }

            // Парсинг минут, секунд и сотых
            int minutes = int.Parse(parts[0]);
            int seconds = int.Parse(parts[1]);
            int fractions = int.Parse(parts[2]);

            // Преобразование в миллисекунды
            long totalMilliseconds = (minutes * 60 * 1000) + (seconds * 1000) + (fractions * 10);
            return totalMilliseconds;
        }

        static int CompareTimes(string time1, string time2)
        {
            long totalMilliseconds1 = ConvertToMilliseconds(time1);
            long totalMilliseconds2 = ConvertToMilliseconds(time2);

            return totalMilliseconds1.CompareTo(totalMilliseconds2);
        }

        public override void Update(float deltaTime)
        {
            if (Vector3.Distance(position, player.position) < 2)
            {
                AudioDoor.PlayAudio(looped: false);
                if(MainLogic.isMultiplayer)
                {
                    (MainLogic.mainWindow.DataContext as MainViewModel).IsWaiting = true;
                    if (!MainLogic.networkManager.isServer)
                    {
                        string curTime = MainLogic.mainWindow._timeElapsed.ToString(@"mm\:ss\.ff");
                        int comparisonResult = CompareTimes(curTime, MainLogic.timeString);

                        if (comparisonResult < 0)
                        {
                            (MainLogic.mainWindow.DataContext as MainViewModel).FinishText = "You won !!!";
                            (MainLogic.mainWindow.DataContext as MainViewModel).IsFinishScreenVisible = true;
                            Console.WriteLine($"Время клиента ({curTime}) меньше чем время сервера ({MainLogic.timeString})");
                            MainLogic.networkManager.SendMessage("Winner: client");
                        }
                        else if (comparisonResult > 0)
                        {
                            (MainLogic.mainWindow.DataContext as MainViewModel).FinishText = "You lost :(";
                            (MainLogic.mainWindow.DataContext as MainViewModel).IsFinishScreenVisible = true;
                            Console.WriteLine($"Время клиента ({curTime}) больше чем время сервера ({MainLogic.timeString})");
                            MainLogic.networkManager.SendMessage("Winner: server");
                        }
                        else
                        {
                            Console.WriteLine($"{curTime} равно {MainLogic.timeString}");
                            MainLogic.networkManager.SendMessage("Winner: nobody");

                        }
                    }
                    MainLogic.finishFlag = true;
                    MainLogic.WaitAnswer();
                }
                else
                {
                    MainLogic.finishFlag = true;
                }
            }
        }

        public override void Start(GL _)
        {
            ;
        }
    }
}
