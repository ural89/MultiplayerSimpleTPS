using Fusion;
using UnityEngine;

public class ClientLauncher : MonoBehaviour
{
    private NetworkRunner runner;
    [SerializeField] private int lobbySceneIndex;
    private NetworkHandler networkHandler;

    private void Awake()
    {
        networkHandler = FindObjectOfType<NetworkHandler>();
    }
    private void Start()
    {
        // DontDestroyOnLoad(this);
        StartGame(GameMode.Client);
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
            Scene = lobbySceneIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
}
