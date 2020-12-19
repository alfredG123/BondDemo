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

    public void GenerateNeighbors()
    {
        GameObject neighbor;
        GameObject room_to_create = null;
        Vector2 position = Vector2.zero;

        map = GameObject.Find("Map");

        MazeManagement maze_manager = GameObject.Find("MazeManagement").GetComponent<MazeManagement>();
        RoomList room_list = maze_manager.GetRoomList();

        maze_manager.UpdateLastRoom(gameObject);

        // Depend on the room itself, choose the room with the correct room
        foreach (TypeDoor door in doors)
        {
            switch (door)
            {
                case TypeDoor.TopDoor:
                    room_to_create = room_list.GetRoom(RoomList.TypeRoom.RoomWithBottomDoor);
                    position = new Vector2(transform.position.x, transform.position.y + distance);
                    break;

                case TypeDoor.BottomDoor:
                    room_to_create = room_list.GetRoom(RoomList.TypeRoom.RoomWithTopDoor);
                    position = new Vector2(transform.position.x, transform.position.y - distance);
                    break;

                case TypeDoor.LeftDoor:
                    room_to_create = room_list.GetRoom(RoomList.TypeRoom.RoomWithRightDoor);
                    position = new Vector2(transform.position.x - distance, transform.position.y);
                    break;

                case TypeDoor.RightDoor:
                    room_to_create = room_list.GetRoom(RoomList.TypeRoom.RoomWithLeftDoor);
                    position = new Vector2(transform.position.x + distance, transform.position.y);
                    break;
            }

            if (VerifyValidPosition(position))
            {
                neighbor = GameObject.Instantiate(room_to_create, position, Quaternion.identity);
                neighbor.transform.SetParent(map.transform);
                neighbor.GetComponent<Room>().GenerateNeighbors();

                neighbor.GetComponent<Room>().SetNeighbor(gameObject);

                neighbors.Add(neighbor);
            }
        }
    }

    private bool VerifyValidPosition(Vector2 position)
    {
        Collider2D game_object_detector = Physics2D.OverlapCircle(position, 0.01f);
        
        bool is_valid = false;

        if (game_object_detector == null)
        {
            is_valid = true;
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

    public void SetNeighbor(GameObject room)
    {
        neighbors.Add(room);
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

        room.GetComponent<Room>().SetNeighbor(gameObject);
        room.GetComponent<Room>().SetMystery();

        neighbors.Add(room);

        return (room);
    }
}
