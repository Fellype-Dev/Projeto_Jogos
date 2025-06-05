using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public GameObject cutscenePanel;
    public VideoPlayer videoPlayer;
    public VideoClip victoryClip;
    public VideoClip defeatClip;

    [Header("Canvas a desativar durante a cutscene")]
    public GameObject[] canvasesToDisable;

    void Start()
    {
        cutscenePanel.SetActive(false);
    }

    public void PlayVictoryCutscene()
    {
        PlayCutscene(victoryClip);
    }

    public void PlayDefeatCutscene()
    {
        PlayCutscene(defeatClip);
    }

    void PlayCutscene(VideoClip clip)
    {
        // Ativa painel da cutscene
        cutscenePanel.SetActive(true);

        // Desativa os canvas do HUD
        foreach (GameObject canvas in canvasesToDisable)
        {
            if (canvas != null)
                canvas.SetActive(false);
        }

        // Inicia vídeo
        videoPlayer.clip = clip;
        videoPlayer.loopPointReached += OnCutsceneFinished;
        videoPlayer.Play();

        // Pausa o tempo do jogo e o áudio geral
        AudioListener.volume = 0f;
        Time.timeScale = 0f;
    }

    void OnCutsceneFinished(VideoPlayer vp)
    {
        Time.timeScale = 1f;
        AudioListener.volume = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MainMenu");
    }

    public void HideCutscenePanel()
    {
        cutscenePanel.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.volume = 1f;
    }
}
