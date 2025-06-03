using UnityEngine;
using UnityEngine.UI;

public class BrightnessController : MonoBehaviour
{
    public Image brightnessPanel;
    public Slider brightnessSlider;

    void Start()
    {
        brightnessSlider.onValueChanged.AddListener(UpdateBrightness);
    }

    void UpdateBrightness(float value)
    {
        Color color = brightnessPanel.color;
        color.a = value;
        brightnessPanel.color = color;
    }
}
