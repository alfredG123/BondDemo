using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagement : MonoBehaviour
{
    [SerializeField] private int _MapSizeX = 0;
    [SerializeField] private int _MapSizeY = 0;
    [SerializeField] private GameObject _CellTemplate = null;
    [SerializeField] private GameObject _MapObject = null;

    [SerializeField] private MainManagement _MainManagement = null;
    [SerializeField] private BattleDisplayHandler _BattleDisplayHanlder = null;

    [SerializeField] private GameObject _NextLevelNotificationObject = null;

    [SerializeField] private CameraMovement _CameraMovement = null;

    [SerializeField] private GameObject _MazePanel = null;


    [SerializeField] private GameObject _MoveText = null;

    private bool _MovePlayer = true;

    private MapGrid _MapGrid = null;
    private readonly float _CellSize = 2f;

    private GridMapCell _TargetCell = null;

    /// <summary>
    /// Initialize global variable and create a map
    /// </summary>
    private void Start()
    {
        Vector2 lower_bound;
        Vector2 upper_bound;

        _MapGrid = new MapGrid(_MapSizeX, _MapSizeY, _CellSize, Vector2.zero, 0.55f, 5, _CellTemplate, _MapObject);

        _MapGrid.CreateMap();

        _MazePanel.SetActive(true);

        lower_bound = _MapGrid.ConvertCoordinateToPosition(0, 0);
        upper_bound = _MapGrid.ConvertCoordinateToPosition(_MapSizeX - 1, _MapSizeY - 1);

        _CameraMovement.SetCameraBound(lower_bound.x, lower_bound.y, upper_bound.x, upper_bound.y);

        _CameraMovement.EnableCameraBound(true);

        _CameraMovement.EnableCameraMovement(true);

        SetMoveMode();
    }

    /// <summary>
    /// Listen to the mouse and key event
    /// </summary>
    private void Update()
    {
        if (_MapObject.activeSelf)
        {
            if (_MovePlayer)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    _TargetCell = _MapGrid.MovePlayerToSelectedCell(_MapGrid.PlayerCurrentCoordinate.x, _MapGrid.PlayerCurrentCoordinate.y + 1);

                    if (_TargetCell != null)
                    {
                        TriggerEvent(_TargetCell);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    _TargetCell = _MapGrid.MovePlayerToSelectedCell(_MapGrid.PlayerCurrentCoordinate.x - 1, _MapGrid.PlayerCurrentCoordinate.y);

                    if (_TargetCell != null)
                    {
                        TriggerEvent(_TargetCell);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    _TargetCell = _MapGrid.MovePlayerToSelectedCell(_MapGrid.PlayerCurrentCoordinate.x, _MapGrid.PlayerCurrentCoordinate.y - 1);

                    if (_TargetCell != null)
                    {
                        TriggerEvent(_TargetCell);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    _TargetCell = _MapGrid.MovePlayerToSelectedCell(_MapGrid.PlayerCurrentCoordinate.x + 1, _MapGrid.PlayerCurrentCoordinate.y);

                    if (_TargetCell != null)
                    {
                        TriggerEvent(_TargetCell);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                _TargetCell = _MapGrid.MovePlayerToSelectedCell(General.GetMousePositionInWorldSpace());

                if (_TargetCell != null)
                {
                    TriggerEvent(_TargetCell);
                }
            }

            if ((_NextLevelNotificationObject.activeSelf) && (Input.GetKeyDown(KeyCode.Return)))
            {
                ClearMap();
            }

            if (!_MapGrid.HasReachableCell)
            {
                _NextLevelNotificationObject.SetActive(true);
            }

            if (_MapObject.transform.childCount == 0)
            {
                _NextLevelNotificationObject.SetActive(false);

                _MapGrid.CreateMap();
            }
        }
    }

    public void ClearMap()
    {
        _MapGrid.ClearMap();
    }

    private void TriggerEvent(GridMapCell cell)
    {
        if (cell.CellType == TypeGridMapCell.Enemy)
        {
            TriggerEnemy();
        }
        else if (cell.CellType == TypeGridMapCell.Treasure)
        {
            FindTreasure();
        }
        else if (cell.CellType == TypeGridMapCell.RestPlace)
        {
            GetRest();
        }
        else if (cell.CellType == TypeGridMapCell.CystalTemple)
        {
            UseCrystal();
        }
        else if (cell.CellType == TypeGridMapCell.WormHole)
        {
            Teleport();
        }
    }

    private void TriggerEnemy()
    {
        if (_MapObject.transform.GetChild(_TargetCell.GameObjectIndexInContainer).GetChild(0).gameObject.GetComponent<EnemySpriteSelector>().EnemyCount == 3)
        {
            _MainManagement.TriggerBattle(_MapObject.transform.GetChild(_TargetCell.GameObjectIndexInContainer).GetChild(0).gameObject.GetComponent<EnemySpriteSelector>().EnemyCount);
        }

        if (_MapObject.transform.GetChild(_TargetCell.GameObjectIndexInContainer).transform.childCount > 0)
        {
            Destroy(_MapObject.transform.GetChild(_TargetCell.GameObjectIndexInContainer).GetChild(0).gameObject);
        }
    }

    private void FindTreasure()
    {
        _MainManagement.GetTreasure();

        if (_MapObject.transform.GetChild(_TargetCell.GameObjectIndexInContainer).transform.childCount > 0)
        {
            Destroy(_MapObject.transform.GetChild(_TargetCell.GameObjectIndexInContainer).GetChild(0).gameObject);
        }
    }

    private void GetRest()
    {
        _MainManagement.TakeRest();

        if (_MapObject.transform.GetChild(_TargetCell.GameObjectIndexInContainer).transform.childCount > 0)
        {
            Destroy(_MapObject.transform.GetChild(_TargetCell.GameObjectIndexInContainer).GetChild(0).gameObject);
        }
    }

    private void UseCrystal()
    {
        _MainManagement.EnterCystalTemple();

        if (_MapObject.transform.GetChild(_TargetCell.GameObjectIndexInContainer).transform.childCount > 0)
        {
            Destroy(_MapObject.transform.GetChild(_TargetCell.GameObjectIndexInContainer).GetChild(0).gameObject);
        }
    }

    private void Teleport()
    {
        if (_MapObject.transform.GetChild(_TargetCell.GameObjectIndexInContainer).transform.childCount > 0)
        {
            Destroy(_MapObject.transform.GetChild(_TargetCell.GameObjectIndexInContainer).GetChild(0).gameObject);
        }

        _TargetCell = _MapGrid.Teleport(_TargetCell);

        if (_MapObject.transform.GetChild(_TargetCell.GameObjectIndexInContainer).transform.childCount > 0)
        {
            Destroy(_MapObject.transform.GetChild(_TargetCell.GameObjectIndexInContainer).GetChild(0).gameObject);
        }
    }

    /// <summary>
    /// Set the map object, and adjust the camera
    /// </summary>
    /// <param name="is_active"></param>
    public void SetUpMapPanel(bool is_active)
    {
        // If the map object is active, reposition the camera to the player's location
        if (is_active)
        {
            General.SetMainCameraPositionXYOnly(_MapGrid.PlayerObject.transform.position);
        }

        _CameraMovement.EnableCameraBound(is_active);

        _CameraMovement.EnableCameraMovement(is_active);

        General.SetUpObject(_MapObject, is_active);
    }

    public void ReCenter()
    {
        _BattleDisplayHanlder.MoveCameraToPlayer();
    }

    public void SetMoveMode()
    {
        if (_MovePlayer)
        {
            General.SetText(_MoveText, "Move Camera");

            _CameraMovement.EnableCameraMovement(true);
        }
        else
        {
            General.SetText(_MoveText, "Move Player");

            _CameraMovement.EnableCameraMovement(false);
        }

        _MovePlayer = !_MovePlayer;
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
