using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManagement : MonoBehaviour
{
    private Dictionary<int, BaseMonster> monster_dictionary;

    private void Awake()
    {
        monster_dictionary = new Dictionary<int, BaseMonster>();

        monster_dictionary.Add(0, new BaseMonster());
        monster_dictionary.Add(1, new Grassy());
        monster_dictionary.Add(2, new Firess());
        monster_dictionary.Add(3, new Watress());
        monster_dictionary.Add(4, new Earthy());
        monster_dictionary.Add(5, new Wince());
    }

    public int MinEntryNumber
    {
        get { return (1); }
    }

    public int MaxEntryNumber
    {
        get { return (monster_dictionary.Count); }
    }

    public BaseMonster GetMonsterInfo(int monster_entry_number)
    {
        return (monster_dictionary[monster_entry_number]);
    }
}
