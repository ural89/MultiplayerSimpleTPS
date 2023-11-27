using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerMeshInRealShip : NetworkBehaviour
{
    private Player player;
    public void SetPlayer(Player player) => this.player = player;
    public override void Render()
    {
        if (player != null)
            transform.localPosition = player.transform.localPosition;
    }
}
