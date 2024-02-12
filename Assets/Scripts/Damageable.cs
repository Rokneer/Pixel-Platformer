using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Damageable : MonoBehaviour
{
    private Animator animator;

    public UnityEvent<int, Vector2> damageableHit;
    
    [SerializeField]
    private bool isInvicible = false;
    private float timeSinceHit = 0;
    public float invicibilityTime = 0.25f;

    [SerializeField]
    private int _maxHealth = 3;
    public int MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    [SerializeField]
    private int _health = 3;
    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health <= 0)
                IsAlive = false;
        }
    }

    [SerializeField]
    private bool _isAlive = true;
    public bool IsAlive
    {
        get => _isAlive;
        set
        {
            _isAlive = value;
            animator.SetBool("isAlive", value);
        }
    }
    public bool LockVelocity
    {
        get => animator.GetBool("lockVelocity");
        set => animator.SetBool("lockVelocity", value);
    }

    #region Lifecycle
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isInvicible)
        {
            if (timeSinceHit > invicibilityTime)
            {
                isInvicible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }
    #endregion
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvicible)
        {
            Health -= damage;
            isInvicible = true;

            animator.SetTrigger("hit");
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);

            return true;
        }
        return false;
    }
}
