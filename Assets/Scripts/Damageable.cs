using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator), typeof(Health))]
public class Damageable : MonoBehaviour
{
    private Animator animator;
    private Health healthPool;

    public UnityEvent<int, Vector2> damageableHit;

    [SerializeField]
    private AudioClip[] damageSoundClips;
    [SerializeField]
    private bool isInvicible = false;
    private float timeSinceHit = 0;
    public float invicibilityTime = 0.25f;

    public bool LockVelocity
    {
        get => animator.GetBool("lockVelocity");
        set => animator.SetBool("lockVelocity", value);
    }

    #region Lifecycle
    private void Awake()
    {
        animator = GetComponent<Animator>();
        healthPool = GetComponent<Health>();
    }

    private void Update()
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
        if (healthPool.IsAlive && !isInvicible)
        {
            healthPool.CurrentHealth -= damage;
            isInvicible = true;

            animator.SetTrigger("hit");
            SoundFXManager.Instance.PlayRandomSoundFXClip(damageSoundClips,transform,1f);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);

            return true;
        }
        return false;
    }
}
