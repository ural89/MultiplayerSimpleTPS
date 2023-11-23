using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerList : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private int gameSceneIndex = 2;
    [SerializeField] private TMP_Text playerNamePrefab;
    [SerializeField] private Transform playersContainer;
    [Networked(OnChanged = nameof(OnReadyPlayerCountChanged))] private int readyPlayersCount { get; set; } = 0;


    [Networked, Capacity(10)] private NetworkLinkedList<PlayerRef> players { get; }
    [Networked, Capacity(10)] private NetworkDictionary<PlayerRef, string> playerNames { get; }

    [Networked] private PlayerRef leaderPlayer { get; set; }
    public override void Spawned()
    {
        if (!Object.HasStateAuthority) return;
        players.Clear();
        leaderPlayer = default;

        foreach (var player in NetworkHandler.ActivePlayersInServer)
        {
            players.Add(player);
        }

    }
    public void SetNameForPlayer(PlayerRef playerRef, string name) //Not in use right now
    {
        playerNames.Add(Object.InputAuthority, name);
        NetworkHandler.ActivePlayerNames[playerRef] = name;
        UpdatePlayerList();
    }
    public void PlayerJoined(PlayerRef player)
    {
        if (Object.HasStateAuthority)
            if (!leaderPlayer.IsValid)
                leaderPlayer = player;

        if (Object.HasStateAuthority)
        {
            players.Add(player);

        }
        //Object.AssignInputAuthority(player);
        UpdatePlayerList();
    }



    public void PlayerLeft(PlayerRef player)
    {
        if (Object.HasStateAuthority)
            players.Remove(player);


        UpdatePlayerList();
    }



    public void UpdatePlayerList()
    {
        StartCoroutine(lateUpdate());
        IEnumerator lateUpdate()
        {
            yield return new WaitForSeconds(0.3f);
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
    public void OnReadyToggle(bool isReady)
    {
        RPC_ToggledReady(isReady);


    }
    private static void OnReadyPlayerCountChanged(Changed<LobbyPlayerList> changed)
    {
        changed.Behaviour.OnReadyPlayerCountChanged();
    }

    private void OnReadyPlayerCountChanged()
    {
        if (readyPlayersCount == players.Count)
            Runner.SetActiveScene(gameSceneIndex);

    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, Channel = RpcChannel.Reliable)]
    public void RPC_ToggledReady(bool isReady)
    {
        if (isReady)
            readyPlayersCount++;
        else
            readyPlayersCount--;
        Debug.Log(readyPlayersCount);
    }
    /*  private void StartGameScene()
     {
         RPC_StartGame();
       
     }
     [Rpc(RpcSources.All, RpcTargets.StateAuthority, Channel = RpcChannel.Reliable)]
     public void RPC_StartGame()
     {
         Runner.SetActiveScene(2);
     }
  */

}
