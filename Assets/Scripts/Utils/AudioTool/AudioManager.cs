using System.Collections.Generic;
using UnityEngine;

namespace Utils.AudioTool
{
    public static class AudioManager
    {
        private static readonly AudioListener _listener;
        private static readonly Dictionary<AudioTrackType, AudioTrack> _tracksDictionary;
        private static readonly List<AudioPlayer> _players;

        private static string _audioLibraryPath;
        
        static AudioManager()
        {
            _listener = new GameObject("AudioListener").AddComponent<AudioListener>();
            Object.DontDestroyOnLoad(_listener.gameObject);
            
            _players = new List<AudioPlayer>();
            _tracksDictionary = new Dictionary<AudioTrackType, AudioTrack>();

            var audioLibrary = Resources.Load<AudioLibrary>("Configs/AudioLibrary");

            if (audioLibrary == null)
                throw new UnityException($"[AudioManager] No audio library found at Resources/Configs/AudioLibrary");

            for (int i = 0; i < audioLibrary.tracks.Count; i++)
            {
                var track = audioLibrary.tracks[i];
                _tracksDictionary.Add(track.trackType, track);
            }
        }
        
        
        public static AudioTrack FindTrack(AudioTrackType trackType)
        {
            return _tracksDictionary.ContainsKey(trackType) ? _tracksDictionary[trackType] : null;
        }

        public static async void PlayOneShot(AudioTrack track, AudioCategoryType categoryType)
        {
            var player = GetFreePlayer();
            await player.SetTrack(track, categoryType)
                .PlayAsync();
            player.Dispose();
        }
        
        public static AudioPlayer GetFreePlayer()
        {
            var player = FindDisposedPlayer();
            player ??= CreatePlayer();
            player.isDisposed = false;
            return player;
        }
        
        private static AudioPlayer CreatePlayer()
        {
            var go = new GameObject($"AudioSource {_players.Count}");
            go.AddComponent<AudioSource>();
            go.transform.SetParent(_listener.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            var player = go.AddComponent<AudioPlayer>();
            _players.Add(player);
            return player;
        }

        private static AudioPlayer FindDisposedPlayer()
        {
            return _players.Find(n => n.isDisposed);
        }
    }
}