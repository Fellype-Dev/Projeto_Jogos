using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemSpawner : MonoBehaviour
{
    [Header("Configurações de Spawn")]
    [Tooltip("Lista de posições possíveis para spawnar os itens.")]
    public List<Transform> spawnPoints;

    [Tooltip("Prefab do item a ser spawnado.")]
    public GameObject itemPrefab;

    [Tooltip("Número mínimo de itens a serem spawnados.")]
    public int minItemsToSpawn = 3;

    [Tooltip("Número máximo de itens a serem spawnados.")]
    public int maxItemsToSpawn = 8;

    private List<Transform> availableSpawnPoints;

    void Start()
    {
        // Clona a lista de pontos para evitar modificações na lista original
        availableSpawnPoints = new List<Transform>(spawnPoints);

        // Define um número aleatório de itens a serem spawnados
        int numberOfItemsToSpawn = Random.Range(minItemsToSpawn, maxItemsToSpawn + 1);

        SpawnItems(numberOfItemsToSpawn);
    }

    /// <summary>
    /// Spawna os itens aleatoriamente nos pontos disponíveis.
    /// </summary>
    /// <param name="numberOfItems">Número de itens a serem spawnados.</param>
    private void SpawnItems(int numberOfItems)
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogWarning("Não há pontos de spawn suficientes para todos os itens.");
                break;
            }

            // Escolhe um ponto de spawn aleatório
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[randomIndex];

            // Instancia o item no ponto escolhido
            Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);

            // Remove o ponto da lista para evitar spawns duplicados
            availableSpawnPoints.RemoveAt(randomIndex);
        }
    }
}