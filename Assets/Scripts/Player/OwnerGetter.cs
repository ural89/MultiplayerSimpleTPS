using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class OwnerGetter : MonoBehaviour
{
    private PlayerRef owner;
    public PlayerRef GetOwner => owner;
    public void SetOwner(PlayerRef owner) => this.owner = owner;
}
