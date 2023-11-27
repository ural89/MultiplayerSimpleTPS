using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
public class Player : NetworkBehaviour
{
    [SerializeField] private NetworkObject playerMesh;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private LayerMask shipLayerMask;
    private Ship ship;
    [SerializeField] private Weapon weapon;
    private NetworkHandler networkHandler;
    public int PlayerID { get; private set; }
    public static Player Local { get; private set; }
    private PlayerCameraHandler cameraHandler;
    private PlayerMovementHandler playerMovementHandler;
    private AmmoAmount ammoAmount;
    private HitboxRoot hitboxRoot;
    private PlayerInputHandler playerInputHandler;
    private Hitbox[] hitboxes;
    private Renderer[] renderers;
    private float pickupRadius = 2f;
    private Health health;

    private OwnerGetter ownerGetter;
    private ControlType controlType = ControlType.character;

    public NetworkObject GetPlayerMesh => playerMesh;
    private enum ControlType
    {
        character,
        ship
    }
    private void Awake()
    {
        ship = FindObjectOfType<Ship>();
        ownerGetter = FindObjectOfType<OwnerGetter>();
        health = GetComponent<Health>();
        ammoAmount = GetComponent<AmmoAmount>();
        cameraHandler = GetComponent<PlayerCameraHandler>();
        hitboxRoot = GetComponent<HitboxRoot>();
        hitboxes = GetComponentsInChildren<Hitbox>();
        playerInputHandler = GetComponent<PlayerInputHandler>();
        networkHandler = FindObjectOfType<NetworkHandler>();
        playerMovementHandler = GetComponent<PlayerMovementHandler>();

    }



    private void NetworkHandler_Input(NetworkRunner runner, NetworkInput input) //TODO: carry this to network input handler or somehting like that
    {
        if (Object != null)
            if (Object.HasInputAuthority)
            {
                input.Set(playerInputHandler.GetNetworkInputData());
            }
    }

    public override void Spawned()
    {

        var pm = Runner.Spawn(playerMesh, Vector3.zero, Quaternion.identity, Object.InputAuthority, (r, o) =>
        {
            o.transform.SetParent(ship.transform);
            o.GetComponent<PlayerMeshInRealShip>().SetPlayer(this);
        });
        if (Object.HasInputAuthority)
        {
            Local = this;
            ownerGetter.SetOwner(Object.InputAuthority);
            networkHandler.Input += NetworkHandler_Input;

            cameraHandler.UpdateCameraSettings(pm.GetComponent<PlayerMeshInRealShip>());
        }
        PlayerID = Object.InputAuthority;
        health.OnDie += Healht_OnDie;
        transform.SetParent(FindObjectOfType<ShipCollider>().transform);

        SpawnManager.Instance.OnPlayerSpawn(Object.InputAuthority);
    }

    private void Healht_OnDie()
    {
        renderers = GetComponentsInChildren<Renderer>();
        foreach (var hb in hitboxes)
        {
            hb.HitboxActive = false;
            hitboxRoot.SetHitboxActive(hb, false);
        }
        foreach (var _renderer in renderers)
        {
            //_renderer.enabled = false;

        }

        Invoke(nameof(LateDespawn), 1f);
    }
    private void LateDespawn()
    {
        if (Object.HasInputAuthority)
            networkHandler.Input -= NetworkHandler_Input;

        if (Object.HasStateAuthority)
            Runner.Despawn(Object);
    }
    public override void FixedUpdateNetwork()
    {
        CheckInput();
        CheckForAmmoPickup();
        CheckForShipInputArea();

    }
    private void CheckForAmmoPickup()
    {
        if (Object.HasStateAuthority)
        {

            List<LagCompensatedHit> lagCompensatedHits = new();
            if (Runner.LagCompensation.OverlapSphere(transform.position, pickupRadius, Object.InputAuthority, lagCompensatedHits, pickupLayerMask, HitOptions.IncludePhysX) > 0)
            {


                var ammoPickup = lagCompensatedHits[0].GameObject.GetComponent<AmmoPickup>();
                if (ammoPickup != null && ammoPickup.Object != null)
                {
                    ammoPickup.OnPicked(this);
                    ammoAmount.AddAmmo(1);

                }
            }
        }

    }
    private void CheckForShipInputArea()
    {
        Collider[] colliders = new Collider[1];
        if (Runner.GetPhysicsScene().OverlapSphere(transform.position, 0.1f, colliders, shipLayerMask, QueryTriggerInteraction.Collide) > 0)
        {
            var shipInputArea = colliders[0].GetComponent<ShipInputArea>();
            if (shipInputArea != null)
                TakeControlOfShip(shipInputArea);
        }
    }
    private void TakeControlOfShip(ShipInputArea shipInputArea)
    {
        controlType = ControlType.ship;
        playerMovementHandler.SetCanMove(false);
        shipInputArea.TakeControlOfShip(Object.InputAuthority);
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
        SpawnManager.Instance.OnPlayerDie(Object.InputAuthority);
    }
}
