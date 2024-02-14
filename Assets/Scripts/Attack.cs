using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage = 1;
    public Vector2 knockback = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Damageable>(out Damageable damageable))
        {
            Vector2 deliveredKnockback =
                transform.parent.localScale.x > 0
                    ? knockback
                    : new Vector2(-knockback.x, knockback.y);
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);
            if (gotHit)
            {
                Debug.Log(collision.name + " got hit for " + attackDamage);
            }
        }
    }
}
