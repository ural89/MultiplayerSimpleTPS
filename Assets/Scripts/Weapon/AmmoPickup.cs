using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoPickup : NetworkBehaviour
{
      private NetworkBool HasPicked { get; set; } = false;
/*     [SerializeField] private Renderer _renderer;

    private static void OnHasPickedChanged(Changed<AmmoPickup> changed)
    {
        if (changed.Behaviour != null )
            changed.Behaviour.EnableObject(!changed.Behaviour.HasPicked);
    } */
    public void EnableObject(bool isEnabled)
    {
        // gameObject.SetActive(isEnabled);
    }
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_OnPicked(Player player)
    {
       
        if (HasPicked) return;
        HasPicked = true;
        // Runner.Despawn(Object);
        // return;
        gameObject.SetActive(false);
    }

}
