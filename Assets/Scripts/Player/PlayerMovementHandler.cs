using System;
using Fusion;
using Fusion.KCC;
using UnityEngine;

public class PlayerMovementHandler : NetworkBehaviour
{
    public Action<Vector3> OnMove;
    private bool canMove = true; //TODO: make this networked if needed;
    private KCC kcc;
    [Networked] private Vector3 currentPosition { get; set; }
    private Vector3 moveDelta;
    private float initialAngleToShip;
    private Ship ship;
    private void Awake()
    {

        kcc = GetComponent<KCC>();

    }
    public override void Spawned()
    {
        ship = FindObjectOfType<Ship>();
        currentPosition = transform.position;
    }
    public void SetCanMove(bool canMove)
    {
        kcc.SetInputDirection(Vector3.zero);
        this.canMove = canMove;
    }

    public override void FixedUpdateNetwork()
    {
        moveDelta = Vector3.zero;

        if (GetInput(out NetworkInputData networkInputData) && canMove)
        {
            moveDelta = new Vector3(networkInputData.MoveDirection.x, 0, networkInputData.MoveDirection.y);

            if (!networkInputData.LookDirection.AlmostEquals(Vector3.zero))
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(networkInputData.LookDirection.x, 0, networkInputData.LookDirection.y), Vector3.up);
                kcc.SetLookRotation(lookRotation);
            }
        }
        currentPosition = transform.position;



        currentPosition += moveDelta * Runner.DeltaTime * 5;
        OnMove?.Invoke(moveDelta);
        UpdateMovementToShipPosition();
    }

    private void UpdateMovementToShipPosition() //TODO: carry this to late update
    {
        Vector3 playerToShipCenter = transform.position - ship.transform.position;

        // Calculate the angle between the forward direction of the ship and the player-to-ship-center vector
        float angle = Vector3.SignedAngle(Vector3.forward, playerToShipCenter.normalized, Vector3.up);

        // Incorporate the ship's rotation
        angle += ShipMovement.rotationDegree;

        // Convert the angle to radians
        float angleInRadians = Mathf.Deg2Rad * angle;

        // Calculate the new player position based on ship's position, rotated angle, and distance
        float distanceFromShip = playerToShipCenter.magnitude;
        Vector3 newPosition = ShipMovement.MoveDelta +
            new Vector3(Mathf.Sin(angleInRadians), 0, Mathf.Cos(angleInRadians)) * distanceFromShip;

        Debug.Log("angle " + angle + " Distance to ship " + distanceFromShip);
        Debug.DrawLine(newPosition, newPosition + Vector3.up * 100, Color.white, 0.5f);

        // Set the player's position using KCC
        kcc.SetPosition(currentPosition + ShipMovement.MoveDelta);

    }

}

