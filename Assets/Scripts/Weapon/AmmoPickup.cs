using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class AmmoPickup : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnHasPickedChanged))] private NetworkBool hasPicked { get; set; } = false;


    private static void OnHasPickedChanged(Changed<AmmoPickup> changed)
    {
        changed.Behaviour.PickedHasChanged();
    }
    public void PickedHasChanged()
    {
        Runner.Despawn(Object);
    }
    public void OnPicked()
    {
        if (Object.HasStateAuthority)
            hasPicked = true;
     
    }
}
