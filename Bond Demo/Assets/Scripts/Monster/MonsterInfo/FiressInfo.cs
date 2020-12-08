using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiressInfo : BaseMonsterInfo
{
    public FiressInfo()
    {
        name = "Firess";
        entry_number = 2;

        health = 8;
        attack = 2;
        defense = 0;
        speed = 3;

        weakness = new List<Attribute>();
        weakness.Add(Attribute.Water);

        talent = new MaximumFlame();
    }
}
