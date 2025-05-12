using UnityEngine;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    // Gerenciamento de Caixas
    public bool hasBox = false; // Indica se o jogador está carregando uma caixa
    public GameObject currentBox; // A caixa mais próxima
    private GameObject carregadaBox; // Caixa que o jogador está carregando
    public List<GameObject> boxObjects; // Lista das caixas disponíveis no jogo

    // Gerenciamento de HDs
    public bool hasHD = false; // Indica se o jogador está carregando um HD
    public bool hdInserted = false;
    public bool transferComplete = false;
    public bool hdComDados = false;
    public GameObject currentHD; // O HD mais próximo
    private GameObject carregadoHD; // HD que o jogador está carregando
    public List<GameObject> hdObjects; // Lista dos HDs disponíveis no jogo

    // Inventário e interação geral
    public Transform inventorySlot; // Posição onde o item será colocado no jogador
    public float interactionDistance = 5f; // Distância máxima para interação
    public List<GameObject> dropZones; // Lista de alçapões (áreas de depósito)
    public List<GameObject> ListaComputadores; // Lista de computadores
    public List<GameObject> ListaServidores; // Lista de servidores
    public PlayerInteractionUI interactionUI; // Interface de interação com o jogador
    public TransferManager transferManager; // Gerenciador de transferência para HD
    public TaskHUD taskHUD; // HUD de tarefas
    public GameObject droppedHDPrefab; // Prefab do HD dropado

    private void Start()
    {
        // Inicializa listas
        boxObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Box"));
        hdObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("HD"));
        Debug.Log($"HDs encontrados: {hdObjects.Count}");
        ListaComputadores = new List<GameObject>(GameObject.FindGameObjectsWithTag("Computador"));
        ListaServidores = new List<GameObject>(GameObject.FindGameObjectsWithTag("Servidor"));
    }

    private void Update()
    {
        // Interação ao pressionar "E"
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!hasBox && currentBox != null && IsNear(currentBox))
            {
                // Coletar Caixa
                hasBox = true;
                carregadaBox = currentBox;

                carregadaBox.SetActive(false); // Desativa a caixa no mundo
                carregadaBox.transform.SetParent(inventorySlot); // Move para o inventário
                carregadaBox.transform.localPosition = Vector3.zero;
                carregadaBox.transform.localRotation = Quaternion.identity;

                boxObjects.Remove(carregadaBox); // Remove da lista de caixas disponíveis
                currentBox = null;

                Debug.Log("Caixa coletada.");
            }
            else if (hasBox && IsNear(GetNearestDropZone()))
            {
                // Depositar Caixa
                hasBox = false;

                Debug.Log("Caixa depositada com sucesso no alçapão.");

                Destroy(carregadaBox); // Destroi a caixa carregada
                carregadaBox = null;
            }
            else if (!hasHD && currentHD != null && IsNear(currentHD))
            {
                // Coletar HD
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
            else if (hasHD && IsNearComputador() && !hdInserted && !hdComDados)
            {
                // Inserir HD no Computador
                hdInserted = true;
                hasHD = false;

                if (carregadoHD != null)
                {
                    carregadoHD.SetActive(false);
                    Debug.Log("HD inserido no computador.");
                }
            }
            else if (hdInserted && transferComplete && IsNearComputador())
            {
                // Retirar HD com Dados
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
            else if (hasHD && IsNearServidor() && hdComDados)
            {
                // Inserir HD no Servidor
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

        if (Input.GetKeyDown(KeyCode.G) && hasHD && carregadoHD != null)
        {
            // Dropar HD
            hasHD = false;

            Vector3 dropPosition = transform.position + transform.forward * 1.5f + Vector3.up * 0.5f;
            GameObject droppedHD = Instantiate(droppedHDPrefab, dropPosition, Quaternion.identity);
            droppedHD.tag = "HD";

            hdObjects.Add(droppedHD);
            Destroy(carregadoHD);
            carregadoHD = null;

            Debug.Log("HD dropado.");
        }

        if (Input.GetKeyDown(KeyCode.Q) && hdInserted && !transferComplete)
        {
            // Iniciar Transferência
            transferManager.StartTransfer();
        }
    }

    private void LateUpdate()
    {
        currentBox = GetNearestBox();
        currentHD = GetNearestHD();

        if (currentBox != null && !hasBox && IsNear(currentBox))
        {
            interactionUI.MostrarTexto("[E] Pegar Caixa");
        }
        else if (hasBox && GetNearestDropZone() != null)
        {
            interactionUI.MostrarTexto("[E] Depositar Caixa");
        }
        else if (currentHD != null && !hasHD && IsNear(currentHD))
        {
            interactionUI.MostrarTexto("[E] Pegar HD");
        }
        else if (IsNearComputador() && hasHD && !hdInserted && !hdComDados)
        {
            interactionUI.MostrarTexto("[E] Inserir HD no Computador");
        }
        else if (hdInserted && !transferComplete && IsNearComputador())
        {
            interactionUI.MostrarTexto("[Q] Iniciar Transferência");
        }
        else if (hdInserted && transferComplete && IsNearComputador())
        {
            interactionUI.MostrarTexto("[E] Retirar HD com Dados");
        }
        else if (IsNearServidor() && hasHD && hdComDados)
        {
            interactionUI.MostrarTexto("[E] Inserir HD no Servidor");
        }
        else
        {
            interactionUI.EsconderTexto();
        }
    }

    private GameObject GetNearestBox()
    {
        return GetNearestObject(boxObjects);
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
                if (distance < nearestDistance && distance <= interactionDistance)
                {
                    nearestHD = hd;
                    nearestDistance = distance;
                }
            }
        }

        Debug.Log(nearestHD != null ? $"HD mais próximo: {nearestHD.name}" : "Nenhum HD próximo detectado.");
        return nearestHD;
    }

    private GameObject GetNearestDropZone()
    {
        GameObject nearestDropZone = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject dropZone in dropZones)
        {
            if (dropZone != null)
            {
                float distance = Vector3.Distance(transform.position, dropZone.transform.position);
                if (distance < nearestDistance && distance <= interactionDistance)
                {
                    nearestDropZone = dropZone;
                    nearestDistance = distance;
                }
            }
        }

        return nearestDropZone;
    }

    private GameObject GetNearestObject(List<GameObject> objects)
    {
        GameObject nearest = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < nearestDistance)
                {
                    nearest = obj;
                    nearestDistance = distance;
                }
            }
        }

        return nearest;
    }

    private bool IsNear(GameObject target)
    {
        if (target == null)
        {
            Debug.Log("O alvo para verificar proximidade é nulo.");
            return false;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);
        Debug.Log($"Distância para {target.name}: {distance}");
        return distance < interactionDistance;
    }

    private bool IsNearComputador()
    {
        return IsNearAny(ListaComputadores);
    }

    private bool IsNearServidor()
    {
        return IsNearAny(ListaServidores);
    }

    private bool IsNearAny(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null && IsNear(obj))
            {
                return true;
            }
        }
        return false;
    }
}