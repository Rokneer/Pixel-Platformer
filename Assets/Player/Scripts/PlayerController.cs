using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(TrailRenderer))]
[RequireComponent(typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private Animator _animator;
    private TrailRenderer _trail;
    private TouchingDirections _touchingDirections;
    private Damageable _damageable;

    public float walkSpeed = 5f;
    public float airSpeed = 4f;
    public float jumpImpulse = 7f;
    public float dashImpulse = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1.2f;
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;

    [SerializeField]
    private AudioClip jumpSoundFX;

    [SerializeField]
    private AudioClip dashSoundFX;

    [SerializeField]
    private AudioClip dashReadySoundFX;

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get => _isMoving;
        set
        {
            _isMoving = value;
            _animator.SetBool("isMoving", value);
        }
    }
    public bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get => _isFacingRight;
        set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
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
            _trail.emitting = value;
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
        set =>
            _coyoteTimeCounter = _touchingDirections.IsGrounded
                ? coyoteTime
                : Mathf.Clamp(value, 0f, coyoteTime);
    }
    public float _jumpBufferCounter;
    public float JumpBufferCounter
    {
        get => _jumpBufferCounter;
        set => _jumpBufferCounter = Mathf.Clamp(value, 0f, jumpBufferTime);
    }
    public float CurrentMoveSpeed
    {
        get
        {
            if (IsMoving && CanMove && !_touchingDirections.IsOnWall)
            {
                if (_touchingDirections.IsGrounded)
                {
                    return walkSpeed;
                }
                return airSpeed;
            }
            return 0;
        }
    }
    public bool CanMove => _animator.GetBool("canMove");
    public bool IsAlive => _animator.GetBool("isAlive");

    #region Lifecycle
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _trail = GetComponent<TrailRenderer>();
        _touchingDirections = GetComponent<TouchingDirections>();
        _damageable = GetComponent<Damageable>();
    }

    private void Update()
    {
        CoyoteTimeCounter -= Time.deltaTime;
        if (_touchingDirections.IsGrounded)
        {
            JumpBufferCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!IsDashing && !_damageable.LockVelocity)
        {
            _rb.velocity = new(_moveInput.x * CurrentMoveSpeed, _rb.velocity.y);
        }
        _animator.SetFloat("yVelocity", _rb.velocity.y);
    }
    #endregion

    #region Functions
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
        else if (moveInput.x == 0)
        {
            return;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (IsAlive)
        {
            _moveInput = context.ReadValue<Vector2>();
            IsMoving = _moveInput != Vector2.zero;
            SetFacingDirection(_moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (CanMove)
        {
            if (context.started)
            {
                JumpBufferCounter = jumpBufferTime;
            }
            if (CoyoteTimeCounter > 0f && JumpBufferCounter > 0f)
            {
                _animator.SetTrigger("jump");
                SoundFXManager.Instance.PlaySoundFXClip(jumpSoundFX, transform, 1f);
                _rb.velocity = new(_rb.velocity.x, jumpImpulse);
                JumpBufferCounter = 0f;
            }
            if (context.canceled && _rb.velocity.y > 0)
            {
                _rb.velocity = new(_rb.velocity.x, _rb.velocity.y * 0.6f);
                CoyoteTimeCounter = 0f;
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && CanDash && IsAlive)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        CanDash = false;
        IsDashing = true;
        _rb.velocity = new(transform.localScale.x * dashImpulse, 0f);
        SoundFXManager.Instance.PlaySoundFXClip(dashSoundFX, transform, 1f);
        yield return new WaitForSeconds(dashTime);
        IsDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        SoundFXManager.Instance.PlaySoundFXClip(dashReadySoundFX, transform, 1f);
        CanDash = true;
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        _rb.velocity = new Vector2(knockback.x, _rb.velocity.y + knockback.y);
    }
    #endregion
}
