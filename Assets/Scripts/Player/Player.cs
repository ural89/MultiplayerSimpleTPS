using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
public class Player : NetworkBehaviour
{

    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private Weapon weapon;
    public static Player Local { get; private set; }
    private PlayerCameraHandler cameraHandler;
    private AmmoAmount ammoAmount;
    private float pickupRadius = 2f;
   
    private void Awake()
    {
        ammoAmount = GetComponent<AmmoAmount>();
        cameraHandler = GetComponent<PlayerCameraHandler>();
    }


    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;

        }
        

        GameManager.Instance.OnPlayerSpawn(Object.InputAuthority);
        cameraHandler.UpdateCameraSettings();
    }
    public override void FixedUpdateNetwork()
    {
        CheckInput();
        CheckForAmmoPickup();

    }
    private void CheckForAmmoPickup()
    {
        if (Object.HasStateAuthority)
        {

            List<LagCompensatedHit> lagCompensatedHits = new();
            if (Runner.LagCompensation.OverlapSphere(transform.position, pickupRadius, Object.InputAuthority, lagCompensatedHits, pickupLayerMask, HitOptions.IncludePhysX) > 0)
            {


                var ammoPickup = lagCompensatedHits[0].GameObject.GetComponent<AmmoPickup>();
                if (ammoPickup != null)
                {
                    ammoPickup.OnPicked(this);
                    ammoAmount.AddAmmo(1);
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
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        GameManager.Instance.OnPlayerDie(Object.InputAuthority);
    }
}
