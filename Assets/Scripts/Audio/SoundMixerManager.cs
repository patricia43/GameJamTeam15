using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }

    //public void SetSoundFXVolume(float level)
    //{
    //    audioMixer.SetFloat("SFXVolume", Mathf.Log10(level) * 20f);
    //}

    //public void SetMusicVolume(float level)
    //{
    //    audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    //}

    public void SetMusicVolume(float level)
    {
        level = Mathf.Clamp(level, 0.0001f, 1f);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("MusicVol", level);
    }

    public void SetSoundFXVolume(float level)
    {
        level = Mathf.Clamp(level, 0.0001f, 1f);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("SFXVol", level);
    }
}
