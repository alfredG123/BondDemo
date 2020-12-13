using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Level", menuName ="BOND/Level")]
public class MonsterInLevel : ScriptableObject
{
    [SerializeField] private int game_level;
    [SerializeField] private List<GameObject> monsters_in_level = null;

    public List<GameObject> MonstersInLevel
    {
        get => (monsters_in_level);
    }
}
