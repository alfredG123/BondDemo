using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private (int x, int y) _grid_position;
    private TypeRoom _room_type;
    private List<TypeDoor> _open_doors;
    private bool _is_visited = false;
    private int _GameObjectIndexInContainer;

    public Room((int x, int y) grid_position, TypeRoom room_type)
    {
        _grid_position = grid_position;
        _room_type = room_type;

        _open_doors = new List<TypeDoor>();
    }

    public (int x, int y) GridPosition
    {
        get => (_grid_position);
    }

    public TypeRoom RoomType
    {
        get => (_room_type);
        set => _room_type = value;
    }

    public List<TypeDoor> OpenDoors
    {
        get => (_open_doors);
    }

    public bool IsVisited
    {
        get => (_is_visited);

        set
        {
            _is_visited = value;
        }
    }

    public void AddDoor(TypeDoor door)
    {
        if (_open_doors.Contains(door))
        {
            return;
        }

        _open_doors.Add(door);
    }

    public int GameObjectIndexInContainer
    {
        get => _GameObjectIndexInContainer;

        set
        {
            _GameObjectIndexInContainer = value;
        }
    }
}
