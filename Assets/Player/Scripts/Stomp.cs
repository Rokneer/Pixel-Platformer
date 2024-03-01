using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Stomp : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D _stompCollider;
    private TouchingDirections _touchingDirections;

    public float bounceForce = 8.0f;

    #region Lifecycle
    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _touchingDirections = GetComponentInParent<TouchingDirections>();
        _stompCollider = GetComponent<BoxCollider2D>();
        _stompCollider.enabled = false;
    }

    private void Update()
    {
        _stompCollider.enabled = !_touchingDirections.IsGrounded;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            _rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }
    #endregion
}
