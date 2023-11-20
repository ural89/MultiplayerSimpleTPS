using Fusion;
using TMPro;
using UnityEngine;

public class LobbyPlayerList : MonoBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private TMP_Text playerNamePrefab;
    private NetworkHandler networkHandler;

    public void PlayerJoined(PlayerRef player)
    {
        UpdatePlayerList();
    }

    public void PlayerLeft(PlayerRef player)
    {
        UpdatePlayerList();
    }

    private void Awake()
    {
        networkHandler = FindObjectOfType<NetworkHandler>();
    }
    private void Start()
    {
        Invoke(nameof(UpdatePlayerList) ,1);
        Debug.Log(networkHandler.ActivePlayersInServer.Count);
       // Debug.Log("started lobby list");
    }
    private void UpdatePlayerList()
    {
        foreach (var playerName in networkHandler.ActivePlayersInServer)
        {

            Debug.Log(playerName);
        }
    }

}
