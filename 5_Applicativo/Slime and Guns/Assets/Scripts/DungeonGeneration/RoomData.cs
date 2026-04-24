using UnityEngine;

public class RoomData : MonoBehaviour
{
    [Header("Lascia (0,0) per calcolo automatico dai Renderer figli")]
    public Vector2 roomSize = Vector2.zero;

    public Transform[] doorPoints;

    // Bounds calcolati a runtime
    [HideInInspector] public Bounds bounds;

    void Awake()
    {
        // Raccoglie i door points per tag
        var doors = new System.Collections.Generic.List<Transform>();
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("DoorPoint"))
                doors.Add(child);
        }
        doorPoints = doors.ToArray();

        RecalculateBounds();
    }

    public void RecalculateBounds()
    {
        // Prova prima con Tilemap
        var tilemaps = GetComponentsInChildren<UnityEngine.Tilemaps.Tilemap>();
        if (tilemaps.Length > 0)
        {
            bounds = new Bounds(transform.position, Vector3.zero);
            foreach (var tm in tilemaps)
            {
                tm.CompressBounds();
                bounds.Encapsulate(tm.localBounds.center + transform.position);
                bounds.Encapsulate(new Bounds(
                    tm.localBounds.center + transform.position,
                    tm.localBounds.size));
            }
            return;
        }

        // Fallback: usa tutti i Renderer figli
        var renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            bounds = renderers[0].bounds;
            foreach (var r in renderers)
                bounds.Encapsulate(r.bounds);
            return;
        }

        // Ultimo fallback: roomSize manuale
        bounds = new Bounds(transform.position,
                            new Vector3(roomSize.x, roomSize.y, 1f));
    }

    // Disegna i bounds in scena per debug
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}