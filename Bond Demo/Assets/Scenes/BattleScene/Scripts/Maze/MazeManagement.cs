using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManagement : MonoBehaviour
{
    [SerializeField] List<GameObject> possible_starting_position = null;
    [SerializeField] GameObject start_position_icon = null;
    [SerializeField] GameObject end_position_icon = null;
    [SerializeField] List<RoomList> map_templates = null;
    [SerializeField] GameObject map = null;

    private GameObject last_room = null;
    private float wait_time = .5f;
    private int level = 0;
    private bool need_end_room = true;
    private GameObject player;
    private List<GameObject> all_rooms;

    private void Awake()
    {
        GameObject room;

        all_rooms = new List<GameObject>();

        int rand = UnityEngine.Random.Range(0, possible_starting_position.Count);
        
        room = GameObject.Instantiate(possible_starting_position[rand], Vector2.zero, Quaternion.identity);
        room.transform.SetParent(map.transform);

        player = GameObject.Instantiate(start_position_icon, possible_starting_position[rand].transform.position, Quaternion.identity);
        player.transform.SetParent(map.transform);
    }

    private void Update()
    {
        if (need_end_room)
        {
            if (wait_time < 0)
            {
                GameObject room = GameObject.Instantiate(end_position_icon, last_room.transform.position, Quaternion.identity);

                room.transform.SetParent(map.transform);

                need_end_room = false;
            }
            else
            {
                wait_time -= Time.deltaTime;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D game_object_detector;
            Vector2 mouse_position = GeneralScripts.GetMousePositionInWorldSpace();

            game_object_detector = Physics2D.OverlapCircle(mouse_position, 0.1f);

            if (game_object_detector != null)
            {
                float distance = Vector2.Distance(player.transform.localPosition, game_object_detector.gameObject.transform.localPosition);

                Debug.Log(distance);

                if (distance <= 2f)
                {
                    player.transform.localPosition = game_object_detector.gameObject.transform.localPosition;
                }
            }
        }

        if ((last_room != null) && (player.transform.localPosition == last_room.transform.localPosition))
        {
            CreateNewMap();
        }
    }

    private void CreateNewMap()
    {
        foreach (GameObject room in all_rooms)
        {
            Destroy(room);
        }

        GameObject start_room;

        int rand = UnityEngine.Random.Range(0, possible_starting_position.Count);

        start_room = GameObject.Instantiate(possible_starting_position[rand], player.transform.localPosition, Quaternion.identity);
        start_room.transform.SetParent(map.transform);
    }

    public void RoomUpdate(GameObject room)
    {
        all_rooms.Add(room);

        last_room = room;

        wait_time = .5f;
    }

    public RoomList GetRoomList()
    {
        return (map_templates[level]);
    }
}
