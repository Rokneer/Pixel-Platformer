using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Health : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private AudioClip deathSoundFX;

    [SerializeField]
    private int _maxHealth = 3;
    public int MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    [SerializeField]
    private int _currentHealth = 3;
    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            if (_currentHealth <= 0)
            {
                IsAlive = false;
            }
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
            if (!value)
            {
                SoundFXManager.Instance.PlaySoundFXClip(deathSoundFX, transform, 1f);
            }
        }
    }

    #region Lifecycle
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    #endregion
}
