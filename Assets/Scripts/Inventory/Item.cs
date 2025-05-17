using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;  // Nome do item
    public string itemDescription;  // Descrição do item
    private bool playerNearby = false;  // Indica se o jogador está perto
    public PlayerInteractionUI interactionUI; // Referência à interface de interação

    void OnTriggerEnter(Collider other)
    {
        // Verifica se o jogador entrou na área de interação
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            interactionUI.MostrarTexto("[E] Pegar " + itemName);
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Verifica se o jogador está na área e pressionou a tecla E
        if (playerNearby && other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            InventoryManager inventory = other.GetComponent<InventoryManager>();

            if (inventory != null)
            {
                // Adiciona o item ao inventário
                inventory.AddItem(gameObject);
            }

            // Desativa o item após coletar
            gameObject.SetActive(false);
            interactionUI.EsconderTexto();
            playerNearby = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Verifica se o jogador saiu da área de interação
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            interactionUI.EsconderTexto();
        }
    }
}