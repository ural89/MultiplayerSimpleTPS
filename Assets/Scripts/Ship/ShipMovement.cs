using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ShipMovement : NetworkBehaviour
{
    public static Vector3 MoveDelta;
    private NetworkRigidbody rb;
    private void Awake()
    {
        rb = GetComponent<NetworkRigidbody>();
    }
    public void UpdateInput(Vector2 movementInput)
    {
        MoveDelta = new Vector3(movementInput.x, 0, movementInput.y) * Runner.DeltaTime;
        rb.Rigidbody.MovePosition(transform.position + MoveDelta);
    }
}
