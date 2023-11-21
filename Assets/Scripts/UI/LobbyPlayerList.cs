using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerList : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private TMP_Text playerNamePrefab;
    [SerializeField] private Transform playersContainer;
    [SerializeField] private Button startButton;
    [Networked, Capacity(10)] private NetworkLinkedList<PlayerRef> players { get; }
    [Networked, Capacity(10)] private NetworkDictionary<PlayerRef, string> playerNames { get; }

    [Networked] private PlayerRef leaderPlayer { get; set; }
    public void SetNameForPlayer(PlayerRef playerRef, string name) //Not in use right now
    {
        playerNames.Add(Object.InputAuthority, name);
        UpdatePlayerList();
    }
    public void PlayerJoined(PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            players.Add(player);
            if (!leaderPlayer.IsValid)
            {
                leaderPlayer = player;
                startButton.gameObject.SetActive(true);
            }
        }

        Object.AssignInputAuthority(player);
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

    public void StartGameScene()
    {
        Runner.SetActiveScene(2);
    }

}
