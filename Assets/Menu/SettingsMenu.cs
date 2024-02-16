using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume",volume);
    }
    public void SetFullScreen(bool isFullscreen){
        Screen.fullScreen = isFullscreen;
    }
}
