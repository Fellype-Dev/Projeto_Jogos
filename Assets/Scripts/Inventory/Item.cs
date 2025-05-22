using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;  // Nome do item
    public string itemDescription;  // Descrição do item
    private bool playerNearby = false;  // Indica se o jogador está perto
    public PlayerInteractionUI interactionUI; // Referência à interface de interação

    private void Start()
    {
        if (interactionUI == null)
        {
            interactionUI = FindObjectOfType<PlayerInteractionUI>();
            if (interactionUI == null)
            {
                Debug.LogError("PlayerInteractionUI não encontrado na cena!");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            interactionUI?.MostrarTexto("[E] Pegar " + itemName);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (playerNearby && other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            InventoryManager inventory = other.GetComponent<InventoryManager>();

            if (inventory != null)
            {
                inventory.AddItem(gameObject);
            }

            gameObject.SetActive(false);
            interactionUI?.EsconderTexto();
            playerNearby = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            interactionUI?.EsconderTexto();
        }
    }
}
