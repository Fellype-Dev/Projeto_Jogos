using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;  // Singleton instance

    [Header("UI References")]
    public TMP_InputField playerNameInput;
    public TMP_InputField lobbyCodeInput;
    public GameObject introLobby;
    public GameObject lobbyPanel;
    public TMP_Text[] playerNameText;
    public TMP_Text lobbyCodeText;

    [Header("Lobby Data")]
    public Lobby joinedLobby;  // Corrigido de "JoinnedLobby" para "joinedLobby"

    private Lobby hostLobby;
    private int maxPlayers = 4;

    private void Awake()
    {
        // Implementação do Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persiste entre cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    private async Task Authenticate()
    {
        if (AuthenticationService.Instance.IsSignedIn)
            return;

        AuthenticationService.Instance.ClearSessionToken();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("Usuário logado como: " + AuthenticationService.Instance.PlayerId);
    }

    public async void CreateLobby()
    {
        await Authenticate();

        CreateLobbyOptions options = new CreateLobbyOptions
        {
            Player = GetPlayer()
        };

        hostLobby = await Lobbies.Instance.CreateLobbyAsync("Lobby", maxPlayers, options);
        joinedLobby = hostLobby;
        Debug.Log("Lobby Created: " + hostLobby.LobbyCode);

        // Inicia o heartbeat do lobby
        InvokeRepeating(nameof(SendLobbyHeartBeat), 10, 10);

        // Atualiza a UI
        ShowPlayers();
        lobbyCodeText.text = joinedLobby.LobbyCode;
        introLobby.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public async void JoinLobbyByCode()
    {
        await Authenticate();

        JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions
        {
            Player = GetPlayer()
        };

        joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCodeInput.text, options);
        Debug.Log("Entrou no lobby: " + joinedLobby.LobbyCode);

        // Atualiza a UI
        ShowPlayers();
        lobbyCodeText.text = joinedLobby.LobbyCode;
        introLobby.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                { "name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerNameInput.text) }
            }
        };
    }

    private async void SendLobbyHeartBeat()
    {
        if (hostLobby == null)
            return;

        await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
        Debug.Log("Atualizou o lobby");
        await UpdateLobby();
        ShowPlayers();
    }

    private void ShowPlayers()
    {
        if (joinedLobby == null || playerNameText == null)
            return;

        for (int i = 0; i < joinedLobby.Players.Count && i < playerNameText.Length; i++)
        {
            if (joinedLobby.Players[i].Data.ContainsKey("name"))
            {
                playerNameText[i].text = joinedLobby.Players[i].Data["name"].Value;
            }
        }
    }

    private async Task UpdateLobby()
    {
        if (joinedLobby == null)
            return;

        joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
    }

    private void OnDestroy()
    {
        // Limpa callbacks e timers
        CancelInvoke(nameof(SendLobbyHeartBeat));
    }
}