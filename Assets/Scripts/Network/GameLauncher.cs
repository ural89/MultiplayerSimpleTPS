using UnityEngine;


public class GameLauncher : MonoBehaviour
{
    [SerializeField] private ServerLauncher serverLauncher;
    [SerializeField] private ClientLauncher clientLauncher;
    private void Start()
    {
#if UNITY_SERVER
        Instantiate(serverLauncher);
#else
        Instantiate(clientLauncher);
#endif
    }
}
