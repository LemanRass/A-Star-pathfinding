using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.AudioTool
{
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource _source;
        private AudioTrack _track;
        public AudioCategoryType categoryType { get; private set; }

        public bool isPlaying { get; private set; }
        public bool isDisposed { get; set; }

        public float playingSeconds { get; private set; }

        public event Action onFinishedEvent;

        private TaskCompletionSource<bool> _onFinishPromise;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (isPlaying)
            {
                playingSeconds += Time.deltaTime;
                if (playingSeconds >= _track.clip.length)
                {
                    OnPlayFinished();
                }
            }
        }

        public AudioPlayer SetParent(Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            return this;
        }
        
        public AudioPlayer SetTrack(AudioTrack track, AudioCategoryType category)
        {
            if (isPlaying)
            {
                Stop();
            }
            
            _track = track;
            _source.clip = _track.clip;
            categoryType = category;
            return this;
        }

        public void Play(bool loop = false, float delay = 0.0f)
        {
            _source.loop = loop;
            isPlaying = true;
            _source.PlayDelayed(delay);
        }

        public async Task PlayAsync()
        {
            _onFinishPromise = new TaskCompletionSource<bool>();
            Play(false);
            await _onFinishPromise.Task;
        }

        public void Pause()
        {
            isPlaying = false;
            _source.Pause();
        }

        public void Resume()
        {
            isPlaying = true;
            _source.UnPause();
        }

        public void Stop()
        {
            isPlaying = false;
            playingSeconds = 0.0f;
            _source.Stop();
        }

        public void Dispose()
        {
            Stop();
            isPlaying = false;
            isDisposed = true;
        }

        private void OnPlayFinished()
        {
            if (!_source.loop)
            {
                Stop();
                onFinishedEvent?.Invoke();
                _onFinishPromise?.SetResult(true);
            }
        }

        public override string ToString()
        {
            return _track.clip.name;
        }
    }
}