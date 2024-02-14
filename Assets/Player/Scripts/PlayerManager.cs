using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class PlayerManager : MonoBehaviour
{
    private Damageable damageable;

    public float keys;
    public float energy;
    public float coins;

    #region Lifecycle
    private void Awake()
    {
        damageable = GetComponent<Damageable>();
    }
    #endregion

    #region Functions
    public bool PickUpItem(GameObject obj)
    {
        switch (obj.tag)
        {
            case "Health":
                obj.GetComponent<Health>().Heal(damageable);
                Debug.Log("Picked up a " + obj.tag);
                return true;
            case "Key":
                keys++;
                obj.GetComponent<Key>().AddKey();
                Debug.Log("Picked up a " + obj.tag);
                return true;
            case "Energy":
                energy++;
                obj.GetComponent<Energy>().AddEnergy();
                Debug.Log("Picked up a " + obj.tag);
                return true;
            case "Coin":
                coins++;
                obj.GetComponent<Coin>().AddCoin();
                Debug.Log("Picked up a " + obj.tag);
                return true;
            default:
                Debug.LogError($"ERROR: No handler implemented for tag {obj.tag}");
                return false;
        }
    }
    #endregion
}
