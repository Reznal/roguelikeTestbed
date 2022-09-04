using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string LobbyName = "game_lobby";
    private const int MaxPlayers = 8;
    
    private string _lobbyId;

    private RelayHostData _hostData;
    private RelayJoinData _joinData;
    
    public async void Start()
    {
        //Initialise unit services
        await UnityServices.InitializeAsync();
        
        // Setup events listener
        SetupEvents();
        
        // Unity login
        await SignInAnonymouslyAsync();
    }

    #region UnityLogin
    private void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
        };

        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.Log(err);
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Player signed out");
        };
    }

    private async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anon succeeded!");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    #endregion

    #region Lobby

    public async void FindMatch()
    {
        Debug.Log("Looking for a lobby....");

        try
        {
            QuickJoinLobbyOptions options = new QuickJoinLobbyOptions();
            Lobby lobby = await Lobbies.Instance.QuickJoinLobbyAsync(options);
            
            Debug.Log($"Joined Lobby: {lobby.Id}");
            Debug.Log($"Lobby Players: {lobby.Players.Count}");

            string joinCode = lobby.Data["joinCode"].Value;
            
            Debug.Log($"Received code: {joinCode}");

            JoinAllocation allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
            
            // Create objects
            _joinData = new RelayJoinData
            {
                Key = allocation.Key,
                Port = (ushort) allocation.RelayServer.Port,
                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                ConnectionData = allocation.ConnectionData,
                HostConnectionData = allocation.HostConnectionData,
                IpAddress = allocation.RelayServer.IpV4
            };
            
            //Set transport data
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                _joinData.IpAddress,
                _joinData.Port,
                _joinData.AllocationIDBytes,
                _joinData.Key,
                _joinData.ConnectionData,
                _joinData.HostConnectionData);
            
            //Start client
            NetworkManager.Singleton.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("Cannot find a lobby.");
            Debug.LogException(e);
            CreateMatch();
        }
    }

    private async void CreateMatch()
    {
        Debug.Log("Creating lobby...");

        // Eternal connections - may be 1 less than max players??
        const int maxConnections = 1;
        
        try
        {
            //Create relay object
            Allocation allocation = await Relay.Instance.CreateAllocationAsync(maxConnections);
            _hostData = new RelayHostData
            {
                Key = allocation.Key,
                Port = (ushort)allocation.RelayServer.Port,
                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                ConnectionData = allocation.ConnectionData,
                IpAddress = allocation.RelayServer.IpV4,
                // Retrieve join code
                JoinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId)
            };

            // Put join code in lobby data
            CreateLobbyOptions options = new CreateLobbyOptions
            {
                IsPrivate = false,
                Data = new Dictionary<string, DataObject>()
                {
                    { "joinCode", new DataObject(visibility: DataObject.VisibilityOptions.Member, value: _hostData.JoinCode) }
                }
            };

            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(LobbyName, MaxPlayers, options);
            _lobbyId = lobby.Id;
            
            Debug.Log($"Created lobby: {lobby.Id}");

            // Heartbeat to lobby every 15 seconds
            StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));
            
            // Set transports data
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                _hostData.IpAddress,
                _hostData.Port,
                _hostData.AllocationIDBytes,
                _hostData.Key,
                _hostData.ConnectionData);
            
            // Start host
            NetworkManager.Singleton.StartHost();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log($"Error creating lobby: {e}");
            throw;
        }
    }

    private static IEnumerator HeartbeatLobbyCoroutine(string lobbyId, int waitForSeconds)
    {
        WaitForSecondsRealtime delay = new WaitForSecondsRealtime(waitForSeconds);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            Debug.Log("Lobby heartbeat");
            yield return delay;
        }
        // ReSharper disable once IteratorNeverReturns
    }

    #endregion

    private void OnDestroy()
    {
        Lobbies.Instance.DeleteLobbyAsync(_lobbyId);
    }
}

public struct RelayHostData
{
    public string JoinCode;
    public string IpAddress;
    public ushort Port;
    public Guid AllocationID;
    public byte[] AllocationIDBytes;
    public byte[] ConnectionData;
    public byte[] Key;
}

public struct RelayJoinData
{
    public string JoinCode;
    public string IpAddress;
    public ushort Port;
    public Guid AllocationID;
    public byte[] AllocationIDBytes;
    public byte[] ConnectionData;
    public byte[] HostConnectionData;
    public byte[] Key;
}
