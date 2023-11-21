using Fusion;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [SerializeField] private Transform projectileSlot;
    [SerializeField] private Projectile projectile;
    [SerializeField] private float fireSpeed = 20f;
    [SerializeField] private ParticleSystem fireParticle;
    [SerializeField] private AudioSource fireSFX;
    [Networked(OnChanged = nameof(OnHasFiredChanged))] private NetworkBool hasFired { get; set; }

    private static void OnHasFiredChanged(Changed<Weapon> changed)
    {
        changed.Behaviour.HasFiredChanged();
    }
    private void HasFiredChanged()
    {
        fireParticle.Play();
        fireSFX.pitch = Random.Range(0.8f, 1.2f);
        fireSFX.Play();
    }
    private AmmoAmount ammoAmount;

    private void Awake()
    {
        ammoAmount = GetComponentInParent<AmmoAmount>();
    }
    public void Fire(PlayerRef owner)
    {
        if (!ammoAmount.HasAmmo) return;
        hasFired = !hasFired;
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
