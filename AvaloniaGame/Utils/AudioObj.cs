using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Avalonia.Platform;
using LibVLCSharp.Shared;

namespace AvaloniaGame.Utils
{
    public class AudioObject
    {
        private Uri[] audioFilesPath;
        private static int curAudioFile = 0;

        // NOTE: Здесь раньше была загрузка wav или mp3, что попадётся первым
        public virtual void LoadAudio(string audioFolder)
        {
            audioFilesPath = AssetLoader.GetAssets(new Uri("avares://AvaloniaGame/Assets/Audio/" + audioFolder), null)
                .Where(uri => {
                    var ext = Path.GetExtension(uri.AbsolutePath); 
                    if (ext == ".wav" || ext == ".mp3")
                    {
                        return true;
                    } else {
                        return false;
                    }
                })
                .ToArray();

            if (audioFilesPath.Length == 0)
            {
                throw new Exception("No audio files!");
            }
        }
        private void PlayAudioNoLooped()
        {
            if (curAudioFile >= audioFilesPath.Count())
            {
                curAudioFile = 0;
            }
            using (var libVLC = new LibVLC())
            using (var media = new Media(libVLC, audioFilesPath[curAudioFile].AbsolutePath, FromType.FromPath))
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
                using (var media = new Media(libVLC, audioFilesPath[0].AbsolutePath, FromType.FromPath))
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
