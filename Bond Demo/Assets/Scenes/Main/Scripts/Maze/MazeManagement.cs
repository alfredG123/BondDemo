using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManagement : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] int min_rooms_to_generate;
    [SerializeField] int map_size_x;
    [SerializeField] int map_size_y;
    [SerializeField] GameObject room_template;
    [SerializeField] GameObject map;
    [SerializeField] GameObject player_prefab;
    [SerializeField] GameObject battle_manager;
#pragma warning restore 0649

    private Grid<Room> _rooms;
    private List<(int x, int y)> _occupied_positions;
    private Queue<Room> _room_queue;
    private bool[] _empty_room_position;
    private float _room_size = 2;
    private int _number_room;
    private (int x, int y) _player_current_position;
    private GameObject _player_object;
    private bool _need_end_room;

    private void Start()
    {
        _rooms = new Grid<Room>(map_size_x, map_size_y, _room_size, new Vector2(-10.5f, -4f));

        _room_queue = new Queue<Room>();

        _occupied_positions = new List<(int x, int y)>();

        _empty_room_position = new bool[4];

        CreateMap();
    }

    private void Update()
    {
        Room room_get_chosen;

        if (map.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                room_get_chosen = _rooms.GetValue(GeneralScripts.GetMousePositionInWorldSpace());

                if (room_get_chosen != null)
                {
                    if (CheckPlayerNeighbor(room_get_chosen))
                    {
                        _player_object.transform.position = _rooms.GetGridPositionInWorldPosition(room_get_chosen.GridPosition.x, room_get_chosen.GridPosition.y);
                        _player_current_position = room_get_chosen.GridPosition;
                        
                        if ((room_get_chosen.RoomType == TypeRoom.Normal) && (!room_get_chosen.IsVisited))
                        {
                            room_get_chosen.IsVisited = true;

                            //battle_manager.GetComponent<BattleManagement>().TriggerEncounter();
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                room_get_chosen = _rooms.GetValue(_player_object.transform.position);

                if (room_get_chosen != null)
                {
                    if (room_get_chosen.RoomType == TypeRoom.NextLevel)
                    {
                        DestoryRooms();
                    }
                }
            }

            if (map.transform.childCount == 0)
            {
                StartCoroutine("WaitForDeletionToCreation");
            }
        }
    }

    private void DestoryRooms()
    {
        for (int i = 0; i < map.transform.childCount; i++)
        {
            Destroy(map.transform.GetChild(i).gameObject);
        }

        _rooms.ClearGrid();

        _occupied_positions.Clear();
    }

    private IEnumerator WaitForDeletionToCreation()
    {
        yield return new WaitForSeconds(1f);
        
        CreateMap();
    }

    private void CreateMap()
    {
        _number_room = 0;

        _need_end_room = true;

        if (min_rooms_to_generate > _rooms.Length)
        {
            min_rooms_to_generate = _rooms.Length / 2;
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
        int min_neighbors;

        while (true)
        {
            room_to_create_neighbors = _room_queue.Dequeue();

            number_neighbors = GetEmptyNeighborsCount(room_to_create_neighbors.GridPosition.x, room_to_create_neighbors.GridPosition.y);

            if (number_neighbors > 0)
            {
                min_neighbors = 1;

                if (number_neighbors > 1)
                {
                    min_neighbors = 2;
                }

                number_of_rooms = Random.Range(min_neighbors, number_neighbors + 1);
            }

            CreateRoom(number_of_rooms, room_to_create_neighbors);

            if (_room_queue.Count == 0)
            {
                break;
            }
        }
    }

    private void CreateRoom(int number_of_rooms, Room current_room)
    {
        (int x, int y) grid_position;
        int rand;
        TypeRoom room_type;

        for (int i = 0; i < number_of_rooms; i++)
        {
            do
            {
                rand = Random.Range(0, 4);
            } while (_empty_room_position[rand] == false);

            if (_empty_room_position[i] == true)
            {
                grid_position = GetGridPosition(current_room, i);

                room_type = TypeRoom.Normal;

                if (_need_end_room)
                {
                    rand = Random.Range(0, 4);

                    if ((rand == 3) || (_number_room >= min_rooms_to_generate))
                    {
                        room_type = TypeRoom.NextLevel;

                        _need_end_room = false;
                    }
                }

                CreateRoom(grid_position.x, grid_position.y, room_type, i, current_room);
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

        _rooms.SetValue(x, y, new_room);
        _occupied_positions.Add((x, y));

        if (_number_room < min_rooms_to_generate)
        {
            _room_queue.Enqueue(new_room);
        }

        _number_room++;
    }

    private int GetEmptyNeighborsCount(int x, int y)
    {
        int number_of_empty_neighbors = 0;

        if (CheckPosition(x - 1, y))
        {
            number_of_empty_neighbors++;

            _empty_room_position[0] = true;
        }
        else
        {
            _empty_room_position[0] = false;
        }

        if (CheckPosition(x + 1, y))
        {
            number_of_empty_neighbors++;

            _empty_room_position[1] = true;
        }
        else
        {
            _empty_room_position[1] = false;
        }

        if (CheckPosition(x, y - 1))
        {
            number_of_empty_neighbors++;

            _empty_room_position[2] = true;
        }
        else
        {
            _empty_room_position[2] = false;
        }

        if (CheckPosition(x, y + 1))
        {
            number_of_empty_neighbors++;

            _empty_room_position[3] = true;
        }
        else
        {
            _empty_room_position[3] = false;
        }

        return (number_of_empty_neighbors);
    }

    private bool CheckPlayerNeighbor(Room room_to_move)
    {
        bool is_neighbor = false;

        if (room_to_move.GridPosition.y == _player_current_position.y)
        {
            if (room_to_move.GridPosition.x - 1 == _player_current_position.x)
            {
                if (room_to_move.OpenDoors.Contains(TypeDoor.LeftDoor))
                {
                    is_neighbor = true;
                }
            }
            else if (room_to_move.GridPosition.x + 1 == _player_current_position.x)
            {
                if (room_to_move.OpenDoors.Contains(TypeDoor.RightDoor))
                {
                    is_neighbor = true;
                }
            }
        }
        else if (room_to_move.GridPosition.x == _player_current_position.x)
        {
            if (room_to_move.GridPosition.y - 1 == _player_current_position.y)
            {
                if (room_to_move.OpenDoors.Contains(TypeDoor.BottomDoor))
                {
                    is_neighbor = true;
                }
            }
            else if (room_to_move.GridPosition.y + 1 == _player_current_position.y)
            {
                if (room_to_move.OpenDoors.Contains(TypeDoor.TopDoor))
                {
                    is_neighbor = true;
                }
            }
        }

        return (is_neighbor);
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

        if ((is_valid) && (_occupied_positions.Contains((x, y))))
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
                if (_rooms.GetValue(i,j) != null)
                {
                    room_object = GameObject.Instantiate(room_template, _rooms.GetGridPositionInWorldPosition(i, j), Quaternion.identity);

                    room_to_create = _rooms.GetValue(i, j);

                    if (room_to_create.RoomType == TypeRoom.Entry)
                    {
                        Vector3 position = _rooms.GetGridPositionInWorldPosition(i, j);

                        _player_object = GameObject.Instantiate(player_prefab, position, Quaternion.identity);
                        _player_object.transform.SetParent(map.transform);

                        _player_current_position.x = room_to_create.GridPosition.x;
                        _player_current_position.y = room_to_create.GridPosition.y;

                        GeneralScripts.SetMainCameraPositionXYOnly(position);
                    }

                    room_object.GetComponent<RoomSpriteSelection>().SetSprite(room_to_create.OpenDoors, room_to_create.RoomType);

                    room_object.transform.SetParent(map.transform);
                }
            }
        }
    }

    public void SetMapVisibility(bool is_visible)
    {
        if (_player_object != null)
        {
            GeneralScripts.SetMainCameraPositionXYOnly(_player_object.transform.position);
        }

        map.SetActive(is_visible);
    }
}
