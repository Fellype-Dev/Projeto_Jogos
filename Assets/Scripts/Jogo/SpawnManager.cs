using Unity.Netcode;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    public static SpawnManager Instance;
    
    [Header("Configuração")]
    public Transform[] spawnPoints;  // Pontos de spawn no mapa
    
    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Chamado pelo GameManager quando um jogador entra
    public Vector3 GetSpawnPosition(int playerIndex)
{
    if (spawnPoints == null || spawnPoints.Length == 0)
    {
        Debug.LogError("Nenhum ponto de spawn configurado!");
        return Vector3.zero;
    }

    Vector3 pos = spawnPoints[playerIndex % spawnPoints.Length].position;
    Debug.Log($"[SpawnManager] Posição de spawn retornada: {pos}");
    return pos;
}
}