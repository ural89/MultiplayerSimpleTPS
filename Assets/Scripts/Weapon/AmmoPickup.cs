using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class AmmoPickup : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnHasPickedChanged))] public NetworkBool HasPicked { get; set; } = false;
    [SerializeField] public Renderer _renderer;

    private static void OnHasPickedChanged(Changed<AmmoPickup> changed)
    {
        // changed.Behaviour._renderer.enabled = false;
        // Debug.Log("Picked up " + changed.Behaviour.HasPicked);
    }
  
    public void OnPicked(Player player)
    {
        if (HasPicked) return;
        // if(player.Object.HasStateAuthority)
            _renderer.enabled = false;
        HasPicked = true;

    }
    public void PickedHasChanged()
    {
        /*   //  if (Object.HasStateAuthority)
          Debug.Log("Changing render");
          _renderer.enabled = false;
          // Runner.Despawn(Object); */
    }
}
