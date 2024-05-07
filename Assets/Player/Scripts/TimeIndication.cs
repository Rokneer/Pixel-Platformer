using UnityEngine;
using UnityEngine.UI;

public class TimeIndication : MonoBehaviour
{
    private Slider _lightSlider;

    [SerializeField]
    private Image _sliderLightIcon;

    [SerializeField]
    private LampController _lampController;

    private void Awake()
    {
        _lightSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        float mappedLight = Mathf.Clamp01(
            _lampController.LightTimer / _lampController.maxLightTimer
        );
        _lightSlider.value = mappedLight;
        _sliderLightIcon.enabled = mappedLight > 0;
    }
}
