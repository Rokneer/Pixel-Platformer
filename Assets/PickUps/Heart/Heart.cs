using UnityEngine;

public class Heart : MonoBehaviour
{
    public int healingAmount = 1;

    #region Functions
    public void Heal(Health healthPool)
    {
        healthPool.CurrentHealth += healingAmount;
    }
    #endregion
}
