using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMapCell
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="grid_position"></param>
    /// <param name="room_type"></param>
    public GridMapCell((int x, int y) grid_position, TypeGridMapCell room_type)
    {
        GridPosition = grid_position;
        CellType = room_type;
        IsOccupied = false;
    }

    #region Properties
    public (int x, int y) GridPosition { get; private set; }

    public TypeGridMapCell CellType { get; set; }

    public int GameObjectIndexInContainer { get; set; }

    public bool IsOccupied { get; set; }
    #endregion

    /// <summary>
    /// Mark the cell as a wall
    /// </summary>
    public void DisableCell()
    {
        CellType = TypeGridMapCell.Wall;
    }
}
