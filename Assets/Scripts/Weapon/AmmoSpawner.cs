using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class AmmoSpawner : NetworkBehaviour
{
    [Networked] private TickTimer spawnTime { get; set; }
    [SerializeField] private AmmoPickup ammoPickupPrefab;
    [SerializeField] private float minSpawnX, maxSpawnX, minSpawnZ, maxSpawnZ;
    [SerializeField] private float spawnTimeDuration = 1f;
    public override void Spawned()
    {
        if (Object.HasStateAuthority)
            spawnTime = TickTimer.CreateFromSeconds(Runner, spawnTimeDuration);
    }
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            if (spawnTime.ExpiredOrNotRunning(Runner))
            {
                SpawnAmmo();
            }
        }
    }

    private void SpawnAmmo()
    {
        spawnTime = TickTimer.CreateFromSeconds(Runner, spawnTimeDuration);
        Runner.Spawn(ammoPickupPrefab, new Vector3(Random.Range(minSpawnX, maxSpawnX), 1, Random.Range(minSpawnZ, maxSpawnZ)), Quaternion.identity);
       
    }
}
