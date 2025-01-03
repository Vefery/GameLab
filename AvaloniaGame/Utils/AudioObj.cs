using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Platform;
using LibVLCSharp.Shared;

namespace AvaloniaGame.Utils
{
    public class AudioObject
    {
        private Uri[] audioFilesPath;
        private string tmpAudio;
        private static int curAudioFile = 0;

        public AudioObject()
        {
            // GetTempPath(): C:/Users/*UserName*/AppData/Local/Temp/ или /tmp/
            tmpAudio = Path.Combine(Path.GetTempPath(), "GameLabAudio");
            if (!Directory.Exists(tmpAudio))
            {
                Directory.CreateDirectory(tmpAudio);
            }
        }

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

            foreach (var path in audioFilesPath)
            {
                if (Path.Exists(Path.Combine(tmpAudio, Path.GetFileName(path.AbsolutePath))))
                    continue;
                var InputStream = AssetLoader.Open(path);
                using (var fileStream = File.Create(Path.Combine(tmpAudio, Path.GetFileName(path.AbsolutePath))))
                {
                    InputStream.Seek(0, SeekOrigin.Begin);
                    InputStream.CopyTo(fileStream);
                }
            }
        }
        private void PlayAudioNoLooped()
        {
            if (curAudioFile >= audioFilesPath.Count())
            {
                curAudioFile = 0;
            }
            using (var libVLC = new LibVLC())
            using (var media = new Media(libVLC, Path.Combine(tmpAudio, Path.GetFileName(audioFilesPath[curAudioFile].AbsolutePath)), FromType.FromPath))
            {
                var mediaPlayer = new MediaPlayer(libVLC);
                mediaPlayer.Play(media);
            }
            curAudioFile++;
        }
        private void PlayAudioLooped()
        {
            Task.Run( () => {

                using (var libVLC = new LibVLC("--input-repeat=1000000000"))
                using (var media = new Media(libVLC, Path.Combine(tmpAudio, Path.GetFileName(audioFilesPath[0].AbsolutePath)), FromType.FromPath))
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
