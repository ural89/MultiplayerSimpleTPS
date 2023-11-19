using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [Networked] private float healthAmount { get; set; } = 10;
    private float predictedHealthAmount;
    private bool isDead = false;
    private float fullHealth;
    public void TakeDamage(float damageTaken) => healthAmount -= damageTaken;
    public float GetHealthNormilized => predictedHealthAmount / fullHealth;
    public bool IsDead => isDead;
    public override void Spawned()
    {
        fullHealth = healthAmount;
    }
    public override void FixedUpdateNetwork()
    {
        predictedHealthAmount = healthAmount;
        if (Object.HasStateAuthority)
            if (healthAmount <= 0)
            {
                isDead = true;
                Runner.Despawn(Object);
            }
    }
}
