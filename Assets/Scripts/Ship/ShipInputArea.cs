using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ShipInputArea : NetworkBehaviour
{
    private Ship ship;
    private void Awake()
    {
        ship = GetComponentInParent<Ship>();
    }
    public void TakeControlOfShip(PlayerRef player) => ship.SetInputAuthority(player);
}
