using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraHandler : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    [SerializeField] private PlayerMeshInRealShip player;
    private bool isWorking = false;

    public void UpdateCameraSettings(PlayerMeshInRealShip player)
    {
        isWorking = (player != null);

        {
            this.player = player;

        }

    }
    private void LateUpdate()
    {
        if (isWorking && Player.Local != null)
            Camera.main.transform.position = player.transform.position + offset;
        //  Debug.Log(playerMesh);
    }

}
