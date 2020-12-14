using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomList : MonoBehaviour
{
    [SerializeField] private List<GameObject> rooms_with_top_door = null;
    [SerializeField] private List<GameObject> rooms_with_bottom_door = null;
    [SerializeField] private List<GameObject> rooms_with_left_door = null;
    [SerializeField] private List<GameObject> rooms_with_right_door = null;

    [SerializeField] private GameObject wall = null;

    #region

    public List<GameObject> TopDoorRooms
    {
        get => (rooms_with_top_door);
    }

    public List<GameObject> BottomDoorRooms
    {
        get => (rooms_with_bottom_door);
    }

    public List<GameObject> LeftDoorRooms
    {
        get => (rooms_with_left_door);
    }

    public List<GameObject> RightDoorRooms
    {
        get => (rooms_with_right_door);
    }

    public GameObject Wall
    {
        get => (wall);
    }

    #endregion
}
