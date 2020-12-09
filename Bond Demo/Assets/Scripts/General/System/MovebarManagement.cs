using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovebarManagement
{
    private List<BaseMonsterInfo> monster_list;
    private int current_monster = 0;

    public MovebarManagement()
    {
        // Need to be fixed
        monster_list = new List<BaseMonsterInfo>();
    }

    public void AddMonsterToFight(BaseMonsterInfo monster_to_add)
    {
        monster_list.Add(monster_to_add);
    }

    public BaseMonsterInfo GetFirstMonster()
    {
        BaseMonsterInfo monster = (BaseMonsterInfo) monster_list[current_monster];

        current_monster++;

        if (current_monster >= monster_list.Count)
        {
            current_monster = 0;
        }

        return (monster);
    }
}
