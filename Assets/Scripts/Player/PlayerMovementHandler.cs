using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.KCC;
using UnityEngine;

public class PlayerMovementHandler : NetworkBehaviour, IPlayerLeft
{
    [SerializeField] private GameObject head;
    
    [Networked(OnChanged = nameof(OnHeadActivationChanged))] private NetworkBool isActive { get; set; } = true;

    private static void OnHeadActivationChanged(Changed<PlayerMovementHandler> changed)
    {
        changed.Behaviour.head.SetActive(changed.Behaviour.isActive);
    }

    private float moveSpeed = 3f;
    private PlayerInputHandler playerInputHandler;
    private NetworkHandler networkHandler;
    private KCC kcc;
    private void Awake()
    {
        networkHandler = FindObjectOfType<NetworkHandler>();
        playerInputHandler = GetComponent<PlayerInputHandler>();
        kcc = GetComponent<KCC>();

    }
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            networkHandler.Input += NetworkHandler_Input;
        }

    }

    private void NetworkHandler_Input(NetworkRunner runner, NetworkInput input)
    {
        if (Object.HasInputAuthority)
        {
            input.Set(playerInputHandler.GetNetworkInputData());
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData networkInputData))
        {

            kcc.SetInputDirection(new Vector3(networkInputData.MoveDirection.x, 0, networkInputData.MoveDirection.y));
            if (!Vector3.Equals(networkInputData.LookDirection, Vector3.zero))
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(networkInputData.LookDirection.x, 0, networkInputData.LookDirection.y), Vector3.up);
                kcc.SetLookRotation(lookRotation);
            }
            if (networkInputData.IsFirePressed)
            {
                isActive = !isActive;
                
            }

        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (Object.InputAuthority == player)
        {
            networkHandler.Input -= NetworkHandler_Input;
        }


    }
}

