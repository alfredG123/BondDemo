using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    // from 1 = 4
    // 1 = a room with a top door
    // 2 = a room with a bottom door
    // 3 = a room with a left door
    // 4 = a room with a right door
    private int room_to_repawn;
    private RoomList room_list;
    private bool has_spawned = false;
    private MazeManagement maze_manager;
    private GameObject map;

    public bool HasSpawned
    {
        get => (has_spawned);
    }

    private void Awake()
    {
        maze_manager = GameObject.Find("MazeManagement").GetComponent<MazeManagement>();
        room_list = maze_manager.GetRoomList();

        map = GameObject.Find("Map");

        if (transform.position.x > 0)
        {
            room_to_repawn = 3;
        }
        else if(transform.position.x < 0)
        {
            room_to_repawn = 4;
        }
        else if (transform.position.y > 0)
        {
            room_to_repawn = 2;
        }
        else if (transform.position.y < 0)
        {
            room_to_repawn = 1;
        }

        Invoke(nameof(SpawnRoom), 0.1f);
    }

    private void SpawnRoom()
    {
        GameObject room = null;

        if (!has_spawned)
        {
            if (room_to_repawn == 1)
            {
                room = GameObject.Instantiate(room_list.GetRoom(RoomList.TypeRoom.RoomWithTopDoor), transform.position, Quaternion.identity);
                room.transform.SetParent(map.transform);
            }
            else if (room_to_repawn == 2)
            {
                room = GameObject.Instantiate(room_list.GetRoom(RoomList.TypeRoom.RoomWithBottomDoor), transform.position, Quaternion.identity);
                room.transform.SetParent(map.transform);
            }
            else if (room_to_repawn == 3)
            {
                room = GameObject.Instantiate(room_list.GetRoom(RoomList.TypeRoom.RoomWithLeftDoor), transform.position, Quaternion.identity);
                room.transform.SetParent(map.transform);
            }
            else if (room_to_repawn == 4)
            {
                room = GameObject.Instantiate(room_list.GetRoom(RoomList.TypeRoom.RoomWithRightDoor), transform.position, Quaternion.identity);
                room.transform.SetParent(map.transform);
            }

            //maze_manager.RoomUpdate(gameObject);

            has_spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RoomSpawner"))
        {
            if ((!collision.GetComponent<RoomGenerator>().HasSpawned) && (!has_spawned))
            {
                GameObject room;

                room = GameObject.Instantiate(room_list.Wall, transform.position, Quaternion.identity);

                room.transform.SetParent(map.transform);

                Destroy(gameObject);
            }

            has_spawned = true;
        }
    }
}
