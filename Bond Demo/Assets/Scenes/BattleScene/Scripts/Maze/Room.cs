using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private (int x, int y) grid_position;
    private TypeRoom room_type;
    private List<TypeDoor> open_doors;

    public Room((int x, int y) _grid_position, TypeRoom _room_type)
    {
        grid_position = _grid_position;
        room_type = _room_type;

        open_doors = new List<TypeDoor>();
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
        if (open_doors.Contains(door))
        {
            return;
        }

        open_doors.Add(door);
    }
}
