using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingPanel; // Painel com a animação de loading

    public void PlayGame()
    {
        StartCoroutine(LoadNextSceneAsync());
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    IEnumerator LoadNextSceneAsync()
    {
        loadingPanel.SetActive(true); // Ativa o painel com a animação
        yield return new WaitForSeconds(0.1f); // Garante atualização da UI

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
