using Fusion;
using UnityEngine;
public class Player : NetworkBehaviour
{
    [SerializeField] private Renderer _renderer;
    public Renderer Renderer => _renderer;
    [Networked(OnChanged = nameof(ColorChange))] private Color color { get; set; }

    private static void ColorChange(Changed<Player> changed)
    {
        changed.Behaviour.Renderer.material.color = changed.Behaviour.color;
    }

   
    public override void Spawned()
    {
        color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
    }
}
