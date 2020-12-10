using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Firess", menuName = "Monsters/Firess")]
public class FiressData : MonsterData
{
    private void Awake()
    {
        name = "Firess";

        health = 8;
        attack = 2;
        defense = 0;
        speed = 3;

        weakness = new List<Attribute>();
        weakness.Add(Attribute.Water);

        talent = new MaximumFlame();
    }
}
