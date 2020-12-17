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

    public void GenerateNeighbors()
    {
        GameObject neighbor;
        GameObject room_to_create = null;
        Vector2 position = Vector2.zero;

        GameObject map = GameObject.Find("Map");

        MazeManagement maze_manager = GameObject.Find("MazeManagement").GetComponent<MazeManagement>();
        RoomList room_list = maze_manager.GetRoomList();

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
}
