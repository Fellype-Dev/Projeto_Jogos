using UnityEngine;
using System.Collections.Generic; // Para usar List

public class PlayerInteraction : MonoBehaviour
{
    public bool hasHD = false;
    public bool hdInserted = false;
    public bool transferComplete = false;
    public bool hdComDados = false;

    public GameObject listaHD; // Referência ao objeto vazio que contém os HDs
    public GameObject computer;
    public GameObject server;
    public TransferManager transferManager;
    public PlayerInteractionUI interactionUI;
    public TaskHUD taskHUD; // Referência ao TaskHUD

    private List<GameObject> hdObjects = new List<GameObject>(); // Lista de HDs no jogo

    private void Start()
    {
        // Carregar todos os HDs dentro do objeto "ListaHD"
        PopulateHDList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Pegar o HD
            GameObject targetHD = GetClosestHD();
            if (targetHD != null && !hasHD && IsNear(targetHD))
            {
                hasHD = true;
                targetHD.SetActive(false); // Remove o HD da cena (não pode mais ser pego)
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
                // Verifique se taskHUD não é nulo antes de chamar CompleteTask
                if (taskHUD != null)
                {
                    taskHUD.CompleteTask();
                }
                else
                {
                    Debug.LogError("taskHUD não foi configurado corretamente!");
                }
                Debug.Log("Tarefa concluída com sucesso!");

                // Remover o HD permanentemente da cena
                Destroy(targetHD); // Remove o HD coletado da cena
            }
        }

        // Iniciar a transferência de dados
        if (Input.GetKeyDown(KeyCode.Q) && hdInserted && !transferComplete)
        {
            transferManager.StartTransfer();
        }
    }

    private void LateUpdate()
    {
        GameObject closestHD = GetClosestHD();

        if (closestHD != null && IsNear(closestHD) && !hasHD)
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
                interactionUI.MostrarTexto("[Q] Iniciar Transferência");
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
        return Vector3.Distance(transform.position, target.transform.position) < 3f; // Distância de interação
    }

    // Função para retornar o HD mais próximo do personagem
    private GameObject GetClosestHD()
    {
        GameObject closestHD = null;
        float closestDistance = float.MaxValue;

        // Itera sobre todos os HDs para encontrar o mais próximo
        foreach (GameObject hd in hdObjects)
        {
            if (hd != null)
            {
                float distance = Vector3.Distance(transform.position, hd.transform.position);
                if (distance < closestDistance)
                {
                    closestHD = hd;
                    closestDistance = distance;
                }
            }
        }

        return closestHD;
    }

    // Função para preencher a lista de HDs com os objetos filhos de ListaHD
    private void PopulateHDList()
    {
        hdObjects.Clear(); // Limpar qualquer conteúdo anterior da lista

        // Itera sobre todos os filhos do objeto "ListaHD" e adiciona à lista de HDs
        foreach (Transform child in listaHD.transform)
        {
            hdObjects.Add(child.gameObject);
        }
    }
}
