using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float bounceForce = 15.0f;

    [SerializeField]
    private AudioClip trampolineSoundFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundFXManager.Instance.PlaySoundFXClip(trampolineSoundFX, transform, 1f);
            collision
                .gameObject
                .GetComponent<Rigidbody2D>()
                .AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}
