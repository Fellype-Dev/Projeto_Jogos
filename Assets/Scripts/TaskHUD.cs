using TMPro;
using UnityEngine;

public class TaskHUD : MonoBehaviour
{
    public TextMeshProUGUI taskText;  // Referência ao TextMeshPro no HUD (Canvas)
    public int totalTasks;
    public int completedTasks = 0;

    void Start()
    {
        // Inicializa totalTasks com um valor aleatório entre 2 e 5
        totalTasks = Random.Range(2, 6); // A faixa é [2, 6), ou seja, 2 a 5

        if (taskText != null)
        {
            UpdateTaskText();
        }
        else
        {
            Debug.LogError("taskText is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        // Atualiza constantemente o texto com as tarefas
        if (taskText != null)
        {
            UpdateTaskText();
        }
        else
        {
            Debug.LogError("taskText is null in Update!");
        }
    }

    // Função para atualizar o texto das tarefas
    void UpdateTaskText()
    {
        if (taskText != null)
        {
            taskText.text = $"{completedTasks}/{totalTasks} - Transferência de Dados";
        }
    }

    // Função que pode ser chamada para completar uma tarefa
    public void CompleteTask()
    {
        if (completedTasks < totalTasks)
        {
            completedTasks++;
            UpdateTaskText();  // Certifique-se de atualizar o texto imediatamente
            Debug.Log($"Task {completedTasks}/{totalTasks} completed.");
        }
    }
}