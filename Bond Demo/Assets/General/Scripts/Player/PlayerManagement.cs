using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    private readonly List<Monster> monsters_in_party = new List<Monster>();

    public void AddMonsterToParty(Monster _chosen_monster)
    {
        _chosen_monster.JoinParty();
        monsters_in_party.Add(_chosen_monster);
    }

    public Monster GetMonsterFromParty(int _party_index)
    {
        return (monsters_in_party[_party_index]);
    }
}
