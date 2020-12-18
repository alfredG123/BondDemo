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
    private List<GameObject> all_rooms;
    private GameObject end;
    private GameObject starting_room;

    private void Awake()
    {
        //all_rooms = new List<GameObject>();

        int rand = UnityEngine.Random.Range(0, possible_starting_position.Count);
        
        starting_room = GameObject.Instantiate(possible_starting_position[rand], Vector2.zero, Quaternion.identity);
        starting_room.transform.SetParent(map.transform);
        starting_room.GetComponent<Room>().GenerateNeighbors();

        player = GameObject.Instantiate(start_position_icon, possible_starting_position[rand].transform.position, Quaternion.identity);
        player.transform.SetParent(map.transform);
    }

    private void Update()
    {
        if (need_end_room)
        {
            if (wait_time < 0)
            {
                end = GameObject.Instantiate(end_position_icon, last_room.transform.position, Quaternion.identity);

                end.transform.SetParent(map.transform);

                need_end_room = false;
            }
            else
            {
                wait_time -= Time.deltaTime;
            }
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Collider2D game_object_detector;
        //    Vector2 mouse_position = GeneralScripts.GetMousePositionInWorldSpace();

        //    game_object_detector = Physics2D.OverlapCircle(mouse_position, 0.1f);

        //    if (game_object_detector != null)
        //    {
        //        float distance = Vector2.Distance(player.transform.localPosition, game_object_detector.gameObject.transform.localPosition);

        //        if (distance <= 2f)
        //        {
        //            player.transform.localPosition = game_object_detector.gameObject.transform.localPosition;
        //        }
        //    }
        //}

        //if (Input.GetMouseButtonDown(1))
        //{
        //    Debug.Log(last_room.name);
        //    Debug.Log(player.transform.localPosition);
        //    Debug.Log(last_room.transform.localPosition);
        //}

        //if ((last_room != null) && (player.transform.localPosition == last_room.transform.localPosition))
        //{
        //    CreateNewMap();
        //}
    }

    private void CreateNewMap()
    {
        //foreach (GameObject room in all_rooms)
        //{
        //    Destroy(room);
        //}

        //Destroy(end);

        //GameObject start_room;

        //int rand = UnityEngine.Random.Range(0, possible_starting_position.Count);

        //start_room = GameObject.Instantiate(possible_starting_position[rand], player.transform.localPosition, Quaternion.identity);
        //start_room.transform.SetParent(map.transform);

        //need_end_room = true;
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
