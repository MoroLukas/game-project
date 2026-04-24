using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public Vector2 size; //larghezza e altezza della stanza

    public Transform doorUp;
    public Transform doorDown;
    public Transform doorLeft;
    public Transform doorRight;

    public bool hasUp;
    public bool hasDown;
    public bool hasLeft;
    public bool hasRight;
}