using Fusion;
using UnityEngine;
public class Player : NetworkBehaviour
{
    [SerializeField] private Renderer _renderer;
    [Networked(OnChanged = nameof(ColorChange))] private Color color { get; set; }
    public Renderer Renderer => _renderer;

    public static Player Local { get; private set; }
    private PlayerCameraHandler cameraHandler;
    private void Awake()
    {
        cameraHandler = GetComponent<PlayerCameraHandler>();
    }

    private static void ColorChange(Changed<Player> changed)
    {
        changed.Behaviour.Renderer.material.color = changed.Behaviour.color;
    }


    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
        }
        color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        cameraHandler.UpdateCameraSettings();
    }
}
