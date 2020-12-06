using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private readonly BaseMonster linked_monster;

    public PlayerData(BaseMonster _linked_monster)
    {
        linked_monster = _linked_monster;
    }

    public BaseMonster LinkedMonster
    {
        get { return (linked_monster); }
    }
}
