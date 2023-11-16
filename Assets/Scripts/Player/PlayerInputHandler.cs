using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isFireButtonPressed = false;

    // Update is called once per frame
    void Update()
    {
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");

     
        if (Input.GetButtonDown("Jump"))
            isFireButtonPressed = true;
    }
    public NetworkInputData GetNetworkInputData()
    {
        var networkInputData = new NetworkInputData();

        networkInputData.LookDirection = moveInputVector.normalized;

        networkInputData.MoveDirection = moveInputVector; //TODO: look at mouse position

        networkInputData.IsFirePressed = isFireButtonPressed;
        isFireButtonPressed = false;

        return networkInputData;

    }
}
