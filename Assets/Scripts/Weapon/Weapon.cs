using Fusion;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [SerializeField] private Transform projectileSlot;
    [SerializeField] private Projectile projectile;
    [Networked] private int ammoAmount { get; set; } = 3;
    private float moveSpeed = 10f;
    public void AddAmmo(int ammoToAdd)
    {
        ammoAmount += ammoToAdd;
    }
    public void Fire(PlayerRef owner)
    {
        if (ammoAmount <= 0) return;
        NetworkObjectPredictionKey key = new NetworkObjectPredictionKey { Byte0 = (byte)owner.RawEncoded, Byte1 = (byte)Runner.Simulation.Tick };

        void InitProjectile(NetworkRunner r, NetworkObject o)
        {
            o.GetComponent<Projectile>().Init(projectileSlot.position, transform.forward * moveSpeed);
        }

        Runner.Spawn(projectile, projectileSlot.position, projectileSlot.rotation, owner, InitProjectile
        , key);
        ammoAmount--;
    }
}
