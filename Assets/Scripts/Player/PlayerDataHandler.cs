using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerDataHandler : NetworkBehaviour
{
    [Networked] private ref PlayerData PlayerDataRef => ref MakeRef<PlayerData>();
    public PlayerData GetPlayerData => PlayerDataRef;
    private LobbyPlayerList lobbyPlayerList;
  
    private Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
        lobbyPlayerList = FindAnyObjectByType<LobbyPlayerList>();
    }
    public void SetName(String name)
    {
        PlayerDataRef.Name = name;
        lobbyPlayerList.SetNameForPlayer(player.Object.InputAuthority, name);
    }
    public override void Spawned()
    {
        SetName("ural");
    }
}
