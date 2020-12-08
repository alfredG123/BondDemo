using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabStorage : MonoBehaviour
{
    private Dictionary<int, GameObject> monster_prefab_dictionary;
    private MonsterManagement monster_look_up;

    private void Awake()
    {
        monster_look_up = GetComponent<MonsterManagement>();
        monster_prefab_dictionary = new Dictionary<int, GameObject>();
    }

    public GameObject GetMonsterPrefab(int entry_number)
    {
        GameObject monster_prefab;
        
        monster_prefab_dictionary.TryGetValue(entry_number, out monster_prefab);

        if (monster_prefab == null)
        {
            monster_prefab = Resources.Load<GameObject>("Prefabs/Monsters/PlayableMonsters/" + monster_look_up.GetMonsterInfo(entry_number).MonsterName);
            monster_prefab_dictionary.Add(entry_number, monster_prefab);
        }

        return (monster_prefab);
    }
}
