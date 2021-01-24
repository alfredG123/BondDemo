﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManagement : MonoBehaviour
{
    [SerializeField] int _MapSizeX = 0;
    [SerializeField] int _MapSizeY = 0;
    [SerializeField] GameObject _RoomTemplate = null;
    [SerializeField] GameObject _MapObject = null;

    [SerializeField] GameObject _PlayerPrefab = null;
    [SerializeField] GameObject _EnemeyPrefab = null;

    [SerializeField] MainManagement _MainManagement = null;

    [SerializeField] GameObject _NextLevelNotificationObject = null;

    private BaseGrid<Room> _MapGrid = null;
    private TypeRoom[,] _NoteMap = null;
    private readonly float _CellSize = 2f;
    private readonly int _SmoothingCount = 5;
    private readonly int _NoiseDensity = 55;

    private bool _PlayerNeedInitialized = true;
    private GameObject _PlayerObject = null;
    private (int x, int y) _PlayerCoordinate = (0, 0);
    private (int x, int y) _PlayerPreviousCoordinate = (0, 0);
    private (int x, int y)[] _PlayerPreviousReachable;
    private bool[] _PlayerPreviousReachableIsSet;

    private void Start()
    {
        _MapGrid = new BaseGrid<Room>(_MapSizeX, _MapSizeY, _CellSize, Vector2.zero);

        _NoteMap = new TypeRoom[_MapSizeX, _MapSizeY];

        _PlayerPreviousReachable = new (int x, int y)[4];

        _PlayerPreviousReachableIsSet = new bool[4];

        CreateMap();

        GenerateRoomObjects();

        ShowEnemyOnMap();

        ShowPlayerOnMap();
    }

    private void Update()
    {
        Room room_get_chosen;
        bool has_reachable_cell;

        if (_MapObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                room_get_chosen = _MapGrid.GetValue(General.GetMousePositionInWorldSpace());

                if (room_get_chosen != null)
                {
                    if (room_get_chosen.RoomType != TypeRoom.Wall)
                    {
                        if (CheckReachable(room_get_chosen.GridPosition.x, room_get_chosen.GridPosition.y))
                        {
                            _PlayerPreviousCoordinate = _PlayerCoordinate;

                            _PlayerObject.transform.position = _MapGrid.ConvertCoordinateToPosition(room_get_chosen.GridPosition.x, room_get_chosen.GridPosition.y);

                            _PlayerCoordinate = room_get_chosen.GridPosition;

                            DisablePreviousCell();

                            has_reachable_cell = ShowPlayerOnMap();

                            if (room_get_chosen.RoomType == TypeRoom.Enemy)
                            {
                                _MainManagement.TriggerBattle();

                                if (_MapObject.transform.GetChild(room_get_chosen.GameObjectIndexInContainer).transform.childCount > 0)
                                {
                                    Destroy(_MapObject.transform.GetChild(room_get_chosen.GameObjectIndexInContainer).GetChild(0).gameObject);
                                }
                            }

                            if (!has_reachable_cell)
                            {
                                _NextLevelNotificationObject.SetActive(true);
                            }
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Goes to next level!");
            }
        }
    }

    private void CreateMap()
    {
        int neighbor_count;

        // Initialize all cells
        for (int i = 0; i < _MapSizeX; i++)
        {
            for (int j = 0; j < _MapSizeY; j++)
            {
                if ((i == 0) || (i == _MapSizeX - 1) || (j == 0) || (j == _MapSizeY - 1))
                {
                    _MapGrid.SetValue(i, j, new Room((i, j), TypeRoom.Wall));
                }
                else if (_NoiseDensity > Random.Range(0, 100))
                {
                    _MapGrid.SetValue(i, j, new Room((i, j), TypeRoom.Wall));
                }
                else
                {
                    _MapGrid.SetValue(i, j, new Room((i, j), TypeRoom.Normal));
                }
            }
        }

        //Smoothing
        for (int t = 0; t < _SmoothingCount; t++)
        {
            for (int i = 1; i < _MapSizeX - 1; i++)
            {
                for (int j = 1; j < _MapSizeY - 1; j++)
                {
                    neighbor_count = GetWallCount(i, j);

                    if (neighbor_count > 4)
                    {
                        _NoteMap[i, j] = TypeRoom.Wall;
                    }
                    else
                    {
                        _NoteMap[i, j] = TypeRoom.Normal;
                    }
                }
            }

            for (int i = 0; i < _MapSizeX; i++)
            {
                for (int j = 0; j < _MapSizeY; j++)
                {
                    _MapGrid.GetValue(i, j).RoomType = _NoteMap[i, j];
                }
            }
        }
    }

    private void GenerateRoomObjects()
    {
        GameObject room_object;
        Room room_to_create;
        int game_object_index = 0;

        for (int i = 0; i < _MapSizeX; i++)
        {
            for (int j = 0; j < _MapSizeY; j++)
            {
                room_object = GameObject.Instantiate(_RoomTemplate, _MapGrid.ConvertCoordinateToPosition(i, j), Quaternion.identity);

                room_to_create = _MapGrid.GetValue(i, j);

                room_object.GetComponent<RoomSpriteSelection>().SetSprite(room_to_create.RoomType);

                room_to_create.GameObjectIndexInContainer = game_object_index;

                room_object.transform.SetParent(_MapObject.transform);

                game_object_index++;
            }
        }
    }

    private int GetWallCount(int x, int y)
    {
        int wall_count = 0;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if ((i < 0) || (i >= _MapSizeX) || (j < 0) || (j >= _MapSizeY))
                {
                    wall_count++;
                }
                else if ((i != x) || (j != y))
                {
                    if (_MapGrid.GetValue(i,j).RoomType == TypeRoom.Wall)
                    {
                        wall_count++;
                    }
                }
            }
        }

        return (wall_count);
    }

    private bool ShowPlayerOnMap()
    {
        Vector3 position;
        bool has_reachable_cell = true;

        if (_PlayerNeedInitialized)
        {
            int x ;
            int y;

            do
            {
                x = Random.Range(1, _MapSizeX);
                y = Random.Range(1, _MapSizeY);
            } while ((_MapGrid.GetValue(x, y).RoomType == TypeRoom.Wall) || (_MapGrid.GetValue(x, y).RoomType == TypeRoom.Enemy));

            position = _MapGrid.ConvertCoordinateToPosition(x, y);

            _PlayerObject = GameObject.Instantiate(_PlayerPrefab, position, Quaternion.identity);
            _PlayerObject.transform.SetParent(_MapObject.transform);
            _PlayerCoordinate = (x, y);

            General.SetMainCameraPositionXYOnly(position);

            _PlayerNeedInitialized = false;
        }
        else
        {
            ResetPreviousReachableCell();
        }

        has_reachable_cell = SetReachableCell(_PlayerCoordinate.x, _PlayerCoordinate.y);

        return (has_reachable_cell);
    }

    private void ShowEnemyOnMap()
    {
        GameObject enemy_object;
        Vector3 position;
        int x;
        int y;
        int enemy_count = 10;
        Room cell;
        int enemy_count_in_battle;

        for (int i = 0; i < enemy_count; i++)
        {
            do
            {
                x = Random.Range(1, _MapSizeX);
                y = Random.Range(1, _MapSizeY);
            } while ((_MapGrid.GetValue(x, y).RoomType == TypeRoom.Wall) || (_MapGrid.GetValue(x, y).RoomType == TypeRoom.Enemy));

            position = _MapGrid.ConvertCoordinateToPosition(x, y);

            cell = _MapGrid.GetValue(x, y);
            cell.RoomType = TypeRoom.Enemy;

            enemy_object = GameObject.Instantiate(_EnemeyPrefab, position, Quaternion.identity);
            enemy_object.transform.SetParent(_MapObject.transform.GetChild(cell.GameObjectIndexInContainer).transform);

            enemy_count_in_battle = Random.Range(1, 4);
            GameObject text_mesh_object = new GameObject("EnemyCount", typeof(TextMesh));
            Transform text_mesh_transform = text_mesh_object.transform;
            text_mesh_transform.SetParent(enemy_object.transform, false);
            text_mesh_transform.localPosition = new Vector2(-.5f, 2.5f);
            TextMesh text_mesh = text_mesh_object.GetComponent<TextMesh>();
            text_mesh.text = enemy_count_in_battle.ToString();
            text_mesh.fontSize = 15;
            text_mesh.color = Color.red;
        }
    }

    private bool SetReachableCell(int x, int y)
    {
        bool has_reachable_cell = false;
        Room cell;

        if (CheckReachable(x - 1, y))
        {
            cell = _MapGrid.GetValue(x - 1, y);

            _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<RoomSpriteSelection>().SetColorForReachable(true);

            _PlayerPreviousReachable[0] = (x - 1, y);

            _PlayerPreviousReachableIsSet[0] = true;

            has_reachable_cell = true;
        }

        if (CheckReachable(x + 1, y))
        {
            cell = _MapGrid.GetValue(x + 1, y);

            _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<RoomSpriteSelection>().SetColorForReachable(true);

            _PlayerPreviousReachable[1] = (x + 1, y);

            _PlayerPreviousReachableIsSet[1] = true;

            has_reachable_cell = true;
        }

        if (CheckReachable(x, y - 1))
        {
            cell = _MapGrid.GetValue(x, y - 1);

            _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<RoomSpriteSelection>().SetColorForReachable(true);

            _PlayerPreviousReachable[2] = (x, y - 1);

            _PlayerPreviousReachableIsSet[2] = true;

            has_reachable_cell = true;
        }

        if (CheckReachable(x, y + 1))
        {
            cell = _MapGrid.GetValue(x, y + 1);

            _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<RoomSpriteSelection>().SetColorForReachable(true);

            _PlayerPreviousReachable[3] = (x, y + 1);

            _PlayerPreviousReachableIsSet[3] = true;

            has_reachable_cell = true;
        }

        return (has_reachable_cell);
    }

    private bool CheckReachable(int x, int y)
    {
        Room cell;
        bool is_reachable = false;

        if ((!is_reachable) && (x == _PlayerCoordinate.x + 1) && (y == _PlayerCoordinate.y))
        {
            is_reachable = true;
        }

        if ((!is_reachable) && (x == _PlayerCoordinate.x - 1) && (y == _PlayerCoordinate.y))
        {
            is_reachable = true;
        }

        if ((!is_reachable) && (x == _PlayerCoordinate.x) && (y == _PlayerCoordinate.y - 1))
        {
            is_reachable = true;
        }

        if ((!is_reachable) && (x == _PlayerCoordinate.x) && (y == _PlayerCoordinate.y + 1))
        {
            is_reachable = true;
        }

        if (is_reachable)
        {
            cell = _MapGrid.GetValue(x, y);

            if (cell.RoomType == TypeRoom.Wall)
            {
                is_reachable = false;
            }
        }

        return (is_reachable);
    }

    private void ResetPreviousReachableCell()
    {
        Room cell;

        for (int i = 0; i < _PlayerPreviousReachable.Length; i++)
        {
            if (_PlayerPreviousReachableIsSet[i])
            {
                cell = _MapGrid.GetValue(_PlayerPreviousReachable[i].x, _PlayerPreviousReachable[i].y);

                _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<RoomSpriteSelection>().SetColorForReachable(false);

                _PlayerPreviousReachableIsSet[i] = false;
            }
        }
    }

    private void DisablePreviousCell()
    {
        Room cell;

        cell = _MapGrid.GetValue(_PlayerPreviousCoordinate.x, _PlayerPreviousCoordinate.y);

        cell.RoomType = TypeRoom.Wall;

        _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<RoomSpriteSelection>().SetSprite(cell.RoomType);
    }

    public void SetMapVisibility(bool is_visible)
    {
        if (is_visible)
        {
            General.SetMainCameraPositionXYOnly(_PlayerObject.transform.position);
        }

        _MapObject.SetActive(is_visible);
    }

#if REDO
    [SerializeField] int min_rooms_to_generate;
    [SerializeField] GameObject room_template;
    [SerializeField] GameObject map;
    [SerializeField] GameObject player_prefab;
    [SerializeField] GameObject battle_manager;
    [SerializeField] int final_level;
    [SerializeField] GameObject end_room_text;

    private Grid<Room> _rooms;
    private List<(int x, int y)> _occupied_positions;
    private Queue<Room> _room_queue;
    private bool[] _empty_room_position;
    private float _room_size = 2;
    private int _number_room;
    private (int x, int y) _player_current_position;
    private GameObject _player_object;
    private bool _need_end_room;
    private int level;
    private bool _new_map;

    private void Start()
    {
        level = 0;

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
                room_get_chosen = _rooms.GetValue(General.GetMousePositionInWorldSpace());

                if (room_get_chosen != null)
                {
                    if (CheckPlayerNeighbor(room_get_chosen))
                    {
                        _player_object.transform.position = _rooms.GetGridPositionInWorldPosition(room_get_chosen.GridPosition.x, room_get_chosen.GridPosition.y);
                        _player_current_position = room_get_chosen.GridPosition;

                        end_room_text.SetActive(false);

                        if ((room_get_chosen.RoomType == TypeRoom.Normal) && (!room_get_chosen.IsVisited))
                        {
                            room_get_chosen.IsVisited = true;

                            battle_manager.GetComponent<BattleProgressionManagement>().TriggerEncounter();
                        }
                        else if (room_get_chosen.RoomType == TypeRoom.NextLevel)
                        {
                            end_room_text.SetActive(true);
                        }
                        else if(room_get_chosen.RoomType == TypeRoom.Final)
                        {
                            General.SetText(end_room_text, "Press Enter to challenge the final boss.");

                            end_room_text.SetActive(true);
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

                        level++;
                    }
                    else if (room_get_chosen.RoomType == TypeRoom.Final)
                    {
                        General.ReturnToTitleSceneForErrors("FINAL", "WIN");
                    }

                    end_room_text.SetActive(false);
                }
            }

            if (map.transform.childCount == 0)
            {
                _new_map = true;

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
        yield return new WaitForSeconds(.2f);
        
        if (_new_map)
        {
            if (level > final_level)
            {
                CreateFinalMap();
            }
            else
            {
                CreateMap();
            }

            _new_map = false;
        }
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

    private void CreateFinalMap()
    {
        Room current_room;

        // Create an entry room
        (int x, int y) entry_room_position = (0,0);
        CreateRoom(entry_room_position.x, entry_room_position.y, TypeRoom.Entry, 0, null);

        current_room = _rooms.GetValue(entry_room_position.x, entry_room_position.y);

        entry_room_position.x++;
        CreateRoom(entry_room_position.x, entry_room_position.y, TypeRoom.Normal, 1, current_room);

        current_room = _rooms.GetValue(entry_room_position.x, entry_room_position.y);

        entry_room_position.x++;
        CreateRoom(entry_room_position.x, entry_room_position.y, TypeRoom.Normal, 1, current_room);

        current_room = _rooms.GetValue(entry_room_position.x, entry_room_position.y);

        entry_room_position.x++;
        CreateRoom(entry_room_position.x, entry_room_position.y, TypeRoom.Normal, 1, current_room);

        current_room = _rooms.GetValue(entry_room_position.x, entry_room_position.y);

        entry_room_position.x++;
        CreateRoom(entry_room_position.x, entry_room_position.y, TypeRoom.Normal, 1, current_room);

        current_room = _rooms.GetValue(entry_room_position.x, entry_room_position.y);

        entry_room_position.x++;
        CreateRoom(entry_room_position.x, entry_room_position.y, TypeRoom.Final, 1, current_room);

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

                        General.SetMainCameraPositionXYOnly(position);
                    }
                    
                    if ((_need_end_room) && (room_to_create.RoomType == TypeRoom.Normal))
                    {
                        if (room_to_create.OpenDoors.Count == 1)
                        {
                            room_to_create.RoomType = TypeRoom.NextLevel;

                            _need_end_room = false;
                        }
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
            General.SetMainCameraPositionXYOnly(_player_object.transform.position);
        }

        map.SetActive(is_visible);
    }
#endif
}
