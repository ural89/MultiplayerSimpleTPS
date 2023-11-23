using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sample.DedicatedServer.Utils;
public class ServerLauncher : MonoBehaviour
{

    private NetworkRunner runner;
    [SerializeField] private int gameSceneIndex;
    private NetworkHandler networkHandler;
    private DedicatedServerConfig serverConfig;
    private void Awake()
    {
        networkHandler = FindObjectOfType<NetworkHandler>();
    }
    private void Start()
    {
#if UNITY_EDITOR
#if UNITY_SERVER
    Debug.LogError("Running in server mode!");
#endif
#endif
        StartGame(GameMode.Server);
    }
    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        var serverConfig = DedicatedServerConfig.Resolve();
        runner = gameObject.AddComponent<NetworkRunner>();
        runner.ProvideInput = true;
        runner.AddCallbacks(networkHandler);

        // Start or join (depends on gamemode) a session with a specific name
        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = serverConfig.SessionName,
            Scene = gameSceneIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
}
