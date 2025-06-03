using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingsPanelUI; // Painel de configurações
    [SerializeField] private GameObject blurVolume;       // Volume com Depth of Field para o blur

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("[PauseMenuManager] Tecla ESC pressionada");

            if (settingsPanelUI == null || pauseMenuUI == null)
            {
                Debug.LogError("[PauseMenuManager] Algum painel não foi atribuído no Inspector!");
                return;
            }

            Debug.Log($"[PauseMenuManager] isPaused: {isPaused}, settingsPanelUI.activeSelf: {settingsPanelUI.activeSelf}");

            if (settingsPanelUI.activeSelf)
            {
                Debug.Log("[PauseMenuManager] Fechando configurações");
                CloseSettings();
            }
            else if (isPaused)
            {
                Debug.Log("[PauseMenuManager] Retomando jogo");
                Resume();
            }
            else
            {
                Debug.Log("[PauseMenuManager] Pausando jogo");
                Pause();
            }
        }
    }

    public void Resume()
    {
        Debug.Log("[PauseMenuManager] Resume chamado");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (settingsPanelUI != null) settingsPanelUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        if (blurVolume != null)
        {
            blurVolume.SetActive(false);
        }
        else
        {
            Debug.LogWarning("[PauseMenuManager] blurVolume está nulo");
        }
    }

    void Pause()
    {
        Debug.Log("[PauseMenuManager] Pause chamado");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
        else
        {
            Debug.LogError("[PauseMenuManager] pauseMenuUI está nulo!");
        }

        Time.timeScale = 0f;
        isPaused = true;

        if (blurVolume != null)
        {
            blurVolume.SetActive(true);
        }
        else
        {
            Debug.LogWarning("[PauseMenuManager] blurVolume está nulo");
        }
    }

    public void OpenSettings()
    {
        Debug.Log("[PauseMenuManager] Abrindo painel de configurações");

        if (settingsPanelUI != null) settingsPanelUI.SetActive(true);
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
    }

    public void CloseSettings()
    {
        Debug.Log("[PauseMenuManager] Fechando painel de configurações");

        if (settingsPanelUI != null) settingsPanelUI.SetActive(false);
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Debug.Log("[PauseMenuManager] Carregando cena do menu principal");
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("[PauseMenuManager] QuitGame chamado");
        Application.Quit();
    }
}
