using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManagement : MonoBehaviour
{
    [SerializeField] int number_of_rooms_to_generate;
    [SerializeField] int map_size_x;
    [SerializeField] int map_size_y;
    [SerializeField] GameObject room_template;
    [SerializeField] GameObject map;

    private Grid<Room> rooms;
    private List<(int x, int y)> occupied_positions;
    private Queue<Room> room_queue;
    private bool[] empty_room_position;
    private float room_size = 2;
    private int number_room;

    private void Awake()
    {
        rooms = new Grid<Room>(map_size_x, map_size_y, room_size, new Vector2(-10.5f, -4f));

        room_queue = new Queue<Room>();

        occupied_positions = new List<(int x, int y)>();

        empty_room_position = new bool[4];

        CreateMap();
    }

    private void CreateMap()
    {
        number_room = 0;

        if (number_of_rooms_to_generate > rooms.Length)
        {
            number_of_rooms_to_generate = rooms.Length / 2;
        }

        // Create an entry room
        (int x, int y) entry_room_position = (Mathf.RoundToInt(map_size_x / 2), Mathf.RoundToInt(map_size_y / 2));
        CreateRoom(entry_room_position.x, entry_room_position.y, TypeRoom.Entry, 0, null);

        CreateRooms();

        GenerateRoomObjects();
    }

    private void CreateRooms()
    {
        Room room_to_create_neighbors;
        int number_neighbors;
        int number_of_rooms = 0;

        while (true)
        {
            room_to_create_neighbors = room_queue.Dequeue();

            number_neighbors = GetEmptyNeighborsCount(room_to_create_neighbors.GridPosition.x, room_to_create_neighbors.GridPosition.y);

            if (number_neighbors > 0)
            {
                number_of_rooms = Random.Range(1, number_neighbors + 1);
            }

            CreateRoom(number_of_rooms, room_to_create_neighbors);

            if (room_queue.Count == 0)
            {
                break;
            }
        }
    }

    private void CreateRoom(int number_of_rooms, Room current_room)
    {
        (int x, int y) grid_position;
        int rand;

        for (int i = 0; i < number_of_rooms; i++)
        {
            do
            {
                rand = Random.Range(0, 4);
            } while (empty_room_position[rand] == false);

            if (empty_room_position[i] == true)
            {
                grid_position = GetGridPosition(current_room, i);

                CreateRoom(grid_position.x, grid_position.y, TypeRoom.Normal, i, current_room);
            }
        }
    }

    private void CreateRoom(int x, int y, TypeRoom room_type, int empty_room_index, Room current_room)
    {
        Room new_room = new Room((x, y), room_type);

        if (current_room != null)
        {
            SetDoors(new_room, current_room, empty_room_index);
        }

        rooms.SetValue(x, y, new_room);
        occupied_positions.Add((x, y));

        if (number_room < number_of_rooms_to_generate)
        {
            room_queue.Enqueue(new_room);
        }

        number_room++;
    }

    private int GetEmptyNeighborsCount(int x, int y)
    {
        int number_of_empty_neighbors = 0;

        if (CheckPosition(x - 1, y))
        {
            number_of_empty_neighbors++;

            empty_room_position[0] = true;
        }
        else
        {
            empty_room_position[0] = false;
        }

        if (CheckPosition(x + 1, y))
        {
            number_of_empty_neighbors++;

            empty_room_position[1] = true;
        }
        else
        {
            empty_room_position[1] = false;
        }

        if (CheckPosition(x, y - 1))
        {
            number_of_empty_neighbors++;

            empty_room_position[2] = true;
        }
        else
        {
            empty_room_position[2] = false;
        }

        if (CheckPosition(x, y + 1))
        {
            number_of_empty_neighbors++;

            empty_room_position[3] = true;
        }
        else
        {
            empty_room_position[3] = false;
        }

        return (number_of_empty_neighbors);
    }

    private bool CheckPosition(int x, int y)
    {
        bool is_valid = true;

        if ((is_valid) && (x < 0))
        {
            is_valid = false;
        }

        if ((is_valid) && (x >= map_size_x))
        {
            is_valid = false;
        }

        if ((is_valid) && (y < 0))
        {
            is_valid = false;
        }
        
        if ((is_valid) && (y >= map_size_y))
        {
            is_valid = false;
        }

        if ((is_valid) && (occupied_positions.Contains((x, y))))
        {
            is_valid = false;
        }

        return (is_valid);
    }

    private (int x, int y) GetGridPosition(Room current_room, int position_index)
    {
        (int x, int y) position;

        if (position_index == 0)
        {
            position = (current_room.GridPosition.x - 1, current_room.GridPosition.y);
        }
        else if (position_index == 1)
        {
            position = (current_room.GridPosition.x + 1, current_room.GridPosition.y);
        }
        else if (position_index == 2)
        {
            position = (current_room.GridPosition.x, current_room.GridPosition.y - 1);
        }
        else
        {
            position = (current_room.GridPosition.x, current_room.GridPosition.y + 1);
        }

        return (position);
    }

    private void SetDoors(Room new_room, Room current_room, int empty_room_index)
    {
        if (empty_room_index == 0)
        {
            new_room.AddDoor(TypeDoor.RightDoor);
            current_room.AddDoor(TypeDoor.LeftDoor);
        }
        else if (empty_room_index == 1)
        {
            new_room.AddDoor(TypeDoor.LeftDoor);
            current_room.AddDoor(TypeDoor.RightDoor);
        }
        else if (empty_room_index == 2)
        {
            new_room.AddDoor(TypeDoor.TopDoor);
            current_room.AddDoor(TypeDoor.BottomDoor);
        }
        else
        {
            new_room.AddDoor(TypeDoor.BottomDoor);
            current_room.AddDoor(TypeDoor.TopDoor);
        }
    }

    private void GenerateRoomObjects()
    {
        GameObject room_object;
        Room room_to_create;

        for (int i = 0; i < map_size_x; i++)
        {
            for (int j = 0; j < map_size_y; j++)
            {
                if (rooms.GetValue(i,j) != null)
                {
                    room_object = GameObject.Instantiate(room_template, rooms.GetGridPositionInWorldPosition(i, j), Quaternion.identity);

                    room_to_create = rooms.GetValue(i, j);

                    //Debug.Log("GRID " + room_to_create.GridPosition + " WORLD" + rooms.GetGridPositionInWorldPosition(i, j));

                    room_object.GetComponent<RoomSpriteSelection>().SetSprite(room_to_create.OpenDoors, room_to_create.RoomType);

                    room_object.transform.SetParent(map.transform);
                }
            }
        }
    }
}
