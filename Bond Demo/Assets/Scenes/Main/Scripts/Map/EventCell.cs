﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCell
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="grid_position"></param>
    /// <param name="room_type"></param>
    public EventCell((int x, int y) grid_position, EventMap.EventCellType room_type)
    {
        GridPosition = grid_position;
        CellType = room_type;
        CellTypeOnNextIteration = room_type;
    }

    #region Properties
    public (int x, int y) GridPosition { get; private set; }

    public EventMap.EventCellType CellType { get; set; }

    public EventMap.EventCellType CellTypeOnNextIteration { get; set; }

    public bool IsVisited { get; set; } = false;

    public int GameObjectIndexInContainer { get; set; }

    public int DestinationPartIndex { get; set; }
    public int DestinationWormholeIndex { get; set; }


    public int DestinatioX { get; set; }
    public int DestinatioY { get; set; }
    #endregion

    /// <summary>
    /// Mark the cell as a wall
    /// </summary>
    public void DisableCell()
    {
        CellType = EventMap.EventCellType.Block;
    }
}
