using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class NetworkHandler : MonoBehaviour, INetworkRunnerCallbacks
{

    private Action<NetworkRunner, PlayerRef> playerJoinCallback, playerLeftCallback;
    public void AddPlayerConnectionCallbacks(Action<NetworkRunner, PlayerRef> OnPlayerJoin, Action<NetworkRunner, PlayerRef> OnPlayerLeft)
    {
        playerJoinCallback += OnPlayerJoin;
        playerLeftCallback += OnPlayerLeft;
    }
    public void RemovePlayerConnectionCallbacks(Action<NetworkRunner, PlayerRef> OnPlayerJoin, Action<NetworkRunner, PlayerRef> OnPlayerLeft)
    {
        playerJoinCallback -= OnPlayerJoin;
        playerLeftCallback -= OnPlayerLeft;
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            playerJoinCallback?.Invoke(runner, player);
            /* var characterClone = runner.Spawn(character, Vector3.zero, Quaternion.identity, player);
            characters.Add(player, characterClone);//TODO: characterSpawner script will spawn after game starts. Not here! */
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            playerLeftCallback?.Invoke(runner, player);
   
        }
    }
    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnDisconnectedFromServer(NetworkRunner runner) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }


    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
}
