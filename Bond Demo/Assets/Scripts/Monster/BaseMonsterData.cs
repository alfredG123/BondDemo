using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonsterData
{
    Dictionary<int, BaseMonster> monster_dictionary;

    public BaseMonsterData()
    {
        monster_dictionary = new Dictionary<int, BaseMonster>();
        monster_dictionary.Add(0, new BaseMonster());
        monster_dictionary.Add(1, new PlantTypeMonster("Grassy", 1, 10, 1, 0, 1));
        monster_dictionary.Add(2, new FireTypeMonster("Firess", 1, 10, 1, 0, 1));
        monster_dictionary.Add(3, new WaterTypeMonster("Watress", 1, 10, 1, 0, 1));
        monster_dictionary.Add(4, new EarthTypeMonster("Earthy", 1, 10, 1, 0, 1));
        monster_dictionary.Add(5, new WindTypeMonster("Wince", 1, 10, 1, 0, 1));
    }

    public BaseMonster GetMonster(int entry_number)
    {
        return (monster_dictionary[entry_number]);
    }
}
