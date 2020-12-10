using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Grassy", menuName ="Monsters/Grassy")]
public class GrassyData : MonsterData
{
    private void Awake()
    {
        monster_name = "Grassy";

        health = 10;
        attack = 1;
        defense = 0;
        speed = 2;

        weakness = new List<Attribute>();
        weakness.Add(Attribute.Fire);
        weakness.Add(Attribute.Wind);
    }
}
