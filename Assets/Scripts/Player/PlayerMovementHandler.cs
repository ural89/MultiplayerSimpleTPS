using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.KCC;
using UnityEngine;

public class PlayerMovementHandler : NetworkBehaviour, IPlayerLeft
{
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


        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (Object.InputAuthority == player)
        {
            networkHandler.Input -= NetworkHandler_Input;
            Debug.LogWarning("player input deactivated");
        }


    }
}

