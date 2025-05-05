using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public List<GameObject> itens = new List<GameObject>();  // Lista de itens no inventário
    public int capacidadeMaxima = 2;  // Quantidade máxima de itens que o jogador pode carregar

    // Verifica se o jogador tem espaço no inventário
    public bool TemEspaco()
    {
        return itens.Count < capacidadeMaxima;
    }

    // Adiciona um item ao inventário
    public void AdicionarItem(GameObject item)
    {
        if (TemEspaco())
        {
            itens.Add(item);
        }
        else
        {
            Debug.Log("Inventário cheio.");
        }
    }

    // Remove um item do inventário
    public void RemoverItem(GameObject item)
    {
        if (itens.Contains(item))
        {
            itens.Remove(item);
        }
    }
}
