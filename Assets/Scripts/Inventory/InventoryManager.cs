using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> inventory = new List<GameObject>();  // Lista do inventário com objetos
    public int maxItems = 2;  // Limite de itens no inventário
    public Transform itemDropPosition;  // Posição onde o item será droppado

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))  // Tecla E para pegar o item
        {
            TryPickupItem();
        }

        if (Input.GetKeyDown(KeyCode.G))  // Tecla G para dropar o item
        {
            TryDropItem();
        }
    }

    // Método para pegar o item
    void TryPickupItem()
    {
        // Tenta pegar o item mais próximo (não há interação com o mundo, é baseado na distância do jogador)
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2f))
        {
            Item item = hit.collider.GetComponent<Item>();
            if (item != null)
            {
                AddItem(item.gameObject);
            }
        }
    }

    // Método para dropar o item
    void TryDropItem()
    {
        if (inventory.Count > 0)  // Verifica se há algum item no inventário
        {
            // Dropar o último item do inventário
            GameObject itemToDrop = inventory[inventory.Count - 1];
            inventory.RemoveAt(inventory.Count - 1);
            
            // Posiciona o item na posição do drop (pode ser ajustada conforme necessário)
            itemToDrop.transform.position = itemDropPosition.position;

            // Ativa o item no mundo novamente (caso tenha sido desativado ao ser adicionado ao inventário)
            itemToDrop.SetActive(true);
            Debug.Log("Item droppado: " + itemToDrop.name);
        }
        else
        {
            Debug.Log("Inventário vazio! Não há nada para dropar.");
        }
    }

    // Método para adicionar item ao inventário
    public void AddItem(GameObject item)
    {
        if (inventory.Count < maxItems)  // Verifica se há espaço no inventário
        {
            inventory.Add(item);
            item.SetActive(false);  // Desativa o item no mundo quando ele é adicionado ao inventário
            Debug.Log(item.name + " adicionado ao inventário!");
        }
        else
        {
            Debug.Log("Inventário cheio! Não é possível adicionar mais itens.");
        }
    }

    // Exibir itens do inventário (para debug)
    public void ShowInventory()
    {
        foreach (GameObject item in inventory)
        {
            Debug.Log("Item no Inventário: " + item.name + " | Tag: " + item.tag);
        }
    }
}
