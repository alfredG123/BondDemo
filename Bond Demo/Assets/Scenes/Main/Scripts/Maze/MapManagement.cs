﻿using System.Collections;
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

            SetMoveMode(true);
        }
        else
        {
            _CameraMovement.EnableCameraMovement(is_active);
        }

        _CameraMovement.EnableCameraBound(is_active);

        General.SetUpObject(_MapObject, is_active);
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
                General.SetText(_MoveText, "Move Player");

                _CameraMovement.EnableCameraMovement(true);
            }
            else
            {
                General.SetText(_MoveText, "Move Camera");

                _CameraMovement.EnableCameraMovement(false);
            }

            _MovePlayer = !_MovePlayer;
        }
    }
}
