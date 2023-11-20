using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class LobbyPlayerList : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private TMP_Text playerNamePrefab;
    [SerializeField] private Transform playersContainer;
    private NetworkHandler networkHandler;

    [Networked, Capacity(10)] private NetworkLinkedList<PlayerRef> players { get; }
    [Networked, Capacity(10)] private NetworkDictionary<PlayerRef, string> playerNames { get; }
    public void SetNameForPlayer(string name)
    {
        playerNames.Add(Object.InputAuthority, name);
        UpdatePlayerList();
    }
    public void PlayerJoined(PlayerRef player)
    {
        if (Object.HasStateAuthority)
            players.Add(player);
            
        Object.AssignInputAuthority(player);
        UpdatePlayerList();
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (Object.HasStateAuthority)
            players.Remove(player);
        UpdatePlayerList();
    }

    private void Awake()
    {
        networkHandler = FindObjectOfType<NetworkHandler>();
    }

    private void UpdatePlayerList()
    {
        foreach (Transform child in playersContainer)
        {
            if (child != playersContainer)
            {
                Destroy(child.gameObject);
            }
        }
        foreach (var playerName in players)
        {

            var playerNameClone = Instantiate(playerNamePrefab, playersContainer);
            playerNameClone.text = playerName.ToString();
        }
    }

}
