using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LampController : MonoBehaviour
{
    private Light2D lampLight;

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
            switch (value)
            {
                case 0:
                    lampLight.pointLightOuterRadius = 0;
                    lampLight.intensity = 0;
                    break;
                case <= 20:
                    lampLight.pointLightOuterRadius = 3;
                    lampLight.intensity = 2;
                    break;
                case <= 40:
                    lampLight.pointLightOuterRadius = 4;
                    lampLight.intensity = 3;
                    break;
                case 60:
                default:
                    lampLight.pointLightOuterRadius = 5;
                    lampLight.intensity = 4;
                    break;
            }
            _lightTimer = Mathf.Clamp(value, 0, maxLightTimer);
        }
    }

    #region Lifecycle
    private void Awake()
    {
        lampLight = GetComponent<Light2D>();
    }

    void Update()
    {
        LightTimer -= Time.deltaTime;
    }
    #endregion
}
