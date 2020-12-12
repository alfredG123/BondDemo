using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Level", menuName ="BOND/Level")]
public class MonsterInLevel : ScriptableObject
{
    public int game_level;
    public List<MonsterData> monsters_in_level;
}
