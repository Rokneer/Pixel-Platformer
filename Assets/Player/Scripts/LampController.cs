using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LampController : MonoBehaviour
{
    private Light2D lampLight;

    public float minLightIntesity = 3;
    public float maxLightIntesity = 5;

    [SerializeField]
    private float _lightIntesity = 5f;
    public float LightIntesity { get => _lightIntesity; set => _lightIntesity = Mathf.Clamp(value, minLightIntesity, maxLightIntesity);
    }

    [SerializeField]
    private float _lightTimer = 60f;
    public float LightTimer { get => _lightTimer; set
        {
            switch (value)
            {
                case <= 20:
                    LightIntesity = 3;
                    lampLight.intensity = LightIntesity;
                    break;
                case <= 40:
                    LightIntesity = 4;
                    lampLight.intensity = LightIntesity;
                    break;
                case 60:
                default:
                    LightIntesity = 5;
                    lampLight.intensity = LightIntesity;
                    break; 
            }
            _lightTimer = value;
        }
    }

    private void Awake() {
        lampLight = GetComponent<Light2D>();
    }

    void Update()
    {
        LightTimer -= Time.deltaTime;
    }

    
}
