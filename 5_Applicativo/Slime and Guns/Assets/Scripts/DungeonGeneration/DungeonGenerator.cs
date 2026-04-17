using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public Grid grid;

    public GameObject startingRoom;
    public GameObject[] roomPrefabs;

    public GameObject corridorHorizontal;
    public GameObject corridorVertical;

    public int roomCount = 10;

    private HashSet<Vector2Int> used = new HashSet<Vector2Int>();

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        Vector2Int current = Vector2Int.zero;

        SpawnRoom(current, startingRoom);

        for (int i = 0; i < roomCount; i++)
        {
            Vector2Int dir = GetDir();
            Vector2Int next = current + dir;

            if (used.Contains(next))
                continue;

            SpawnRoom(next, roomPrefabs[Random.Range(0, roomPrefabs.Length)]);
            SpawnCorridor(current, next);

            current = next;
        }
    }

    void SpawnRoom(Vector2Int cell, GameObject prefab)
    {
        Vector3 pos = grid.CellToWorld(new Vector3Int(cell.x, cell.y, 0));

        GameObject room = Instantiate(prefab, pos, Quaternion.identity);

        used.Add(cell);
    }

    void SpawnCorridor(Vector2Int a, Vector2Int b)
    {
        Vector3 pa = grid.CellToWorld(new Vector3Int(a.x, a.y, 0));
        Vector3 pb = grid.CellToWorld(new Vector3Int(b.x, b.y, 0));

        Vector3 mid = (pa + pb) / 2f;

        if (a.x != b.x)
            Instantiate(corridorHorizontal, mid, Quaternion.identity);
        else
            Instantiate(corridorVertical, mid, Quaternion.identity);
    }

    Vector2Int GetDir()
    {
        int r = Random.Range(0, 4);

        return r switch
        {
            0 => Vector2Int.up,
            1 => Vector2Int.down,
            2 => Vector2Int.left,
            _ => Vector2Int.right
        };
    }
}