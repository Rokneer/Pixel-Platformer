using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Damageable))]
public class PlayerManager : MonoBehaviour
{
    private Damageable damageable;

    public Image healthImage;
    public List<Sprite> healthSprites = new();
    public Image energyImage;
    public List<Sprite> energySprites = new();
    public Image keyImage;
    public TextMeshProUGUI coinText;

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
                obj.GetComponent<Key>().AddKey();
                Debug.Log("Picked up a " + obj.tag);
                return true;
            case "Energy":
                obj.GetComponent<Energy>().AddEnergy();
                Debug.Log("Picked up a " + obj.tag);
                return true;
            case "Coin":
                obj.GetComponent<Coin>().AddCoin();
                Debug.Log("Picked up a " + obj.tag);
                return true;
            default:
                Debug.LogWarning($"WARNING: No handler implemented for tag {obj.tag}");
                return false;
        }
    }
    #endregion
}
