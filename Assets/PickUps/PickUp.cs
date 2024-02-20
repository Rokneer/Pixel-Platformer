using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField]
    private AudioClip pickUpSoundFX;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerManager>(out PlayerManager manager))
        {
            bool pickedUp = manager.PickUpItem(gameObject);
            if (pickedUp)
            {
                SoundFXManager.Instance.PlaySoundFXClip(pickUpSoundFX, transform, 1f);
                Destroy(gameObject);
            }
        }
    }
}
