using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : BaseGrid<GridMapCell>
{
    private readonly float _NoiseDensity = 0;
    private readonly int _SmoothingCount = 0;

    private readonly GameObject _CellTemplate = null;
    private readonly GameObject _MapObject = null;

    private readonly List<GridMapCell> _UnoccupiedCells = new List<GridMapCell>();

    private (int x, int y) _PlayerCurrentCoordinate = (0, 0);

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="cell_size"></param>
    /// <param name="origin_point"></param>
    public MapGrid(int width, int height, float cell_size, Vector2 origin_point, float noise_density, int smoothing_count, GameObject cell_template, GameObject map_object)
    : base(width, height, cell_size, origin_point)
    {
        _NoiseDensity = noise_density;
        _SmoothingCount = smoothing_count;

        _CellTemplate = cell_template;
        _MapObject = map_object;
    }

    public GameObject PlayerObject { get; private set; }

    public bool HasReachableCell { get; private set; }

    /// <summary>
    /// Create a grid for the map, and generate the objects
    /// </summary>
    public void CreateMap()
    {
        GenerateGridMap();

        GenerateRoomObjects();

        SetPlayerOnMap();

        SetEnemyOnMap();

        SetTreasureOnMap();

        SetRestPlaceOnMap();

        SetCystalTempleOnMap();

        SetLeftOverCells();
    }

    public void ClearMap()
    {
        foreach (Transform child in _MapObject.transform)
        {
            Object.Destroy(child.gameObject);
        }

        ClearGrid();
    }

    /// <summary>
    /// Randomly generate a map using cellluar automata
    /// </summary>
    private void GenerateGridMap()
    {
        InitializeGridMap();

        ApplySmoothingToGridMap();
    }

    /// <summary>
    /// Randomly set each cell as a normal or wall cell
    /// </summary>
    private void InitializeGridMap()
    {
        // Initialize all cells
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if ((i == 0) || (i == Width - 1) || (j == 0) || (j == Height - 1))
                {
                    SetValue(i, j, new GridMapCell((i, j), TypeGridMapCell.Wall));
                }
                else if (_NoiseDensity > Random.Range(0, 1f))
                {
                    SetValue(i, j, new GridMapCell((i, j), TypeGridMapCell.Wall));
                }
                else
                {
                    SetValue(i, j, new GridMapCell((i, j), TypeGridMapCell.Normal));
                }
            }
        }
    }

    /// <summary>
    /// Apply smoothing to the randomly generated grid
    /// </summary>
    private void ApplySmoothingToGridMap()
    {
        int neighbor_count;
        GridMapCell cell;

        for (int t = 0; t < _SmoothingCount; t++)
        {
            _UnoccupiedCells.Clear();

            for (int i = 1; i < Width - 1; i++)
            {
                for (int j = 1; j < Height - 1; j++)
                {
                    neighbor_count = GetWallCount(i, j);

                    cell = GetValue(i, j);

                    if (neighbor_count > 4)
                    {
                        GetValue(i, j).CellTypeOnNextIteration = TypeGridMapCell.Wall;
                    }
                    else
                    {
                        GetValue(i, j).CellTypeOnNextIteration = TypeGridMapCell.Normal;

                        _UnoccupiedCells.Add(cell);
                    }
                }
            }

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    cell = GetValue(i, j);
                    if (cell.CellType != cell.CellTypeOnNextIteration)
                    {
                        cell.CellType = cell.CellTypeOnNextIteration;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Count the number of walls in the neighbors
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int GetWallCount(int x, int y)
    {
        int wall_count = 0;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if ((i < 0) || (i >= Width) || (j < 0) || (j >= Height))
                {
                    wall_count++;
                }
                else if ((i != x) || (j != y))
                {
                    if (GetValue(i, j).CellType == TypeGridMapCell.Wall)
                    {
                        wall_count++;
                    }
                }
            }
        }

        return (wall_count);
    }

    /// <summary>
    /// Instantiate tiles for the map
    /// </summary>
    private void GenerateRoomObjects()
    {
        GameObject room_object;
        GridMapCell room_to_create;
        int game_object_index = 0;

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                room_object = GameObject.Instantiate(_CellTemplate, ConvertCoordinateToPosition(i, j), Quaternion.identity);

                room_to_create = GetValue(i, j);

                room_object.GetComponent<GridMapCellSpriteSelector>().SetSprite(room_to_create.CellType);

                room_to_create.GameObjectIndexInContainer = game_object_index;

                room_object.transform.SetParent(_MapObject.transform);

                game_object_index++;
            }
        }
    }

    /// <summary>
    /// Randomly choose a valid grid map cell, and set the player at the position of the cell
    /// </summary>
    public void SetPlayerOnMap()
    {
        GridMapCell cell = _UnoccupiedCells[Random.Range(0, _UnoccupiedCells.Count)]; ;

        Vector3 position = ConvertCoordinateToPosition(cell.GridPosition.x, cell.GridPosition.y);

        _UnoccupiedCells.Remove(cell);

        PlayerObject = GameObject.Instantiate(AssetsLoader.Assets.PlayerPrefab, position, Quaternion.identity);
        PlayerObject.transform.SetParent(_MapObject.transform);
        _PlayerCurrentCoordinate = (cell.GridPosition.x, cell.GridPosition.y);

        SetReachableCell(cell.GridPosition.x, cell.GridPosition.y, true);

        General.SetMainCameraPositionXYOnly(position);
    }

    /// <summary>
    /// Randomly choose a valid grid map cell, and set it as an enemy cell.
    /// </summary>
    public void SetEnemyOnMap()
    {
        GameObject enemy_object;
        Vector3 position;
        int random_value;
        int single_enemy_density = 60;
        int duo_enemy_density = 40;
        int trio_enemy_density = 20;
        int[] enemy_density = { single_enemy_density, duo_enemy_density, trio_enemy_density };
        GridMapCell cell;
        int enemy_count_per_encounter = 3;
        List<GridMapCell> visited_list = new List<GridMapCell>();

        for (int i = 0; i < enemy_count_per_encounter; i++)
        {
            for (int j = 0; j < _UnoccupiedCells.Count; j++)
            {
                cell = _UnoccupiedCells[j];

                random_value = Random.Range(0, 100);

                if (random_value < enemy_density[i])
                {
                    position = ConvertCoordinateToPosition(cell.GridPosition.x, cell.GridPosition.y);

                    cell.CellType = TypeGridMapCell.Enemy;

                    enemy_object = GameObject.Instantiate(AssetsLoader.Assets.EnemeyPrefab, position, Quaternion.identity);

                    enemy_object.GetComponent<EnemySpriteSelector>().SetSprite(i);

                    enemy_object.transform.SetParent(_MapObject.transform.GetChild(cell.GameObjectIndexInContainer).transform);

                    visited_list.Add(cell);
                }
            }

            for (int j = 0; j < visited_list.Count; j++)
            {
                _UnoccupiedCells.Remove(visited_list[j]);
            }

            visited_list.Clear();
        }
    }

    public void SetTreasureOnMap()
    {
        GridMapCell cell;
        List<GridMapCell> visited_list = new List<GridMapCell>();
        int random_value;
        Vector3 position;
        int treasure_density = 20;
        GameObject teasure_object;

        for (int i = 0; i < _UnoccupiedCells.Count; i++)
        {
            cell = _UnoccupiedCells[i];

            random_value = Random.Range(0, 100);

            if (random_value < treasure_density)
            {
                position = ConvertCoordinateToPosition(cell.GridPosition.x, cell.GridPosition.y);

                cell.CellType = TypeGridMapCell.Treasure;

                teasure_object = GameObject.Instantiate(AssetsLoader.Assets.TreasurePrefab, position, Quaternion.identity);

                teasure_object.transform.SetParent(_MapObject.transform.GetChild(cell.GameObjectIndexInContainer).transform);

                visited_list.Add(cell);
            }
        }

        for (int i = 0; i < visited_list.Count; i++)
        {
            _UnoccupiedCells.Remove(visited_list[i]);
        }

        visited_list.Clear();
    }


    public void SetRestPlaceOnMap()
    {
        GridMapCell cell;
        List<GridMapCell> visited_list = new List<GridMapCell>();
        int random_value;
        Vector3 position;
        int rest_place_density = 20;
        GameObject rest_place_object;

        for (int i = 0; i < _UnoccupiedCells.Count; i++)
        {
            cell = _UnoccupiedCells[i];

            random_value = Random.Range(0, 100);

            if (random_value < rest_place_density)
            {
                position = ConvertCoordinateToPosition(cell.GridPosition.x, cell.GridPosition.y);

                cell.CellType = TypeGridMapCell.RestPlace;

                rest_place_object = GameObject.Instantiate(AssetsLoader.Assets.RestPlacePrefab, position, Quaternion.identity);

                rest_place_object.transform.SetParent(_MapObject.transform.GetChild(cell.GameObjectIndexInContainer).transform);

                visited_list.Add(cell);
            }
        }

        for (int i = 0; i < visited_list.Count; i++)
        {
            _UnoccupiedCells.Remove(visited_list[i]);
        }

        visited_list.Clear();
    }

    public void SetCystalTempleOnMap()
    {
        GridMapCell cell;
        List<GridMapCell> visited_list = new List<GridMapCell>();
        int random_value;
        Vector3 position;
        int cystal_temple_density = 20;
        GameObject cystal_temple_object;

        for (int i = 0; i < _UnoccupiedCells.Count; i++)
        {
            cell = _UnoccupiedCells[i];

            random_value = Random.Range(0, 100);

            if (random_value < cystal_temple_density)
            {
                position = ConvertCoordinateToPosition(cell.GridPosition.x, cell.GridPosition.y);

                cell.CellType = TypeGridMapCell.CystalTemple;

                cystal_temple_object = GameObject.Instantiate(AssetsLoader.Assets.CystalTempleOnPrefab, position, Quaternion.identity);

                cystal_temple_object.transform.SetParent(_MapObject.transform.GetChild(cell.GameObjectIndexInContainer).transform);

                visited_list.Add(cell);
            }
        }

        for (int i = 0; i < visited_list.Count; i++)
        {
            _UnoccupiedCells.Remove(visited_list[i]);
        }

        visited_list.Clear();
    }

    public void SetLeftOverCells()
    {
        GridMapCell cell;
        List<GridMapCell> visited_list = new List<GridMapCell>();
        Vector3 position;
        GameObject enemy_object;

        for (int i = 0; i < _UnoccupiedCells.Count; i++)
        {
            cell = _UnoccupiedCells[i];

            position = ConvertCoordinateToPosition(cell.GridPosition.x, cell.GridPosition.y);

            cell.CellType = TypeGridMapCell.Enemy;

            enemy_object = GameObject.Instantiate(AssetsLoader.Assets.EnemeyPrefab, position, Quaternion.identity);

            enemy_object.GetComponent<EnemySpriteSelector>().SetSprite(1);

            enemy_object.transform.SetParent(_MapObject.transform.GetChild(cell.GameObjectIndexInContainer).transform);

            visited_list.Add(cell);
        }

        for (int i = 0; i < visited_list.Count; i++)
        {
            _UnoccupiedCells.Remove(visited_list[i]);
        }

        visited_list.Clear();
    }

    public GridMapCell MovePlayerToSelectedCell(Vector3 cell_position)
    {
        GridMapCell cell = GetValue(cell_position);
        bool is_reachable = false;

        if (cell.CellType != TypeGridMapCell.Wall)
        {
            is_reachable = CheckReachable(cell.GridPosition.x, cell.GridPosition.y);

            if (is_reachable)
            {
                SetReachableCell(_PlayerCurrentCoordinate.x, _PlayerCurrentCoordinate.y, false);

                DisablePreviousCell(_PlayerCurrentCoordinate.x, _PlayerCurrentCoordinate.y);

                PlayerObject.transform.position = ConvertCoordinateToPosition(cell.GridPosition.x, cell.GridPosition.y);

                _PlayerCurrentCoordinate = cell.GridPosition;

                SetReachableCell(_PlayerCurrentCoordinate.x, _PlayerCurrentCoordinate.y, true);
            }
        }

        if (!is_reachable)
        {
            cell = null;
        }

        return (cell);
    }

    /// <summary>
    /// Set the color of the reachable grid map cell, and return if there is any reachable grid map cell
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool SetReachableCell(int x, int y, bool is_visible)
    {
        GridMapCell cell;

        HasReachableCell = false;

        if (CheckReachable(x - 1, y))
        {
            cell = GetValue(x - 1, y);

            _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<GridMapCellSpriteSelector>().SetColorForReachable(is_visible);

            HasReachableCell = true;
        }

        if (CheckReachable(x + 1, y))
        {
            cell = GetValue(x + 1, y);

            _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<GridMapCellSpriteSelector>().SetColorForReachable(is_visible);

            HasReachableCell = true;
        }

        if (CheckReachable(x, y - 1))
        {
            cell = GetValue(x, y - 1);

            _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<GridMapCellSpriteSelector>().SetColorForReachable(is_visible);

            HasReachableCell = true;
        }

        if (CheckReachable(x, y + 1))
        {
            cell = GetValue(x, y + 1);

            _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<GridMapCellSpriteSelector>().SetColorForReachable(is_visible);

            HasReachableCell = true;
        }

        return (HasReachableCell);
    }

    /// <summary>
    /// Check if the grid map cell is reachable
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool CheckReachable(int x, int y)
    {
        GridMapCell cell;
        bool is_reachable = false;

        if ((!is_reachable) && (x == _PlayerCurrentCoordinate.x + 1) && (y == _PlayerCurrentCoordinate.y))
        {
            is_reachable = true;
        }

        if ((!is_reachable) && (x == _PlayerCurrentCoordinate.x - 1) && (y == _PlayerCurrentCoordinate.y))
        {
            is_reachable = true;
        }

        if ((!is_reachable) && (x == _PlayerCurrentCoordinate.x) && (y == _PlayerCurrentCoordinate.y - 1))
        {
            is_reachable = true;
        }

        if ((!is_reachable) && (x == _PlayerCurrentCoordinate.x) && (y == _PlayerCurrentCoordinate.y + 1))
        {
            is_reachable = true;
        }

        if (is_reachable)
        {
            cell = GetValue(x, y);

            if ((cell == null) || (cell.CellType == TypeGridMapCell.Wall))
            {
                is_reachable = false;
            }
        }

        return (is_reachable);
    }

    /// <summary>
    /// Set the previous cell as a wall
    /// </summary>
    private void DisablePreviousCell(int x, int y)
    {
        GridMapCell cell = GetValue(x, y);

        cell.DisableCell();

        _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<GridMapCellSpriteSelector>().SetSprite(cell.CellType);
    }
}
