using Fusion;
using UnityEngine;

public class ClientLauncher : MonoBehaviour
{
    private NetworkRunner runner;
    private NetworkHandler networkHandler;
    [SerializeField] private int lobbySceneIndex = 1;
    private void Awake()
    {
        networkHandler = FindObjectOfType<NetworkHandler>();
    }
    private void Start()
    {
        
        StartGame(GameMode.Client); //TODO: change this to Client mode when testing dedicated server
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
            //SessionName = "urals",
            Scene = lobbySceneIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
}
