using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;  // Nome do item
    public string itemDescription;  // Descrição do item

    void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que colidiu com o item é o jogador
        if (other.CompareTag("Player"))
        {
            InventoryManager inventory = other.GetComponent<InventoryManager>();

            if (inventory != null)
            {
                // Adiciona o item ao inventário
                inventory.AddItem(gameObject);  // Passa o próprio objeto do item
            }

            // Desativa o item para evitar pegar múltiplas vezes
            gameObject.SetActive(false);
        }
    }
}
