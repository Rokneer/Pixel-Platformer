using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health), typeof(Damageable))]
public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;
    public static PlayerManager Instance => _instance;
    private Health _healthPool;
    private LampController _lampEnergy;

    public Image playerImage;
    public List<Sprite> playerSprites = new();
    public Image healthImage;
    public List<Sprite> healthSprites = new();
    public Sprite emptySprite;
    public TextMeshProUGUI playerCoinText;
    public TextMeshProUGUI totalCoinText;
    public int maxCoinValue = 99;
    public Transform currentCheckpoint;

    private int _hearts;
    public int Hearts
    {
        get => _hearts;
        set
        {
            _hearts = Mathf.Clamp(value, 0, _healthPool.MaxHealth);
            switch (_hearts)
            {
                case 3:
                    healthImage.sprite = healthSprites[_hearts - 1];
                    break;
                case 2:
                    healthImage.sprite = healthSprites[_hearts - 1];
                    break;
                case 1:
                    healthImage.sprite = healthSprites[_hearts - 1];
                    break;
                case 0:
                    healthImage.sprite = emptySprite;
                    playerImage.sprite = playerSprites[1];
                    break;
                default:
                    Debug.LogError($"ERROR: Invalid health value of {value}");
                    break;
            }
        }
    }
    private int _coins = 0;
    public int Coins
    {
        get => _coins;
        set
        {
            _coins = Mathf.Clamp(value, 0, maxCoinValue);
            playerCoinText.text = value.ToString();
        }
    }

    #region Lifecycle
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        _healthPool = GetComponent<Health>();
        _lampEnergy = GetComponentInChildren<LampController>();
    }

    private void Update()
    {
        Hearts = _healthPool.CurrentHealth;
    }
    #endregion

    #region Functions
    public bool PickUpItem(GameObject pickUp)
    {
        switch (pickUp.tag)
        {
            case "Health":
                return UpdateUIHealth(pickUp);
            case "Key":
                return UpdateUIKey(pickUp);
            case "Energy":
                return UpdateUIEnergy(pickUp);
            case "Coin":
                return UpdateUICoin(pickUp);
            case "Gem":
                return ActivateLevelCompleteUI();
            default:
                Debug.LogWarning($"WARNING: No handler implemented for tag {pickUp.tag}");
                return false;
        }
    }

    private bool UpdateUIHealth(GameObject pickUp)
    {
        bool wasHealed = pickUp.GetComponent<Heart>().Heal(_healthPool);
        return wasHealed;
    }

    private bool UpdateUIKey(GameObject pickUp)
    {
        pickUp.GetComponent<Key>().AddKey();
        return true;
    }

    private bool UpdateUIEnergy(GameObject pickUp)
    {
        _lampEnergy.LightTimer += pickUp.GetComponent<Energy>().AddEnergy();
        return true;
    }

    private bool UpdateUICoin(GameObject pickUp)
    {
        Coins += pickUp.GetComponent<Coin>().AddCoin();
        return true;
    }

    private bool ActivateLevelCompleteUI()
    {
        GameManager.Instance.gameOverUI.GetComponentInChildren<TextMeshProUGUI>().text = "Winner";
        GameManager.Instance.ActivateGameOverUI();
        return true;
    }

    #endregion
}
