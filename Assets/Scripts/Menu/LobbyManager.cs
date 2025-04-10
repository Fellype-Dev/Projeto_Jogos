using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Netcode.Transports.UTP;

public class LobbyManager : MonoBehaviour
{
    // Singleton pattern para acesso global
    public static LobbyManager Instance;

    // Variáveis existentes (UI)
    public TMP_InputField playerNameInput, lobbyCodeInput;
    public GameObject introLobby, lobbyPanel;
    public TMP_Text[] playerNameText;
    public TMP_Text lobbyCodeText;
    
    // Novas variáveis para Netcode
    public GameObject playButton; // Botão "Play" (visível apenas para o host)
    public string gameSceneName = "GameScene"; // Nome da cena do jogo

    // Variáveis existentes (Lobby)
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private int maxPlayers = 4;

    private void Awake()
    {
        // Implementação do Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método Start existente
    async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    // --- Métodos Existente (Mantidos sem alterações) ---
    async Task Authenticate()
    {
        if (AuthenticationService.Instance.IsSignedIn)
            return;

        AuthenticationService.Instance.ClearSessionToken();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("Usuário logado como: " + AuthenticationService.Instance.PlayerId);
    }

    async public void CreateLobby()
    {
        await Authenticate();

        CreateLobbyOptions options = new CreateLobbyOptions
        {
            Player = GetPlayer()
        };

        hostLobby = await Lobbies.Instance.CreateLobbyAsync("Lobby", maxPlayers, options);
        joinedLobby = hostLobby;
        Debug.Log("Lobby Created: " + hostLobby.LobbyCode);
        
        InvokeRepeating("UpdateLobby", 1.1f, 3f); // Atualização mais frequente
        ShowPlayers();
        
        lobbyCodeText.text = joinedLobby.LobbyCode;
        introLobby.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    async public void JoinLobbyByCode()
    {
        await Authenticate();

        JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions
        {
            Player = GetPlayer()
        };

        joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCodeInput.text, options);
        Debug.Log("Entrou no lobby: " + joinedLobby.LobbyCode);
        
        InvokeRepeating("UpdateLobby", 1.1f, 3f);
        ShowPlayers();
        
        lobbyCodeText.text = joinedLobby.LobbyCode;
        introLobby.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                { "name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerNameInput.text) }
            }
        };
    }

    void ShowPlayers()
    {
        for (int i = 0; i < joinedLobby.Players.Count; i++)
        {
            playerNameText[i].text = joinedLobby.Players[i].Data["name"].Value;
        }

        // Ativa/desativa o botão "Play" apenas para o host
        playButton.SetActive(IsHost());
    }

    // --- Novos Métodos para Netcode ---
    private bool IsHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    async public void StartGame()
    {
        if (!IsHost()) return;

        try
        {
            // Marca o lobby como "jogo iniciado"
            var options = new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {"gameStarted", new DataObject(DataObject.VisibilityOptions.Member, "true")},
                    {"hostAddress", new DataObject(DataObject.VisibilityOptions.Member, "127.0.0.1")} // Substitua por IP se for multiplayer remoto
                }
            };
            
            await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, options);

            // Inicia o host e carrega a cena do jogo
            NetworkManager.Singleton.SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
            NetworkManager.Singleton.StartHost();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Erro ao iniciar o jogo: " + e.Message);
        }
    }

    async void UpdateLobby()
    {
        if (joinedLobby == null) return;

        try
        {
            joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
            ShowPlayers();

            // Se o jogo começou e este jogador não é o host, conecta-se
            if (joinedLobby.Data != null && 
                joinedLobby.Data.ContainsKey("gameStarted") && 
                joinedLobby.Data["gameStarted"].Value == "true" &&
                !IsHost())
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                    joinedLobby.Data["hostAddress"].Value, 7777);
                NetworkManager.Singleton.StartClient();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Erro ao atualizar lobby: " + e.Message);
        }
    }
}