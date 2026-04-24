using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject startingRoom;
    public GameObject[] roomPrefabs;

    public GameObject corridorHorizontal;
    public GameObject corridorVertical;

    public float padding = 0.4f; // spazio extra tra stanze (IN WORLD SPACE)

    private GameObject currentRoom;

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        currentRoom = Instantiate(startingRoom, Vector3.zero, Quaternion.identity);

        for (int i = 0; i < 10; i++)
        {
            Vector2Int dir = GetDir();

            GameObject prefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
            GameObject newRoom = Instantiate(prefab);

            if (!PlaceRoom(currentRoom, newRoom, dir))
            {
                Destroy(newRoom);
                continue;
            }

            SpawnCorridor(currentRoom, newRoom);

            currentRoom = newRoom;
        }
    }

    bool PlaceRoom(GameObject a, GameObject b, Vector2Int dir)
    {
        RoomData A = a.GetComponent<RoomData>();
        RoomData B = b.GetComponent<RoomData>();

        Vector3 offset = Vector3.zero;

        float ax = A.size.x;
        float ay = A.size.y;
        float bx = B.size.x;
        float by = B.size.y;

        if (dir == Vector2Int.right)
            offset = new Vector3((ax + bx) / 2f + padding, 0, 0);

        if (dir == Vector2Int.left)
            offset = new Vector3(-((ax + bx) / 2f + padding), 0, 0);

        if (dir == Vector2Int.up)
            offset = new Vector3(0, (ay + by) / 2f + padding, 0);

        if (dir == Vector2Int.down)
            offset = new Vector3(0, -((ay + by) / 2f + padding), 0);

        b.transform.position = a.transform.position + offset;

        return true;
    }

    void SpawnCorridor(GameObject a, GameObject b)
    {
        Vector3 mid = (a.transform.position + b.transform.position) / 2f;

        Vector2 diff = b.transform.position - a.transform.position;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
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