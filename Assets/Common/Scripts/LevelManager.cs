using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance => _instance;

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

    public void LoadLevel(int ID)
    {
        SceneManager.LoadSceneAsync(ID);
        PauseManager.Instance.ResumeGame();
    }

    public void ResetCurrentLevel()
    {
        PauseManager.Instance.ResumeGame();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
