using UnityEngine;

public class Heart : MonoBehaviour
{
    public int healingAmount = 1;

    #region Functions
    public bool Heal(Health healthPool)
    {
        if (healthPool.IsAlive && healthPool.CurrentHealth < healthPool.MaxHealth)
        {
            healthPool.CurrentHealth += healingAmount;
            return true;
        }
        return false;
    }
    #endregion
}
