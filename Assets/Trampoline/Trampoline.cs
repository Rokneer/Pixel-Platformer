using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Trampoline : MonoBehaviour
{
    private BoxCollider2D bounceCollider;

    public float _bounceForce = 15.0f;

    [SerializeField]
    private AudioClip _trampolineSoundFX;

    private void Awake()
    {
        bounceCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Bounce(collision));
    }

    #region Functions
    private IEnumerator Bounce(Collider2D collision)
    {
        SoundFXManager.Instance.PlaySoundFXClip(_trampolineSoundFX, transform, 1f);
        collision
            .gameObject
            .GetComponent<Rigidbody2D>()
            .AddForce(Vector2.up * _bounceForce, ForceMode2D.Impulse);
        bounceCollider.enabled = false;
        yield return new WaitForSeconds(0.1f);
        bounceCollider.enabled = true;
    }
    #endregion
}
