using System.Collections.Generic;
using UnityEngine;

public class EventMap : BaseGrid<GridMapCell>
{
    public enum EventCellType
    {
        Block,
        Enemy,
        Open,
        Treasure,
        RestPlace,
        CystalTemple,
        WormHole,
        SurvivedSpirit,
        Final
    }

    private readonly float _NoiseDensity = 0;
    private readonly int _SmoothingCount = 0;

    private readonly GameObject _CellTemplate = null;
    private readonly GameObject _MapObject = null;

    private readonly List<GridMapCell> _UnoccupiedCells = new List<GridMapCell>();

    private readonly List<List<GridMapCell>> _IsolatedGroups = new List<List<GridMapCell>>();

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="cell_size"></param>
    /// <param name="origin_point"></param>
    public EventMap(int width, int height, float cell_size, Vector2 origin_point, float noise_density, int smoothing_count, GameObject cell_template, GameObject map_object)
    : base(width, height, cell_size, origin_point)
    {
        _NoiseDensity = noise_density;
        _SmoothingCount = smoothing_count;

        _CellTemplate = cell_template;
        _MapObject = map_object;
    }

    public GameObject PlayerObject { get; private set; }
    public (int x, int y) PlayerCurrentCoordinate { get; private set; } = (0, 0);
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

        for (int i = _IsolatedGroups.Count - 1; i >= 0; i--)
        {
            for (int j = _IsolatedGroups[i].Count - 1; j >= 0; j--)
            {
                _IsolatedGroups[i].RemoveAt(j);
            }

            _IsolatedGroups.RemoveAt(i);
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
    /// Randomly set each cell as a normal or block cell
    /// </summary>
    private void InitializeGridMap()
    {
        // Initialize all cells
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                // Set the border cells to be block
                if ((i == 0) || (i == Width - 1) || (j == 0) || (j == Height - 1))
                {
                    SetValue(i, j, new GridMapCell((i, j), EventCellType.Block));
                }

                // If success, then the current cell is block
                else if (GeneralRandom.RollDiceAndCheckIfSuccess(_NoiseDensity))
                {
                    SetValue(i, j, new GridMapCell((i, j), EventCellType.Block));
                }

                // Otherwise, the current cell is open
                else
                {
                    SetValue(i, j, new GridMapCell((i, j), EventCellType.Open));
                }
            }
        }
    }

    /// <summary>
    /// Apply smoothing to the randomly generated grid
    /// </summary>
    private void ApplySmoothingToGridMap()
    {
        int block_count;
        GridMapCell cell;
        int block_required_count = 4;

        // Apply smoothing for number of times
        for (int t = 0; t < _SmoothingCount; t++)
        {
            _UnoccupiedCells.Clear();

            // Re-define each cell by its neighbors
            for (int i = 1; i < Width - 1; i++)
            {
                for (int j = 1; j < Height - 1; j++)
                {
                    block_count = GetBlockCount(i, j);

                    cell = GetValue(i, j);

                    // If the current cell has 4 blocks around it, then it becomes a block
                    if (block_count > block_required_count)
                    {
                        GetValue(i, j).CellTypeOnNextIteration = EventCellType.Block;
                    }

                    // Other, then the current cell becomes open
                    else
                    {
                        GetValue(i, j).CellTypeOnNextIteration = EventCellType.Open;

                        _UnoccupiedCells.Add(cell);
                    }
                }
            }

            // Apply changes
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    cell = GetValue(i, j);

                    cell.CellType = cell.CellTypeOnNextIteration;
                }
            }
        }
    }

    /// <summary>
    /// Remove open cells that are surrounding by all blocks
    /// </summary>
    private void RemoveIsolatedCell()
    {
        int block_count;
        GridMapCell cell;
        int isolated_required_block_count = 8;
        List<GridMapCell> isolated_cell_list = new List<GridMapCell>();

        while (true)
        {
            // Go through all open cells 
            for (int j = 0; j < _UnoccupiedCells.Count; j++)
            {
                cell = _UnoccupiedCells[j];

                block_count = GetBlockCount(cell.GridPosition.x, cell.GridPosition.y);

                // If the current cell is surrounding by blocks, add it to the list
                if (block_count == isolated_required_block_count)
                {
                    isolated_cell_list.Add(cell);
                }
            }

            // If there is no isolated cell, exit
            if (isolated_cell_list.Count == 0)
            {
                break;
            }
            else
            {
                // Turn the isolated open cell to a block
                for (int j = 0; j < isolated_cell_list.Count; j++)
                {
                    isolated_cell_list[j].CellType = EventCellType.Block;

                    _UnoccupiedCells.Remove(isolated_cell_list[j]);
                }

                // Reset the list
                isolated_cell_list.Clear();
            }
        }
    }

    /// <summary>
    /// Create a list of isolated cell groups
    /// </summary>
    private void DefineIsolatedParts()
    {
        int count = 0;
        GridMapCell cell;

        // Get through each open cells to group them
        for (int i = 0; i < _UnoccupiedCells.Count; i++)
        {
            cell = _UnoccupiedCells[i];

            // If the cell is not visited before, add it to the list
            if ((!cell.IsVisited) && (cell.CellType == EventCellType.Open))
            {
                _IsolatedGroups.Add(new List<GridMapCell>());

                AddNeighborToList(cell.GridPosition.x, cell.GridPosition.y, _IsolatedGroups[count]);

                count++;
            }
        }
    }

    /// <summary>
    /// Add the neighbor cell to the list
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="isolated_group"></param>
    private void AddNeighborToList(int x, int y, List<GridMapCell> isolated_group)
    {
        GridMapCell cell = GetValue(x, y);
        cell.IsVisited = true;

        isolated_group.Add(cell);

        // Check the neighbor at the left
        if (IsValid(x - 1, y))
        {
            AddNeighborToList(x - 1, y, isolated_group);
        }

        // Check the neighbor at the right
        if (IsValid(x + 1, y))
        {
            AddNeighborToList(x + 1, y, isolated_group);
        }

        // Check the neighbor at the top
        if (IsValid(x, y - 1))
        {
            AddNeighborToList(x, y - 1, isolated_group);
        }

        // Check the neighbor at the bottom
        if (IsValid(x, y + 1))
        {
            AddNeighborToList(x, y + 1, isolated_group);
        }
    }

    /// <summary>
    /// Check if the cell at the specified coordinate is valid
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool IsValid(int x, int y)
    {
        bool is_valid = false;

        // Check if the coordinate is in range
        if ((x >= 0) && (x < Width) && (y >= 0) && (y < Height))
        {
            GridMapCell cell = GetValue(x, y);

            // Check if the cell is open and not visited before
            if ((!cell.IsVisited) && (cell.CellType == EventCellType.Open))
            {
                is_valid = true;
            }
        }

        return (is_valid);
    }

    /// <summary>
    /// Count the number of blocks in the neighbors
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int GetBlockCount(int x, int y)
    {
        int block_count = 0;

        // Go through each cell, and sum up all blocks
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                // An invalid coordinate is counted as blocks
                if ((i < 0) || (i >= Width) || (j < 0) || (j >= Height))
                {
                    block_count++;
                }

                // Check if the cell is block
                else if ((i != x) || (j != y))
                {
                    if (GetValue(i, j).CellType == EventCellType.Block)
                    {
                        block_count++;
                    }
                }
            }
        }

        return (block_count);
    }

    /// <summary>
    /// Instantiate tiles for the map
    /// </summary>
    private void GenerateRoomObjects()
    {
        GameObject room_object;
        GridMapCell room_to_create;
        int game_object_index = 0;
        
        // Go through all cells, and create an object for each of them
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                room_object = GameObject.Instantiate(_CellTemplate, GetPosition(i, j), Quaternion.identity);

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

        worm_hole = AssetsLoader.Assets.LoadGameObject("WormHole", LoadObjectEnum.Map);

        for (int i = 1; i < _IsolatedGroups.Count; i++)
        {
            do
            {
                random1 = Random.Range(0, _IsolatedGroups[i - 1].Count);
                random2 = Random.Range(0, _IsolatedGroups[i].Count);
            } while ((_IsolatedGroups[i - 1][random1].CellType == EventCellType.WormHole) || (_IsolatedGroups[i][random2].CellType == EventCellType.WormHole));

            cell1 = _IsolatedGroups[i - 1][random1];
            cell2 = _IsolatedGroups[i][random2];

            cell1.CellType = EventCellType.WormHole;
            cell2.CellType = EventCellType.WormHole;

            cell1.DestinatioX = cell2.GridPosition.x;
            cell1.DestinatioY = cell2.GridPosition.y;
            cell2.DestinatioX = cell1.GridPosition.x;
            cell2.DestinatioY = cell1.GridPosition.y;

            _UnoccupiedCells.Remove(cell1);
            _UnoccupiedCells.Remove(cell2);

            position1 = GetPosition(cell1.GridPosition.x, cell1.GridPosition.y);
            wormhole1_object = GameObject.Instantiate(worm_hole, position1, Quaternion.identity);
            wormhole1_object.transform.SetParent(_MapObject.transform.GetChild(cell1.GameObjectIndexInContainer).transform);

            position2 = GetPosition(cell2.GridPosition.x, cell2.GridPosition.y);
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

        player = AssetsLoader.Assets.LoadGameObject("SmileFace", LoadObjectEnum.Map);

        if (_UnoccupiedCells.Count > 0)
        {
            GridMapCell cell = _UnoccupiedCells[Random.Range(0, _UnoccupiedCells.Count)];

            Vector3 position = GetPosition(cell.GridPosition.x, cell.GridPosition.y);

            _UnoccupiedCells.Remove(cell);

            PlayerObject = GameObject.Instantiate(player, position, Quaternion.identity);
            PlayerObject.transform.SetParent(_MapObject.transform);
            PlayerCurrentCoordinate = (cell.GridPosition.x, cell.GridPosition.y);

            SetReachableCell(cell.GridPosition.x, cell.GridPosition.y, true);

            GeneralInput.SetMainCameraPositionXYOnly(position);
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

        enemy = AssetsLoader.Assets.LoadGameObject("Enemy", LoadObjectEnum.Map);

        for (int i = 1; i <= enemy_count_per_encounter; i++)
        {
            for (int j = 0; j < _UnoccupiedCells.Count; j++)
            {
                cell = _UnoccupiedCells[j];

                random_value = Random.Range(0, 100);

                if (random_value < enemy_density[i - 1])
                {
                    position = GetPosition(cell.GridPosition.x, cell.GridPosition.y);

                    cell.CellType = EventCellType.Enemy;

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

        treasure = AssetsLoader.Assets.LoadGameObject("TreasureBox", LoadObjectEnum.Map);

        for (int i = 0; i < _UnoccupiedCells.Count; i++)
        {
            cell = _UnoccupiedCells[i];

            random_value = Random.Range(0, 100);

            if (random_value < treasure_density)
            {
                position = GetPosition(cell.GridPosition.x, cell.GridPosition.y);

                cell.CellType = EventCellType.Treasure;

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

        rest = AssetsLoader.Assets.LoadGameObject("RestPlace", LoadObjectEnum.Map);

        for (int i = 0; i < _UnoccupiedCells.Count; i++)
        {
            cell = _UnoccupiedCells[i];

            random_value = Random.Range(0, 100);

            if (random_value < rest_place_density)
            {
                position = GetPosition(cell.GridPosition.x, cell.GridPosition.y);

                cell.CellType = EventCellType.RestPlace;

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

        cystal_temple = AssetsLoader.Assets.LoadGameObject("Cystal", LoadObjectEnum.Map);

        for (int i = 0; i < _UnoccupiedCells.Count; i++)
        {
            cell = _UnoccupiedCells[i];

            random_value = Random.Range(0, 100);

            if (random_value < cystal_temple_density)
            {
                position = GetPosition(cell.GridPosition.x, cell.GridPosition.y);

                cell.CellType = EventCellType.CystalTemple;

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

        survived_spirit = AssetsLoader.Assets.LoadGameObject("SurvivedSpirit", LoadObjectEnum.Map);

        for (int i = 0; i < _UnoccupiedCells.Count; i++)
        {
            cell = _UnoccupiedCells[i];

            position = GetPosition(cell.GridPosition.x, cell.GridPosition.y);

            cell.CellType = EventCellType.SurvivedSpirit;

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
        GeneralInput.SetMainCameraPositionXYOnly(GetPosition(cell.DestinatioX, cell.DestinatioY));
        return (MovePlayerToSelectedCell(GetPosition(cell.DestinatioX, cell.DestinatioY), true));
    }

    public GridMapCell MovePlayerToSelectedCell(Vector3 cell_position, bool is_teleport = false)
    {
        GetCoordinate(cell_position, out int x, out int y);

        GridMapCell cell = MovePlayerToSelectedCell(x, y, is_teleport);

        return (cell);
    }

    public GridMapCell MovePlayerToSelectedCell(int x, int y, bool is_teleport = false)
    {
        GridMapCell cell = GetValue(x, y);
        bool is_reachable = false;

        if (cell != null)
        {
            if (cell.CellType != EventCellType.Block)
            {
                is_reachable = CheckReachable(cell.GridPosition.x, cell.GridPosition.y);

                if (is_reachable || is_teleport)
                {
                    SetReachableCell(PlayerCurrentCoordinate.x, PlayerCurrentCoordinate.y, false);

                    DisablePreviousCell(PlayerCurrentCoordinate.x, PlayerCurrentCoordinate.y);

                    PlayerObject.transform.position = GetPosition(cell.GridPosition.x, cell.GridPosition.y);

                    PlayerCurrentCoordinate = cell.GridPosition;

                    SetReachableCell(PlayerCurrentCoordinate.x, PlayerCurrentCoordinate.y, true);
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

        if ((!is_reachable) && (x == PlayerCurrentCoordinate.x + 1) && (y == PlayerCurrentCoordinate.y))
        {
            is_reachable = true;
        }

        if ((!is_reachable) && (x == PlayerCurrentCoordinate.x - 1) && (y == PlayerCurrentCoordinate.y))
        {
            is_reachable = true;
        }

        if ((!is_reachable) && (x == PlayerCurrentCoordinate.x) && (y == PlayerCurrentCoordinate.y - 1))
        {
            is_reachable = true;
        }

        if ((!is_reachable) && (x == PlayerCurrentCoordinate.x) && (y == PlayerCurrentCoordinate.y + 1))
        {
            is_reachable = true;
        }

        if (is_reachable)
        {
            cell = GetValue(x, y);

            if ((cell == null) || (cell.CellType == EventCellType.Block))
            {
                is_reachable = false;
            }
        }

        return (is_reachable);
    }

    /// <summary>
    /// Set the previous cell as a block
    /// </summary>
    private void DisablePreviousCell(int x, int y)
    {
        GridMapCell cell = GetValue(x, y);

        cell.DisableCell();

        _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<GridMapCellSpriteSelector>().SetSprite(cell.CellType);
    }
}
