using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ShipMovement : NetworkBehaviour
{
   public void UpdateInput(Vector2 movementInput)
   {
        Debug.Log("Movement recieved " + movementInput);
   }
}
