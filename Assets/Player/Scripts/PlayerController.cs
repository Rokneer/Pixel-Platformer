using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(TrailRenderer))]
[RequireComponent(typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;
    public static PlayerController Instance => _instance;
    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private Animator _animator;
    private TrailRenderer _trail;
    private ParticleSystem _dashRechargeParticles;
    private TouchingDirections _touchingDirections;
    private Damageable _damageable;

    public GameObject pauseMenu;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float airSpeed = 4f;
    public float jumpImpulse = 7f;

    [Header("Dash")]
    public float dashImpulse = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1.2f;

    [Header("Coyote")]
    public float coyoteTime = 0.2f;

    [Header("Jump Buffer")]
    public float jumpBufferTime = 0.2f;

    [Header("Sound Effects")]
    [SerializeField]
    private AudioClip jumpSoundFX;

    [SerializeField]
    private AudioClip dashSoundFX;

    [SerializeField]
    private AudioClip dashReadySoundFX;

    [Header("Map")]
    [SerializeField]
    private GameObject map;

    [SerializeField]
    private GameObject miniMap;

    [SerializeField]
    [Header("Movement")]
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

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction mapAction;
    private InputAction pauseAction;

    #region Lifecycle
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _trail = GetComponent<TrailRenderer>();
        _touchingDirections = GetComponent<TouchingDirections>();
        _damageable = GetComponent<Damageable>();
        _dashRechargeParticles = GetComponent<ParticleSystem>();

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        dashAction = playerInput.actions["Dash"];
        mapAction = playerInput.actions["Map"];
        pauseAction = playerInput.actions["Pause"];
    }

    private void Start()
    {
        jumpAction.started += OnJump;
        jumpAction.canceled += OnJump;
        dashAction.started += OnDash;
        mapAction.started += OnMap;
        mapAction.canceled += OnMap;
        pauseAction.started += OnPause;
    }

    private void OnDisable()
    {
        jumpAction.started -= OnJump;
        jumpAction.canceled -= OnJump;
        dashAction.started -= OnDash;
        mapAction.started -= OnMap;
        mapAction.canceled -= OnMap;
        pauseAction.started -= OnPause;
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
        Movement();
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

    public void Movement()
    {
        if (IsAlive && !PauseManager.Instance.IsPaused)
        {
            _moveInput = moveAction.ReadValue<Vector2>();
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
        _dashRechargeParticles.Play();
        CanDash = true;
    }

    public void OnMap(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            miniMap.SetActive(false);
            map.SetActive(true);
        }
        if (context.canceled)
        {
            miniMap.SetActive(true);
            map.SetActive(false);
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started && PauseManager.Instance.canPause)
        {
            if (!PauseManager.Instance.IsPaused)
            {
                pauseMenu.SetActive(true);
                PauseManager.Instance.PauseGame();
                PauseManager.Instance.ManageMouseVisibility(true);
            }
            else if (PauseManager.Instance.IsPaused)
            {
                pauseMenu.SetActive(false);
                PauseManager.Instance.ResumeGame();
                PauseManager.Instance.ManageMouseVisibility(false);
            }
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        _rb.velocity = new Vector2(knockback.x, _rb.velocity.y + knockback.y);
    }
    #endregion
}
