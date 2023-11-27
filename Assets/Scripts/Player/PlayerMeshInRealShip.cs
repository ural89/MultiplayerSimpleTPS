using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeshInRealShip : MonoBehaviour
{
    private Player player;
    public void SetPlayer(Player player) => this.player = player;
    private void LateUpdate()
    {
        if (player != null)
            transform.localPosition = player.transform.localPosition;
    }
}
