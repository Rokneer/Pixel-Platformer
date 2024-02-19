using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public void SetFullScreen(bool isFullscreen){
        Screen.fullScreen = isFullscreen;
    }
}
