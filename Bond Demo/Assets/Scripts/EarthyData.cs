using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Earthy", menuName = "Monsters/Earthy")]
public class EarthyData : MonsterData
{
    private void Awake()
    {
        name = "Earthy";

        health = 12;
        attack = 1;
        defense = 2;
        speed = 1;

        weakness = new List<Attribute>();
        weakness.Add(Attribute.Plant);

        talent = new RockArmor();
    }
}
