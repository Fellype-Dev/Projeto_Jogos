using System.Collections;
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

    public TMP_InputField playerNameInput, lobbyCodeInput;
    public GameObject introLobby, lobbyPanel;
    public TMP_Text[] playerNameText;
    public TMP_Text lobbyCodeText;
    Lobby hostLobby, JoinnedLobby;
    int maxPlayers = 4;

    async void Start()
    {
        await UnityServices.InitializeAsync();
    }

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

        CreateLobbyOptions options = new CreateLobbyOptions{
            Player = GetPlayer()
        };

        hostLobby = await Lobbies.Instance.CreateLobbyAsync("Lobby", maxPlayers, options);
        JoinnedLobby = hostLobby;
        Debug.Log("Lobby Created: " + hostLobby.LobbyCode);
        InvokeRepeating("SendLobbyHeartBeat", 10, 10);
        ShowPlayers();
        lobbyCodeText.text = JoinnedLobby.LobbyCode;
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

        JoinnedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCodeInput.text, options);
        Debug.Log("Entrou no lobby: " + JoinnedLobby.LobbyCode);
        ShowPlayers();
        lobbyCodeText.text = JoinnedLobby.LobbyCode;
        introLobby.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    Player GetPlayer()
    {

        Player player = new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                { "name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerNameInput.text) }
            }
        };
        return player;
    }


    async void SendLobbyHeartBeat()
    {
        if (hostLobby == null)
            return;

        await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
        Debug.Log("Atualizou o lobby");
        UpdateLobby();
        ShowPlayers();
    }

    void ShowPlayers()
    {

        for (int i = 0; i < JoinnedLobby.Players.Count; i++)
        {
            playerNameText[i].text = JoinnedLobby.Players[i].Data["name"].Value;
        }

    }

    async void UpdateLobby()
    {
        if (JoinnedLobby == null)
            return;

        JoinnedLobby = await LobbyService.Instance.GetLobbyAsync(JoinnedLobby.Id);
    }

}