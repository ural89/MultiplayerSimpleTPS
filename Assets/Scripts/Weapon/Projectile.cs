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
    [Networked] public TickTimer networkedLifeTimer { get; set; }
    [Networked] private FireData _data_Networked { get; set; }
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
            Runner.Despawn(Object, true);
        }
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
