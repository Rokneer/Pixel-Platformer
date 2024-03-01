using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Health : MonoBehaviour
{
    private Animator _animator;

    [SerializeField]
    private AudioClip _deathSoundFX;

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
            _animator.SetBool("isAlive", value);
            if (!value)
            {
                SoundFXManager.Instance.PlaySoundFXClip(_deathSoundFX, transform, 1f);
            }
        }
    }

    #region Lifecycle
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    #endregion
}
