using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplaySettingsController : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Slider brightnessSlider;

    private Resolution[] resolutions;

    void Start()
    {
        // Preencher dropdown com resoluções disponíveis
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        var options = new System.Collections.Generic.List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = SettingsManager.Instance.currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = SettingsManager.Instance.isFullScreen;
        brightnessSlider.value = SettingsManager.Instance.brilho;

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
    }

    public void SetResolution(int index)
    {
        SettingsManager.Instance.currentResolutionIndex = index;
        SettingsManager.Instance.AplicarConfiguracoes();
        SettingsManager.Instance.SalvarConfiguracoes();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        SettingsManager.Instance.isFullScreen = isFullscreen;
        SettingsManager.Instance.AplicarConfiguracoes();
        SettingsManager.Instance.SalvarConfiguracoes();
    }

    public void SetBrightness(float value)
    {
        SettingsManager.Instance.brilho = value;
        SettingsManager.Instance.AplicarConfiguracoes();
        SettingsManager.Instance.SalvarConfiguracoes();
    }
}
