using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Projectile : NetworkBehaviour, IPredictedDespawnBehaviour
{
    [Networked] public TickTimer networkedLifeTimer { get; set; }
    private TickTimer _predictedLifeTimer;
    private float lifeDurationSec = 2f;

    private TickTimer lifeTimer
    {
        get => Object.IsPredictedSpawn ? _predictedLifeTimer : networkedLifeTimer;
        set { if (Object.IsPredictedSpawn) _predictedLifeTimer = value; else networkedLifeTimer = value; }
    }
    public void Init()
    {
        lifeTimer = TickTimer.CreateFromSeconds(Runner, lifeDurationSec);
       

    }
    public override void FixedUpdateNetwork()
    {
        transform.position += 
    }

    public void PredictedDespawn()
    {
        
    }

    public void PredictedDespawnFailed()
    {
        
    }
}
