using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    Vector2 moveInput = Vector2.zero;
    Vector2 lookDirectionInput = Vector2.zero;
    bool isFireButtonPressed = false;

    // Update is called once per frame
    void Update()
    {
        //move direction
        //TODO: if player.Object.HasInputAuth
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        //look direction
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            
            // LookAtHitDirection(hit);
        }


        if (Input.GetMouseButtonDown(0))
            isFireButtonPressed = true;
    }

    private void LookAtHitDirection(RaycastHit hit)
    {
        Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        var normDirection = (targetPosition - transform.position).normalized;
        lookDirectionInput = new Vector2(normDirection.x, normDirection.z);


        Quaternion targetRotation = Quaternion.LookRotation(lookDirectionInput);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
    }

    public NetworkInputData GetNetworkInputData()
    {
        var networkInputData = new NetworkInputData();

        networkInputData.LookDirection = lookDirectionInput;

        networkInputData.MoveDirection = moveInput; //TODO: look at mouse position

        networkInputData.IsFirePressed = isFireButtonPressed;
        isFireButtonPressed = false;

        return networkInputData;

    }
}
