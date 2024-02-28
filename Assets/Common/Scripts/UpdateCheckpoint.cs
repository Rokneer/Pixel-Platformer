using UnityEngine;

public class UpdateCheckpoint : MonoBehaviour
{
    public Transform spawnPoint;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerManager>().currentCheckpoint = spawnPoint;
        }
    }
}
