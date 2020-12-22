using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private Vector2 position;
    private TypeRoom room_type;

    public Room(Vector2 _position, TypeRoom _room_type)
    {
        position = _position;
        room_type = _room_type;
    }
}
