using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public bool hasHD = false;
    public bool hdInserted = false;
    public bool transferComplete = false;
    public bool hdComDados = false;

    public GameObject hdObject;
    public GameObject computer;
    public GameObject server;
    public TransferManager transferManager;
    public PlayerInteractionUI interactionUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Pegar o HD
            if (!hasHD && IsNear(hdObject))
            {
                hasHD = true;
                hdObject.SetActive(false);
                Debug.Log("HD coletado.");
            }
            // Inserir no computador (somente se HD não tiver dados)
            else if (hasHD && IsNear(computer) && !hdInserted && !hdComDados)
            {
                hdInserted = true;
                hasHD = false;
                Debug.Log("HD inserido no computador.");
            }
            // Retirar HD após transferência
            else if (hdInserted && transferComplete && IsNear(computer))
            {
                hdInserted = false;
                hasHD = true;
                transferComplete = false;
                Debug.Log("HD retirado com dados.");
            }
            // Inserir no servidor (somente se HD tiver dados)
            else if (hasHD && IsNear(server) && hdComDados)
            {
                hasHD = false;
                hdComDados = false; // Dados foram entregues
                Debug.Log("Tarefa concluída com sucesso!");
            }
        }

        // Iniciar a transferência de dados
        if (Input.GetKeyDown(KeyCode.Space) && hdInserted && !transferComplete)
        {
            transferManager.StartTransfer();
        }
    }

    private void LateUpdate()
    {
        if (IsNear(hdObject) && !hasHD)
        {
            interactionUI.MostrarTexto("[E] Pegar HD");
        }
        else if (IsNear(computer))
        {
            if (hasHD && !hdInserted && !hdComDados)
            {
                interactionUI.MostrarTexto("[E] Inserir HD no Computador");
            }
            else if (hdInserted && !transferComplete)
            {
                interactionUI.MostrarTexto("[SPACE] Iniciar Transferência");
            }
            else if (hdInserted && transferComplete)
            {
                interactionUI.MostrarTexto("[E] Retirar HD com Dados");
            }
            else
            {
                interactionUI.EsconderTexto();
            }
        }
        else if (IsNear(server) && hasHD && hdComDados)
        {
            interactionUI.MostrarTexto("[E] Inserir HD no Servidor");
        }
        else
        {
            interactionUI.EsconderTexto();
        }
    }

    private bool IsNear(GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position) < 4f; // Aumentado para 4f
    }
}
