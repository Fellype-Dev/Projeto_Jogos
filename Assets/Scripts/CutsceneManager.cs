using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    public GameObject cutscenePanel; // O painel com a RawImage
    public VideoPlayer videoPlayer;  // O VideoPlayer
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
        videoPlayer.Play();
        AudioListener.volume = 0f;
        Time.timeScale = 0f; // Pausa o jogo (UI e vídeo continuam)
    }

    // Chame esse método em um botão para fechar a cutscene e destravar o jogo
    public void HideCutscenePanel()
    {
        cutscenePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}