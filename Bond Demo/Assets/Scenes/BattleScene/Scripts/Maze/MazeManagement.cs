using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManagement : MonoBehaviour
{
    [SerializeField] private List<GameObject> possible_starting_position = null;
    [SerializeField] private GameObject start_position_icon = null;
    [SerializeField] private GameObject end_position_icon = null;
    [SerializeField] private List<RoomList> map_templates = null;
    [SerializeField] private GameObject map = null;
    [SerializeField] private List<GameObject> base_room = null;

    private GameObject last_room = null;
    private float wait_time = .5f;
    private int level = 0;
    private bool need_end_room = true;
    private GameObject player;
    private GameObject end;
    private GameObject current_room;

    private void Awake()
    {
        int rand = UnityEngine.Random.Range(0, possible_starting_position.Count);
        
        current_room = GameObject.Instantiate(possible_starting_position[rand], Vector2.zero, Quaternion.identity);
        current_room.transform.SetParent(map.transform);
        current_room.GetComponent<Room>().GenerateNeighbors();

        player = GameObject.Instantiate(start_position_icon, current_room.transform.position, Quaternion.identity);
        player.transform.SetParent(map.transform);
    }

    private void Update()
    {
        //if (need_end_room)
        //{
        //    if (wait_time < 0)
        //    {
        //        end = GameObject.Instantiate(end_position_icon, last_room.transform.position, Quaternion.identity);

        //        end.transform.SetParent(map.transform);

        //        need_end_room = false;
        //    }
        //    else
        //    {
        //        wait_time -= Time.deltaTime;
        //    }
        //}

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D game_object_detector;
            Vector2 mouse_position = GeneralScripts.GetMousePositionInWorldSpace();
            GameObject room;

            game_object_detector = Physics2D.OverlapCircle(mouse_position, 0.1f);

            if (game_object_detector != null)
            {
                if(game_object_detector.GetComponent<Room>() == null)
                {
                    return;
                }

                float distance = Vector2.Distance(player.transform.localPosition, game_object_detector.gameObject.transform.localPosition);

                if (distance <= 2f)
                {
                    room = game_object_detector.gameObject;

                    if (current_room.GetComponent<Room>().CheckIsNeighbor(room))
                    {
                        if (current_room.GetComponent<Room>().GetMystery())
                        {
                            current_room.SetActive(false);
                        }

                        current_room = room;
                        current_room.GetComponent<Room>().GenerateNeighbors();

                        player.transform.localPosition = game_object_detector.gameObject.transform.localPosition;
                    }
                    else
                    {
                        TypeDoor direction;

                        float x = player.transform.localPosition.x - game_object_detector.gameObject.transform.localPosition.x;
                        float y = player.transform.localPosition.y - game_object_detector.gameObject.transform.localPosition.y;

                        GameObject mystery_room;

                        if (x > 0)
                        {
                            direction = TypeDoor.LeftDoor;
                            mystery_room = base_room[3];
                        }
                        else if(x < 0)
                        {
                            direction = TypeDoor.RightDoor;
                            mystery_room = base_room[2];
                        }
                        else if (y > 0)
                        {
                            direction = TypeDoor.BottomDoor;
                            mystery_room = base_room[0];
                        }
                        else
                        {
                            direction = TypeDoor.TopDoor;
                            mystery_room = base_room[1];
                        }

                        if (current_room.GetComponent<Room>().NeedMysteryRoom(direction))
                        {
                            GameObject visited_mystery_room = current_room.GetComponent<Room>().GetMysteryRoom(direction);

                            if (visited_mystery_room == null)
                            {
                                current_room = current_room.GetComponent<Room>().CreateMysterRoom(direction, mystery_room);
                            }
                            else
                            {
                                current_room = visited_mystery_room;
                            }

                            player.transform.localPosition = game_object_detector.gameObject.transform.localPosition;
                        }
                    }
                }
            }
        }

        if ((end != null) && (player.transform.localPosition == end.transform.localPosition))
        {
            //CreateNewMap();
        }
    }

    private void CreateNewMap()
    {
        GameObject map = GameObject.Find("Map");

        Vector2 position = player.transform.position;

        int child_count = map.transform.childCount;

        if (map != null)
        {
            for (int i = 0; i < child_count; i++)
            {
                Destroy(map.transform.GetChild(i).gameObject);
            }
        }

        int rand = UnityEngine.Random.Range(0, possible_starting_position.Count);

        current_room = GameObject.Instantiate(possible_starting_position[rand], map.transform, false);
        current_room.transform.position = position;
        current_room.GetComponent<Room>().GenerateNeighbors();

        player = GameObject.Instantiate(start_position_icon, current_room.transform.position, Quaternion.identity);
        player.transform.SetParent(map.transform);

        need_end_room = true;
    }

    public void UpdateLastRoom(GameObject room)
    {
        last_room = room;

        wait_time = .2f;
    }

    public RoomList GetRoomList()
    {
        return (map_templates[level]);
    }
}
