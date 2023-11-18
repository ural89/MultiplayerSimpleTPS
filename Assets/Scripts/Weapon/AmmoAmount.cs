using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class AmmoAmount : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnAmmoAmountChanged))] private int ammoAmount { get; set; } = 3;



    [SerializeField] private GameObject ammoPickupMesh;
    [SerializeField] private Transform ammoPickupSlot;
    private Stack<GameObject> ammoPickupMeshes = new();
    public int GetAmmoAmount => ammoAmount;
    public bool HasAmmo => ammoAmount > 0;
    public void SpendAmmo(int ammoToSpend)
    {
        ammoAmount -= ammoToSpend;
        UpdateAmmoMeshes();
    }
    public void AddAmmo(int ammoToAdd)
    {
        ammoAmount += ammoToAdd;
        if (Object.HasInputAuthority)
        {
            Debug.Log(ammoAmount);
        }
        UpdateAmmoMeshes();
    }
    public override void Spawned()
    {

        UpdateAmmoMeshes();
    }
    private static void OnAmmoAmountChanged(Changed<AmmoAmount> changed)
    {
        changed.Behaviour.UpdateAmmoMeshes();
    }
    private void UpdateAmmoMeshes()
    {

        var meshesToCreate = ammoAmount - ammoPickupMeshes.Count;
        if (meshesToCreate > 0)
        {
            for (int i = 0; i < meshesToCreate; i++)
            {
                var mesh = Instantiate(ammoPickupMesh, ammoPickupSlot);
                mesh.transform.localPosition = Vector3.up * ammoPickupMeshes.Count;
                ammoPickupMeshes.Push(mesh);

            }
        }
        else
        {
            var meshesToRemove = ammoPickupMeshes.Count - ammoAmount;
            if (meshesToRemove > 0)
            {
                for (int i = 0; i < meshesToRemove; i++)
                {
                    var meshToRemove = ammoPickupMeshes.Pop();
                    Destroy(meshToRemove);

                }
            }
        }

    }

}
