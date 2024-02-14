using UnityEngine;

public class Health : MonoBehaviour
{
    public int healingAmount = 1;

    #region Functions
    public void Heal(Damageable healthPool)
    {
        healthPool.Health += healingAmount;
    }
    #endregion
}
