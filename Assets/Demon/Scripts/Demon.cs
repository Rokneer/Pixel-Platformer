using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
[RequireComponent(typeof(TouchingDirections), typeof(Damageable))]
public class Demon : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private TouchingDirections _touchingDirections;
    private Damageable _damageable;

    public DetectionZone cliffZone;
    public float walkSpeed = 3f;
    public float walkStopRate = 0.05f;

    public enum WalkableDirection
    {
        Right,
        Left
    }

    private Vector2 _walkDirectionVector = Vector2.right;
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
                        _walkDirectionVector = Vector2.right;
                        break;
                    case WalkableDirection.Left:
                        _walkDirectionVector = Vector2.left;
                        break;
                    default:
                        Debug.LogError($"ERROR: Invalid walkable direction of type {value}");
                        break;
                }
            }
            _walkDirection = value;
        }
    }
    public bool CanMove => _animator.GetBool("canMove");

    #region Lifecycle
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _touchingDirections = GetComponent<TouchingDirections>();
        _damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {
        if (_touchingDirections.IsGrounded && _touchingDirections.IsOnWall)
        {
            FlipDirection();
        }
        if (!_damageable.LockVelocity)
        {
            _rb.velocity = CanMove
                ? new Vector2(walkSpeed * _walkDirectionVector.x, _rb.velocity.y)
                : new Vector2(Mathf.Lerp(_rb.velocity.x, 0, walkStopRate), _rb.velocity.y);
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

    public void OnCliffDetected()
    {
        if (_touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
    #endregion
}
