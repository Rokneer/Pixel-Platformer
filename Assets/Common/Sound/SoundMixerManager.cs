using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField]
    private Options options;

    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Slider masterVolumeSlider;

    [SerializeField]
    private Slider sfxVolumeSlider;

    [SerializeField]
    private Slider musicVolumeSlider;

    private void Awake()
    {
        SetMasterVolume(options.masterVolumeLevel);
        masterVolumeSlider.value = options.masterVolumeLevel;
        SetSoundFXVolume(options.sfxVolumeLevel);
        sfxVolumeSlider.value = options.sfxVolumeLevel;
        SetMusicVolume(options.musicVolumeLevel);
        musicVolumeSlider.value = options.musicVolumeLevel;
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20f);
        options.masterVolumeLevel = volume;
    }

    public void SetSoundFXVolume(float volume)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(volume) * 20f);
        options.sfxVolumeLevel = volume;
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20f);
        options.musicVolumeLevel = volume;
    }
}
