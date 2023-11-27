using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipMovement : NetworkBehaviour
{
    [SerializeField] private float constRotationSpeed = 1f;
    [SerializeField] private float constMoveSpeed = 0f;
    public  Vector3 MoveDelta;
    public  float rotationDegree;
    private NetworkRigidbody rb;
    private void Awake()
    {
        MoveDelta = Vector3.zero;
        rotationDegree = 0;
        rb = GetComponent<NetworkRigidbody>();
    }
    public override void FixedUpdateNetwork()
    {
        rotationDegree = transform.eulerAngles.y;
        if (constRotationSpeed != 0)
            RotateShip(constRotationSpeed);
        if (constMoveSpeed != 0)
            UpdateInput(new Vector2(0, constMoveSpeed));
    }

    public void UpdateInput(Vector2 movementInput)
    {

        MoveDelta = new Vector3(0, 0, movementInput.y) * Runner.DeltaTime * 5;

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
