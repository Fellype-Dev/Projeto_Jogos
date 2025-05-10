using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemSpawner : MonoBehaviour
{
    [Header("Configura��es de Spawn")]
    [Tooltip("Lista de posi��es poss�veis para spawnar os itens.")]
    public List<Transform> spawnPoints;

    [Tooltip("Prefab do item a ser spawnado.")]
    public GameObject itemPrefab;

    [Tooltip("N�mero m�nimo de itens a serem spawnados.")]
    public int minItemsToSpawn = 3;

    [Tooltip("N�mero m�ximo de itens a serem spawnados.")]
    public int maxItemsToSpawn = 8;

    private List<Transform> availableSpawnPoints;

    void Start()
    {
        // Clona a lista de pontos para evitar modifica��es na lista original
        availableSpawnPoints = new List<Transform>(spawnPoints);

        // Define um n�mero aleat�rio de itens a serem spawnados
        int numberOfItemsToSpawn = Random.Range(minItemsToSpawn, maxItemsToSpawn + 1);

        SpawnItems(numberOfItemsToSpawn);
    }

    /// <summary>
    /// Spawna os itens aleatoriamente nos pontos dispon�veis.
    /// </summary>
    /// <param name="numberOfItems">N�mero de itens a serem spawnados.</param>
    private void SpawnItems(int numberOfItems)
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogWarning("N�o h� pontos de spawn suficientes para todos os itens.");
                break;
            }

            // Escolhe um ponto de spawn aleat�rio
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[randomIndex];

            // Instancia o item no ponto escolhido
            Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);

            // Remove o ponto da lista para evitar spawns duplicados
            availableSpawnPoints.RemoveAt(randomIndex);
        }
    }
}