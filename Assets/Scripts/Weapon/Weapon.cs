using Fusion;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [SerializeField] private Transform projectileSlot;
    [SerializeField] private Projectile projectile;
    [SerializeField] private float fireSpeed = 20f;
    private AmmoAmount ammoAmount;

    private void Awake()
    {
        ammoAmount = GetComponentInParent<AmmoAmount>();
    }
    public void Fire(PlayerRef owner)
    {
        if (!ammoAmount.HasAmmo) return;
        NetworkObjectPredictionKey key = new NetworkObjectPredictionKey { Byte0 = (byte)owner.RawEncoded, Byte1 = (byte)Runner.Simulation.Tick };

        void InitProjectile(NetworkRunner r, NetworkObject o)
        {
            o.GetComponent<Projectile>().Init(projectileSlot.position, transform.forward * fireSpeed);
        }

        Runner.Spawn(projectile, projectileSlot.position, projectileSlot.rotation, owner, InitProjectile
        , key);
        ammoAmount.SpendAmmo(1);
    }
}
