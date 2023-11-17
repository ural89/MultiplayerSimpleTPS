using System;
using Fusion;
using UnityEngine;
public class Player : NetworkBehaviour
{

    [SerializeField] private Weapon weapon;
    [SerializeField] private LayerMask pickupLayerMask;
    public static Player Local { get; private set; }
    private PlayerCameraHandler cameraHandler;
    private float pickupRadius = 3f;


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
    public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority)
        {
            CheckInput();
            CheckForAmmoPickup();
        }
    }
    private void CheckForAmmoPickup()
    {
        Collider[] results = new Collider[1];
        if (Runner.GetPhysicsScene().OverlapSphere(transform.position, pickupRadius, results, pickupLayerMask, QueryTriggerInteraction.Collide) > 0)
        {
            
            Debug.Log("adding ammo 5");
            weapon.AddAmmo(5);
        }

    }
    private void CheckInput()
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
