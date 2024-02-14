using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
[RequireComponent(typeof(TouchingDirections), typeof(Damageable))]
public class Demon : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private TouchingDirections touchingDirections;
    private Damageable damageable;

    public DetectionZone cliffZone;
    public float walkSpeed = 3f;
    public float walkStopRate = 0.05f;

    public enum WalkableDirection
    {
        Right,
        Left
    }

    private Vector2 walkDirectionVector = Vector2.right;
    private WalkableDirection _walkDirection;

    public WalkableDirection WalkDirection
    {
        get => _walkDirection;
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(
                    gameObject.transform.localScale.x * -1,
                    gameObject.transform.localScale.y
                );
                switch (value)
                {
                    case WalkableDirection.Right:
                        walkDirectionVector = Vector2.right;
                        break;
                    case WalkableDirection.Left:
                        walkDirectionVector = Vector2.left;
                        break;
                    default:
                        Debug.LogError($"ERROR: Invalid walkable direction of type {value}");
                        break;
                }
            }
            _walkDirection = value;
        }
    }
    public bool CanMove => animator.GetBool("canMove");

    #region Lifecycle
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }
        if (!damageable.LockVelocity)
        {
            rb.velocity = CanMove
                ? new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y)
                : new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }
    }
    #endregion
    #region Functions
    private void FlipDirection()
    {
        switch (WalkDirection)
        {
            case WalkableDirection.Right:
                WalkDirection = WalkableDirection.Left;
                break;
            case WalkableDirection.Left:
                WalkDirection = WalkableDirection.Right;
                break;
            default:
                Debug.LogError($"ERROR: Invalid walkable direction of type {WalkDirection}");
                break;
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        Debug.Log("Ouchiee");
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
    #endregion
}
