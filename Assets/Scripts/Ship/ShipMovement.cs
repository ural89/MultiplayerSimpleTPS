using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipMovement : NetworkBehaviour
{
    public static Vector3 MoveDelta;
    public static float rotationDegree;
    private NetworkRigidbody rb;
    private void Awake()
    {
        rb = GetComponent<NetworkRigidbody>();
    }
    public override void FixedUpdateNetwork()
    {
        rotationDegree = transform.eulerAngles.y;
        RotateShip(1);
    }
 
    public void UpdateInput(Vector2 movementInput)
    {
        // Calculate movement in the forward direction
        MoveDelta = new Vector3(0, 0, movementInput.y) * Runner.DeltaTime * 5;

        // Move the ship
        rb.Rigidbody.MovePosition(transform.position + MoveDelta);
    
        RotateShip(movementInput.x);
    }

    private void RotateShip(float movementInput)
    {
        // Calculate rotation based on the input
        float rotationAngle = movementInput * Runner.DeltaTime * 50; // Adjust the multiplier for rotation speed

        // Create a rotation quaternion
        Quaternion rotation = Quaternion.Euler(0, rotationAngle, 0);

        // Apply rotation to the rigidbody
        rb.Rigidbody.MoveRotation(rb.Rigidbody.rotation * rotation);
    }
}
