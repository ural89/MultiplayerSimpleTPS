using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class SpawnHandler : NetworkBehaviour
{
    [SerializeField] private Player characterPrefab;
    private NetworkHandler networkHandler;
    private void Awake()
    {
        networkHandler = FindObjectOfType<NetworkHandler>();
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
        Debug.Log("Active players count: " + networkHandler.ActivePlayersInServer.Count + " Alive players count: " + GameManager.Instance.playersAlive.Count);
        if (networkHandler.ActivePlayersInServer.Count > GameManager.Instance.playersAlive.Count)
        {
            List<PlayerRef> playersNotAlive = networkHandler.ActivePlayersInServer.Except(GameManager.Instance.playersAlive).ToList();
            foreach (var player in playersNotAlive)
            {

                var characterClone = Runner.Spawn(characterPrefab, Vector3.up * 2, Quaternion.identity, player);
                if (!characters.ContainsKey(player))
                    characters.Add(player, characterClone);

            }
        }
        Debug.Assert(networkHandler.ActivePlayersInServer.Count <= GameManager.Instance.playersAlive.Count, "more players alive then people in server");
        Debug.Assert(characters.Count == GameManager.Instance.playersAlive.Count, "characters dict and players alive count not equal");
    }

}
