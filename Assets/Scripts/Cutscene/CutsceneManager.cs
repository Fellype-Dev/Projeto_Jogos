using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public GameObject cutscenePanel;
    public VideoPlayer videoPlayer;
    public VideoClip victoryClip;
    public VideoClip defeatClip;
    

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
        cutscenePanel.SetActive(true);
        videoPlayer.clip = clip;
        videoPlayer.loopPointReached += OnCutsceneFinished; // Evento chamado no fim do vídeo
        videoPlayer.Play();

        AudioListener.volume = 0f;
        Time.timeScale = 0f; // Pausa o jogo, mas o vídeo segue
    }

void OnCutsceneFinished(VideoPlayer vp)
{
    Time.timeScale = 1f;
    AudioListener.volume = 1f;

    Cursor.lockState = CursorLockMode.None; // Libera o mouse
    Cursor.visible = true;                 // Torna o cursor visível

    SceneManager.LoadScene("MainMenu");
}
    public void HideCutscenePanel() // opcional: se quiser permitir pular com botão
    {
        cutscenePanel.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.volume = 1f;
    }
}
