using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Character : NetworkBehaviour
{
    private float moveSpeed = 3f;

    private NetworkCharacterControllerPrototype cc;

    public override void Spawned()
    {

        cc = GetComponent<NetworkCharacterControllerPrototype>();
     
    }
    public override void FixedUpdateNetwork()
    {
        cc.Move(transform.forward);
    }
    public void Despawn()
    {
        Runner.Despawn(Object);
    }
}
