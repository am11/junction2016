using UnityEngine;

public class PlayerVisualizer : MonoBehaviour
{
    public MapCoordinates MapCoords;
    public UnityCoordinates UnityCoords;
    public bool Visible;
    public SpriteRenderer spriteRenderer;
	public PlayerData data;
    // Use this for initialization
    void Start()
    {
        Visible = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.enabled = true;
    }

    internal void UpdatePosition()
    {
		MapCoords = new MapCoordinates(data.latitude, data.longitude);
        UnityCoords = CoordinateHelpers.MapCoordinatesToUnity(MapCoords);
        transform.position = new Vector3(UnityCoords.X, 1f, UnityCoords.Y);
    }
}
