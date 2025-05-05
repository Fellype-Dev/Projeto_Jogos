using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransferManager : MonoBehaviour
{
    [Header("Referências da UI")]
    public GameObject transferUI;
    public Image progressBar;
    public float transferTime = 5f;

    [HideInInspector] public bool isTransferring = false;

    public void StartTransfer()
    {
        if (!isTransferring)
        {
            StartCoroutine(TransferData());
        }
    }

    private IEnumerator TransferData()
    {
        isTransferring = true;

        if (transferUI != null)
            transferUI.SetActive(true);

        float elapsed = 0f;

        while (elapsed < transferTime)
        {
            elapsed += Time.deltaTime;

            if (progressBar != null)
                progressBar.fillAmount = elapsed / transferTime;

            yield return null;
        }

        if (transferUI != null)
            transferUI.SetActive(false);

        isTransferring = false;

        // Informar que a transferência terminou
        PlayerInteraction player = FindObjectOfType<PlayerInteraction>();
        if (player != null)
        {
            player.transferComplete = true;
            player.hdComDados = true; // Marcar que o HD agora possui dados
        }
    }
}
