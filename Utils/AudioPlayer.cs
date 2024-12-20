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
    public static class AudioPlayer
    {
        private static List<string> audioFilesPath;
        private static int curAudioFile = 0;

        public static void LoadAudio(string filePath)
        {
            audioFilesPath = Directory.GetFiles(filePath + "/Audio/Steps", "*.wav").ToList();
        }

        public static void Play()
        {
            if(curAudioFile >= audioFilesPath.Count())
            {
                curAudioFile = 0;
            }
            using var audioFileReader = new AudioFileReader(audioFilesPath[curAudioFile]);
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFileReader);
                outputDevice.Play();
                curAudioFile++;
            }
        }
    }
}

