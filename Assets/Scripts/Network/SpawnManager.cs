using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class SpawnManager : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private Transform spawnPointsParent;
    [SerializeField] private Player characterPrefab;
   
    private Transform[] spawnPoints;
    private Dictionary<PlayerRef, Player> characters = new();
    private List<PlayerRef> playersAlive = new();
   

    public static SpawnManager Instance = null;

    private void Awake()
    {
        Instance = this;

        spawnPoints = spawnPointsParent.GetComponentsInChildren<Transform>();
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (playersAlive.Contains(player))
        {
            Runner.Despawn(characters[player].Object);
            playersAlive.Remove(player);

        }
        Debug.Log("player left");
        characters.Remove(player);
       

    }

    public void PlayerJoined(PlayerRef player)
    {

      


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
        //Debug.Log("Active players count: " + activePlayersInServer.Count + " Alive players count: " + playersAlive.Count);

        if (NetworkHandler.ActivePlayersInServer.Count > playersAlive.Count)
        {
            List<PlayerRef> playersNotAlive = NetworkHandler.ActivePlayersInServer.Except(playersAlive).ToList();
            foreach (var player in playersNotAlive)
            {

                var randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                var characterClone = Runner.Spawn(characterPrefab, randomSpawnPoint.position, Quaternion.identity, player);
                if (!characters.ContainsKey(player))
                    characters.Add(player, characterClone);
     

            }
        }
        Debug.Assert(NetworkHandler.ActivePlayersInServer.Count <= playersAlive.Count, "more players alive then people in server"+ NetworkHandler.ActivePlayersInServer.Count + " : " + playersAlive.Count);
        Debug.Assert(characters.Count == playersAlive.Count, "characters dict and players alive count not equal " + NetworkHandler.ActivePlayersInServer.Count + " : " + characters.Count);
    }


}