using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.GameLogic;
using NAudio.Wave;

namespace MazeGame.Utils
{
    public static class AudioDoor
    {
        private static string audioFilePath;

        public static void LoadAudio(string assetsPath)
        {
            audioFilePath = assetsPath + "/Audio/Door/findExit.wav";
        }

        public static void Play()
        {
            var audioFileReader = new AudioFileReader(audioFilePath);
            var outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFileReader);
            outputDevice.Play();
        }
    }
}

