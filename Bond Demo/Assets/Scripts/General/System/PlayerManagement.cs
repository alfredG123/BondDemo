using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    // need to be private
    public PlayerData player_data;
    public MonsterData debug_monster;

    private void Awake()
    {
        // Clear the party
        // For dev
        player_data.Monsters_in_party.Clear();
        player_data.Monsters_in_party.Add(debug_monster);
    }

    public void SetLinkedMonster(MonsterData _chosen_monster)
    {
        _chosen_monster.is_linked_monster = true;
        _chosen_monster.fight_with_player = true;
        player_data.Monsters_in_party.Add(_chosen_monster);
    }

    private void OnApplicationQuit()
    {
        // Clear the party
        player_data.Monsters_in_party.Clear();
    }
}
