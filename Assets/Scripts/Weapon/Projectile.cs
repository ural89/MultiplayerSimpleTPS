using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Projectile : NetworkBehaviour, IPredictedDespawnBehaviour
{
    [SerializeField]  private LayerMask hitMask;
    [Networked] public TickTimer networkedLifeTimer { get; set; }
    private TickTimer _predictedLifeTimer;
    private float lifeDurationSec = 2f;

    private TickTimer lifeTimer
    {
        get => Object.IsPredictedSpawn ? _predictedLifeTimer : networkedLifeTimer;
        set { if (Object.IsPredictedSpawn) _predictedLifeTimer = value; else networkedLifeTimer = value; }
    }

    [Networked]
    private int _fireTick { get; set; }
    [Networked]
    private Vector3 _firePosition { get; set; }
    [Networked]
    private Vector3 _fireVelocity { get; set; }

    // Same method can be used both for FUN and Render calls

    public void Init()
    {
        lifeTimer = TickTimer.CreateFromSeconds(Runner, lifeDurationSec);


    }
    public override void Render()
    {

    }
    public override void FixedUpdateNetwork()
    {
        if (IsProxy == true)
            return;

        // Previous and next position is calculated based on the initial parameters.
        // There is no point in actually moving the object in FUN.
        var previousPosition = GetMovePosition(Runner.Tick - 1);
        var nextPosition = GetMovePosition(Runner.Tick);

        var direction = nextPosition - previousPosition;

        if (Runner.LagCompensation.Raycast(previousPosition, direction, direction.magnitude, Object.InputAuthority,
                 out var hit, hitMask, HitOptions.IncludePhysX | HitOptions.IgnoreInputAuthority))
        {
            Runner.Despawn(Object, true);
        }
        transform.position = nextPosition;
    }

    public void PredictedDespawn()
    {

    }

    public void PredictedDespawnFailed()
    {

    }
    private Vector3 GetMovePosition(float currentTick)
    {
        float time = (currentTick - _fireTick) * Runner.DeltaTime;

        if (time <= 0f)
            return _firePosition;

        return _firePosition + _fireVelocity * time;
    }
}
