using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator), typeof(Health))]
public class Damageable : MonoBehaviour
{
    private Animator _animator;
    private Health _healthPool;

    public UnityEvent<int, Vector2> damageableHit;

    [SerializeField]
    private AudioClip[] _damageSoundClips;
    [SerializeField]
    private bool _isInvicible = false;
    private float _timeSinceHit = 0;
    public float invicibilityTime = 0.25f;

    public bool LockVelocity
    {
        get => _animator.GetBool("lockVelocity");
        set => _animator.SetBool("lockVelocity", value);
    }

    #region Lifecycle
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _healthPool = GetComponent<Health>();
    }

    private void Update()
    {
        if (_isInvicible)
        {
            if (_timeSinceHit > invicibilityTime)
            {
                _isInvicible = false;
                _timeSinceHit = 0;
            }
            _timeSinceHit += Time.deltaTime;
        }
    }
    #endregion
    public bool Hit(int damage, Vector2 knockback)
    {
        if (_healthPool.IsAlive && !_isInvicible)
        {
            _healthPool.CurrentHealth -= damage;
            _isInvicible = true;

            _animator.SetTrigger("hit");
            SoundFXManager.Instance.PlayRandomSoundFXClip(_damageSoundClips,transform,1f);
            LockVelocity = true;
            damageableHit.Invoke(damage, knockback);

            return true;
        }
        return false;
    }
}
