using UnityEngine;

public class PlayerVisualizer : MonoBehaviour
{
    public MapCoordinates MapCoords;
    public UnityCoordinates UnityCoords;
    public bool Visible;
    public SpriteRenderer spriteRenderer;
    // Use this for initialization
    void Start()
    {
        Visible = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.enabled = Visible;
    }

    internal void UpdatePosition()
    {
        UnityCoords = CoordinateHelpers.MapCoordinatesToUnity(MapCoords);
        transform.position = new Vector3(UnityCoords.X, 1f, UnityCoords.Y);
    }
}
