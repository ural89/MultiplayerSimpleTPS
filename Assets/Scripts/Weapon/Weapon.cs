using Fusion;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [SerializeField] private Transform projectileSlot;
    [SerializeField] private Projectile projectile;
    private float moveSpeed = 10f;
    public void Fire(PlayerRef owner)
    {
        NetworkObjectPredictionKey key = new NetworkObjectPredictionKey { Byte0 = (byte)owner.RawEncoded, Byte1 = (byte)Runner.Simulation.Tick };
        
        void InitProjectile(NetworkRunner r, NetworkObject o)
        {
            o.GetComponent<Projectile>().Init(projectileSlot.position, transform.forward * moveSpeed);
        }

        Runner.Spawn(projectile, projectileSlot.position, projectileSlot.rotation, owner, InitProjectile
        , key);
    }
}
