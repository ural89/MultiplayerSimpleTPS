using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class RotateConstantly : NetworkBehaviour
{
    Rigidbody m_Rigidbody;
    Vector3 m_EulerAngleVelocity;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody>();

        //Set the angular velocity of the Rigidbody (rotating around the Y axis, 100 deg/sec)
        m_EulerAngleVelocity = new Vector3(0, 30, 0);
    }
    public override void FixedUpdateNetwork()
    {
        if (Input.GetKey(KeyCode.W))
        {
            m_Rigidbody.MovePosition(transform.position + transform.forward * 3 * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.S))
        {
            m_Rigidbody.MovePosition(transform.position - transform.forward * 3 * Time.deltaTime);

        }
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);
    }
   
}
