using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipMovement : NetworkBehaviour
{
    [SerializeField] private float constRotationSpeed = 1f;
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
        RotateShip(constRotationSpeed);
    }
 
    public void UpdateInput(Vector2 movementInput)
    {
        MoveDelta = new Vector3(0, 0, movementInput.y) * Runner.DeltaTime * 5;
        rb.Rigidbody.MovePosition(transform.position + MoveDelta);
    
        RotateShip(movementInput.x);
    }

    private void RotateShip(float movementInput)
    {

        float rotationAngle = movementInput * Runner.DeltaTime * 50; // Adjust the multiplier for rotation speed 
        Quaternion rotation = Quaternion.Euler(0, rotationAngle, 0);
        rb.Rigidbody.MoveRotation(rb.Rigidbody.rotation * rotation);
    }
}
