using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watress : BaseMonster
{
    public Watress()
    {
        name = "Watress";
        entry_number = 3;

        health = 10;
        attack = 1;
        defense = 1;
        speed = 3;

        weakness = new List<Attribute>();
        weakness.Add(Attribute.Plant);

        talent = new BlessingWave();
    }
}
