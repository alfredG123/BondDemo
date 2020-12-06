using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private PlayerData player_data = null;

    public PlayerData Player
    {
        get { return(player_data); }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetUpPlayer(BaseMonster _chosen_monster)
    {
        if (player_data == null)
        {
            player_data = new PlayerData(_chosen_monster);
        }
    }
}
