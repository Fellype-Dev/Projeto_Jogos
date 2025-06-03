using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Audio")]
    public AudioMixer audioMixer;
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;

    [Header("Controles")]
    public float mouseSensitivity = 1f;

    [Header("Tela")]
    public float brilho = 1f;
    public bool isFullScreen = true;
    public int currentResolutionIndex;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CarregarConfiguracoes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SalvarConfiguracoes()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);
        PlayerPrefs.SetFloat("Brilho", brilho);
        PlayerPrefs.SetInt("Fullscreen", isFullScreen ? 1 : 0);
        PlayerPrefs.SetInt("ResolutionIndex", currentResolutionIndex);
        PlayerPrefs.Save();
    }

    public void CarregarConfiguracoes()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f);
        brilho = PlayerPrefs.GetFloat("Brilho", 1f);
        isFullScreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);

        AplicarConfiguracoes();
    }

    public void AplicarConfiguracoes()
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);

        Screen.fullScreen = isFullScreen;

        Resolution[] resolutions = Screen.resolutions;
        if (currentResolutionIndex >= 0 && currentResolutionIndex < resolutions.Length)
        {
            Resolution res = resolutions[currentResolutionIndex];
            Screen.SetResolution(res.width, res.height, isFullScreen);
        }

        GameObject brilhoObj = GameObject.Find("BrilhoOverlay");
        if (brilhoObj != null)
        {
            Image overlay = brilhoObj.GetComponent<Image>();
            overlay.color = new Color(0, 0, 0, 1f - brilho);
        }
    }
}
