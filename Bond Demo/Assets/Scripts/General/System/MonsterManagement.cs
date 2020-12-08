using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManagement : MonoBehaviour
{
    private Dictionary<int, BaseMonsterInfo> monster_dictionary;

    private void Awake()
    {
        monster_dictionary = new Dictionary<int, BaseMonsterInfo>();

        monster_dictionary.Add(0, new BaseMonsterInfo());
        monster_dictionary.Add(1, new GrassyInfo());
        monster_dictionary.Add(2, new FiressInfo());
        monster_dictionary.Add(3, new WatressInfo());
        monster_dictionary.Add(4, new EarthyInfo());
        monster_dictionary.Add(5, new WinceInfo());
    }

    public int MinEntryNumber
    {
        get { return (1); }
    }

    public int MaxEntryNumber
    {
        get { return (monster_dictionary.Count); }
    }

    public BaseMonsterInfo GetMonsterInfo(int monster_entry_number)
    {
        return (monster_dictionary[monster_entry_number]);
    }
}
