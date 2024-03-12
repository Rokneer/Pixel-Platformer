using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public bool hasWon = false;
    public GameObject gameOverUI;
    public GameObject playerUI;

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

    private void Update()
    {
        if (!PlayerController.Instance.IsAlive && !gameOverUI.activeSelf)
        {
            gameOverUI.SetActive(true);
            playerUI.SetActive(false);
            PauseManager.Instance.ManageMouseVisibility(true);
        }
    }
}
