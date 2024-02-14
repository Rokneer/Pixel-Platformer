using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Stomp : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D stompCollider;
    private TouchingDirections touchingDirections;

    public float bounceForce = 8.0f;

    #region Lifecycle
    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        touchingDirections = GetComponentInParent<TouchingDirections>();
        stompCollider = GetComponent<BoxCollider2D>();
        stompCollider.enabled = false;
    }

    private void Update()
    {
        stompCollider.enabled = !touchingDirections.IsGrounded;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }
    #endregion
}
