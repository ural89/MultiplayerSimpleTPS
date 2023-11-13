using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Character : NetworkBehaviour
{
    private float moveSpeed = 3f;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public override void FixedUpdateNetwork()
    {
        rb.MovePosition(transform.position + Vector3.forward * Runner.DeltaTime * moveSpeed);
    }
    public void Despawn()
    {
        Runner.Despawn(Object);
    }
}
