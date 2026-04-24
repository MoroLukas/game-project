using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Prefab stanze")]
    public GameObject[] roomPrefabs;

    [Header("Numero stanze")]
    public int roomCount = 8;

    [Header("Padding minimo tra stanze (in unità world)")]
    public float padding = 2f;

    [Header("Area di spawn")]
    public Vector2 spawnArea = new Vector2(120f, 80f);

    [Header("Prefab corridoio (sprite 1x1 scalabile)")]
    public GameObject corridorPrefab;

    [Header("Padding extra per i corridoi")]
    public float corridorWidth = 1.5f;

    private List<GameObject> spawnedRooms = new List<GameObject>();
    private List<Bounds> spawnedBounds = new List<Bounds>();

    void Start() => GenerateDungeon();

    public void GenerateDungeon()
    {
        foreach (var r in spawnedRooms) Destroy(r);
        spawnedRooms.Clear();
        spawnedBounds.Clear();

        PlaceRooms();
        ConnectRoomsWithMST();
    }

    // ── Placement con Bounds reali ──────────────────────────────────────────

    void PlaceRooms()
    {
        int maxAttempts = 300;

        for (int i = 0; i < roomCount; i++)
        {
            GameObject prefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];

            // Istanzia temporaneamente per leggere i bounds reali PRIMA di piazzare
            var temp = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            var data = temp.GetComponent<RoomData>();
            if (data == null) data = temp.AddComponent<RoomData>();
            data.RecalculateBounds();

            // Dimensione reale della stanza
            Vector3 size = data.bounds.size;
            Destroy(temp);

            bool placed = false;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                Vector3 pos = new Vector3(
                    Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f),
                    Random.Range(-spawnArea.y / 2f, spawnArea.y / 2f),
                    0f
                );

                // Bounds candidato nella posizione tentata
                Bounds candidate = new Bounds(pos, size);
                // Espandi del padding
                candidate.Expand(padding);

                if (!OverlapsExisting(candidate))
                {
                    // Piazza per davvero
                    var room = Instantiate(prefab, pos, Quaternion.identity, transform);
                    room.name = $"Room_{i}_{prefab.name}";

                    var roomData = room.GetComponent<RoomData>();
                    if (roomData == null) roomData = room.AddComponent<RoomData>();
                    roomData.RecalculateBounds();

                    spawnedRooms.Add(room);
                    // Salva bounds SENZA padding per i corridoi
                    spawnedBounds.Add(new Bounds(pos, size));

                    placed = true;
                    break;
                }
            }

            if (!placed)
                Debug.LogWarning($"[DungeonGen] Stanza {i} ({prefab.name}) non piazzata — prova ad allargare spawnArea.");
        }
    }

    bool OverlapsExisting(Bounds candidate)
    {
        foreach (var b in spawnedBounds)
        {
            // Bounds.Intersects già gestisce rettangoli di qualsiasi dimensione
            Bounds padded = b;
            padded.Expand(padding);
            if (candidate.Intersects(padded))
                return true;
        }
        return false;
    }

    // ── MST + corridoi ──────────────────────────────────────────────────────

    void ConnectRoomsWithMST()
    {
        if (spawnedRooms.Count < 2) return;

        var connected = new List<int> { 0 };
        var remaining = new List<int>();
        for (int i = 1; i < spawnedRooms.Count; i++) remaining.Add(i);

        while (remaining.Count > 0)
        {
            int bestFrom = -1, bestTo = -1;
            float bestDist = float.MaxValue;

            foreach (int c in connected)
                foreach (int r in remaining)
                {
                    float d = Vector3.Distance(
                        spawnedRooms[c].transform.position,
                        spawnedRooms[r].transform.position);
                    if (d < bestDist) { bestDist = d; bestFrom = c; bestTo = r; }
                }

            // Connetti dal bordo della stanza, non dal centro
            Vector3 exitA = ClosestBorderPoint(spawnedBounds[bestFrom],
                                               spawnedRooms[bestTo].transform.position);
            Vector3 exitB = ClosestBorderPoint(spawnedBounds[bestTo],
                                               spawnedRooms[bestFrom].transform.position);

            SpawnCorridor(exitA, exitB);

            connected.Add(bestTo);
            remaining.Remove(bestTo);
        }
    }

    // Trova il punto sul bordo del bounds più vicino a un target
    Vector3 ClosestBorderPoint(Bounds b, Vector3 target)
    {
        Vector3 dir = (target - b.center).normalized;
        // Proietta la direzione sulle 4 facce del rettangolo
        float scaleX = (dir.x != 0) ? Mathf.Abs(b.extents.x / dir.x) : float.MaxValue;
        float scaleY = (dir.y != 0) ? Mathf.Abs(b.extents.y / dir.y) : float.MaxValue;
        float scale = Mathf.Min(scaleX, scaleY);
        return b.center + dir * scale;
    }

    void SpawnCorridor(Vector3 from, Vector3 to)
    {
        if (corridorPrefab == null) return;

        Vector3 mid = (from + to) / 2f;
        float length = Vector3.Distance(from, to);
        float angle = Mathf.Atan2(to.y - from.y, to.x - from.x) * Mathf.Rad2Deg;

        var corridor = Instantiate(corridorPrefab, mid, Quaternion.Euler(0, 0, angle), transform);
        corridor.transform.localScale = new Vector3(length, corridorWidth, 1f);
    }

    // ── Gizmos: visualizza bounds e connessioni in Editor ──────────────────

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.3f);
        foreach (var b in spawnedBounds)
            Gizmos.DrawWireCube(b.center, b.size);
    }
}