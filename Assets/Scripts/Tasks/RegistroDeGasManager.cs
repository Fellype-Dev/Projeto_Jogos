using UnityEngine;
using TMPro;

public class RegistrosDeGasManager : MonoBehaviour
{
    public static RegistrosDeGasManager instance;
    public int totalRegistros = 4;
    private int registrosFechados = 0;

    public TextMeshProUGUI textoTaskCompleta; // Arraste um TMP para mostrar "Task completa!", se quiser

    void Awake()
    {
        instance = this;
        if (textoTaskCompleta != null)
            textoTaskCompleta.gameObject.SetActive(false);
    }

    public void RegistroFechado(int idRegistro)
    {
        registrosFechados++;
        Debug.Log($"Registro {idRegistro} fechado! ({registrosFechados}/{totalRegistros})");

        if (registrosFechados >= totalRegistros)
        {
            Debug.Log("Todos os registros fechados! TASK COMPLETA!");
            if (textoTaskCompleta != null)
            {
                textoTaskCompleta.text = "Todos os registros fechados! Task completa!";
                textoTaskCompleta.gameObject.SetActive(true);
            }
            // Aqui você pode chamar HUD ou outro evento do seu sistema
        }
    }
}