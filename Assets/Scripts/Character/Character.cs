using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.KCC;
using UnityEngine;

public class Character : NetworkBehaviour
{
    private float moveSpeed = 3f;

    private KCC kcc;

    public override void Spawned()
    {

        kcc = GetComponent<KCC>();
     
    }
    public override void FixedUpdateNetwork()
    {
       
    }
    public void Despawn()
    {
        Runner.Despawn(Object);
    }
}
