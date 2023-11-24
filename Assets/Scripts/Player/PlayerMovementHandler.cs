using Fusion;
using Fusion.KCC;
using UnityEngine;

public class PlayerMovementHandler : NetworkBehaviour
{
    [SerializeField] private GameObject head;

    [Networked(OnChanged = nameof(OnHeadActivationChanged))] private NetworkBool isActive { get; set; } = true;

    private static void OnHeadActivationChanged(Changed<PlayerMovementHandler> changed)
    {
        //changed.Behaviour.head.SetActive(changed.Behaviour.isActive);
    }

    private float moveSpeed = 3f;
    private PlayerInputHandler playerInputHandler;
 
    private KCC kcc;
    private void Awake()
    {
    
        playerInputHandler = GetComponent<PlayerInputHandler>();
        kcc = GetComponent<KCC>();

    }
   
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData networkInputData))
        {

            kcc.SetInputDirection(new Vector3(networkInputData.MoveDirection.x, 0, networkInputData.MoveDirection.y));
            if (!networkInputData.LookDirection.AlmostEquals(Vector3.zero))
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(networkInputData.LookDirection.x, 0, networkInputData.LookDirection.y), Vector3.up);
                kcc.SetLookRotation(lookRotation);
            }
            if (networkInputData.IsFirePressed)
            {
                isActive = !isActive;

            }

        }
    }

   
}

