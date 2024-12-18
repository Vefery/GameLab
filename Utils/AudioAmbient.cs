using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Utils
{
    public static class AudioAmbient
    {
        public static void PlayAmbient(string assetsPath)
        {
            Task.Run(() =>
            {
                string audioFilePath = assetsPath + "Audio/Ambient/ambient.wav";
                using (var audioFile = new AudioFileReader(audioFilePath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();

                    // Обработчик события завершения воспроизведения
                    outputDevice.PlaybackStopped += (sender, e) =>
                    {
                        audioFile.Position = 0;
                        outputDevice.Play();
                    };
                    Console.ReadKey(); // Магия говно-кода?)))
                }
            });


        }
    }
}
