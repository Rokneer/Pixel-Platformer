using UnityEngine;

[RequireComponent(typeof(Animator), (typeof(CapsuleCollider2D)))]
public class TouchingDirections : MonoBehaviour
{
    private Animator animator;
    private CapsuleCollider2D touchingCollider;

    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float cellingDistance = 0.05f;
    public float wallDistance = 0.1f;
    private readonly RaycastHit2D[] groundHits = new RaycastHit2D[5];
    private readonly RaycastHit2D[] cellingHits = new RaycastHit2D[5];
    private readonly RaycastHit2D[] wallHits = new RaycastHit2D[5];

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
            animator.SetBool("isGrounded", value);
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
            animator.SetBool("isOnCelling", value);
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
            animator.SetBool("isOnWall", value);
        }
    }

    #region Lifecycle
    private void Awake()
    {
        animator = GetComponent<Animator>();
        touchingCollider = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        IsGrounded =
            touchingCollider.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;

        IsOnCelling =
            touchingCollider.Cast(Vector2.up, castFilter, cellingHits, cellingDistance) > 0;

        IsOnWall =
            touchingCollider.Cast(WallCheckDirection, castFilter, wallHits, wallDistance) > 0;
    }
    #endregion
}
