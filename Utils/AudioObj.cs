﻿using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenTK.Graphics.OpenGL.GL;

namespace MazeGame.Utils
{
    public class AudioObject
    {
        private List<string> audioFilesPath;
        private static int curAudioFile = 0;
        public virtual void LoadAudio(string assetsPath, string audiFolder)
        {
            audioFilesPath = Directory.GetFiles(assetsPath + "/Audio/" + audiFolder, "*.wav").ToList();
            if (audioFilesPath.Count == 0)
                audioFilesPath = Directory.GetFiles(assetsPath + "/Audio/" + audiFolder, "*.mp3").ToList();
            if (audioFilesPath.Count == 0)
                throw new Exception("No audio files!");
        }
        private void PlayAudioNoLooped()
        {
            if (curAudioFile >= audioFilesPath.Count())
            {
                curAudioFile = 0;
            }
            using (var libVLC = new LibVLC())
            using (var media = new Media(libVLC, audioFilesPath[curAudioFile], FromType.FromPath))
            {
                var mediaPlayer = new MediaPlayer(libVLC);
                mediaPlayer.Play(media);
            }
            curAudioFile++;
        }
        private void PlayAudioLooped()
        {
            Task.Run(() =>
            {
                using (var libVLC = new LibVLC("--input-repeat=1000000000"))
                using (var media = new Media(libVLC, audioFilesPath[0], FromType.FromPath))
                {
                    var mediaPlayer = new MediaPlayer(libVLC);
                    mediaPlayer.Media = media;
                    mediaPlayer.Play();
                }
            });
        }
        public virtual void PlayAudio(bool looped)
        {
            if (looped)
                PlayAudioLooped();
            else
                PlayAudioNoLooped();
        }
    }
}