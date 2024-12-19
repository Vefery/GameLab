﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGame.GameLogic;
using NAudio.Wave;

namespace MazeGame.Utils
{
    public static class AudioEvents
    {
        private static List<string> audioFilesPath;
        private static int curAudioFile = 0;

        public static void LoadAudio(string filePath)
        {
            audioFilesPath = Directory.GetFiles(filePath + "/Audio/Events", "*.mp3").ToList();
        }

        public static void Play()
        {
            if(curAudioFile >= audioFilesPath.Count())
            {
                curAudioFile = 0;
            }
            var audioFileReader = new AudioFileReader(audioFilesPath[curAudioFile]);
            var outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFileReader);
            outputDevice.Play();
            curAudioFile++;
        }
    }
}
