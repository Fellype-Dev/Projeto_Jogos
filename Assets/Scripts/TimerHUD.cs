using UnityEngine;
using TMPro;

public class TimerHUD : MonoBehaviour
{
    public float tempoTotal = 480f; // 8 minutos

    [Header("Referências de UI")]
    public TextMeshProUGUI timerText;
    public TaskHUD taskHUD;
    public CutsceneManager cutsceneManager; // NOVO!

    private float tempoRestante;
    private bool terminou = false;

    void Start()
    {
        tempoRestante = tempoTotal;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (terminou) return;

        tempoRestante -= Time.deltaTime;
        if (tempoRestante < 0)
        {
            tempoRestante = 0;
            terminou = true;
            Derrota();
        }

        AtualizarHUD();

        if (taskHUD != null && TodasTasksConcluidas() && !terminou)
        {
            terminou = true;
            Vitoria();
        }
    }

    void AtualizarHUD()
    {
        int minutos = Mathf.FloorToInt(tempoRestante / 60F);
        int segundos = Mathf.FloorToInt(tempoRestante % 60F);
        if (timerText != null)
            timerText.text = $"{minutos:00}:{segundos:00}";
    }

    bool TodasTasksConcluidas()
    {
        if (taskHUD == null) return false;
        foreach (var task in taskHUD.TaskNames)
        {
            if (taskHUD.TaskProgress[task] < taskHUD.TaskGoals[task])
                return false;
        }
        return true;
    }

    void Vitoria()
    {
        Debug.Log("VITÓRIA!");
        if (cutsceneManager != null) cutsceneManager.PlayVictoryCutscene();
    }

    void Derrota()
    {
        Debug.Log("DERROTA!");
        if (cutsceneManager != null) cutsceneManager.PlayDefeatCutscene();
    }
}