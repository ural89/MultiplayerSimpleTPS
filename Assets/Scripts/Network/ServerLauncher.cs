using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class ServerLauncher : MonoBehaviour
{

    private NetworkRunner runner;
    [SerializeField] private int gameSceneIndex;
    private NetworkHandler networkHandler;

    private void Awake()
    {
        networkHandler = FindObjectOfType<NetworkHandler>();
    }
    private void Start()
    {
        
        StartGame(GameMode.Server);
    }
    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        runner = gameObject.AddComponent<NetworkRunner>();
        runner.ProvideInput = true;
        runner.AddCallbacks(networkHandler);

        // Start or join (depends on gamemode) a session with a specific name
        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = gameSceneIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
}
