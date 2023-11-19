using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    private List<PlayerRef> playersAlive = new();
    private List<PlayerRef> activePlayersInServer = new();
    public static GameManager Instance = null;
    private PlayerRef debugDead;
    private void Awake()
    {
        Instance = this;
    }
    public void OnPlayerJoin(PlayerRef player)
    {
        activePlayersInServer.Add(player);
    }
    public void OnPlayerLeft(PlayerRef player)
    {
        activePlayersInServer.Remove(player);
    }
    public void OnPlayerSpawn(PlayerRef playerSpawned)
    {
        if (!Object.HasStateAuthority) return;

        playersAlive.Add(playerSpawned);
        Debug.Log("Player spawned " + playerSpawned);
    }
    public void OnPlayerDie(PlayerRef deadPlayer)
    {
        if (Object != null)
            if (!Object.HasStateAuthority) return;
        debugDead = deadPlayer;
        playersAlive.Remove(deadPlayer);
        Debug.Log("Player dead " + deadPlayer);
    }
    public override void FixedUpdateNetwork()
    {
        if(Object.HasStateAuthority)
        {
            UpdateRespawnDead();
        }
    }
    private void UpdateRespawnDead()
    {
        if(activePlayersInServer.Count > playersAlive.Count)
        {
            Debug.Log("someone is dead");
            Runner.Spawn()
        }
    }
}
