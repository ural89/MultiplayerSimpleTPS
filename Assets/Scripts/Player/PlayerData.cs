using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public struct PlayerData : INetworkStruct //TODO: create a component playerdatahandler.

{
    // The easiest and cleanest way to use a string in an INetworkStruct
    // is to use the Fusion NetworkString<> type as a field.
    // _16 here indicates that we are allocating a capacity of 16 characters.
    public NetworkString<_16> Name;

    // Optionally a regular string can be used as a Property (not a field)
    [Networked]       // Notifies the ILWeaver to extend this property
    [Capacity(16)]    // allocates memory for 16 characters
    [UnityMultiline]  // Optional attribute to force multi-line in inspector.
    public string StringProperty { get => default; set { } }
}


