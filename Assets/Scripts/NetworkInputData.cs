using UnityEngine;
using Fusion;


public struct NetworkInputData : INetworkInput
{
    public Vector2 MoveDirection;
    public Vector2 LookDirection;
    public NetworkBool IsFirePressed;
}
