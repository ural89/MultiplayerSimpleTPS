using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraHandler : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    private bool isWorking = false;
    public void UpdateCameraSettings()
    {
        isWorking = (Player.Local != null);
    }
    private void LateUpdate()
    {
        if (isWorking && Player.Local != null)
            Camera.main.transform.position = Player.Local.transform.position + offset;
    }

}
