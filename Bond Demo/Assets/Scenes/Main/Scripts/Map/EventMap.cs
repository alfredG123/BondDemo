using System;
using System.Collections.Generic;
using UnityEngine;

public class EventMap : BaseGrid<EventCell>
{
    public enum EventCellType
    {
        Block,
        Open,
        EnemySolo,
        EnemyDuo,
        EnemyTrio,
        Treasure,
        RestPlace,
        CystalTemple,
        WormHole,
        SurvivedSpirit,
        Final
    }

    private readonly float _NoiseDensity = 0;
    private readonly int _SmoothingCount = 0;

    private readonly GameObject _MapObject = null;

    private readonly List<EventCell> _OpenCells = new List<EventCell>();

    private readonly List<List<EventCell>> _IsolatedGroups = new List<List<EventCell>>();

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="cell_size"></param>
    /// <param name="origin_point"></param>
    public EventMap(int width, int height, float cell_size, Vector2 origin_point, float noise_density, int smoothing_count, GameObject map_object)
    : base(width, height, cell_size, origin_point)
    {
        _NoiseDensity = noise_density;
        _SmoothingCount = smoothing_count;

        _MapObject = map_object;
    }

    public GameObject PlayerObject { get; private set; }
    public (int x, int y) PlayerCurrentCoordinate { get; private set; } = (0, 0);
    public bool HasReachableCell { get; private set; }
    public Vector3 PlayerPosition
    {
        get
        {
            return (GetPosition(PlayerCurrentCoordinate.x, PlayerCurrentCoordinate.y));
        }
    }

    /// <summary>
    /// Create a grid for the map, and generate the objects
    /// </summary>
    public void CreateMap()
    {
        GenerateGridMap();

        GenerateRoomObjects();

        SetWormHole();

        SetPlayerOnMap();

        SetEnemyOnMap(single_enemy_density:.8f, duo_enemy_density:.6f, trio_enemy_density:.1f);

        SetTreasureOnMap(treasure_density:.2f);

        SetRestPlaceOnMap(rest_place_density:.2f);

        SetCystalTempleOnMap(cystal_temple_density:.2f);

        SetSurvivedSpirit(survived_spirit_density:.2f);

        FillUpOpenCellsWithEnemy();
    }

    public void ClearMap()
    {
        foreach (Transform child in _MapObject.transform)
        {
            UnityEngine.Object.Destroy(child.gameObject);
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
                    SetValue(i, j, new EventCell((i, j), EventCellType.Block));
                }

                // If success, then the current cell is block
                else if (GeneralRandom.RollDiceAndCheckIfSuccess(_NoiseDensity))
                {
                    SetValue(i, j, new EventCell((i, j), EventCellType.Block));
                }

                // Otherwise, the current cell is open
                else
                {
                    SetValue(i, j, new EventCell((i, j), EventCellType.Open));
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
        EventCell cell;
        int block_required_count = 4;

        // Apply smoothing for number of times
        for (int t = 0; t < _SmoothingCount; t++)
        {
            _OpenCells.Clear();

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

                        _OpenCells.Add(cell);
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
        EventCell cell;
        int isolated_required_block_count = 8;
        List<EventCell> isolated_cell_list = new List<EventCell>();

        while (true)
        {
            // Go through all open cells 
            for (int j = 0; j < _OpenCells.Count; j++)
            {
                cell = _OpenCells[j];

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

                    _OpenCells.Remove(isolated_cell_list[j]);
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
        EventCell cell;

        // Get through each open cells to group them
        for (int i = 0; i < _OpenCells.Count; i++)
        {
            cell = _OpenCells[i];

            // If the cell is not visited before, add it to the list
            if ((!cell.IsVisited) && (cell.CellType == EventCellType.Open))
            {
                _IsolatedGroups.Add(new List<EventCell>());

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
    private void AddNeighborToList(int x, int y, List<EventCell> isolated_group)
    {
        EventCell cell = GetValue(x, y);
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
            EventCell cell = GetValue(x, y);

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
        GameObject cell_object;
        EventCell event_cell;
        int cell_index = 0;
        GameObject cell_prefab;

        cell_prefab = AssetsLoader.Assets.LoadGameObject("EventCell", LoadObjectEnum.Map);

        // Go through all cells, and create an object for each of them
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                event_cell = GetValue(i, j);

                event_cell.GameObjectIndexInContainer = cell_index;

                cell_object = GameObject.Instantiate(cell_prefab, GetPosition(i, j), Quaternion.identity);

                cell_object.GetComponent<EventCellSpriteSelector>().SetSprite(event_cell.CellType);

                cell_object.transform.SetParent(_MapObject.transform);

                cell_index++;
            }
        }
    }

    /// <summary>
    /// Pick two cells from two isolated group, and create a wormhole event to connect them
    /// </summary>
    private void SetWormHole()
    {
        int wormhole1_index;
        int wormhole2_index;
        GameObject wormhole1_object;
        GameObject wormhole2_object;
        Vector2 wormhole1_position;
        Vector2 wormhole2_position;
        EventCell wormhole1_cell;
        EventCell wormhole2_cell;
        GameObject worm_hole_prefab;
        int min_group_required = 2;

        // If there is not enough group, exit
        if (_IsolatedGroups.Count < min_group_required)
        {
            return;
        }

        worm_hole_prefab = AssetsLoader.Assets.LoadGameObject("WormHole", LoadObjectEnum.Map);

        for (int i = 1; i < _IsolatedGroups.Count; i++)
        {
            // Find two cells in the grid for the entry and exit
            do
            {
                wormhole1_index = GeneralRandom.GetRandomNumberInRange(0, _IsolatedGroups[i - 1].Count);
                wormhole2_index = GeneralRandom.GetRandomNumberInRange(0, _IsolatedGroups[i].Count);
            } while ((_IsolatedGroups[i - 1][wormhole1_index].CellType == EventCellType.WormHole) || (_IsolatedGroups[i][wormhole2_index].CellType == EventCellType.WormHole));

            // Get two selected cells
            wormhole1_cell = _IsolatedGroups[i - 1][wormhole1_index];
            wormhole2_cell = _IsolatedGroups[i][wormhole2_index];

            // Set the type
            wormhole1_cell.CellType = EventCellType.WormHole;
            wormhole2_cell.CellType = EventCellType.WormHole;

            // Connection two cells
            wormhole1_cell.DestinatioX = wormhole2_cell.GridPosition.x;
            wormhole1_cell.DestinatioY = wormhole2_cell.GridPosition.y;
            wormhole2_cell.DestinatioX = wormhole1_cell.GridPosition.x;
            wormhole2_cell.DestinatioY = wormhole1_cell.GridPosition.y;

            // Remove two cells as they are used
            _OpenCells.Remove(wormhole1_cell);
            _OpenCells.Remove(wormhole2_cell);

            // Create the wormhole1 cell
            wormhole1_position = GetPosition(wormhole1_cell.GridPosition.x, wormhole1_cell.GridPosition.y);
            wormhole1_object = GameObject.Instantiate(worm_hole_prefab, wormhole1_position, Quaternion.identity);
            wormhole1_object.transform.SetParent(_MapObject.transform.GetChild(wormhole1_cell.GameObjectIndexInContainer).transform);

            // Create the wormhole2 cell
            wormhole2_position = GetPosition(wormhole2_cell.GridPosition.x, wormhole2_cell.GridPosition.y);
            wormhole2_object = GameObject.Instantiate(worm_hole_prefab, wormhole2_position, Quaternion.identity);
            wormhole2_object.transform.SetParent(_MapObject.transform.GetChild(wormhole2_cell.GameObjectIndexInContainer).transform);
        }
    }

    /// <summary>
    /// Randomly choose a valid grid map cell, and set the player at the position of the cell
    /// </summary>
    private void SetPlayerOnMap()
    {
        GameObject player_prefab;
        EventCell cell;
        Vector3 position;

        player_prefab = AssetsLoader.Assets.LoadGameObject("SmileFace", LoadObjectEnum.Map);

        // Get an open cell from the list
        cell = _OpenCells[GeneralRandom.GetRandomNumberInRange(0, _OpenCells.Count)];
        
        // Get the position of the cell
        position = GetPosition(cell.GridPosition.x, cell.GridPosition.y);

        // Remove the cell from the open list
        _OpenCells.Remove(cell);

        // Create the object
        PlayerObject = GameObject.Instantiate(player_prefab, position, Quaternion.identity);
        PlayerObject.transform.SetParent(_MapObject.transform);

        // Record the coordinate
        PlayerCurrentCoordinate = (cell.GridPosition.x, cell.GridPosition.y);

        // Colorize the cells to which the player can move
        DisplayReachableCell(cell.GridPosition.x, cell.GridPosition.y, true);
    }

    /// <summary>
    /// Randomly choose a valid grid map cell, and set it as an enemy cell.
    /// </summary>
    private void SetEnemyOnMap(float single_enemy_density, float duo_enemy_density, float trio_enemy_density)
    {
        float[] enemy_density = { single_enemy_density, duo_enemy_density, trio_enemy_density };
        int enemy_count_per_encounter = 3;
        GameObject enemy_prefab;
        EventCellType enemy_type;

        // For each enemy count, generate event for it
        for (int i = 1; i <= enemy_count_per_encounter; i++)
        {
            // Get the prefab that represents the count
            enemy_prefab = GetEnemyPrefab(i);

            // Get the evnt type that represents the count
            enemy_type = GetEnemyType(i);

            // Place enemy events randomly on the map
            for (int j = 0; j < _OpenCells.Count; j++)
            {
                if (GeneralRandom.RollDiceAndCheckIfSuccess(enemy_density[i - 1]))
                {
                    GenerateEvent(j, enemy_type, enemy_prefab);
                }
            }
        }
    }

    /// <summary>
    /// Get the prefab based on the enemy count
    /// </summary>
    /// <param name="enemy_count"></param>
    /// <returns></returns>
    private GameObject GetEnemyPrefab(int enemy_count)
    {
        GameObject enemy_prefab = null;

        if (enemy_count == 1)
        {
            enemy_prefab = AssetsLoader.Assets.LoadGameObject("EnemySolo", LoadObjectEnum.Map);
        }
        else if (enemy_count == 2)
        {
            enemy_prefab = AssetsLoader.Assets.LoadGameObject("EnemyDuo", LoadObjectEnum.Map);
        }
        else if (enemy_count == 3)
        {
            enemy_prefab = AssetsLoader.Assets.LoadGameObject("EnemyTrio", LoadObjectEnum.Map);
        }

        return (enemy_prefab);
    }

    /// <summary>
    /// Determine the event type based on the enemy count
    /// </summary>
    /// <param name="enemy_count"></param>
    /// <returns></returns>
    private EventCellType GetEnemyType(int enemy_count)
    {
        EventCellType enemy_type = EventCellType.EnemySolo;

        if (enemy_count == 1)
        {
            enemy_type = EventCellType.EnemySolo;
        }
        else if (enemy_count == 2)
        {
            enemy_type = EventCellType.EnemyDuo;
        }
        else if (enemy_count == 3)
        {
            enemy_type = EventCellType.EnemyTrio;
        }

        return (enemy_type);
    }

    /// <summary>
    /// Randomly choose a valid grid map cell, and set it as a teasure.
    /// </summary>
    private void SetTreasureOnMap(float treasure_density)
    {
        GameObject treasure_prefab;

        treasure_prefab = AssetsLoader.Assets.LoadGameObject("TreasureBox", LoadObjectEnum.Map);

        // Place treasure events randomly on the map
        for (int i = 0; i < _OpenCells.Count; i++)
        {
            if (GeneralRandom.RollDiceAndCheckIfSuccess(treasure_density))
            {
                GenerateEvent(i, EventCellType.Treasure, treasure_prefab);
            }
        }
    }

    /// <summary>
    /// Randomly choose a valid grid map cell, and set it as a rest place.
    /// </summary>
    private void SetRestPlaceOnMap(float rest_place_density)
    {
        GameObject rest_place_prefab;

        rest_place_prefab = AssetsLoader.Assets.LoadGameObject("RestPlace", LoadObjectEnum.Map);

        // Place rest place events randomly on the map
        for (int i = 0; i < _OpenCells.Count; i++)
        {
            if (GeneralRandom.RollDiceAndCheckIfSuccess(rest_place_density))
            {
                GenerateEvent(i, EventCellType.RestPlace, rest_place_prefab);
            }
        }
    }

    /// <summary>
    /// Randomly choose a valid grid map cell, and set it as a cystal temple
    /// </summary>
    private void SetCystalTempleOnMap(float cystal_temple_density)
    {
        GameObject cystal_temple_prefab;

        cystal_temple_prefab = AssetsLoader.Assets.LoadGameObject("Cystal", LoadObjectEnum.Map);

        // Place cystal temple events randomly on the map
        for (int i = 0; i < _OpenCells.Count; i++)
        {
            if (GeneralRandom.RollDiceAndCheckIfSuccess(cystal_temple_density))
            {
                GenerateEvent(i, EventCellType.CystalTemple, cystal_temple_prefab);
            }
        }
    }

    /// <summary>
    /// Randomly choose a valid grid map cell, and set it as a survived spirit event type
    /// </summary>
    private void SetSurvivedSpirit(float survived_spirit_density)
    {
        GameObject survived_spirit_prefab;

        survived_spirit_prefab = AssetsLoader.Assets.LoadGameObject("SurvivedSpirit", LoadObjectEnum.Map);

        // Place survived spirit events randomly on the map
        for (int i = 0; i < _OpenCells.Count; i++)
        {
            if (GeneralRandom.RollDiceAndCheckIfSuccess(survived_spirit_density))
            {
                GenerateEvent(i, EventCellType.SurvivedSpirit, survived_spirit_prefab);
            }
        }
    }

    /// <summary>
    /// Fill up open cells with enemy event
    /// </summary>
    private void FillUpOpenCellsWithEnemy()
    {
        GameObject enemy_prefab;
        int empty = 0;
        int first_item_index = 0;

        enemy_prefab = AssetsLoader.Assets.LoadGameObject("EnemySolo", LoadObjectEnum.Map);

        // Fill up all open cells with solo enemy event
        while(_OpenCells.Count > empty)
        {
            GenerateEvent(first_item_index, EventCellType.EnemySolo, enemy_prefab);
        }
    }

    /// <summary>
    /// Instantiate a event prefab on the specified cell
    /// </summary>
    /// <param name="open_cell_index"></param>
    /// <param name="event_type"></param>
    /// <param name="event_prefab"></param>
    private void GenerateEvent(int open_cell_index, EventCellType event_type, GameObject event_prefab)
    {
        EventCell cell;
        Vector3 position;
        GameObject event_object;

        cell = _OpenCells[open_cell_index];

        position = GetPosition(cell.GridPosition.x, cell.GridPosition.y);

        cell.CellType = event_type;

        event_object = GameObject.Instantiate(event_prefab, position, Quaternion.identity);

        event_object.transform.SetParent(_MapObject.transform.GetChild(cell.GameObjectIndexInContainer).transform);

        _OpenCells.Remove(cell);
    }

    /// <summary>
    /// Move the player object to the specified cell
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    public EventCell Teleport(EventCell cell)
    {
        return (MovePlayerToSelectedCell(GetPosition(cell.DestinatioX, cell.DestinatioY), true));
    }

    /// <summary>
    /// Move the player object to the specified cell
    /// </summary>
    /// <param name="cell_position"></param>
    /// <param name="is_teleport"></param>
    /// <returns></returns>
    public EventCell MovePlayerToSelectedCell(Vector3 cell_position, bool is_teleport = false)
    {
        GetCoordinate(cell_position, out int x, out int y);

        EventCell cell = MovePlayerToSelectedCell(x, y, is_teleport);

        return (cell);
    }

    /// <summary>
    /// Move the player object to the specified cell
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="is_teleport"></param>
    /// <returns></returns>
    public EventCell MovePlayerToSelectedCell(int x, int y, bool is_teleport = false)
    {
        EventCell cell = GetValue(x, y);
        bool is_reachable = false;

        // If there is a cell at the specified coordinate, move the player object to there
        if (cell != null)
        {
            // Verify the cell is valid
            if (cell.CellType != EventCellType.Block)
            {
                is_reachable = CheckReachable(cell.GridPosition.x, cell.GridPosition.y);

                // Verify the cell is within reach, or the action is teleporting
                if (is_reachable || is_teleport)
                {
                    DisplayReachableCell(PlayerCurrentCoordinate.x, PlayerCurrentCoordinate.y, false);

                    DisablePreviousCell(PlayerCurrentCoordinate.x, PlayerCurrentCoordinate.y);

                    PlayerObject.transform.position = GetPosition(cell.GridPosition.x, cell.GridPosition.y);

                    PlayerCurrentCoordinate = cell.GridPosition;

                    DisplayReachableCell(PlayerCurrentCoordinate.x, PlayerCurrentCoordinate.y, true);
                }
            }
        }

        // Reset the cell if it is invalid
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
    private bool DisplayReachableCell(int x, int y, bool is_visible)
    {
        EventCell cell;

        HasReachableCell = false;

        if (CheckReachable(x - 1, y))
        {
            cell = GetValue(x - 1, y);

            _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<EventCellSpriteSelector>().SetColorForReachable(is_visible);

            HasReachableCell = true;
        }

        if (CheckReachable(x + 1, y))
        {
            cell = GetValue(x + 1, y);

            _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<EventCellSpriteSelector>().SetColorForReachable(is_visible);

            HasReachableCell = true;
        }

        if (CheckReachable(x, y - 1))
        {
            cell = GetValue(x, y - 1);

            _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<EventCellSpriteSelector>().SetColorForReachable(is_visible);

            HasReachableCell = true;
        }

        if (CheckReachable(x, y + 1))
        {
            cell = GetValue(x, y + 1);

            _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<EventCellSpriteSelector>().SetColorForReachable(is_visible);

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
        EventCell cell;
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
        EventCell cell = GetValue(x, y);

        cell.DisableCell();

        _MapObject.transform.GetChild(cell.GameObjectIndexInContainer).GetComponent<EventCellSpriteSelector>().SetSprite(cell.CellType);
    }
}
