using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    private PlayerController _player;

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
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!_player.IsAlive && !gameOverUI.activeSelf)
        {
            gameOverUI.SetActive(true);
            playerUI.SetActive(false);
        } 
    }
}
