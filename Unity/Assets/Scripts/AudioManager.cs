using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] private AudioSource audioSource;

    public void AudioTransition(int audioIndex)
    {
        audioSource.PlayOneShot(audioClips[audioIndex - 1]);
    }

    public float AudioLength(int audioIndex)
    {
        return audioClips[audioIndex - 1].length;
    }
}
