using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Carregar valores salvos
        masterSlider.value = SettingsManager.Instance.masterVolume;
        musicSlider.value = SettingsManager.Instance.musicVolume;
        sfxSlider.value = SettingsManager.Instance.sfxVolume;

        // Listeners
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float value)
    {
        SettingsManager.Instance.masterVolume = value;
        SettingsManager.Instance.AplicarConfiguracoes();
        SettingsManager.Instance.SalvarConfiguracoes();
    }

    public void SetMusicVolume(float value)
    {
        SettingsManager.Instance.musicVolume = value;
        SettingsManager.Instance.AplicarConfiguracoes();
        SettingsManager.Instance.SalvarConfiguracoes();
    }

    public void SetSFXVolume(float value)
    {
        SettingsManager.Instance.sfxVolume = value;
        SettingsManager.Instance.AplicarConfiguracoes();
        SettingsManager.Instance.SalvarConfiguracoes();
    }
}
