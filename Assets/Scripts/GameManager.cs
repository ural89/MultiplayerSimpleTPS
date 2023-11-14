using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private Character characterPrefab;
    [SerializeField] private Transform ship;
    private NetworkHandler networkHandler;
    private Dictionary<PlayerRef, Character> characters = new();

    private void Start()
    {

    }
    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
           
            networkHandler = FindObjectOfType<NetworkHandler>();
            networkHandler.AddPlayerConnectionCallbacks(NetworkHandler_OnPlayerJoin, NetworkHandler_OnPlayerLeft);
        }

    }

    public void NetworkHandler_OnPlayerJoin(NetworkRunner runner, PlayerRef player)
    {
        var characterClone = runner.Spawn(characterPrefab, Vector3.up * 2, Quaternion.identity, player, SetParent);
        void SetParent(NetworkRunner runner, NetworkObject no)
        {
            no.transform.SetParent(ship);
        }
        characters.Add(player, characterClone);
    }
    private void NetworkHandler_OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        runner.Despawn(characters[player].Object, false);
    }


}
