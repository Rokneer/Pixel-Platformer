using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private Options options;

    [SerializeField]
    private Toggle fullscreenToggle;

    private bool isFullScreen = true;

    private void Awake()
    {
        isFullScreen = options.isFullscreen;
        fullscreenToggle.isOn = options.isFullscreen;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void SetFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
        isFullScreen = fullScreen;
        options.isFullscreen = fullScreen;
        Cursor.lockState = fullScreen ? CursorLockMode.Confined : CursorLockMode.None;
    }
}
