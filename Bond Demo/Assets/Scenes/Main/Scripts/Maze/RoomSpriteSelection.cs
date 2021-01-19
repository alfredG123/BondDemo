using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpriteSelection : MonoBehaviour
{
    [SerializeField] private List<Sprite> _RoomSpiritList = null;

    private Color _DefaultColor;

    public void SetSprite(TypeRoom room_type)
    {
        if (_RoomSpiritList == null)
        {
            _RoomSpiritList = new List<Sprite>();
        }

        int sprite_index = 0;

        if (room_type == TypeRoom.Wall)
        {
            sprite_index = 0;
        }
        else if (room_type == TypeRoom.Normal)
        {
            sprite_index = 1;
        }

        this.gameObject.GetComponent<SpriteRenderer>().sprite = _RoomSpiritList[sprite_index];

        _DefaultColor = this.gameObject.GetComponent<SpriteRenderer>().color;
    }

    public void SetColorForReachable(bool is_reachable)
    {
        if (_DefaultColor == null)
        {
            General.ReturnToTitleSceneForErrors("SetColorForReachable", "_DefaultColor is not set");
        }

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
