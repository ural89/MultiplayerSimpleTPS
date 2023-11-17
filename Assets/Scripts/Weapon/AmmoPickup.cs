using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoPickup : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnHasPickedChanged))] private NetworkBool HasPicked { get; set; } = false;
    [SerializeField] private Renderer _renderer;

    private static void OnHasPickedChanged(Changed<AmmoPickup> changed)
    {
        //changed.Behaviour._renderer.enabled = !changed.Behaviour.HasPicked;
        changed.Behaviour.EnableObject(!changed.Behaviour.HasPicked);
        // Debug.Log("Picked up " + changed.Behaviour.HasPicked);
    }
    public void EnableObject(bool isEnabled)
    {
        gameObject.SetActive(isEnabled);
    }
    public override void Spawned()
    {
       // _renderer.enabled = !HasPicked;
    }
    public void OnPicked(Player player)
    {

        if (HasPicked) return;
        //if(player.Object.HasStateAuthority)
        // _renderer.enabled = false;
        HasPicked = true;


    }

}
