using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private static PauseManager _instance;
    public static PauseManager Instance => _instance;

    private bool _isPaused = false;
    public bool IsPaused
    {
        get => _isPaused;
        set
        {
            _isPaused = value;
            TimeManager.Instance.isTimerActive = value;
        }
    }

    public bool canPause = true;

    private void Awake()
    {
        _isPaused = false;
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
        IsPaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1;
    }

    public void ManageMouseVisibility(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.Confined : CursorLockMode.Locked;
    }
}
