using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class GameManager : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
  
    private Dictionary<PlayerRef, Player> characters = new();
    private List<PlayerRef> playersAlive = new();

  
    public static GameManager Instance = null;

    private void Awake()
    {
        Instance = this;
        


    }
    private void Start()
    {
        DontDestroyOnLoad(this);

    }

    public void PlayerLeft(PlayerRef player)
    {
        if (playersAlive.Contains(player))
        {
            Runner.Despawn(characters[player].Object);
            playersAlive.Remove(player);

        }
        characters.Remove(player);
        // networkHandler.ActivePlayersInServer.Remove(player);

    }

    public void PlayerJoined(PlayerRef player)
    {

        // networkHandler.ActivePlayersInServer.Add(player);


    }
    public void OnPlayerSpawn(PlayerRef playerSpawned)
    {
        if (Object != null)
            if (!Object.HasStateAuthority) return;

        playersAlive.Add(playerSpawned);

    }
    public void OnPlayerDie(PlayerRef deadPlayer)
    {
        if (Object != null)
            if (!Object.HasStateAuthority) return;
        characters.Remove(deadPlayer);
        playersAlive.Remove(deadPlayer);

    }

}
