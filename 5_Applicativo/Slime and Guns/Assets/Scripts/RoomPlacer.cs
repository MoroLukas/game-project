using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomPlacer : MonoBehaviour
{
    public GameObject startingRoom;   // prefab della stanza iniziale
    public GameObject[] otherRooms;   // array di prefab delle altre stanze
    public GameObject player;
    public Transform gridTransform;
    public Vector2Int gridSize = new Vector2Int(150, 150); // dimensione della griglia
    public int maxAttempts = 100;

    private List<Bounds> placedRooms = new List<Bounds>();

    void Start()
    {
        PlaceRooms();
    }

    void PlaceRooms()
    {
        // Posiziona la starting room in basso
        Vector2Int startPos = new Vector2Int(
            Random.Range(0, gridSize.x - 1),
            0 
        );
        // Genera la starting room dentro la Grid
        Vector3 worldPos = gridTransform.GetComponent<Grid>().CellToWorld(new Vector3Int(startPos.x, startPos.y, 0));
        GameObject startObj = Instantiate(startingRoom, worldPos, Quaternion.identity, gridTransform);

        // Usa il collider principale della stanza
        Bounds startRoomBounds = startObj.GetComponent<Collider2D>().bounds;

        // Posiziona il giocatore al centro della stanza
        player.transform.position = new Vector3(
            Mathf.Round(startRoomBounds.center.x),
            Mathf.Round(startRoomBounds.center.y),
            0
        );

        // Salva i bounds per evitare sovrapposizioni
        placedRooms.Add(startRoomBounds);

        // Posiziona le altre stanze
        foreach (GameObject roomPrefab in otherRooms)
        {
            bool placed = false;
            int attempts = 0;

            while (!placed && attempts < maxAttempts)
            {
                Vector2Int pos = new Vector2Int(
                    Random.Range(0, gridSize.x - 1),
                    Random.Range(0, gridSize.y - 1)
                );

                Vector3 roomWorldPos = gridTransform.GetComponent<Grid>().CellToWorld(new Vector3Int(pos.x, pos.y, 0));
                GameObject roomObj = Instantiate(roomPrefab, roomWorldPos, Quaternion.identity, gridTransform);
                Bounds roomBounds = roomObj.GetComponent<Collider2D>().bounds;

                bool overlaps = false;
                foreach (Bounds b in placedRooms)
                {
                    if (b.Intersects(roomBounds))
                    {
                        overlaps = true;
                        break;
                    }
                }

                if (overlaps)
                {
                    Destroy(roomObj);
                    attempts++;
                }
                else
                {
                    placedRooms.Add(roomBounds);
                    placed = true;
                }
            }
        }
    }
}