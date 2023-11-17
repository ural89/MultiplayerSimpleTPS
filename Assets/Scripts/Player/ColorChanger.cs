using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ColorChanger : NetworkBehaviour
{
    [SerializeField] private Renderer _renderer;
    [Networked(OnChanged = nameof(ColorChange))] private Color color { get; set; }
    public Renderer Renderer => _renderer;

    private static void ColorChange(Changed<ColorChanger> changed)
    {
        changed.Behaviour.Renderer.material.color = changed.Behaviour.color;
    }
    public override void Spawned()
    {
        color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

    }
}
