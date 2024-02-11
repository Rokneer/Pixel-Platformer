using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(TrailRenderer))]
[RequireComponent(typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    TrailRenderer trail;
    TouchingDirections touchingDirections;
    Damageable damageable;

    public float walkSpeed = 5f;
    public float airSpeed = 4f;
    public float jumpImpulse = 7f;
    public float dashImpulse = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1.2f;

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get => _isMoving;
        set
        {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }
    public float CurrentMoveSpeed
    {
        get
        {
            if (IsMoving && CanMove && !touchingDirections.IsOnWall)
            {
                if (touchingDirections.IsGrounded)
                    return walkSpeed;
                return airSpeed;
            }
            return 0;
        }
    }
    public bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get => _isFacingRight;
        set
        {
            if (_isFacingRight != value)
                transform.localScale *= new Vector2(-1, 1);
            _isFacingRight = value;
        }
    }
    public bool _isDashing = false;
    public bool IsDashing
    {
        get => _isDashing;
        set
        {
            _isDashing = value;
            trail.emitting = value;
        }
    }
    public bool _canDash = true;
    public bool CanDash
    {
        get => _canDash;
        set => _canDash = IsAlive && value;
    }

    public bool CanMove => animator.GetBool("canMove");
    public bool IsAlive => animator.GetBool("isAlive");

    #region Lifecycle
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        trail = GetComponent<TrailRenderer>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    void FixedUpdate()
    {
        if (!IsDashing && !damageable.LockVelocity)
            rb.velocity = new(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
            IsFacingRight = true;
        else if (moveInput.x < 0 && IsFacingRight)
            IsFacingRight = false;
        else if (moveInput.x == 0)
            return;
    }
    #endregion

    #region Functions
    public void OnMove(InputAction.CallbackContext context)
    {
        if (IsAlive)
        {
            moveInput = context.ReadValue<Vector2>();
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
            IsMoving = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger("jump");
            rb.velocity = new(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && CanDash && IsAlive)
            StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        CanDash = false;
        IsDashing = true;
        rb.velocity = new(transform.localScale.x * dashImpulse, 0f);
        yield return new WaitForSeconds(dashTime);
        IsDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        CanDash = true;
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
    #endregion
}
