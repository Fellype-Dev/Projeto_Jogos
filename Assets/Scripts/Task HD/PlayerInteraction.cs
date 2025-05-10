using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    public bool hasHD = false;
    public bool hdInserted = false;
    public bool transferComplete = false;
    public bool hdComDados = false;

    public GameObject computer;
    public GameObject server;
    public TransferManager transferManager;
    public PlayerInteractionUI interactionUI;
    public TaskHUD taskHUD;

    public GameObject droppedHDPrefab; // Prefab do HD que será instanciado ao dropar
    public Transform inventorySlot; // Posição onde o HD aparece no inventário

    public List<GameObject> hdObjects;
    private GameObject currentHD;      // HD mais próximo
    private GameObject carregadoHD;    // HD que o jogador está carregando

    private void Start()
    {
        hdObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("HD"));
    }

    private void Update()
    {
        // PEGAR OU INTERAGIR COM HD (E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!hasHD && currentHD != null && IsNear(currentHD))
            {
                hasHD = true;
                carregadoHD = currentHD;

                carregadoHD.SetActive(false);
                carregadoHD.transform.SetParent(inventorySlot);
                carregadoHD.transform.localPosition = Vector3.zero;
                carregadoHD.transform.localRotation = Quaternion.identity;

                hdObjects.Remove(carregadoHD);
                currentHD = null;

                Debug.Log("HD coletado.");
            }
            else if (hasHD && IsNear(computer) && !hdInserted && !hdComDados)
            {
                hdInserted = true;
                hasHD = false;

                carregadoHD.SetActive(false);
                Debug.Log("HD inserido no computador.");
            }
            else if (hdInserted && transferComplete && IsNear(computer))
            {
                hdInserted = false;
                hasHD = true;
                transferComplete = false;
                hdComDados = true;

                carregadoHD.SetActive(true);
                carregadoHD.transform.SetParent(inventorySlot);
                carregadoHD.transform.localPosition = Vector3.zero;
                carregadoHD.transform.localRotation = Quaternion.identity;

                Debug.Log("HD retirado com dados.");
            }
            else if (hasHD && IsNear(server) && hdComDados)
            {
                hasHD = false;
                hdComDados = false;

                if (taskHUD != null)
                    taskHUD.CompleteTask();
                else
                    Debug.LogError("taskHUD não foi configurado corretamente!");

                Debug.Log("Tarefa concluída com sucesso!");

                if (carregadoHD != null)
                {
                    Destroy(carregadoHD);
                    carregadoHD = null;
                }
            }
        }

        // DROPAR (G)
        if (Input.GetKeyDown(KeyCode.G) && hasHD && carregadoHD != null)
        {
            hasHD = false;

            Vector3 dropPosition = transform.position + transform.forward * 1.5f + Vector3.up * 0.5f;
            GameObject droppedHD = Instantiate(droppedHDPrefab, dropPosition, Quaternion.identity);
            droppedHD.tag = "HD";

            hdObjects.Add(droppedHD);
            Destroy(carregadoHD);
            carregadoHD = null;

            Debug.Log("HD dropado.");
        }

        // TRANSFERÊNCIA (Q)
        if (Input.GetKeyDown(KeyCode.Q) && hdInserted && !transferComplete)
        {
            transferManager.StartTransfer();
        }
    }

    private void LateUpdate()
    {
        currentHD = GetNearestHD();

        if (currentHD != null && !hasHD && IsNear(currentHD))
        {
            interactionUI.MostrarTexto("[E] Pegar HD");
        }
        else if (IsNear(computer))
        {
            if (hasHD && !hdInserted && !hdComDados)
                interactionUI.MostrarTexto("[E] Inserir HD no Computador");
            else if (hdInserted && !transferComplete)
                interactionUI.MostrarTexto("[Q] Iniciar Transferência");
            else if (hdInserted && transferComplete)
                interactionUI.MostrarTexto("[E] Retirar HD com Dados");
            else
                interactionUI.EsconderTexto();
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
        return Vector3.Distance(transform.position, target.transform.position) < 3f;
    }

    private GameObject GetNearestHD()
    {
        GameObject nearestHD = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject hd in hdObjects)
        {
            if (hd != null)
            {
                float distance = Vector3.Distance(transform.position, hd.transform.position);
                if (distance < nearestDistance)
                {
                    nearestHD = hd;
                    nearestDistance = distance;
                }
            }
        }

        return nearestHD;
    }
}
