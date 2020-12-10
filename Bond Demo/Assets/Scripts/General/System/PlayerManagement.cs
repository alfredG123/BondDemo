using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    // need to be public
    public PlayerData player_data;

    private void Awake()
    {

    }

    public void SetLinkedMonster(MonsterData _chosen_monster)
    {
        _chosen_monster.is_linked_monster = true;
        _chosen_monster.fight_with_player = true;
        player_data.Monsters_in_party.Add(_chosen_monster);
    }
}
