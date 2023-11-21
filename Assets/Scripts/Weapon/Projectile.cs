using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
public struct FireData : INetworkStruct
{
    public int FireTick;
    public Vector3 FirePosition;
    public Vector3 FireVelocity;
    public NetworkBool IsDestroyed;
    public TickTimer LifeCooldown;
    public Vector3 HitPosition;
}

public class Projectile : NetworkBehaviour, IPredictedSpawnBehaviour
{
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private Renderer mesh;
    [Networked] public TickTimer networkedLifeTimer { get; set; }
    [Networked] private FireData _data_Networked { get; set; }
    [Networked(OnChanged = nameof(OnDestroyChanged))] private NetworkBool hasNetworkedDestroyed { get; set; } = false;
    private bool hasPredictedDestroyed;
    private bool hasDestroyed
    {
        get => Object.IsPredictedDespawn ? hasPredictedDestroyed : (bool)hasNetworkedDestroyed;
        set { if (Object.IsPredictedDespawn) hasPredictedDestroyed = value; else hasNetworkedDestroyed = value; }

    }
    [Networked(OnChanged = nameof(HasHitOtherPlayerChanged))] private NetworkBool hasNetworkedHitOtherPlayer { get; set; } = false;


    private bool hasPredictedHitOtherPlayer;
    private bool hasHitOtherPlayer
    {
        get => Object.IsPredictedDespawn ? hasPredictedHitOtherPlayer : (bool)hasNetworkedHitOtherPlayer;
        set { if (Object.IsPredictedDespawn) hasPredictedHitOtherPlayer = value; else hasNetworkedHitOtherPlayer = value; }

    }

    private static void OnDestroyChanged(Changed<Projectile> changed)
    {
        changed.Behaviour.DestroyChanged();
    }
    private void DestroyChanged()
    {

        mesh.enabled = false;
       
    }
    private static void HasHitOtherPlayerChanged(Changed<Projectile> changed)
    {
        changed.Behaviour.HitOtherPlayerChanged();

    }
    private void HitOtherPlayerChanged()
    {
        var data = _data;
        Instantiate(hitEffect, GetMovePosition(Runner.Tick, data), Quaternion.identity);
    }
    private FireData _data_Local;

    public FireData _data
    {
        get => Object.IsPredictedSpawn == true ? _data_Local : _data_Networked;
        set { if (Object.IsPredictedSpawn == true) _data_Local = value; else _data_Networked = value; }
    }
    private float lifeDurationSec = 2f;



    // Same method can be used both for FUN and Render calls

    public void Init(Vector3 firePosition, Vector3 velocity)
    {
        var data = _data;
        // lifeTimer = TickTimer.CreateFromSeconds(Runner, lifeDurationSec);
        data.FireTick = Runner.Tick;
        data.FireVelocity = velocity;
        data.FirePosition = firePosition;
        if (lifeDurationSec > 0f)
        {
            data.LifeCooldown = TickTimer.CreateFromSeconds(Runner, lifeDurationSec);
        }
        _data = data;

    }
    public override void Render()
    {
        //        if(hasDestroyed) return;
        bool isProxy = IsProxy == true && Object.IsPredictedSpawn == false;
        float renderTime = isProxy == true ? Runner.InterpolationRenderTime : Runner.SimulationRenderTime;
        float floatTick = renderTime / Runner.DeltaTime;

        var data = _data;
        transform.position = GetMovePosition(floatTick, data);
    }
    public override void FixedUpdateNetwork()
    {
        if (IsProxy == true)
            return;

        MoveProjectile();

    }

    private void MoveProjectile()
    {
        if (hasDestroyed) return;
        var data = _data;
        if (data.LifeCooldown.Expired(Runner))
        {
            Runner.Despawn(Object);
            return;
        }
        var previousPosition = GetMovePosition(Runner.Tick - 1, data);
        var nextPosition = GetMovePosition(Runner.Tick, data);

        var direction = nextPosition - previousPosition;

        if (Runner.LagCompensation.Raycast(previousPosition, direction, direction.magnitude, Object.InputAuthority,
                 out var hit, hitMask, HitOptions.IncludePhysX | HitOptions.IgnoreInputAuthority))
        {
            var health = hit.GameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(5f, Object.InputAuthority);
                hasHitOtherPlayer = true;

            }
            hasDestroyed = true;
            if (Object.HasStateAuthority)
                Invoke(nameof(LateDespawn), 1f);
        }
    }
    private void LateDespawn()
    {
        Runner.Despawn(Object);

    }


    private Vector3 GetMovePosition(float currentTick, FireData data)
    {
        float time = (currentTick - data.FireTick) * Runner.DeltaTime;

        if (time <= 0f)
            return data.FirePosition;

        return data.FirePosition + data.FireVelocity * time;
    }

    public void PredictedSpawnSpawned()
    {

    }

    public void PredictedSpawnUpdate()
    {

    }

    public void PredictedSpawnRender()
    {
        Render();
    }

    public void PredictedSpawnFailed()
    {

    }

    public void PredictedSpawnSuccess()
    {

    }
}
