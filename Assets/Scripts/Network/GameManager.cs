using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class GameManager : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private Player characterPrefab;
    private Dictionary<PlayerRef, Player> characters = new();
    private List<PlayerRef> playersAlive = new();
    private List<PlayerRef> activePlayersInServer = new();

    public static GameManager Instance = null;

    private void Awake()
    {
        Instance = this;
        //DontDestroyOnLoad(this);

    }

    public void PlayerLeft(PlayerRef player)
    {
        if (playersAlive.Contains(player))
        {
            Runner.Despawn(characters[player].Object);
            playersAlive.Remove(player);

        }
        characters.Remove(player);
        activePlayersInServer.Remove(player);

    }

    public void PlayerJoined(PlayerRef player)
    {

        activePlayersInServer.Add(player);


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
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            UpdateRespawnDead();
        }
    }
    private void UpdateRespawnDead()
    {
        // Debug.Log("Active players count: " + activePlayersInServer.Count + " Alive players count: " + playersAlive.Count);
        if (activePlayersInServer.Count > playersAlive.Count)
        {
            List<PlayerRef> playersNotAlive = activePlayersInServer.Except(playersAlive).ToList();
            foreach (var player in playersNotAlive)
            {

                var characterClone = Runner.Spawn(characterPrefab, Vector3.up * 2, Quaternion.identity, player);
                if (!characters.ContainsKey(player))
                    characters.Add(player, characterClone);

            }
        }
        Debug.Assert(activePlayersInServer.Count <= playersAlive.Count, "more players alive then people in server");
        Debug.Assert(characters.Count == playersAlive.Count, "characters dict and players alive count not equal");
    }


}
