using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManagement : MonoBehaviour
{
    [SerializeField] List<GameObject> possible_starting_position = null;
    [SerializeField] GameObject start_position_icon = null;
    [SerializeField] GameObject end_position_icon = null;

    private GameObject last_room;
    private float wait_time = .5f;

    private void Awake()
    {
        int rand = UnityEngine.Random.Range(0, possible_starting_position.Count);
        GameObject.Instantiate(possible_starting_position[rand], Vector2.zero, Quaternion.identity);
        GameObject.Instantiate(start_position_icon, Vector2.zero, Quaternion.identity);
    }

    private void Update()
    {
        if (wait_time < 0)
        {
            GameObject.Instantiate(end_position_icon, last_room.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            wait_time -= Time.deltaTime;
        }
    }

    public void RoomUpdate(GameObject room)
    {
        last_room = room;

        wait_time = .5f;
    }
}
