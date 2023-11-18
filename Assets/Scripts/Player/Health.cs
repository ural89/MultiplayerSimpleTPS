using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [Networked] private float healthAmount { get; set; } = 5;
    public void TakeDamage(float damageTaken) => healthAmount -= damageTaken;
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
            if (healthAmount <= 0)
            {
                Runner.Despawn(Object);
            }
    }
}
