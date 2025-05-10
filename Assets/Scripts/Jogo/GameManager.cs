using Unity.Netcode;
using UnityEngine;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.Core;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [Header("Player Settings")]
     public string gameSceneName = "CenaDoKekis"; // Mesmo nome usado no LobbyManager
    [SerializeField] private NetworkObject playerPrefab;
    [SerializeField] private float spawnYOffset = 0.5f;

    private Lobby currentLobby;
    private bool hasSpawnedPlayers;

    private void Awake()
    {
        Debug.Log("[GameManager] Awake chamado");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[GameManager] Instância criada e marcada para não ser destruída entre cenas");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("[GameManager] Instância duplicada destruída");
        }
    }

    private async void Start()
    {
        Debug.Log("[GameManager] Start chamado");
        await InitializeServices();
        InitializeNetworkCallbacks();
    }

    private async Task InitializeServices()
    {
        Debug.Log("[GameManager] Inicializando serviços...");
#if UNITY_EDITOR
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("[GameManager] Serviços Unity inicializados e jogador autenticado");
        }
#endif
    }

    private void InitializeNetworkCallbacks()
{
    Debug.Log("[GameManager] Inicializando callbacks de rede...");
    
    if (NetworkManager.Singleton != null)
    {
        Debug.Log("[GameManager] NetworkManager.Singleton encontrado");

        // Verificando se o SceneManager está presente
        if (NetworkManager.Singleton.SceneManager != null)
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete += OnSceneLoaded;
            Debug.Log("[GameManager] OnLoadComplete callback registrado");
        }
        else
        {
            Debug.LogError("[GameManager] SceneManager não encontrado no NetworkManager");
        }

        // Verificando se o OnClientConnectedCallback está presente
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        Debug.Log("[GameManager] OnClientConnectedCallback registrado");
    }
    else
    {
        Debug.LogError("[GameManager] NetworkManager.Singleton é nulo!");
    }
}

    private void OnSceneLoaded(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
{
    Debug.Log($"Scene loaded: {sceneName} by client {clientId}");
    
    // Apenas o servidor deve spawnar jogadores
    if (IsServer && sceneName == gameSceneName && !hasSpawnedPlayers)
    {
        Debug.Log("Server initializing game...");
        InitializeGame();
    }
}

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"[Network] Cliente conectado: {clientId}");
        
        // Para evitar spawn duplicado do host
        if (clientId == NetworkManager.Singleton.LocalClientId && IsServer)
        {
            Debug.Log("[Network] Ignorando spawn, pois é o host");
            return;
        }

        if (IsServer && hasSpawnedPlayers)
        {
            Debug.Log($"[Network] Spawning late joiner: {clientId}");
            SpawnPlayerForClient(clientId);
        }
    }

    private async void InitializeGame()
    {
        Debug.Log("[GameManager] Inicializando o jogo...");
        if (!IsServer || hasSpawnedPlayers)
        {
            Debug.Log("[GameManager] Jogo já inicializado ou não é o servidor");
            return;
        }

        Invoke(nameof(DelayedSpawn), 0.5f);

        try
        {
            if (LobbyManager.Instance != null && LobbyManager.Instance.joinedLobby != null)
            {
                currentLobby = await Lobbies.Instance.GetLobbyAsync(LobbyManager.Instance.joinedLobby.Id);
                Debug.Log("[GameManager] Lobby carregado com sucesso");
                SpawnAllPlayers();
                hasSpawnedPlayers = true;
            }
            else
            {
                Debug.LogError("[GameManager] Instância do LobbyManager ou lobby não encontrado");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[GameManager] Erro ao inicializar o jogo: {e}");
        }
    }

    private void DelayedSpawn()
    {
        Debug.Log("[GameManager] DelayedSpawn chamado");
        if (!IsServer) return;

        try
        {
            if (LobbyManager.Instance?.joinedLobby != null)
            {
                SpawnAllPlayers();
                hasSpawnedPlayers = true;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[GameManager] Erro ao realizar spawn: {e}");
        }
    }

    private void SpawnAllPlayers()
    {
        Debug.Log("[GameManager] Spawning todos os jogadores...");
        if (!IsServer) return;

        int spawnIndex = 0;
        foreach (var player in currentLobby.Players)
        {
            Debug.Log($"[GameManager] Spawning player {player.Id} (Index: {spawnIndex})");
            SpawnPlayer(player.Id, spawnIndex);
            spawnIndex++;
        }
    }

    private void SpawnPlayerForClient(ulong clientId)
    {
        Debug.Log($"[GameManager] Spawn late joiner {clientId}");
        if (!IsServer) return;

        int nextSpawnIndex = NetworkManager.Singleton.ConnectedClients.Count - 1;
        SpawnPlayer(AuthenticationService.Instance.PlayerId, nextSpawnIndex);
    }

    private void SpawnPlayer(string playerId, int spawnIndex)
    {
        Debug.Log($"[GameManager] Spawning player {playerId} at spawnIndex {spawnIndex}");
        Vector3 spawnPosition = GetSpawnPosition(spawnIndex);
        Quaternion spawnRotation = Quaternion.identity;

        NetworkObject playerInstance = Instantiate(
            playerPrefab,
            spawnPosition,
            spawnRotation
        );

        SetupPlayerNetworkObject(playerInstance, playerId);
    }

    private Vector3 GetSpawnPosition(int spawnIndex)
    {
        Debug.Log("[GameManager] Obtendo posição de spawn...");
        if (SpawnManager.Instance != null && spawnIndex < SpawnManager.Instance.spawnPoints.Length)
        {
            Debug.Log("[GameManager] Posição de spawn encontrada no SpawnManager");
            return SpawnManager.Instance.spawnPoints[spawnIndex].position;
        }

        // Fallback spawn logic
        float xPos = spawnIndex % 2 == 0 ? -2f : 2f;
        float zPos = spawnIndex * 2f;
        Debug.Log("[GameManager] Posição de spawn fallback: " + new Vector3(xPos, spawnYOffset, zPos));
        return new Vector3(xPos, spawnYOffset, zPos);
    }

    private void SetupPlayerNetworkObject(NetworkObject playerInstance, string playerId)
    {
        Debug.Log($"[GameManager] Configurando NetworkObject do player {playerId}");
        if (playerId == AuthenticationService.Instance.PlayerId)
        {
            Debug.Log("[GameManager] Definindo a posse do jogador como local");
            playerInstance.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId, true);
        }
        else
        {
            Debug.Log("[GameManager] Procurando cliente correspondente...");
            // Tenta encontrar o cliente correspondente
            foreach (var client in NetworkManager.Singleton.ConnectedClients)
            {
                if (client.Value.PlayerObject != null &&
                    client.Value.PlayerObject.GetComponent<PlayerNetworkData>()?.PlayerId == playerId)
                {
                    playerInstance.SpawnWithOwnership(client.Key, true);
                    Debug.Log("[GameManager] Encontrado e configurado o cliente correspondente");
                    return;
                }
            }

            // Spawn padrão se nenhum cliente correspondente for encontrado
            playerInstance.Spawn();
            Debug.Log("[GameManager] Spawn padrão do jogador sem cliente correspondente");
        }

        // Inicializa dados do jogador
        var playerData = playerInstance.GetComponent<PlayerNetworkData>();
        if (playerData != null)
        {
            Debug.Log("[GameManager] Inicializando dados do jogador");
            playerData.Initialize(playerId);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("[GameManager] OnDestroy chamado");
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnSceneLoaded;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }
}
