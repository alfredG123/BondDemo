using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private Vector2 position;
    private (int x, int y) grid_position;
    private TypeRoom room_type;
    private List<TypeDoor> open_doors;

    public Room(Vector2 _position, (int x, int y) _grid_position, TypeRoom _room_type)
    {
        position = _position;
        grid_position = _grid_position;
        room_type = _room_type;

        open_doors = new List<TypeDoor>();
    }

    public Vector2 Position
    {
        get => (position);
    }

    public (int x, int y) GridPosition
    {
        get => (grid_position);
    }

    public TypeRoom RoomType
    {
        get => (room_type);
    }

    public List<TypeDoor> OpenDoors
    {
        get => (open_doors);
    }

    public void AddDoor(TypeDoor door)
    {
        if (!open_doors.Contains(door))
        {
            open_doors.Add(door);
        }
    }
}
