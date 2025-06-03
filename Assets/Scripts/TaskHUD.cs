using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class TaskHUD : MonoBehaviour
{
    public TextMeshProUGUI taskText; // Referência ao TextMeshPro no HUD (Canvas)

    private Dictionary<string, int> taskGoals = new Dictionary<string, int>();
    private Dictionary<string, int> taskProgress = new Dictionary<string, int>();

    private string[] taskNames = new string[]
    {
        "TRANSFERÊNCIA DE DADOS",
        "TRANSPORTE DE CAIXAS",
        "ANÁLISE DE OPERÁRIOS",
        "MANUTENÇÃO DE EQUIPAMENTOS"
    };

    void Start()
    {
        // Inicializa metas padrão (pode ser sobrescrito depois via SetTotalTasks)
        taskGoals["TRANSFERÊNCIA DE DADOS"] = Random.Range(2, 5);
        taskGoals["TRANSPORTE DE CAIXAS"] = Random.Range(2, 5);
        taskGoals["ANÁLISE DE OPERÁRIOS"] = Random.Range(1, 2);
        taskGoals["MANUTENÇÃO DE EQUIPAMENTOS"] = Random.Range(1, 3);

        foreach (var task in taskNames)
        {
            taskProgress[task] = 0;
        }

        if (taskText != null)
            UpdateTaskText();
        else
            Debug.LogError("taskText is not assigned in the Inspector!");
    }

    void Update()
    {
        if (taskText != null)
            UpdateTaskText();
    }

    void UpdateTaskText()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (var task in taskNames)
        {
            sb.AppendLine($"{taskProgress[task]}/{taskGoals[task]} - {task}");
        }
        taskText.text = sb.ToString();
    }

    public void CompleteTask(string taskType)
    {
        if (taskProgress.ContainsKey(taskType) && taskProgress[taskType] < taskGoals[taskType])
        {
            taskProgress[taskType]++;
            UpdateTaskText();
            Debug.Log($"Task '{taskType}' progress: {taskProgress[taskType]}/{taskGoals[taskType]}");
        }
        else
        {
            Debug.LogWarning($"Task '{taskType}' is already completed or invalid.");
        }
    }

    public void SetTotalTasks(string taskType, int total)
    {
        if (taskGoals.ContainsKey(taskType))
            taskGoals[taskType] = total;
        else
            taskGoals.Add(taskType, total);

        if (!taskProgress.ContainsKey(taskType))
            taskProgress.Add(taskType, 0);
        else
            taskProgress[taskType] = 0;

        UpdateTaskText();
    }
}
