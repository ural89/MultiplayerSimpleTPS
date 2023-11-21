using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    [Networked, Capacity(10)] public NetworkDictionary<PlayerRef, int> playerScores { get; }
    private bool isReady;
    public bool IsReady => isReady;
    public static ScoreManager Instance = null;

    public override void Spawned()
    {
        isReady = true;
    }
    public int GetScore(PlayerRef player)
    {
        if (playerScores.ContainsKey(player))
        {
            return playerScores[player];
        }
        else
        {
            playerScores.Add(player, 0);
            return 0;
        }
    }
    private void Awake()
    {
        Instance = this;
    }

    public void SetScore(PlayerRef player, int score)
    {
        if (!Object.HasStateAuthority) return;
        if (playerScores.ContainsKey(player))
        {
            playerScores.Set(player, playerScores.Get(player) + score);
        }
        else
        {
            playerScores.Add(player, score);
        }
        Debug.Log(player + " score is " + playerScores[player]);
    }
    private void UpdateCached()
    {

    }
}
