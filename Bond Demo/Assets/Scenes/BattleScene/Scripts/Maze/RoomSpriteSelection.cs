using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpriteSelection : MonoBehaviour
{
    [SerializeField] private Sprite L;
    [SerializeField] private Sprite LR;
    [SerializeField] private Sprite LB;
    [SerializeField] private Sprite LT;
    [SerializeField] private Sprite LRB;
    [SerializeField] private Sprite LRT;
    [SerializeField] private Sprite LBT;
    [SerializeField] private Sprite LRBT;
    [SerializeField] private Sprite R;
    [SerializeField] private Sprite RT;
    [SerializeField] private Sprite RB;
    [SerializeField] private Sprite RBT;
    [SerializeField] private Sprite T;
    [SerializeField] private Sprite TB;
    [SerializeField] private Sprite B;

    public void SetSprite(List<TypeDoor> open_doors)
    {
        Sprite room_sprite;

        if (open_doors.Contains(TypeDoor.LeftDoor))
        {
            room_sprite = L;

            if (open_doors.Contains(TypeDoor.RightDoor))
            {
                room_sprite = LR;

                if (open_doors.Contains(TypeDoor.BottomDoor))
                {
                    room_sprite = LRB;

                    if (open_doors.Contains(TypeDoor.TopDoor))
                    {
                        room_sprite = LRBT;
                    }
                }
                else if (open_doors.Contains(TypeDoor.TopDoor))
                {
                    room_sprite = LRT;
                }
            }
            else if (open_doors.Contains(TypeDoor.BottomDoor))
            {
                room_sprite = LB;

                if (open_doors.Contains(TypeDoor.TopDoor))
                {
                    room_sprite = LBT;
                }
            }
            else if (open_doors.Contains(TypeDoor.TopDoor))
            {
                room_sprite = LT;
            }
        }
        else if (open_doors.Contains(TypeDoor.RightDoor))
        {
            room_sprite = R;

            if (open_doors.Contains(TypeDoor.LeftDoor))
            {
                room_sprite = RT;
            }
            else if (open_doors.Contains(TypeDoor.BottomDoor))
            {
                room_sprite = RB;

                if (open_doors.Contains(TypeDoor.TopDoor))
                {
                    room_sprite = RBT;
                }
            }

        }
        else if (open_doors.Contains(TypeDoor.TopDoor))
        {
            room_sprite = T;

            if (open_doors.Contains(TypeDoor.BottomDoor))
            {
                room_sprite = TB;
            }
        }
        else
        {
            room_sprite = B;
        }

        this.gameObject.GetComponent<SpriteRenderer>().sprite = room_sprite;
    }
}
