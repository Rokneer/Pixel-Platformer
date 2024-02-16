using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField]
    private PlayerController player1;

    [SerializeField]
    private PlayerController player2;

    public GameObject GameOverUI;

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
        if (!!player2)
        {
            if (!player1.IsAlive && !player2.IsAlive)
            {
                Debug.Log("Dead together");
                GameOverUI.SetActive(true);
            }
        }
        else
        {
            if (!player1.IsAlive)
            {
                Debug.Log("Dead alone");
                GameOverUI.SetActive(true);
            }
        }
    }
}
