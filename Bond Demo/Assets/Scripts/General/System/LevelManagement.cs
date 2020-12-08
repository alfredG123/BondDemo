using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManagement : MonoBehaviour
{
    public List<BaseMonsterInfo> GenerateMonstersForEnemy()
    {
        BaseMonsterInfo enemy_monster;
        List<BaseMonsterInfo> enemy_monster_team = new List<BaseMonsterInfo>();
        MonsterManagement monster_lookup = GetComponent<MonsterManagement>();

        enemy_monster = monster_lookup.GetMonsterInfo(Random.Range(monster_lookup.MinEntryNumber, monster_lookup.MaxEntryNumber));

        enemy_monster_team.Add(enemy_monster);

        return (enemy_monster_team);
    }
}
