using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpriteSelection : MonoBehaviour
{
    [SerializeField] private List<Sprite> room_sprite_list;

    public void SetSprite(List<TypeDoor> open_doors, TypeRoom room_type)
    {
        string sprite_name = string.Empty;

        if (open_doors.Contains(TypeDoor.LeftDoor))
        {
            sprite_name += 'L';
        }
        
        if (open_doors.Contains(TypeDoor.RightDoor))
        {
            sprite_name += 'R';
        }
        
        if (open_doors.Contains(TypeDoor.TopDoor))
        {
            sprite_name += 'T';
        }

        if (open_doors.Contains(TypeDoor.BottomDoor))
        {
            sprite_name += 'B';
        }

        this.gameObject.GetComponent<SpriteRenderer>().sprite = GetSprite(sprite_name);

        if (room_type == TypeRoom.Entry)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    private Sprite GetSprite(string sprite_name)
    {
        Sprite room_sprite = null;

        for (int i = 0; i < room_sprite_list.Count; i++)
        {
            if (string.Compare(room_sprite_list[i].name, sprite_name) == 0)
            {
                room_sprite = room_sprite_list[i];

                break;
            }
        }

        return (room_sprite);
    }
}
