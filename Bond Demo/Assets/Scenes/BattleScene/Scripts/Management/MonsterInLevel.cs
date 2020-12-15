using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Level", menuName ="BOND/Level/Monsters")]
public class MonsterInLevel : ScriptableObject
{
    [SerializeField] private int game_level;
    [SerializeField] private List<MonsterData> monsters_in_level = null;

    public List<MonsterData> MonstersInLevel
    {
        get => (monsters_in_level);
    }
}
