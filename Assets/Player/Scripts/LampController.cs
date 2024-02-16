using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class LampController : MonoBehaviour
{
    private Light2D lampLight;
    private Damageable healthPool;


    public Image energyImage;
    public List<Sprite> energySprites = new();
    public Sprite emptySprite;
    public float minLightIntesity = 3f;
    public float maxLightIntesity = 5f;
    public float maxLightTimer = 60f;
    public float maxLightRadius = 4f;

    [SerializeField]
    private float _lightIntesity = 5f;
    public float LightIntesity
    {
        get => _lightIntesity;
        set => _lightIntesity = Mathf.Clamp(value, minLightIntesity, maxLightIntesity);
    }

    [SerializeField]
    private float _lightOuterRadius = 4f;
    public float LightOuterRadius
    {
        get => _lightOuterRadius;
        set => _lightOuterRadius = Mathf.Clamp(value, 0, maxLightRadius);
    }

    [SerializeField]
    private float _lightTimer = 60f;
    public float LightTimer
    {
        get => _lightTimer;
        set
        {
            _lightTimer = Mathf.Clamp(value, 0, maxLightTimer);
            switch (value)
            {
                case <= 0:
                    lampLight.pointLightOuterRadius = 0;
                    lampLight.intensity = 0;
                    energyImage.sprite = emptySprite;
                    break;
                case <= 20:
                    lampLight.pointLightOuterRadius = 4;
                    lampLight.intensity = 3;
                    energyImage.sprite = energySprites[0];
                    break;
                case <= 40:
                    lampLight.pointLightOuterRadius = 5;
                    lampLight.intensity = 4;
                    energyImage.sprite = energySprites[1];
                    break;
                case <= 60:
                default:
                    lampLight.pointLightOuterRadius = 6;
                    lampLight.intensity = 5;
                    energyImage.sprite = energySprites[2];
                    break;
            }
        }
    }

    #region Lifecycle
    private void Awake()
    {
        lampLight = GetComponent<Light2D>();
        healthPool = GetComponentInParent<Damageable>();
    }

    void Update()
    {
        LightTimer -= Time.deltaTime;
        if (LightTimer <= 0)
        {
            StartCoroutine(InDarkness());
        }
    }
    #endregion

    #region Functions

    private IEnumerator InDarkness()
    {
        healthPool.Hit(1, new Vector2(0, 0));
        yield return new WaitForSeconds(healthPool.invicibilityTime);
    }
    #endregion
}
