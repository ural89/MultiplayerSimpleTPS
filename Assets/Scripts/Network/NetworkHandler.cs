using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class NetworkHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    #region Callbacks
    public Action<NetworkRunner, PlayerRef> PlayerJoined, PlayerLeft;
    public Action<NetworkRunner, Fusion.NetworkInput> Input;
    public Action<NetworkRunner, PlayerRef, Fusion.NetworkInput> InputMissing;
    public Action<NetworkRunner, ShutdownReason> Shutdown;
    public Action<NetworkRunner> ConnectedToServer;
    public Action<NetworkRunner> DisconnectedFromServer;
    public Action<NetworkRunner, NetworkRunnerCallbackArgs.ConnectRequest, byte[]> ConnectRequest;
    public Action<NetworkRunner, NetAddress, NetConnectFailedReason> ConnectFailed;
    public Action<NetworkRunner, SimulationMessagePtr> UserSimulationMessage;
    public Action<NetworkRunner, List<SessionInfo>> SessionListUpdated;
    public Action<NetworkRunner, Dictionary<string, object>> CustomAuthenticationResponse;
    public Action<NetworkRunner, HostMigrationToken> HostMigration;
    public Action<NetworkRunner, PlayerRef, ArraySegment<byte>> ReliableDataReceived;
    public Action<NetworkRunner> SceneLoadDone;
    public Action<NetworkRunner> SceneLoadStart;
    #endregion
    [SerializeField] private int lobbySceneIndex = 1;
    public static List<PlayerRef> ActivePlayersInServer = null;
    public static Dictionary<PlayerRef, string> ActivePlayerNames = null;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        ActivePlayersInServer = new();
        ActivePlayerNames = new();

    }

    public void SetNameForPlayer(PlayerRef player, string name)
    {
        if(ActivePlayerNames.ContainsKey(player))
        {
            ActivePlayerNames[player] = name;
        }
        else
        {
            ActivePlayerNames.Add(player, name);
        }
    }

    public void AddPlayerConnectionCallbacks(Action<NetworkRunner, PlayerRef> OnPlayerJoin, Action<NetworkRunner, PlayerRef> OnPlayerLeft)
    {
        PlayerJoined += OnPlayerJoin;
        PlayerLeft += OnPlayerLeft;
    }
    public void RemovePlayerConnectionCallbacks(Action<NetworkRunner, PlayerRef> OnPlayerJoin, Action<NetworkRunner, PlayerRef> OnPlayerLeft)
    {
        PlayerJoined -= OnPlayerJoin;
        PlayerLeft -= OnPlayerLeft;
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {

            PlayerJoined?.Invoke(runner, player);
            ActivePlayersInServer.Add(player);
        }

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {

            PlayerLeft?.Invoke(runner, player);
            ActivePlayersInServer.Remove(player);
            if(ActivePlayersInServer.Count == 0)
            {
                runner.SetActiveScene(lobbySceneIndex);
            }
        }

    }
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, Fusion.NetworkInput input)
    {

        if (Input != null)
        {
            Input(runner, input);
        }
    }
    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, Fusion.NetworkInput input) { if (InputMissing != null) { InputMissing(runner, player, input); } }
    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { if (Shutdown != null) { Shutdown(runner, shutdownReason); } }
    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) { if (ConnectedToServer != null) { ConnectedToServer(runner); } }
    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner) { if (DisconnectedFromServer != null) { DisconnectedFromServer(runner); } }
    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { if (ConnectRequest != null) { ConnectRequest(runner, request, token); } }
    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { if (ConnectFailed != null) { ConnectFailed(runner, remoteAddress, reason); } }
    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { if (UserSimulationMessage != null) { UserSimulationMessage(runner, message); } }
    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { if (SessionListUpdated != null) { SessionListUpdated(runner, sessionList); } }
    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { if (CustomAuthenticationResponse != null) { CustomAuthenticationResponse(runner, data); } }
    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { if (HostMigration != null) { HostMigration(runner, hostMigrationToken); } }
    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { if (ReliableDataReceived != null) { ReliableDataReceived(runner, player, data); } }
    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner) { if (SceneLoadDone != null) { SceneLoadDone(runner); } }
    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner) { if (SceneLoadStart != null) { SceneLoadStart(runner); } }
}
