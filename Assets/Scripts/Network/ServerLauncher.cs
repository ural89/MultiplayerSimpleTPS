using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class ServerLauncher : MonoBehaviour
{
    [SerializeField] private int startSceneIndex;
    private NetworkHandler networkHandler;
    private NetworkRunner runner;
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
            Scene = startSceneIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
}
