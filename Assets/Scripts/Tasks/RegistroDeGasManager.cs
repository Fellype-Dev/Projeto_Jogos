using UnityEngine;
using TMPro;

public class RegistrosDeGasManager : MonoBehaviour
{
    public static RegistrosDeGasManager instance;

    public int totalRegistros = 4;
    private int registrosFechados = 0;

    public TaskHUD taskHUD; // Arraste o HUD no inspector

    public TextMeshProUGUI textoTaskCompleta; // Mensagem opcional na UI

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        if (taskHUD == null)
            taskHUD = FindObjectOfType<TaskHUD>();

        if (taskHUD != null)
        {
            taskHUD.SetTotalTasks("MANUTENÇÃO DE EQUIPAMENTOS", totalRegistros);
        }
        else
        {
            Debug.LogWarning("TaskHUD não encontrado no RegistrosDeGasManager!");
        }

        if (textoTaskCompleta != null)
            textoTaskCompleta.gameObject.SetActive(false);
    }

    public void RegistroFechado(int idRegistro)
    {
        registrosFechados++;
        Debug.Log($"Registro {idRegistro} fechado! ({registrosFechados}/{totalRegistros})");

        if (registrosFechados >= totalRegistros)
        {
            Debug.Log("Todos os registros fechados! Task completa!");
            if (textoTaskCompleta != null)
            {
                textoTaskCompleta.text = "Todos os registros fechados! Task completa!";
                textoTaskCompleta.gameObject.SetActive(true);
            }

            if (taskHUD != null)
                taskHUD.CompleteTask("MANUTENÇÃO DE EQUIPAMENTOS");
        }
    }
}
