using UnityEngine;

[RequireComponent(typeof(Animator), (typeof(CapsuleCollider2D)))]
public class TouchingDirections : MonoBehaviour
{
    private Animator _animator;
    private CapsuleCollider2D _touchingCollider;

    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float cellingDistance = 0.05f;
    public float wallDistance = 0.1f;
    private readonly RaycastHit2D[] _groundHits = new RaycastHit2D[5];
    private readonly RaycastHit2D[] _cellingHits = new RaycastHit2D[5];
    private readonly RaycastHit2D[] _wallHits = new RaycastHit2D[5];

    private Vector2 WallCheckDirection =>
        gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    [SerializeField]
    private bool _isGrounded = true;
    public bool IsGrounded
    {
        get => _isGrounded;
        set
        {
            _isGrounded = value;
            _animator.SetBool("isGrounded", value);
        }
    }

    [SerializeField]
    private bool _isOnCelling = false;
    public bool IsOnCelling
    {
        get => _isOnCelling;
        set
        {
            _isOnCelling = value;
            _animator.SetBool("isOnCelling", value);
        }
    }

    [SerializeField]
    private bool _isOnWall = false;
    public bool IsOnWall
    {
        get => _isOnWall;
        set
        {
            _isOnWall = value;
            _animator.SetBool("isOnWall", value);
        }
    }

    #region Lifecycle
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _touchingCollider = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        IsGrounded =
            _touchingCollider.Cast(Vector2.down, castFilter, _groundHits, groundDistance) > 0;

        IsOnCelling =
            _touchingCollider.Cast(Vector2.up, castFilter, _cellingHits, cellingDistance) > 0;

        IsOnWall =
            _touchingCollider.Cast(WallCheckDirection, castFilter, _wallHits, wallDistance) > 0;
    }
    #endregion
}
