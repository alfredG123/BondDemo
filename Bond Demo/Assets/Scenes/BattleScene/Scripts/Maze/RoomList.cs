using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "BOND/Level/Map")]
public class RoomList : ScriptableObject
{
#pragma warning disable 0649
    [SerializeField] private List<RoomSetup> rooms_with_top_door;
    [SerializeField] private List<RoomSetup> rooms_with_bottom_door;
    [SerializeField] private List<RoomSetup> rooms_with_left_door;
    [SerializeField] private List<RoomSetup> rooms_with_right_door;
#pragma warning restore 0649

    [System.Serializable]
    private struct RoomSetup
    {
#pragma warning disable 0649
        public GameObject room;
        public float possibility;
#pragma warning restore 0649
    }

    public enum TypeRoom
    {
        RoomWithTopDoor,
        RoomWithBottomDoor,
        RoomWithLeftDoor,
        RoomWithRightDoor,
    }

    public GameObject GetRoom(TypeRoom open_door_required)
    {
        GameObject room = null;

        switch (open_door_required)
        {
            case TypeRoom.RoomWithTopDoor:
                room = PickRoom(rooms_with_top_door);
                break;

            case TypeRoom.RoomWithBottomDoor:
                room = PickRoom(rooms_with_bottom_door);
                break;

            case TypeRoom.RoomWithLeftDoor:
                room = PickRoom(rooms_with_left_door);
                break;

            case TypeRoom.RoomWithRightDoor:
                room = PickRoom(rooms_with_right_door);
                break;
        }

        return (room);
    }

    private GameObject PickRoom(List<RoomSetup> rooms)
    {
        float rand;
        GameObject room = null;
        int room_index = 0;
        int limit = 100;

        if (rooms.Count == 0)
        {
            GeneralScripts.ReturnToStarterScene("PickRoom");
        }

        if (rooms.Count == 1)
        {
            room = rooms[0].room;
        }

        while (room == null)
        {
            rand = Random.Range(0f, 1f);

            if (room_index >= rooms.Count)
            {
                room_index = 0;
            }

            if (rooms[room_index].possibility <= 0)
            {
                GeneralScripts.ReturnToStarterScene("PickRoom");
            }

            if (rand <= rooms[room_index].possibility)
            {
                if (rooms[room_index].room == null)
                {
                    GeneralScripts.ReturnToStarterScene("PickRoom");
                }

                room = rooms[room_index].room;
            }

            if (limit < 0)
            {
                room = rooms[room_index].room;
            }

            limit--;
            room_index++;
        }

        return (room);
    }
}