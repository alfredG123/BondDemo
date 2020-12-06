using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthy : BaseMonster
{
    public Earthy()
    {
        name = "Earthy";
        entry_number = 4;

        health = 12;
        attack = 1;
        defense = 2;
        speed = 1;

        weakness = new List<Attribute>();
        weakness.Add(Attribute.Plant);

        talent = new RockArmor();
    }
}
