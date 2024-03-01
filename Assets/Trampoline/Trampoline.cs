using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Trampoline : MonoBehaviour
{
    public float _bounceForce = 15.0f;

    [SerializeField]
    private AudioClip _trampolineSoundFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundFXManager.Instance.PlaySoundFXClip(_trampolineSoundFX, transform, 1f);
            collision
                .gameObject
                .GetComponent<Rigidbody2D>()
                .AddForce(Vector2.up * _bounceForce, ForceMode2D.Impulse);
        }
    }
}
