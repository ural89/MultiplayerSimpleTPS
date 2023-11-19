using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public List<PlayerRef> playersAlive = new();
    public static GameManager Instance = null;
    private void Awake()
    {
        Instance = this;
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

        playersAlive.Remove(deadPlayer);
        Debug.Log("Player dead " + deadPlayer);
    }

}
