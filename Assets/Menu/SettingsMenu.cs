using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    private void Awake()
    {
        Screen.fullScreen = true;
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
