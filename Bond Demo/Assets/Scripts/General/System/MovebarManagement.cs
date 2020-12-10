using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovebarManagement
{
    private List<MonsterData> monster_list = new List<MonsterData>();
    private int current_monster = 0;

    public MovebarManagement()
    {
        // Need to be fixed
        monster_list = new List<MonsterData>();
    }

    public void AddMonsterToFight(MonsterData monster_to_add)
    {
        monster_list.Add(monster_to_add);
    }

    public MonsterData GetFirstMonster()
    {
        MonsterData monster = (MonsterData) monster_list[current_monster];

        current_monster++;

        if (current_monster >= monster_list.Count)
        {
            current_monster = 0;
        }

        return (monster);
    }
}
