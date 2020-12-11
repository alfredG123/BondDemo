using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "BOND/Player")]
public class PlayerData : ScriptableObject
{
    // need to be private
    [SerializeField] public List<MonsterData> Monsters_in_party = new List<MonsterData>();
}
