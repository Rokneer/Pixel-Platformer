using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health), typeof(Damageable))]
public class PlayerManager : MonoBehaviour
{
    private Health healthPool;
    private Damageable damageable;

    public Image healthImage;
    public List<Sprite> healthSprites = new();
    public Image energyImage;
    public List<Sprite> energySprites = new();
    public Image keyImage;
    public TextMeshProUGUI coinText;

    private int _hearts;
    public int Hearts { 
        get => _hearts;
        set
        {
            _hearts = Mathf.Clamp(value, 0, healthPool.MaxHealth);
            switch (_hearts)
            {
                case 3:
                    healthImage.sprite = healthSprites[value-1];
                    break;
                case 2:
                    healthImage.sprite = healthSprites[value-1];
                    break;
                case 1:
                    healthImage.sprite = healthSprites[value-1];
                    break;
                default: 
                    Debug.LogError($"ERROR: Invalid health value of {value}");
                    break;
            }
        }
    }


    #region Lifecycle
    private void Awake()
    {
        healthPool = GetComponent<Health>();
        damageable = GetComponent<Damageable>();
    }
    #endregion

    #region Functions
    public bool PickUpItem(GameObject obj)
    {
        switch (obj.tag)
        {
            case "Health":
                Debug.Log("Picked up a " + obj.tag);
                obj.GetComponent<Heart>().Heal(healthPool);
                return true;
            case "Key":
                Debug.Log("Picked up a " + obj.tag);
                obj.GetComponent<Key>().AddKey();
                return true;
            case "Energy":
                Debug.Log("Picked up a " + obj.tag);
                obj.GetComponent<Energy>().AddEnergy();
                return true;
            case "Coin":
                Debug.Log("Picked up a " + obj.tag);
                obj.GetComponent<Coin>().AddCoin();
                return true;
            default:
                Debug.LogWarning($"WARNING: No handler implemented for tag {obj.tag}");
                return false;
        }
    }
    private void UpdateUIHealth(){
        
    }
    private void UpdateUIKey(){

    }
    private void UpdateUIEnergy(){

    }
    private void UpdateUICoin(){

    }
    #endregion
}
