using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCellSpriteSelector : MonoBehaviour
{
    [SerializeField] private List<Sprite> _SpriteList = new List<Sprite>();

    private Color _DefaultColor;

    /// <summary>
    /// Set the sprite for the object and default color
    /// </summary>
    /// <param name="grid_map_cell_type"></param>
    public void SetSprite(EventMap.EventCellType grid_map_cell_type)
    {
        int sprite_index = 0;

        if (grid_map_cell_type == EventMap.EventCellType.Block)
        {
            sprite_index = 0;
        }
        else if (grid_map_cell_type == EventMap.EventCellType.Open)
        {
            sprite_index = 1;
        }

        this.gameObject.GetComponent<SpriteRenderer>().sprite = _SpriteList[sprite_index];

        _DefaultColor = this.gameObject.GetComponent<SpriteRenderer>().color;
    }

    /// <summary>
    /// Set the color the grid map cell based on the flag, is_reachable
    /// </summary>
    /// <param name="is_reachable"></param>
    public void SetColorForReachable(bool is_reachable)
    {
        if (is_reachable)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = _DefaultColor;
        }
    }
}
