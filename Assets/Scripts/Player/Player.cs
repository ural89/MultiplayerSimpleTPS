using System;
using Fusion;
using UnityEngine;
public class Player : NetworkBehaviour
{

    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private Weapon weapon;
    public static Player Local { get; private set; }
    private PlayerCameraHandler cameraHandler;

    private float pickupRadius = 2f;

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
        CheckInput();
        CheckForAmmoPickup();

    }
    private void CheckForAmmoPickup()
    {
        //if (Object.HasInputAuthority)
        {
            Collider[] results = new Collider[1];
            if (Runner.GetPhysicsScene().OverlapSphere(transform.position, pickupRadius, results, pickupLayerMask, QueryTriggerInteraction.Collide) > 0)
            {


                var ammoPickup = results[0].GetComponent<AmmoPickup>();
                if (ammoPickup != null)
                {
                    ammoPickup.OnPicked(this);
                    // weapon.AddAmmo(5);
                }
            }
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
