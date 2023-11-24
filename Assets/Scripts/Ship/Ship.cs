using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Ship : NetworkBehaviour
{
    private ShipMovement shipMovement;
    public void SetInputAuthority(PlayerRef player) => Object.AssignInputAuthority(player);
    private void Awake()
    {
        shipMovement = GetComponent<ShipMovement>();
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData inputData))
        {
            shipMovement.UpdateInput(inputData.MoveDirection);
        }
    }
}
