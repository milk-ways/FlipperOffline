using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioContainer", menuName = "ScriptableObject/AudioContainer", order = 111)]
public class AudioContainer : ScriptableObject
{
    public List<AudioClip> audioClips;
    public List<string> audioClipKey;
}