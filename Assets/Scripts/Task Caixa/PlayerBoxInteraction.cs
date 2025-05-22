using UnityEngine;
using System.Collections.Generic;

public class PlayerBoxInteraction : MonoBehaviour
{
    public bool hasBox = false; // Indica se o jogador est� carregando uma caixa
    public Transform inventorySlot; // Posi��o onde a caixa ser� colocada no jogador
    public GameObject currentBox; // A caixa mais pr�xima
    private GameObject carregadaBox; // Caixa que o jogador est� carregando

    public float interactionDistance = 5f; // Dist�ncia m�xima para intera��o
    public List<GameObject> dropZones; // Lista de al�ap�es (�reas de dep�sito)
    public List<GameObject> boxObjects; // Lista das caixas dispon�veis no jogo
    public List<GameObject> ListaComputadores; // Lista de computadores
    public List<GameObject> ListaServidores; // Lista de servidores

    public PlayerInteractionUI interactionUI; // Interface de intera��o com o jogador

    private void Start()
    {
        // Inicializa a lista de caixas com objetos que possuem a tag "Box"
        boxObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Box"));
    }

    private void Update()
    {
        // Intera��o ao pressionar a tecla "E"
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Pegar uma caixa
            if (!hasBox && currentBox != null && IsNear(currentBox))
            {
                hasBox = true;
                carregadaBox = currentBox;

                carregadaBox.SetActive(false); // Desativa a caixa no mundo
                carregadaBox.transform.SetParent(inventorySlot); // Move para o invent�rio
                carregadaBox.transform.localPosition = Vector3.zero;
                carregadaBox.transform.localRotation = Quaternion.identity;

                boxObjects.Remove(carregadaBox); // Remove da lista de caixas dispon�veis
                currentBox = null;

                Debug.Log("Caixa coletada.");
            }
            // Depositar a caixa em um al�ap�o
            else if (hasBox && GetNearestDropZone() != null)
            {
                hasBox = false;

                GameObject nearestDropZone = GetNearestDropZone();
                Debug.Log("Caixa depositada com sucesso no al�ap�o: " + nearestDropZone.name);

                Destroy(carregadaBox); // Destroi a caixa carregada
                carregadaBox = null;
            }
        }
    }

    private void LateUpdate()
    {
        // Atualiza a refer�ncia para a caixa mais pr�xima
        currentBox = GetNearestBox();

        // Exibir mensagem de intera��o
        if (currentBox != null && !hasBox && IsNear(currentBox))
        {
            interactionUI.MostrarTexto("[E] Pegar Caixa");
        }
        else if (hasBox && GetNearestDropZone() != null)
        {
            interactionUI.MostrarTexto("[E] Depositar Caixa");
        }
        else
        {
            interactionUI.EsconderTexto();
        }
    }

    // Retorna a caixa mais pr�xima do jogador
    private GameObject GetNearestBox()
    {
        GameObject nearestBox = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject box in boxObjects)
        {
            if (box != null)
            {
                float distance = Vector3.Distance(transform.position, box.transform.position);
                if (distance < nearestDistance)
                {
                    nearestBox = box;
                    nearestDistance = distance;
                }
            }
        }

        return nearestBox;
    }

    // Retorna o al�ap�o mais pr�ximo do jogador
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


    // Verifica se o jogador est� perto de um objeto
    private bool IsNear(GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position) < interactionDistance;
    }
}