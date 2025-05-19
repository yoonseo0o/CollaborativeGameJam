using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    public void controlSFXVol(float volume)
    { 
        if (audioMixerGroup == null)
        {
            Debug.LogWarning("AudioMixerGroup¾øÀ½.");
            return;
        }
        if (volume < 0.0001)
        {
            volume = 0.0001f;
        }
        audioMixerGroup.audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }
}
