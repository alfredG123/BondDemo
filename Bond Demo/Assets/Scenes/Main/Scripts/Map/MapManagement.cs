using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagement : MonoBehaviour
{
    [SerializeField] private int _MapSizeX = 0;
    [SerializeField] private int _MapSizeY = 0;
    [SerializeField] private GameObject _MapObject = null;

    [SerializeField] private MainManagement _MainManagement = null;
    [SerializeField] private BattleDisplayHandler _BattleDisplayHanlder = null;

    [SerializeField] private GameObject _NextLevelNotificationObject = null;

    [SerializeField] private CameraMovement _CameraMovement = null;

    [SerializeField] private GameObject _MapPanel = null;

    [SerializeField] private GameObject _CountText = null;

    [SerializeField] private GameObject _MoveText = null;

    private bool _MovePlayer = true;

    private EventMap _MapGrid = null;
    private readonly float _CellSize = 2f;

    private EventCell _TargetCell = null;

    private bool _GamePause = false;

    private int _EventCount = 0;

    /// <summary>
    /// Initialize global variable and create a map
    /// </summary>
    private void Start()
    {
        Vector2 lower_bound;
        Vector2 upper_bound;

        _MapGrid = new EventMap(_MapSizeX, _MapSizeY, _CellSize, Vector2.zero, .55f, 5, _MapObject);

        _MapGrid.CreateMap();

        _MapPanel.SetActive(true);

        lower_bound = _MapGrid.GetPosition(0, 0);
        upper_bound = _MapGrid.GetPosition(_MapSizeX - 1, _MapSizeY - 1);

        _CameraMovement.SetCameraBound(lower_bound.x, lower_bound.y, upper_bound.x, upper_bound.y);

        _CameraMovement.EnableCameraBound(true);

        _CameraMovement.EnableCameraMovement(true);

        _CameraMovement.SetMainCameraPositionXYOnly(_MapGrid.PlayerPosition);

        SetMoveMode();
    }

    /// <summary>
    /// Listen to the mouse and key event
    /// </summary>
    private void Update()
    {
        if ((_MapObject.activeSelf) && (!_GamePause))
        {
            GeneralComponent.SetText(_CountText, "Count: " + _EventCount);

            if (_MovePlayer)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    RemoveCellIcon();

                    _TargetCell = _MapGrid.MovePlayerToSelectedCell(_MapGrid.PlayerCurrentCoordinate.x, _MapGrid.PlayerCurrentCoordinate.y + 1);

                    _CameraMovement.SetTargetPosition(_MapGrid.GetPosition(_MapGrid.PlayerCurrentCoordinate.x, _MapGrid.PlayerCurrentCoordinate.y + 1));

                    if (_TargetCell != null)
                    {
                        TriggerEvent(_TargetCell);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    RemoveCellIcon();

                    _TargetCell = _MapGrid.MovePlayerToSelectedCell(_MapGrid.PlayerCurrentCoordinate.x - 1, _MapGrid.PlayerCurrentCoordinate.y);

                    _CameraMovement.SetTargetPosition(_MapGrid.GetPosition(_MapGrid.PlayerCurrentCoordinate.x - 1, _MapGrid.PlayerCurrentCoordinate.y));

                    if (_TargetCell != null)
                    {
                        TriggerEvent(_TargetCell);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    RemoveCellIcon();

                    _TargetCell = _MapGrid.MovePlayerToSelectedCell(_MapGrid.PlayerCurrentCoordinate.x, _MapGrid.PlayerCurrentCoordinate.y - 1);

                    _CameraMovement.SetTargetPosition(_MapGrid.GetPosition(_MapGrid.PlayerCurrentCoordinate.x, _MapGrid.PlayerCurrentCoordinate.y - 1));

                    if (_TargetCell != null)
                    {
                        TriggerEvent(_TargetCell);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    RemoveCellIcon();

                    _TargetCell = _MapGrid.MovePlayerToSelectedCell(_MapGrid.PlayerCurrentCoordinate.x + 1, _MapGrid.PlayerCurrentCoordinate.y);

                    _CameraMovement.SetTargetPosition(_MapGrid.GetPosition(_MapGrid.PlayerCurrentCoordinate.x + 1, _MapGrid.PlayerCurrentCoordinate.y));

                    if (_TargetCell != null)
                    {
                        TriggerEvent(_TargetCell);
                    }
                }
            }

            // Listen to the mouse left click event
            if (Input.GetMouseButtonDown(0))
            {
                RemoveCellIcon();

                _TargetCell = _MapGrid.MovePlayerToSelectedCell(GeneralInput.GetMousePositionInWorldSpace());

                _CameraMovement.SetTargetPosition(_MapGrid.GetPosition(_MapGrid.PlayerCurrentCoordinate.x, _MapGrid.PlayerCurrentCoordinate.y));

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

                _CameraMovement.SetMainCameraPositionXYOnlyImmediate(_MapGrid.PlayerPosition);
            }
        }
    }

    public void ClearMap()
    {
        _MapGrid.ClearMap();
    }

    private void TriggerEvent(EventCell cell)
    {
        _EventCount++;

        if ((cell.CellType == EventMap.EventCellType.EnemySolo) || (cell.CellType == EventMap.EventCellType.EnemyDuo) || (cell.CellType == EventMap.EventCellType.EnemyTrio))
        {
            TriggerEnemy(cell);
        }
        else if (cell.CellType == EventMap.EventCellType.Treasure)
        {
            FindTreasure();
        }
        else if (cell.CellType == EventMap.EventCellType.RestPlace)
        {
            GetRest();
        }
        else if (cell.CellType == EventMap.EventCellType.CystalTemple)
        {
            UseCrystal();
        }
        else if (cell.CellType == EventMap.EventCellType.SurvivedSpirit)
        {
            MeetSpirit();
        }
        else if (cell.CellType == EventMap.EventCellType.WormHole)
        {
            Teleport();
        }
    }

    private void TriggerEnemy(EventCell cell)
    {
        if (GeneralSetting.IsTestingEnabled())
        {
            if (cell.CellType == EventMap.EventCellType.EnemyDuo)
            {
                _MainManagement.TriggerBattle(GetEnemyCount(cell.CellType));
            }
        }
        else
        {
            _MainManagement.TriggerBattle(GetEnemyCount(cell.CellType));
        }
    }

    private int GetEnemyCount(EventMap.EventCellType cell_type)
    {
        int enemy_count = 0;
        int min_enemy_count = 1;

        if (cell_type == EventMap.EventCellType.EnemySolo)
        {
            enemy_count = 1;
        }
        else if (cell_type == EventMap.EventCellType.EnemyDuo)
        {
            enemy_count = 2;
        }
        else if (cell_type == EventMap.EventCellType.EnemyTrio)
        {
            enemy_count = 3;
        }

        // If the play mode is testing, check the return value
        if (GeneralSetting.IsTestingEnabled())
        {
            GeneralError.CheckIfLess(enemy_count, min_enemy_count, "GetEnemyCount");
        }

        return (enemy_count);
    }

    private void FindTreasure()
    {
        _MainManagement.GetTreasure();
    }

    private void GetRest()
    {
        _MainManagement.TakeRest();
    }

    private void UseCrystal()
    {
        _MainManagement.EnterCystalTemple();
    }

    private void MeetSpirit()
    {
        _MainManagement.MeetSpirit();
    }

    private void Teleport()
    {
        _TargetCell = _MapGrid.Teleport(_TargetCell);
    }

    private void RemoveCellIcon()
    {
        if (_MapObject.transform.GetChild(_MapGrid.GetValue(_MapGrid.PlayerPosition).GameObjectIndexInContainer).childCount > 0)
        {
            Destroy(_MapObject.transform.GetChild(_MapGrid.GetValue(_MapGrid.PlayerPosition).GameObjectIndexInContainer).GetChild(0).gameObject);
        }
    }

    /// <summary>
    /// Set the map object, and adjust the camera
    /// </summary>
    /// <param name="is_active"></param>
    public void SetUpMapPanel(bool is_active)
    {
        _CameraMovement.EnableCameraBound(is_active);

        // If the map object is active, reposition the camera to the player's location
        if (is_active)
        {
            _CameraMovement.SetMainCameraPositionXYOnly(_MapGrid.PlayerObject.transform.position);

            SetMoveMode(true);

            GeneralGameObject.ActivateObject(_MapObject);

            if (_EventCount >= 5)
            {
                ClearMap();
            }
        }
        else
        {
            _CameraMovement.EnableCameraMovement(is_active);

            GeneralGameObject.DeactivateObject(_MapObject);
        }
    }

    public void ReCenter()
    {
        _BattleDisplayHanlder.MoveCameraToPlayer();
    }

    public void SetMoveMode()
    {
        SetMoveMode(false);
    }

    public void SetMoveMode(bool reset)
    {
        if (reset)
        {
            if (_MovePlayer)
            {
                _CameraMovement.EnableCameraMovement(false);
            }
            else
            {
                _CameraMovement.EnableCameraMovement(true);
            }
        }
        else
        {
            if (_MovePlayer)
            {
                GeneralComponent.SetText(_MoveText, "Move Player");

                _CameraMovement.EnableCameraMovement(true);
            }
            else
            {
                GeneralComponent.SetText(_MoveText, "Move Camera");

                _CameraMovement.EnableCameraMovement(false);
            }

            _MovePlayer = !_MovePlayer;
        }
    }

    public void SetPause(bool is_pause)
    {
        _GamePause = is_pause;
    }
}
