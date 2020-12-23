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

    private Room[,] rooms;
    private List<Vector2> occupied_positions;
    private Queue<Room> room_queue;
    private bool[] empty_room_position;
    private float room_size = 2;
    private int number_room;

    private void Awake()
    {
        rooms = new Room[map_size_x * 2, map_size_y * 2];

        room_queue = new Queue<Room>();

        occupied_positions = new List<Vector2>();

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
        CreateRoom(Vector2.zero, map_size_x, map_size_y, TypeRoom.Entry, 0, null);

        CreateRooms();

        GenerateRoomObjects();
    }

    private void CreateRooms()
    {
        Room room_to_create_neighbors;
        int number_neighbors;
        int number_of_rooms;

        while (true)
        {
            room_to_create_neighbors = room_queue.Dequeue();

            number_neighbors = GetEmptyNeighborsCount(room_to_create_neighbors.GridPosition.x, room_to_create_neighbors.GridPosition.y);

            number_of_rooms = Random.Range(0, number_neighbors);

            CreateRoom(number_of_rooms, room_to_create_neighbors);

            if (room_queue.Count == 0)
            {
                break;
            }
        }
    }

    private void CreateRoom(int number_of_rooms, Room current_room)
    {
        Vector2 position;
        (int x, int y) grid_position;

        for (int i = 0; i < empty_room_position.Length; i++)
        {
            if (empty_room_position[i] == true)
            {
                position = GetPosition(current_room, i);

                grid_position = GetGridPosition(current_room, i);

                CreateRoom(position, grid_position.x, grid_position.y, TypeRoom.Normal, i, current_room);
            }
        }
    }

    private void CreateRoom(Vector2 position, int x, int y, TypeRoom room_type, int empty_room_index, Room current_room)
    {
        Room new_room = new Room(position, (x, y), room_type);

        if (current_room != null)
        {
            SetDoors(new_room, current_room, empty_room_index);
        }

        rooms[x, y] = new_room;

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

        if ((is_valid) && (x >= map_size_x * 2))
        {
            is_valid = false;
        }

        if ((is_valid) && (y < 0))
        {
            is_valid = false;
        }
        
        if ((is_valid) && (y >= map_size_y * 2))
        {
            is_valid = false;
        }

        if ((is_valid) && (occupied_positions.Contains(new Vector2(x, y))))
        {
            is_valid = true;
        }

        return (is_valid);
    }

    private Vector2 GetPosition(Room current_room, int position_index)
    {
        Vector2 position;

        if (position_index == 0)
        {
            position = new Vector2(current_room.Position.x - room_size, current_room.Position.y);
        }
        else if (position_index == 1)
        {
            position = new Vector2(current_room.Position.x + room_size, current_room.Position.y);
        }
        else if (position_index == 2)
        {
            position = new Vector2(current_room.Position.x, current_room.Position.y - room_size);
        }
        else
        {
            position = new Vector2(current_room.Position.x, current_room.Position.y + room_size);
        }

        return (position);
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
            new_room.AddDoor(TypeDoor.LeftDoor);
            current_room.AddDoor(TypeDoor.RightDoor);
        }
        else if (empty_room_index == 1)
        {
            new_room.AddDoor(TypeDoor.RightDoor);
            current_room.AddDoor(TypeDoor.LeftDoor);
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

        for (int i = 0; i < map_size_x * 2; i++)
        {
            for (int j = 0; j < map_size_y * 2; j++)
            {
                if (rooms[i,j] != null)
                {
                    room_object = GameObject.Instantiate(room_template, rooms[i, j].Position, Quaternion.identity);

                    room_object.GetComponent<RoomSpriteSelection>().SetSprite(rooms[i,j].OpenDoors);

                    room_object.transform.SetParent(map.transform);
                }
            }
        }
    }
}
