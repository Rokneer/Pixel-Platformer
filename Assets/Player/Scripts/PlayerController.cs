using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(TrailRenderer))]
[RequireComponent(typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private TrailRenderer trail;
    private TouchingDirections touchingDirections;
    private Damageable damageable;

    public float walkSpeed = 5f;
    public float airSpeed = 4f;
    public float jumpImpulse = 7f;
    public float dashImpulse = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1.2f;
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;

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
    public float _coyoteTimeCounter;
    public float CoyoteTimeCounter
    {
        get => _coyoteTimeCounter;
        set => _coyoteTimeCounter = touchingDirections.IsGrounded ?
            coyoteTime : Mathf.Clamp(value, 0f, coyoteTime);
    }
    public float _jumpBufferCounter;
    public float JumpBufferCounter
    {
        get => _jumpBufferCounter;
        set => _jumpBufferCounter =  Mathf.Clamp(value, 0f, jumpBufferTime);
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
        CoyoteTimeCounter -= Time.time;
        if(touchingDirections.IsGrounded)
            JumpBufferCounter -= Time.time;
        if (!IsDashing && !damageable.LockVelocity)
            rb.velocity = new(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        animator.SetFloat("yVelocity", rb.velocity.y);
    }
    #endregion

    #region Functions
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
            IsFacingRight = true;
        else if (moveInput.x < 0 && IsFacingRight)
            IsFacingRight = false;
        else if (moveInput.x == 0)
            return;
    }

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
        if(CanMove)
        {
            
            if (context.started)
            {
                JumpBufferCounter = jumpBufferTime;
            }
            if (CoyoteTimeCounter > 0f && JumpBufferCounter > 0f)
            {
                animator.SetTrigger("jump");
                rb.velocity = new(rb.velocity.x, jumpImpulse);
                JumpBufferCounter = 0f;
            }
            if (context.canceled && rb.velocity.y > 0) {
                rb.velocity = new(rb.velocity.x, rb.velocity.y * 0.6f);
                CoyoteTimeCounter = 0f;
            } 
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
