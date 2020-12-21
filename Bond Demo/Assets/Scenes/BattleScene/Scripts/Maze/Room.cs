using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private List<TypeDoor> doors;
#pragma warning restore 0649

    private List<GameObject> neighbors = new List<GameObject>();
    private float distance = 2f;
    private GameObject map;
    private bool is_mystery = false;
    private GameObject parent;

    public void GenerateNeighbors()
    {
        GameObject neighbor;
        GameObject room_to_create = null;

        Debug.Log("Generate");

        if (transform.childCount == 0)
        {
            return;
        }

        Debug.Log("Pass");

        map = GameObject.Find("Map");

        MazeManagement maze_manager = GameObject.Find("MazeManagement").GetComponent<MazeManagement>();
        RoomList room_list = maze_manager.GetRoomList();

        maze_manager.UpdateLastRoom(gameObject);

        TypeDoor door = TypeDoor.TopDoor;

        for (int i = 0; i < transform.childCount; i++)
        {
            Debug.Log("create room");

            door = GetDoorFromChecker(i);

            switch (door)
            {
                case TypeDoor.TopDoor:
                    room_to_create = room_list.GetRoom(RoomList.TypeRoom.RoomWithBottomDoor);
                    break;

                case TypeDoor.BottomDoor:
                    room_to_create = room_list.GetRoom(RoomList.TypeRoom.RoomWithTopDoor);
                    break;

                case TypeDoor.LeftDoor:
                    room_to_create = room_list.GetRoom(RoomList.TypeRoom.RoomWithLeftDoor);
                    break;

                case TypeDoor.RightDoor:
                    room_to_create = room_list.GetRoom(RoomList.TypeRoom.RoomWithRightDoor);
                    break;
            }

            Debug.Log(door);

            neighbor = GameObject.Instantiate(room_to_create, transform.GetChild(i).transform.position, Quaternion.identity);
            neighbor.transform.SetParent(map.transform);
            neighbor.GetComponent<Room>().SetParent(gameObject);

            neighbors.Add(neighbor);
        }

        //// Depend on the room itself, choose the room with the correct room
        //foreach (TypeDoor door in doors)
        //{
        //    switch (door)
        //    {
        //        case TypeDoor.TopDoor:
        //            room_to_create = room_list.GetRoom(RoomList.TypeRoom.RoomWithBottomDoor);
        //            neighbor_index = 1;
        //            break;

        //        case TypeDoor.BottomDoor:
        //            room_to_create = room_list.GetRoom(RoomList.TypeRoom.RoomWithTopDoor);
        //            neighbor_index = 0;
        //            break;

        //        case TypeDoor.LeftDoor:
        //            room_to_create = room_list.GetRoom(RoomList.TypeRoom.RoomWithRightDoor);
        //            neighbor_index = 3;
        //            break;

        //        case TypeDoor.RightDoor:
        //            room_to_create = room_list.GetRoom(RoomList.TypeRoom.RoomWithLeftDoor);
        //            neighbor_index = 2;
        //            break;
        //    }

        //    if (VerifyValidPosition(neighbor_index))
        //    {
        //        Debug.Log(room_to_create.name);
        //        Debug.Log(neighbor_index);

        //        neighbor = GameObject.Instantiate(room_to_create, transform.GetChild(neighbor_index).transform.position, Quaternion.identity);
        //        neighbor.transform.SetParent(map.transform);

        //        neighbor.GetComponent<Room>().SetParent(gameObject);

        //        //neighbor.GetComponent<Room>().GenerateNeighbors();

        //        neighbors.Add(neighbor);
        //    }
        //}
    }

    private TypeDoor GetDoorFromChecker(int index)
    {
        TypeDoor door = TypeDoor.TopDoor;

        Vector3 position = transform.GetChild(index).position;

        if (position.y > 0)
        {
            door = TypeDoor.TopDoor;
        }
        else if (position.y < 0)
        {
            door = TypeDoor.BottomDoor;
        }
        else if (position.x > 0)
        {
            door = TypeDoor.LeftDoor;
        }
        else if(position.x < 0)
        {
            door = TypeDoor.RightDoor;
        }

        return (door);
    }

    private bool VerifyValidPosition(int index)
    {
        bool is_valid = true;

        if (index >= transform.childCount)
        {
            is_valid = false;
        }

        return (is_valid);
    }

    public bool CheckIsNeighbor(GameObject room)
    {
        bool is_neighbor = false;

        if (neighbors.Contains(room))
        {
            is_neighbor = true;
        }

        return (is_neighbor);
    }

    public void SetParent(GameObject room)
    {
        parent = room;
    }

    public bool NeedMysteryRoom(TypeDoor door)
    {
        bool need = false;

        if (doors.Contains(door))
        {
            need = true;
        }

        return (need);
    }

    // for myster only
    public TypeDoor GetDoor()
    {
        return (doors[0]);
    }

    public GameObject GetMysteryRoom(TypeDoor door)
    {
        GameObject room = null;
        TypeDoor mystery_room = TypeDoor.TopDoor;

        foreach (GameObject neighbor in neighbors)
        {
            switch (door)
            {
                case TypeDoor.TopDoor:
                    mystery_room = TypeDoor.BottomDoor;
                    break;

                case TypeDoor.BottomDoor:
                    mystery_room = TypeDoor.TopDoor;
                    break;

                case TypeDoor.LeftDoor:
                    mystery_room = TypeDoor.RightDoor;
                    break;

                case TypeDoor.RightDoor:
                    mystery_room = TypeDoor.LeftDoor;
                    break;
            }

            if ((neighbor.GetComponent<Room>().GetMystery()) && (neighbor.GetComponent<Room>().GetDoor() == mystery_room))
            {
                room = neighbor;
                neighbor.SetActive(true);
                break;
            }
        }

        return (room);
    }

    public bool GetMystery()
    {
        return (is_mystery);
    }

    public void SetMystery()
    {
        is_mystery = true;

        GetComponent<SpriteRenderer>().sortingLayerName = "MysteryRoom";
    }

    public GameObject CreateMysterRoom(TypeDoor door, GameObject room_to_create)
    {
        Vector2 position = Vector2.zero;

        switch (door)
        {
            case TypeDoor.TopDoor:
                position = new Vector2(transform.position.x, transform.position.y + distance);
                break;

            case TypeDoor.BottomDoor:
                position = new Vector2(transform.position.x, transform.position.y - distance);
                break;

            case TypeDoor.LeftDoor:
                position = new Vector2(transform.position.x - distance, transform.position.y);
                break;

            case TypeDoor.RightDoor:
                position = new Vector2(transform.position.x + distance, transform.position.y);
                break;
        }

        GameObject room = GameObject.Instantiate(room_to_create, position, Quaternion.identity);
        room.transform.SetParent(map.transform);

        room.GetComponent<Room>().SetParent(gameObject);
        room.GetComponent<Room>().SetMystery();

        neighbors.Add(room);

        return (room);
    }
}
