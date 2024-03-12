using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private static PauseManager _instance;
    public static PauseManager Instance => _instance;

    public static bool isPaused = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
    }

    public void ManageMouseVisibility(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.Confined : CursorLockMode.Locked;
    }
}
