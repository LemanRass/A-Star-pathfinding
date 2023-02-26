using System.Collections.Generic;
using UnityEngine;

namespace Utils.AudioTool
{
    [CreateAssetMenu(fileName = "AudioLibrary", menuName = "AudioLibrary")]
    public class AudioLibrary : ScriptableObject
    {
        public List<AudioTrack> tracks;
    }
}