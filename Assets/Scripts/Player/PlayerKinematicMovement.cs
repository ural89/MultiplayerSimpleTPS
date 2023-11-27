using System;
using Fusion;
using Fusion.KCC;
using UnityEngine;

public class PlayerMovementKinematicMovement : NetworkBehaviour
{
    public Action<Vector3> OnMove;
    private bool canMove = true;
    private NetworkRigidbody rb;
    [Networked] private Vector3 currentPosition { get; set; }
    private Vector3 moveDelta;
    private Vector3 initialPosition;
    private Vector3 lastPosition;
    private Ship ship;
    private ShipMovement shipMovement;
    private void Awake()
    {

        rb = GetComponent<NetworkRigidbody>();

    }
    public override void Spawned()
    {
        ship = FindObjectOfType<Ship>();
        shipMovement = ship.GetComponent<ShipMovement>();
        currentPosition = transform.position;
        initialPosition = currentPosition;
        lastPosition = currentPosition;
    }
    public void SetCanMove(bool canMove)
    {

        this.canMove = canMove;
    }

    public override void FixedUpdateNetwork()
    {
        moveDelta = Vector3.zero;

        if (GetInput(out NetworkInputData networkInputData) && canMove)
        {
            moveDelta = new Vector3(networkInputData.MoveDirection.x, 0, networkInputData.MoveDirection.y);
        }

        // currentPosition = transform.position;


        currentPosition += moveDelta * Runner.DeltaTime * 5;
        OnMove?.Invoke(moveDelta);
        UpdateMovementToShipPosition();
    }

    private void UpdateMovementToShipPosition()
    {
        Vector3 playerToShipCenterDirection = currentPosition - ship.transform.position;
        float distanceFromShip = playerToShipCenterDirection.magnitude;


        float angle = Vector3.SignedAngle(Vector3.forward, playerToShipCenterDirection.normalized, Vector3.up);

        angle += shipMovement.rotationDegree;


        float angleInRadians = Mathf.Deg2Rad * angle;


        Vector3 positionOffset = new Vector3(Mathf.Sin(angleInRadians), 0, Mathf.Cos(angleInRadians)) * distanceFromShip;
        Debug.Log("Distance from ship " + distanceFromShip);
        currentPosition += shipMovement.MoveDelta;
      
        
      //  Debug.Log("currentPosition: " + currentPosition + " ship movedelta: " + shipMovement.MoveDelta + " movedelta: " + moveDelta + " position offset: " + positionOffset);
        rb.Rigidbody.MovePosition(currentPosition);


    }

}

