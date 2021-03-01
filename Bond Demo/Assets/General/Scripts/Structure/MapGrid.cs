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

    private readonly List<List<GridMapCell>> _IsolatedParts = new List<List<GridMapCell>>();

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
    public (int x, int y) PlayerCurrentCoordinate { get => _PlayerCurrentCoordinate; }
    public bool HasReachableCell { get; private set; }

    /// <summary>
    /// Create a grid for the map, and generate the objects
    /// </summary>
    public void CreateMap()
    {
        GenerateGridMap();

        GenerateRoomObjects();

        SetWormHole();

        SetPlayerOnMap();

        SetEnemyOnMap();

        SetTreasureOnMap();

        SetRestPlaceOnMap();

        SetCystalTempleOnMap();

        SetSurvivedSpirit();
    }

    public void ClearMap()
    {
        foreach (Transform child in _MapObject.transform)
        {
            Object.Destroy(child.gameObject);
        }

        for (int i = _IsolatedParts.Count - 1; i >= 0; i--)
        {
            for (int j = _IsolatedParts[i].Count - 1; j >= 0; j--)
            {
                _IsolatedParts[i].RemoveAt(j);
            }

            _IsolatedParts.RemoveAt(i);
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

        RemoveIsolatedCell();

        DefineIsolatedParts();
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
        int wall_count;
        GridMapCell cell;

        for (int t = 0; t < _SmoothingCount; t++)
        {
            _UnoccupiedCells.Clear();

            for (int i = 1; i < Width - 1; i++)
            {
                for (int j = 1; j < Height - 1; j++)
                {
                    wall_count = GetWallCount(i, j);

                    cell = GetValue(i, j);

                    if (wall_count > 4)
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

    private void RemoveIsolatedCell()
    {
        int wall_count;
        GridMapCell cell;
        List<GridMapCell> visited_list = new List<GridMapCell>();

        while (true)
        {
            for (int j = 0; j < _UnoccupiedCells.Count; j++)
            {
                cell = _UnoccupiedCells[j];

                wall_count = GetWallCount(cell.GridPosition.x, cell.GridPosition.y);

                if (wall_count == 8)
                {
                    visited_list.Add(cell);
                }
            }

            if (visited_list.Count == 0)
            {
                break;
            }
            else
            {
                for (int j = 0; j < visited_list.Count; j++)
                {
                    visited_list[j].CellType = TypeGridMapCell.Wall;

                    _UnoccupiedCells.Remove(visited_list[j]);
                }

                visited_list.Clear();
            }
        }
    }

    private void DefineIsolatedParts()
    {
        int count = 0;
        GridMapCell cell;

        for (int i = 0; i < _UnoccupiedCells.Count; i++)
        {
            cell = _UnoccupiedCells[i];
            if ((!cell.IsVisited) && (cell.CellType == TypeGridMapCell.Normal))
            {
                _IsolatedParts.Add(new List<GridMapCell>());

                FindNeighbors(cell.GridPosition.x, cell.GridPosition.y, _IsolatedParts[count]);

                count++;
            }
        }
    }

    private void FindNeighbors(int x, int y, List<GridMapCell> isolated_part)
    {
        GridMapCell cell = GetValue(x, y);

        cell.IsVisited = true;

        isolated_part.Add(cell);

        if (IsValid(x - 1, y))
        {
            FindNeighbors(x - 1, y, isolated_part);
        }

        if (IsValid(x + 1, y))
        {
            FindNeighbors(x + 1, y, isolated_part);
        }

        if (IsValid(x, y - 1))
        {
            FindNeighbors(x, y - 1, isolated_part);
        }

        if (IsValid(x, y + 1))
        {
            FindNeighbors(x, y + 1, isolated_part);
        }
    }

    private bool IsValid(int x, int y)
    {
        bool is_valid = false;

        if ((x >= 0) && (x < Width) && (y >= 0) && (y < Height))
        {
            GridMapCell cell = GetValue(x, y);

            if ((!cell.IsVisited) && (cell.CellType == TypeGridMapCell.Normal))
            {
                is_valid = true;
            }
        }

        return (is_valid);
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

    private void SetWormHole()
    {
        int random1;
        int random2;
        GameObject wormhole1_object;
        GameObject wormhole2_object;
        Vector2 position1;
        Vector2 position2;
        GridMapCell cell1;
        GridMapCell cell2;
        GameObject worm_hole;

        worm_hole = AssetsLoader.Assets.LoadGameObject("WormHole", LoadEnum.Map);

        for (int i = 1; i < _IsolatedParts.Count; i++)
        {
            do
            {
                random1 = Random.Range(0, _IsolatedParts[i - 1].Count);
                random2 = Random.Range(0, _IsolatedParts[i].Count);
            } while ((_IsolatedParts[i - 1][random1].CellType == TypeGridMapCell.WormHole) || (_IsolatedParts[i][random2].CellType == TypeGridMapCell.WormHole));

            cell1 = _IsolatedParts[i - 1][random1];
            cell2 = _IsolatedParts[i][random2];

            cell1.CellType = TypeGridMapCell.WormHole;
            cell2.CellType = TypeGridMapCell.WormHole;

            cell1.DestinatioX = cell2.GridPosition.x;
            cell1.DestinatioY = cell2.GridPosition.y;
            cell2.DestinatioX = cell1.GridPosition.x;
            cell2.DestinatioY = cell1.GridPosition.y;

            _UnoccupiedCells.Remove(cell1);
            _UnoccupiedCells.Remove(cell2);

            position1 = ConvertCoordinateToPosition(cell1.GridPosition.x, cell1.GridPosition.y);
            wormhole1_object = GameObject.Instantiate(worm_hole, position1, Quaternion.identity);
            wormhole1_object.transform.SetParent(_MapObject.transform.GetChild(cell1.GameObjectIndexInContainer).transform);

            position2 = ConvertCoordinateToPosition(cell2.GridPosition.x, cell2.GridPosition.y);
            wormhole2_object = GameObject.Instantiate(worm_hole, position2, Quaternion.identity);
            wormhole2_object.transform.SetParent(_MapObject.transform.GetChild(cell2.GameObjectIndexInContainer).transform);
        }
    }

    /// <summary>
    /// Randomly choose a valid grid map cell, and set the player at the position of the cell
    /// </summary>
    public void SetPlayerOnMap()
    {
        GameObject player;

        player = AssetsLoader.Assets.LoadGameObject("SmileFace", LoadEnum.Map);

        if (_UnoccupiedCells.Count > 0)
        {
            GridMapCell cell = _UnoccupiedCells[Random.Range(0, _UnoccupiedCells.Count)];

            Vector3 position = ConvertCoordinateToPosition(cell.GridPosition.x, cell.GridPosition.y);

            _UnoccupiedCells.Remove(cell);

            PlayerObject = GameObject.Instantiate(player, position, Quaternion.identity);
            PlayerObject.transform.SetParent(_MapObject.transform);
            _PlayerCurrentCoordinate = (cell.GridPosition.x, cell.GridPosition.y);

            SetReachableCell(cell.GridPosition.x, cell.GridPosition.y, true);

            General.SetMainCameraPositionXYOnly(position);
        }
    }

    /// <summary>
    /// Randomly choose a valid grid map cell, and set it as an enemy cell.
    /// </summary>
    public void SetEnemyOnMap()
    {
        GameObject enemy_object;
        Vector3 position;
        int random_value;
        int single_enemy_density = 80;
        int duo_enemy_density = 60;
        int trio_enemy_density = 10;
        int[] enemy_density = { single_enemy_density, duo_enemy_density, trio_enemy_density };
        GridMapCell cell;
        int enemy_count_per_encounter = 3;
        List<GridMapCell> visited_list = new List<GridMapCell>();
        GameObject enemy;

        enemy = AssetsLoader.Assets.LoadGameObject("Enemy", LoadEnum.Map);

        for (int i = 1; i <= enemy_count_per_encounter; i++)
        {
            for (int j = 0; j < _UnoccupiedCells.Count; j++)
            {
                cell = _UnoccupiedCells[j];

                random_value = Random.Range(0, 100);

                if (random_value < enemy_density[i - 1])
                {
                    position = ConvertCoordinateToPosition(cell.GridPosition.x, cell.GridPosition.y);

                    cell.CellType = TypeGridMapCell.Enemy;

                    enemy_object = GameObject.Instantiate(enemy, position, Quaternion.identity);

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
        GameObject treasure;

        treasure = AssetsLoader.Assets.LoadGameObject("TreasureBox", LoadEnum.Map);

        for (int i = 0; i < _UnoccupiedCells.Count; i++)
        {
            cell = _UnoccupiedCells[i];

            random_value = Random.Range(0, 100);

            if (random_value < treasure_density)
            {
                position = ConvertCoordinateToPosition(cell.GridPosition.x, cell.GridPosition.y);

                cell.CellType = TypeGridMapCell.Treasure;

                teasure_object = GameObject.Instantiate(treasure, position, Quaternion.identity);

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
        GameObject rest;

        rest = AssetsLoader.Assets.LoadGameObject("RestPlace", LoadEnum.Map);

        for (int i = 0; i < _UnoccupiedCells.Count; i++)
        {
            cell = _UnoccupiedCells[i];

            random_value = Random.Range(0, 100);

            if (random_value < rest_place_density)
            {
                position = ConvertCoordinateToPosition(cell.GridPosition.x, cell.GridPosition.y);

                cell.CellType = TypeGridMapCell.RestPlace;

                rest_place_object = GameObject.Instantiate(rest, position, Quaternion.identity);

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
        GameObject cystal_temple;

        cystal_temple = AssetsLoader.Assets.LoadGameObject("Cystal", LoadEnum.Map);

        for (int i = 0; i < _UnoccupiedCells.Count; i++)
        {
            cell = _UnoccupiedCells[i];

            random_value = Random.Range(0, 100);

            if (random_value < cystal_temple_density)
            {
                position = ConvertCoordinateToPosition(cell.GridPosition.x, cell.GridPosition.y);

                cell.CellType = TypeGridMapCell.CystalTemple;

                cystal_temple_object = GameObject.Instantiate(cystal_temple, position, Quaternion.identity);

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

    public void SetSurvivedSpirit()
    {
        GridMapCell cell;
        List<GridMapCell> visited_list = new List<GridMapCell>();
        Vector3 position;
        GameObject survived_spirit_object;
        GameObject survived_spirit;

        survived_spirit = AssetsLoader.Assets.LoadGameObject("SurvivedSpirit", LoadEnum.Map);

        for (int i = 0; i < _UnoccupiedCells.Count; i++)
        {
            cell = _UnoccupiedCells[i];

            position = ConvertCoordinateToPosition(cell.GridPosition.x, cell.GridPosition.y);

            cell.CellType = TypeGridMapCell.SurvivedSpirit;

            survived_spirit_object = GameObject.Instantiate(survived_spirit, position, Quaternion.identity);

            survived_spirit_object.transform.SetParent(_MapObject.transform.GetChild(cell.GameObjectIndexInContainer).transform);

            visited_list.Add(cell);
        }

        for (int i = 0; i < visited_list.Count; i++)
        {
            _UnoccupiedCells.Remove(visited_list[i]);
        }

        visited_list.Clear();
    }

    public GridMapCell Teleport(GridMapCell cell)
    {
        General.SetMainCameraPositionXYOnly(ConvertCoordinateToPosition(cell.DestinatioX, cell.DestinatioY));
        return (MovePlayerToSelectedCell(ConvertCoordinateToPosition(cell.DestinatioX, cell.DestinatioY), true));
    }

    public GridMapCell MovePlayerToSelectedCell(Vector3 cell_position, bool is_teleport = false)
    {
        GridMapCell cell = GetValue(cell_position);
        bool is_reachable = false;

        if (cell != null)
        {
            if (cell.CellType != TypeGridMapCell.Wall)
            {
                is_reachable = CheckReachable(cell.GridPosition.x, cell.GridPosition.y);

                if (is_reachable || is_teleport)
                {
                    SetReachableCell(_PlayerCurrentCoordinate.x, _PlayerCurrentCoordinate.y, false);

                    DisablePreviousCell(_PlayerCurrentCoordinate.x, _PlayerCurrentCoordinate.y);

                    PlayerObject.transform.position = ConvertCoordinateToPosition(cell.GridPosition.x, cell.GridPosition.y);

                    _PlayerCurrentCoordinate = cell.GridPosition;

                    SetReachableCell(_PlayerCurrentCoordinate.x, _PlayerCurrentCoordinate.y, true);
                }
            }
        }

        if ((cell != null) && (!is_reachable) && (!is_teleport))
        {
            cell = null;
        }

        return (cell);
    }

    public GridMapCell MovePlayerToSelectedCell(int x, int y, bool is_teleport = false)
    {
        GridMapCell cell = GetValue(x, y);
        bool is_reachable = false;

        if (cell != null)
        {
            if (cell.CellType != TypeGridMapCell.Wall)
            {
                is_reachable = CheckReachable(cell.GridPosition.x, cell.GridPosition.y);

                if (is_reachable || is_teleport)
                {
                    SetReachableCell(_PlayerCurrentCoordinate.x, _PlayerCurrentCoordinate.y, false);

                    DisablePreviousCell(_PlayerCurrentCoordinate.x, _PlayerCurrentCoordinate.y);

                    PlayerObject.transform.position = ConvertCoordinateToPosition(cell.GridPosition.x, cell.GridPosition.y);

                    _PlayerCurrentCoordinate = cell.GridPosition;

                    SetReachableCell(_PlayerCurrentCoordinate.x, _PlayerCurrentCoordinate.y, true);
                }
            }
        }

        if ((cell != null) && (!is_reachable) && (!is_teleport))
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
