using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpriteSelection : MonoBehaviour
{
    [SerializeField] private List<Sprite> _RoomSpiritList = null;

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
    }
}
