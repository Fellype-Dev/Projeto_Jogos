using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseSensitivityController : MonoBehaviour
{
    public Slider sensitivitySlider;
    public TMP_InputField sensitivityInput;

    private bool updatingUI = false;

    void Start()
    {
        // Inicializa os valores com o que está salvo
        float savedValue = SettingsManager.Instance.mouseSensitivity;

        sensitivitySlider.value = savedValue;
        sensitivityInput.text = savedValue.ToString("F2");

        // Listeners
        sensitivitySlider.onValueChanged.AddListener(OnSliderChanged);
        sensitivityInput.onEndEdit.AddListener(OnInputChanged);
    }

    void OnSliderChanged(float value)
    {
        if (updatingUI) return;
        updatingUI = true;

        // Atualiza o input field
        sensitivityInput.text = value.ToString("F2");

        // Salva no SettingsManager
        SettingsManager.Instance.mouseSensitivity = value;
        SettingsManager.Instance.SalvarConfiguracoes();

        updatingUI = false;
    }

    void OnInputChanged(string input)
    {
        if (updatingUI) return;
        updatingUI = true;

        float parsed;
        if (float.TryParse(input, out parsed))
        {
            // Clampa para não ultrapassar os limites do slider
            parsed = Mathf.Clamp(parsed, sensitivitySlider.minValue, sensitivitySlider.maxValue);

            sensitivitySlider.value = parsed;
            SettingsManager.Instance.mouseSensitivity = parsed;
            SettingsManager.Instance.SalvarConfiguracoes();
        }
        else
        {
            // Reverte para o valor atual se entrada inválida
            sensitivityInput.text = sensitivitySlider.value.ToString("F2");
        }

        updatingUI = false;
    }
}
