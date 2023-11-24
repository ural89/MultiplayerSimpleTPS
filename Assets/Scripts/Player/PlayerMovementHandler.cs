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
    private void Awake()
    {

        kcc = GetComponent<KCC>();

    }
    public override void Spawned()
    {
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
        if (GetInput(out NetworkInputData networkInputData))
        {
            moveDelta = new Vector3(networkInputData.MoveDirection.x, 0, networkInputData.MoveDirection.y);
            //kcc.SetInputDirection(new Vector3(networkInputData.MoveDirection.x, 0, networkInputData.MoveDirection.y));
            currentPosition = transform.position;
            //if (canMove)
            {
                if (!networkInputData.LookDirection.AlmostEquals(Vector3.zero))
                {
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(networkInputData.LookDirection.x, 0, networkInputData.LookDirection.y), Vector3.up);
                    kcc.SetLookRotation(lookRotation);
                }
            }


        }
        currentPosition += moveDelta * Runner.DeltaTime * 5;
        OnMove?.Invoke(moveDelta);
        UpdateMovementToShipPosition();
    }
    private void UpdateMovementToShipPosition()
    {
        kcc.Data.TargetPosition = currentPosition + ShipMovement.MoveDelta;
    }


}

