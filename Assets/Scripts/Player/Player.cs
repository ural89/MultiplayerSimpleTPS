using System;
using Fusion;
using UnityEngine;
public class Player : NetworkBehaviour
{

    [SerializeField] private Weapon weapon;
    public static Player Local { get; private set; }
    private PlayerCameraHandler cameraHandler;
    private NetworkHandler networkHandler;
    private void Awake()
    {
        networkHandler = FindObjectOfType<NetworkHandler>();
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
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData inputData))
        {
            if (inputData.IsFirePressed)
            {
                weapon.Fire(Object.InputAuthority);
            }
        }
    }
    
}
