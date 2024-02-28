using UnityEngine;

public class ReturnToCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Rigidbody2D>().transform.position = collision
                .GetComponent<PlayerManager>()
                .currentCheckpoint
                .position;
        }
    }
}
