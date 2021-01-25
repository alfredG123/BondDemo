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
        RoomType = room_type;
    }

    #region Properties
    public (int x, int y) GridPosition { get; private set; }

    public TypeGridMapCell RoomType { get; set; }

    public int GameObjectIndexInContainer { get; set; }
    #endregion

    /// <summary>
    /// Mark the cell as a wall
    /// </summary>
    public void DisableCell()
    {
        RoomType = TypeGridMapCell.Wall;
    }
}
