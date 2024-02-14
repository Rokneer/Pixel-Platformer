using UnityEngine;

public class PickUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerManager>(out PlayerManager manager))
        {
            bool pickedUp = manager.PickUpItem(gameObject);
            if (pickedUp)
            {
                Destroy(gameObject);
            }
        }
    }
}
