using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoPickup : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnHasPickedChanged))]private NetworkBool HasPicked { get; set; } = false;


    private static void OnHasPickedChanged(Changed<AmmoPickup> changed)
    {
        if (changed.Behaviour != null)
            changed.Behaviour.EnableObject(!changed.Behaviour.HasPicked);
    }
    public void EnableObject(bool isEnabled)
    {
        gameObject.SetActive(isEnabled);
    }
    // [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void OnPicked(Player player)
    {

        if (HasPicked) return;
        HasPicked = true;
        // Runner.Despawn(Object);
        // return;
        gameObject.SetActive(false);
    }

}
