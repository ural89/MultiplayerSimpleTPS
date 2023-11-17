using Fusion;
using UnityEngine;
public class Player : NetworkBehaviour
{
   

    public static Player Local { get; private set; }
    private PlayerCameraHandler cameraHandler;
    private void Awake()
    {
        cameraHandler = GetComponent<PlayerCameraHandler>();
    }


    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
        }

        cameraHandler.UpdateCameraSettings();
    }
}
