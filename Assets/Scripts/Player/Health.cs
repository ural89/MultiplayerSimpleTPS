using System;
using Fusion;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public Action OnDie;
    [SerializeField] private AudioSource hitSFX;
    [Networked(OnChanged = nameof(OnHealthAmountChanged))] private float healthAmount { get; set; } = 10;

    private static void OnHealthAmountChanged(Changed<Health> changed)
    {
        changed.Behaviour.HealthAmountChanged();
    }
    private void HealthAmountChanged()
    {
        hitSFX.pitch = UnityEngine.Random.Range(0.8f, 1.2f); //TODO: Add this to on change health
        hitSFX.Play();
    }
    private float predictedHealthAmount;
    private bool isDead = false;
    private float fullHealth;
    public void TakeDamage(float damageTaken)
    {
        healthAmount -= damageTaken;

    }
    public float GetHealthNormilized => predictedHealthAmount / fullHealth;
    public bool IsDead => isDead;
    public override void Spawned()
    {
        fullHealth = healthAmount;
    }
    public override void FixedUpdateNetwork()
    {
        predictedHealthAmount = healthAmount;
        // if (Object.HasStateAuthority)
        if (healthAmount <= 0)
        {
            if (!isDead)
                Die();
        }
    }
    private void Die()
    {
        isDead = true;
        OnDie?.Invoke();
        // Runner.Despawn(Object);

    }
}
